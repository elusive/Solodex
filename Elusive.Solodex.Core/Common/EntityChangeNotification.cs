using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elusive.Solodex.Core.Enumerations;
using Elusive.Solodex.Core.Interfaces;

namespace Elusive.Solodex.Core.Common
{
    /// <summary>
    /// Notification of a change to a persisted entity
    /// </summary>
    public class EntityChangeNotification : IEntityIdentifier, IEquatable<EntityChangeNotification>
    {
        private IList<EntityDataChanged> _dataChanged;

        /// <summary>
        /// The type of change
        /// </summary>
        public virtual EntityChangeTypeEnum ChangeType { get; set; }

        /// <summary>
        /// List of properties that changed
        /// </summary>
        public virtual IList<EntityDataChanged> DataChanged
        {
            get { return _dataChanged = _dataChanged ?? new List<EntityDataChanged>(); }
            set { _dataChanged = value; }
        }

        /// <summary>
        /// The actual entity
        /// </summary>
        public virtual object Entity { get; set; }

        /// <summary>
        /// The identifier of the entity
        /// </summary>
        public virtual Guid EntityId
        {
            get { return (Entity as IEntityIdentifier == null) ? Guid.Empty : (Entity as IEntityIdentifier).EntityId; }
        }

        /// <summary>
        /// The type of entity
        /// </summary>
        public virtual Type EntityType { get; set; }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(EntityChangeNotification other)
        {
            return ChangeType == other.ChangeType && EntityId == other.EntityId && EntityType == other.EntityType;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0} {1} {2}", EntityType.Name, EntityId, ChangeType);
        }
    }

    /// <summary>
    /// Provides the old and new values of a property that changed on the entity
    /// </summary>
    public class EntityDataChanged
    {
        /// <summary>
        /// Constructs new instance of the EntityDataChanged class
        /// </summary>
        /// <param name="propertyName">Name of the property that changed</param>
        /// <param name="oldValue">Old value of the property</param>
        /// <param name="newValue">New value of the property</param>
        public EntityDataChanged(string propertyName, object oldValue, object newValue)
        {
            PropertyName = propertyName;
            OldValue = oldValue;
            NewValue = newValue;
        }

        /// <summary>
        /// New value of the property
        /// </summary>
        public object NewValue { get; set; }

        /// <summary>
        /// Old value of the property
        /// </summary>
        public object OldValue { get; set; }

        /// <summary>
        /// Name of the property that changed
        /// </summary>
        public string PropertyName { get; set; }
    }

}
