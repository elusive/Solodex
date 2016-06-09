using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Elusive.Solodex.Core.Enumerations;
using Elusive.Solodex.Core.Models;

namespace Elusive.Solodex.Core.Interfaces
{
    /// <summary>Defines the IQueryFilter interface</summary>
    /// <typeparam name="T">The entity type</typeparam>
    public interface IQueryFilter<T>
        where T : PersistableModel<T>
    {
        /// <summary>
        ///     Gets or sets the initial number of records to omit from the results.  If not specified, results start with the
        ///     first record.
        /// </summary>
        int? FirstRecordDesired { get; set; }

        /// <summary>
        ///     Gets or sets the number of records matching the query.  This value gets set by executing the query.
        /// </summary>
        int? RecordsAvailable { get; set; }

        /// <summary>
        ///     Gets or sets the number of records to include in the results.  If not specified, all records are included in the
        ///     results.
        /// </summary>
        int? RecordsDesired { get; set; }

        /// <summary>
        ///     Adds a filter expression to the query definition
        /// </summary>
        /// <param name="predicate">The predicate expression</param>
        IQueryFilter<T> AddFilter(Expression<Func<T, bool>> predicate);

        /// <summary>Adds an ordering expression to the query definition.  Defaults to ascending order.</summary>
        /// <typeparam name="T2">Resulting type of the sort expression</typeparam>
        /// <param name="sortExpression">The sort expression</param>
        /// <param name="direction">The direction.</param>
        IQueryFilter<T> AddSort<T2>(
            Expression<Func<T, T2>> sortExpression,
            SortDirection direction = SortDirection.Ascending);

        /// <summary>
        ///     Gets the query clauses for the filter
        /// </summary>
        /// <returns>Enumeration of query clauses</returns>
        IEnumerable<Expression<Func<T, bool>>> GetFilterExpressions();

        /// <summary>
        ///     Retrieves the eager load expressions
        /// </summary>
        /// <returns>The eager load expressions</returns>
        IEnumerable<Expression> GetIncludeExpressions();

        /// <summary>
        ///     Retrieves the sort expressions in prioritized order
        /// </summary>
        /// <returns>The ordered sort expressions</returns>
        IEnumerable<Tuple<Expression, SortDirection>> GetSortExpressions();

        /// <summary>
        ///     Defines an entity to eager load in results
        /// </summary>
        /// <typeparam name="T2">The eager-loaded entity type</typeparam>
        /// <param name="expression">The eager load expression</param>
        IQueryFilter<T> Include<T2>(Expression<Func<T, T2>> expression);
        /// <summary>
        /// Selects the specified expression.
        /// </summary>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        IQueryFilter<T, T2> Select<T2>(Expression<Func<T, T2>> expression);
    }

    /// <summary>Defines the IQueryFilter interface</summary>
    /// <typeparam name="T">The entity type</typeparam>
    /// <typeparam name="T2">The result type</typeparam>
    public interface IQueryFilter<T, T2> : IQueryFilter<T>
        where T : PersistableModel<T>
    {
        /// <summary>
        /// Ensures results are distinct.
        /// </summary>
        /// <remarks>MUST be used with Select()</remarks>
        IQueryFilter<T, T2> Distinct();

        /// <summary>
        /// Retrieves the select expression
        /// </summary>
        /// <returns>The select expression</returns>
        Expression GetSelectExpression();

        /// <summary>Defines the select expression</summary>
        /// <param name="expression">The expression.</param>
        /// <typeparam name="T2">The type</typeparam>
        IQueryFilter<T, T2> Select(Expression<Func<T, T2>> expression);

        /// <summary>
        /// Determines if a distinct filter should be applied.
        /// </summary>
        /// <returns>True if a distinct filter should be applied.</returns>
        bool ShouldApplyDistinct();
    }
}