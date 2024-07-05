using DomainHelper.DomainEvents;

namespace Catalog.Domain.DomainEvents
{
    /// <summary>
    /// Event for media group created
    /// </summary>
    public class MtrEventMediaGroupCreated : DomainEventSingle
    {
        #region Properties
        /// <summary>
        /// German name for media group
        /// </summary>
        public string GermanName { get; init; }

        /// <summary>
        /// English name for media group
        /// </summary>
        public string EnglishName { get; init; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="id">Identifier for media item</param>
        /// <param name="germanName">German name for media group</param>
        /// <param name="englishName">English name for media group</param>
        public MtrEventMediaGroupCreated(long id, string germanName, string englishName) : base(id.ToString())
        {
            GermanName = germanName;
            EnglishName = englishName;
        }
        #endregion
    }
}
