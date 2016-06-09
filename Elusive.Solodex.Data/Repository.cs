using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Transactions;
using Elusive.Solodex.Core.Common;
using Elusive.Solodex.Core.Enumerations;
using Elusive.Solodex.Core.Events;
using Elusive.Solodex.Core.Interfaces;
using Elusive.Solodex.Core.Models;
using Prism.Events;

namespace Elusive.Solodex.Data
{
    /// <summary>
    ///     Implementation of IRepository
    /// </summary>
    public class Repository : IRepository
    {
        /// <summary>
        ///     DatabaseManagementService ensures the schema is up to date for Repository usage.
        ///     Will cause the
        /// </summary>
        private static readonly DatabaseManagementService DatabaseManagementService = new DatabaseManagementService();

        /// <summary>
        ///     The EventAggregator
        /// </summary>
        private readonly IEventAggregator _eventAggregator;

        /// <summary>
        ///     Lock to enforce single threaded access to EF context (per repository instance).
        /// </summary>
        private readonly object _repositoryLock = new object();

        /// <summary>
        /// Temp storage for changes in progress to support update notifications
        /// </summary>
        private List<EntityChangeNotification> _changesInProgress = new List<EntityChangeNotification>();

        /// <summary>
        /// The Entity Framework Context
        /// </summary>
        private Entities _context;

        private bool _disposed;

