using System;
using System.Collections.Generic;
using System.Text;

namespace UserInteraction.DTO
{
    public class RatingDTOModel
    {
        /// <summary>
        /// Identifier for rating
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// Identifier for user interaction info this rating belongs to
        /// </summary>
        public long UserInteractionInfoID { get; set; }

        /// <summary>
        /// Username of user that ths rating belongs to
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// The rating value
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// The timestamp when this entity was created
        /// </summary>
        public DateTimeOffset CreateDate { get; set; }
    }
}
