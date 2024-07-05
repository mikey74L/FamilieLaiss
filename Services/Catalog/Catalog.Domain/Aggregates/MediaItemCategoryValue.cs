using DomainHelper.AbstractClasses;
using System.Threading.Tasks;

namespace Catalog.Domain.Aggregates
{
    /// <summary>
    /// Entity for media item assigned category value
    /// </summary>
    public class MediaItemCategoryValue : EntityCreation<long>
    {
        #region Properties
        /// <summary>
        /// Identifier for the media item
        /// </summary>
        public long MediaItemID { get; private set; }

        /// <summary>
        /// The media item this category value entry belongs to
        /// </summary>
        public MediaItem MediaItem { get; private set; }

        /// <summary>
        /// Identifier for the media item
        /// </summary>
        public long CategoryValueID { get; private set; }

        /// <summary>
        /// The media item this category value entry belongs to
        /// </summary>
        public CategoryValue CategoryValue { get; private set; }
        #endregion

        #region C'tor
        /// <summary>
        /// C'tor (called from ef)
        /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected MediaItemCategoryValue()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {

        }

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="mediaItem">The media item the category value belongs to</param>
        /// <param name="valueID">The identifier for the category value</param>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        internal MediaItemCategoryValue(MediaItem mediaItem, long valueID)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            //Übernehmen der Werte
            MediaItem = mediaItem;
            CategoryValueID = valueID;
        }
        #endregion

        #region Delete
        public override Task EntityAddedAsync()
        {
            return Task.CompletedTask;
        }

        public override Task EntityDeletedAsync()
        {
            return Task.CompletedTask;
        }
        #endregion
    }
}
