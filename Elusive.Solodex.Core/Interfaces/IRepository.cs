using System;
using System.Collections.Generic;
using Elusive.Solodex.Core.Models;

namespace Elusive.Solodex.Core.Interfaces
{
    /// <summary>
    /// Defines the API for the local application database
    /// </summary>
    public interface IRepository : IDisposable
    {
        /// <summary>
        /// Gets or sets a value indicating whether TSQL statements should be logged.
        /// </summary>
        bool IsLoggingSql { get; set; }

        /// <summary>
        /// Adds an entity to the repository
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="entity">The entity</param>
        void Add<T>(T entity) where T : PersistableModel<T>;

        /// <summary>
        /// Adds entities to the repository
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="entities">The entities</param>
        void Add<T>(IEnumerable<T> entities) where T : PersistableModel<T>;

        /// <summary>
        /// Deletes an entity from the repository
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="entity">The entity</param>
        void Delete<T>(T entity) where T : PersistableModel<T>;

        /// <summary>
        /// Deletes entities from the repository
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="entities">The entities</param>
        void Delete<T>(IEnumerable<T> entities) where T : PersistableModel<T>;

        /// <summary>
        /// Gets all entities of type T
        /// </summary>
        /// <typeparam name="T">The entity type returned</typeparam>
        /// <returns>All entities of the specified type</returns>
        IEnumerable<T> Get<T>() where T : PersistableModel<T>;

        /// <summary>
        /// Gets all entities of type T
        /// </summary>
        /// <typeparam name="T">The entity type returned</typeparam>
        /// <param name="include">The include path.</param>
        /// <returns>
        /// All entities of the specified type
        /// </returns>
        IEnumerable<T> Get<T>(string include) where T : PersistableModel<T>;
        
        /// <summary>
        /// Saves all changes which have been made to entities in the repository
        /// </summary>
        void Save();
    }
}
