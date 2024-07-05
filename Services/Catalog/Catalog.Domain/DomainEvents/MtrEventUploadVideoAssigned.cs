using DomainHelper.DomainEvents;

namespace Catalog.Domain.DomainEvents
{
    /// <summary>
    /// Event for upload video assigned
    /// </summary>
    public class MtrEventUploadVideoAssigned : DomainEventSingle
    {
        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="id">Identifier for upload item</param>
        public MtrEventUploadVideoAssigned(long id) : base(id.ToString())
        {
        }
        #endregion
    }
}
