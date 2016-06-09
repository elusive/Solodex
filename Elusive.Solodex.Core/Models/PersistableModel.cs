using System;
using Elusive.Solodex.Core.Interfaces;

namespace Elusive.Solodex.Core.Models
{
    /// <summary>Represents a model object that can be persisted to the database</summary>
    /// <typeparam name="T">A type</typeparam>
    public abstract class PersistableModel<T> : ModelBase<T>, IEntityIdentifier
        where T : class
    {
        /// <summary>Initializes a new instance of the <see cref="PersistableModel{T}" /> class.</summary>
        protected PersistableModel()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>Gets or sets Id.</summary>
        public virtual Guid Id { get; set; }

        /// <summary>Gets the entity identifier.</summary>
        public Guid EntityId
        {
            get { return Id; }
        }
    }
}