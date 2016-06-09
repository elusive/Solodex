namespace Elusive.Solodex.Core.Enumerations
{
    /// <summary>
    ///     Defines the change operation to an entity.
    /// </summary>
    public enum EntityChangeTypeEnum
    {
        /// <summary>
        ///     The entity is newly added
        /// </summary>
        Added,

        /// <summary>
        ///     The entity has been modified
        /// </summary>
        Modified,

        /// <summary>
        ///     The entity has been deleted
        /// </summary>
        Deleted,
    }
}