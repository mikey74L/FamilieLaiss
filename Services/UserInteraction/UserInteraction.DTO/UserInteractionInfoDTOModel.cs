using System;
using System.Collections.Generic;
using System.Text;

namespace UserInteraction.DTO
{
    public class UserInteractionInfoDTOModel
    {
        /// <summary>
        /// Identifier for user interaction info
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// The count of ratings (For faster Access as property)
        /// </summary>
        public int RatingCount { get; set; }

        /// <summary>
        /// The average rating over all raitings
        /// </summary>
        public double AverageRating { get; set; }

        /// <summary>
        /// The count of comments (For faster access as property)
        /// </summary>
        public int CommentCount { get; set; }

        /// <summary>
        /// The count of favorites (For faster access as property)
        /// </summary>
        public int FavoriteCount { get; set; }
    }
}
