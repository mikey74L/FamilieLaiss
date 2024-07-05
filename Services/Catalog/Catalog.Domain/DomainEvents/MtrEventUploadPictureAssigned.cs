using DomainHelper.DomainEvents;

namespace Catalog.Domain.DomainEvents
{
    /// <summary>
    /// Event for upload picture assigned
    /// </summary>
    public class MtrEventUploadPictureAssigned : DomainEventSingle
    {
        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="id">Identifier for upload item</param>
        public MtrEventUploadPictureAssigned(long id) : base(id.ToString())
        {
        }
        #endregion
    }
}
