namespace Cm.Domain.Common.Models
{

    /// <summary>
    /// interface for any entity from data base 
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Id for entity
        /// </summary>
        public int Id { get; set; }
    }
}