        private bool _transactionPending;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Repository" /> class.
        /// </summary>
        public Repository(IEventAggregator eventAggregator)
            : this()
        {
            _eventAggregator = eventAggregator;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Repository" /> class.
        /// </summary>
        private Repository()
        {
            try
            {
                lock (DatabaseManagementService)
                {
                    DatabaseManagementService.InitializeDatabase();
                    _context = new Entities();
                    (_context as IObjectContextAdapter).ObjectContext.ContextOptions.LazyLoadingEnabled = false;
                    (_context as IObjectContextAdapter).ObjectContext.SavingChanges += SavingChanges;
                }
            }
            catch (Exception ex)
            {
                throw Core.Common.ExceptionHandler.HandleException<DataAccessException>(
                    "Database error",
                    "A repository instance could not be constructed.  See InnerException for details.",
                    ex);
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether TSQL statements should be logged.
        /// </summary>
        public bool IsLoggingSql { get; set; }

        /// <summary>
        ///     Adds an entity to the repository
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="entity">The entity</param>
        public void Add<T>(T entity) where T : PersistableModel<T>
        {
            Add(new[] {entity}.AsEnumerable());
        }

        /// <summary>
        ///     Adds entities to the repository
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="entities">The entities</param>
        public void Add<T>(IEnumerable<T> entities) where T : PersistableModel<T>
        {
            lock (_repositoryLock)
            {
                try
                {
                    using (new ActivityTracer(_context, IsLoggingSql, "Add<T>(IEnumerable<T>)"))
                    {
                        _context.Set<T>().AddRange(entities);
                    }
                }
                catch (Exception ex)
                {
                    throw ExceptionHandler.HandleException<DataAccessException>(
                        "Database error",
                        "Adding a set of entities to the repository failed.  See InnerException for details.",
                        ex);
                }
            }
        }

        /// <summary>
        ///     Deletes an entity from the repository
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="entity">The entity</param>
        public void Delete<T>(T entity) where T : PersistableModel<T>
        {
            Delete(new[] {entity}.AsEnumerable());
        }

        /// <summary>
        ///     Deletes entities from the repository
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="entities">The entities</param>
        public void Delete<T>(IEnumerable<T> entities) where T : PersistableModel<T>
        {
            lock (_repositoryLock)
            {
                try
                {
                    using (new ActivityTracer(_context, IsLoggingSql, "Delete<T>(IEnumerable<T>)"))
                    {
                        _context.Set<T>().RemoveRange(entities);
                    }
                }
                catch (Exception ex)
                {
                    throw ExceptionHandler.HandleException<DataAccessException>(
                        "Database error",
                        "Removing a set of entities from the repository failed.  See InnerException for details.",
                        ex);
                }
            }
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            lock (_repositoryLock)
            {
                if (!_transactionPending)
                {
                    if (_changesInProgress != null)
                    {
                        _changesInProgress.Clear();
                        _changesInProgress = null;
                    }
                }
                else
                {
                    // Detach all the entities from the context to allow memory cleanup.
                    var entries =
                        (_context as IObjectContextAdapter).ObjectContext.ObjectStateManager.GetObjectStateEntries(
                            EntityState.Added | EntityState.Deleted | EntityState.Modified | EntityState.Unchanged)
                            .Where(x => x.Entity != null)
                            .ToList();
                    foreach (var entry in entries)
                    {
                        if (entry.State != EntityState.Detached)
                        {
                            (_context as IObjectContextAdapter).ObjectContext.Detach(entry.Entity);
                        }
                    }
                }
                if (_context != null)
                {
                    (_context as IObjectContextAdapter).ObjectContext.SavingChanges -= SavingChanges;
                    _context.Dispose();
                    _context = null;
                }
                _disposed = true;
            }
        }


        /// <summary>
        ///     Gets entities based on the filter
        /// </summary>
        /// <typeparam name="T">The entity type returned</typeparam>
        /// <returns>entities of type T </returns>
        public IEnumerable<T> Get<T>() where T : PersistableModel<T>
        {
            lock (_repositoryLock)
            {
                try
                {
                    var objectContext = ((IObjectContextAdapter) _context).ObjectContext;
                    var set = objectContext.CreateObjectSet<T>();
                    return set.ToList();
                }
                catch (Exception ex)
                {
                    throw ExceptionHandler.HandleException<DataAccessException>(
                        "Database error",
                        "A repository query failed.  See InnerException for details.",
                        ex);
                }
            }
        }

        /// <summary>
        /// Gets entities based on the filter
        /// </summary>
        /// <typeparam name="T">The entity type returned</typeparam>
        /// <param name="filter">filter object</param>
        /// <returns>entities that match filter</returns>
        public IEnumerable<T> Get<T>(IQueryFilter<T> filter) where T : PersistableModel<T>
        {
            lock (_repositoryLock)
            {
                try
                {
                    using (new ActivityTracer(_context, IsLoggingSql, "Get<T>()"))
                    {
                        var objectContext = ((IObjectContextAdapter)_context).ObjectContext;
                        var set = objectContext.CreateObjectSet<T>();
                        var query = ConstructQuery<T, T>(set, filter);
                        return query != null ? query.ToList() : null;
                    }
                }
                catch (Exception ex)
                {
                    throw ExceptionHandler.HandleException<DataAccessException>(
                        "Database error",
                        "A repository query failed.  See InnerException for details.",
                        ex);
                }
            }
        }


        /// <summary>
        ///     Saves all changes which have been made to entities in the repository
        /// </summary>
        public void Save()
        {
            lock (_repositoryLock)
            {
                try
                {
                    using (var ts = new TransactionScope())
                    {
                        bool concurrencyIssueResolved = true;
                        while (concurrencyIssueResolved)
                        {
                            try
                            {
                                concurrencyIssueResolved = false;
                                _context.ChangeTracker.DetectChanges();
                                _context.SaveChanges();
                                if (_eventAggregator != null && _changesInProgress.Any())
                                {
                                    using (var auditRepo = new Repository(_eventAggregator))
                                    {
                                        _eventAggregator.GetEvent<AuditEntityChangesEvent>()
                                            .Publish(
                                                new AuditData
                                                {
                                                    Repository = auditRepo,
                                                    Changes = _changesInProgress
                                                });
                                        auditRepo.Save();
                                    }
                                }
                            }
                            catch (DbUpdateConcurrencyException duce)
                            {
                                concurrencyIssueResolved = TryResolveConcurrencyException(duce);
                            }
                        }
                        ts.Complete();
                    }
                }
                catch (DbEntityValidationException validationException)
                {
                    var message = new StringBuilder();
                    foreach (var validationResult in validationException.EntityValidationErrors)
                    {
                        foreach (var validationError in validationResult.ValidationErrors)
                        {
                            message.AppendFormat("{0}: {1}", validationError.PropertyName, validationError.ErrorMessage);
                            message.AppendLine();
                        }
                    }

                    throw ExceptionHandler.HandleException<DataAccessException>(
                        "Database error",
                        string.Format("Saving changes to the repository failed. Validation error: {0}", message),
                        validationException);
                }
                catch (Exception ex)
                {
                    throw ExceptionHandler.HandleException<DataAccessException>(
                        "Database error",
                        "Saving changes to the repository failed.  See InnerException for details.",
                        ex);
                }
            }
        }


        private static bool IsValueType(object obj)
        {
            return obj.GetType().IsValueType;
        }

        private void OnTransactionCompleted(object o, TransactionEventArgs args)
        {
            lock (_repositoryLock)
            {
                if (args.Transaction.TransactionInformation.Status == TransactionStatus.Committed
                    && _changesInProgress.Any())
                {
                    PublishChangeNotifications(_eventAggregator, _changesInProgress);
                }
                _changesInProgress.Clear();
                args.Transaction.TransactionCompleted -= OnTransactionCompleted;
                if (_disposed)
                {
                    _changesInProgress = null;
                }
                _transactionPending = false;
            }
        }

        private void PublishChangeNotifications(
            IEventAggregator eventAggregator,
            IEnumerable<EntityChangeNotification> changes)
        {
            if (changes == null)
            {
                return;
            }
            var publishEvent = eventAggregator.GetEvent<EntityChangedEvent>();
            foreach (var change in changes)
            {
                try
                {
                    publishEvent.Publish(change);
                }
                catch (Exception e)
                {
                    ExceptionHandler.HandleException<DataAccessException>(
                        "Entity change notification handling error",
                        "Exception thrown processing Entity changed event",
                        e);
                }
            }
        }

        private void SavingChanges(object sender, EventArgs e)
        {
            if (_transactionPending == false)
            {
                var trans = Transaction.Current;
                trans.TransactionCompleted += OnTransactionCompleted;
                _transactionPending = true;
            }
            var context = (_context as IObjectContextAdapter).ObjectContext;
            var deletes =
                context.ObjectStateManager.GetObjectStateEntries(EntityState.Deleted)
                    .Where(ose => ose.IsRelationship == false)
                    .Select(
                        ose =>
                            new EntityChangeNotification
                            {
                                ChangeType = EntityChangeTypeEnum.Deleted,
                                EntityType = ObjectContext.GetObjectType(ose.Entity.GetType()),
                                Entity = ose.Entity,
                            });
            var mods =
                context.ObjectStateManager.GetObjectStateEntries(EntityState.Modified)
                    .Where(ose => ose.IsRelationship == false)
                    .Select(
                        ose =>
                            new EntityChangeNotification
                            {
                                ChangeType = EntityChangeTypeEnum.Modified,
                                EntityType = ObjectContext.GetObjectType(ose.Entity.GetType()),
                                Entity = ose.Entity,
                                DataChanged =
                                    new List<EntityDataChanged>(
                                        ose.GetModifiedProperties()
                                            .Where(p => ose.IsPropertyChanged(p))
                                            .Select(
                                                p =>
                                                    new EntityDataChanged(
                                                        p,
                                                        ose.OriginalValues.GetValue(ose.OriginalValues.GetOrdinal(p)),
                                                        ose.CurrentValues.GetValue(ose.CurrentValues.GetOrdinal(p)))))
                            });
            var adds =
                context.ObjectStateManager.GetObjectStateEntries(EntityState.Added)
                    .Where(ose => ose.IsRelationship == false)
                    .Select(
                        ose =>
                            new EntityChangeNotification
                            {
                                ChangeType = EntityChangeTypeEnum.Added,
                                Entity = ose.Entity,
                                EntityType = ObjectContext.GetObjectType(ose.Entity.GetType()),
                            });
            var relations =
                context.ObjectStateManager.GetObjectStateEntries(
                    EntityState.Deleted | EntityState.Modified | EntityState.Added).Where(ose => ose.IsRelationship);

            _changesInProgress = _changesInProgress.Concat(adds).Concat(mods).Concat(deletes).ToList();

            foreach (var relationChange in relations)
            {
                if ((relationChange.UsableValues()[0] as EntityKey).IsTemporary == false)
                {
                    var key = relationChange.UsableValues()[0] as EntityKey;
                    var entity = context.GetObjectByKey(key) as IEntityIdentifier;
                    var ecn = new EntityChangeNotification
                    {
                        ChangeType = EntityChangeTypeEnum.Modified,
                        EntityType = ObjectContext.GetObjectType(entity.GetType()),
                        Entity = entity,
                    };
                    _changesInProgress.Add(ecn);
                }
                if ((relationChange.UsableValues()[1] as EntityKey).IsTemporary == false)
                {
                    var key = relationChange.UsableValues()[1] as EntityKey;
                    var entity = context.GetObjectByKey(key) as IEntityIdentifier;
                    var ecn = new EntityChangeNotification
                    {
                        ChangeType = EntityChangeTypeEnum.Modified,
                        EntityType = ObjectContext.GetObjectType(entity.GetType()),
                        Entity = entity,
                    };
                    _changesInProgress.Add(ecn);
                }
            }
            _changesInProgress =
                _changesInProgress.OrderBy(c => c.ChangeType).ThenBy(c => c.EntityType.Name).Distinct().ToList();
        }

        /// <summary>Constructs the query</summary>
        /// <param name="query">The query.</param>
        /// <param name="filter">The filter.</param>
        /// <typeparam name="T">The source type</typeparam>
        /// <typeparam name="T2">The result type</typeparam>
        /// <returns>The query</returns>
        protected static IQueryable<T2> ConstructQuery<T, T2>(IQueryable<T> query, IQueryFilter<T> filter)
            where T : PersistableModel<T>
        {
            (query as ObjectQuery).MergeOption = MergeOption.OverwriteChanges;
            if (filter == null)
            {
                return query as IQueryable<T2>;
            }

            query = filter.GetFilterExpressions().Aggregate(query, (current, q) => current.Where(q));

            var f2 = filter as IQueryFilter<T, T2>;
            var q2 = query.Select<T, T2>(f2 == null ? null : f2.GetSelectExpression());

            if (f2 != null && f2.ShouldApplyDistinct())
            {
                q2 = q2.Distinct();
            }

            if (filter.RecordsAvailable == null)
            {
                filter.RecordsAvailable = q2.Count();
            }

            q2 = filter.GetIncludeExpressions().Aggregate(q2, (current, e) => current.Include(e));

            var orderBy = filter.GetSortExpressions().FirstOrDefault();
            if (orderBy != null)
            {
                q2 = q2.OrderBy(orderBy).ThenBy(filter.GetSortExpressions().Skip(1));
            }
            if (filter.FirstRecordDesired != null)
            {
                q2 = q2.Skip(filter.FirstRecordDesired.Value);
            }
            if (filter.RecordsDesired != null)
            {
                q2 = q2.Take(filter.RecordsDesired.Value);
            }
            return q2;
        }


        /// <summary>
        /// Tries the resolve concurrency exception.
        /// </summary>
        /// <param name="duce">The duce.</param>
        /// <returns>true if succeeded; otherwise, false</returns>
        private bool TryResolveConcurrencyException(DbUpdateConcurrencyException duce)
        {
            bool concurrencyIssueResolved = false;

            // If a save attempt throws a concurrency exception, we'll apply some general rules to see if we can resolve it automatically.
            // We assume that, if someone else updated FieldA, and we're only updating FieldB, then we can merge those changes.
            // Iterate through all the entities that had a concurrency violation
            foreach (var se in duce.Entries)
            {
                // For each entity, we need to consider the values the entity has/had:
                // 1.) When this repository first loaded it (entityOriginal)
                // 2.) When we attempted to save it (entityCurrent)
                // and 3.) currently in the database (storeCurrent).
                var entityOriginal = se.OriginalValues.Clone();
                var entityCurrent = se.CurrentValues.Clone();
                se.Reload();
                var storeCurrent = se.OriginalValues.Clone();

                // Now walk through each field and determine if: this repo changed it, the store changed it, both, or neither.
                foreach (var propName in entityCurrent.PropertyNames)
                {
                    bool clientChanged = ((entityOriginal[propName] != null)
                                          && entityOriginal[propName].GetType().IsValueType)
                        ? !entityOriginal[propName].Equals(entityCurrent[propName])
                        : (entityOriginal[propName] != entityCurrent[propName]);

                    bool storeChanged = ((entityOriginal[propName] != null)
                                         && entityOriginal[propName].GetType().IsValueType)
                        ? !entityOriginal[propName].Equals(storeCurrent[propName])
                        : (entityOriginal[propName] != storeCurrent[propName]);
                    if (clientChanged && storeChanged)
                    {
                        // If changed to same value, let it go.
                        if (IsValueType(entityOriginal[propName] ?? storeCurrent[propName])
                            ? (entityOriginal[propName] != null
                                ? entityCurrent[propName].Equals(storeCurrent[propName])
                                : storeCurrent[propName].Equals(entityCurrent[propName]))
                            : (entityCurrent[propName] == storeCurrent[propName]))
                        {
                            // Both changed, but we changed it to the same value, so no harm.
                            concurrencyIssueResolved = true;
                        }
                        else
                        {
                            // So, someone else changed the value for this field, and we're trying to change it, and we're NOT trying to change it to the same value.
                            // We can't resolve this.
                            throw new Exception(
                                string.Format(
                                    "Unable to resolve concurrency violation.  Attempted to change {0}.{1} from \"{2}\" to \"{3}\", but it was updated elsewhere to \"{4}\".",
                                    se.Entity.GetType(),
                                    propName,
                                    entityOriginal[propName],
                                    entityCurrent[propName],
                                    storeCurrent[propName]),
                                duce);
                        }
                    }
                    else if (clientChanged)
                    {
                        // Only this repo has tried to change the field.  Restore our change.
                        se.Property(propName).CurrentValue = entityCurrent[propName];
                        concurrencyIssueResolved = true;
                    }
                    else if (storeChanged)
                    {
                        concurrencyIssueResolved = true;
                    }
                }
            }

            return concurrencyIssueResolved;
        }
    }

    public static class EfExtensions
    {
        public static IExtendedDataRecord UsableValues(this ObjectStateEntry entry)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                case EntityState.Detached:
                case EntityState.Unchanged:
                case EntityState.Modified:
                    return entry.CurrentValues;
                case EntityState.Deleted:
                    return (IExtendedDataRecord)entry.OriginalValues;
                default:
                    throw new InvalidOperationException("This entity state should not exist.");
            }
        }
    }
}