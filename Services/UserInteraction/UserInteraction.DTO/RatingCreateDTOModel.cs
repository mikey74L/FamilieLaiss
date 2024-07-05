using System;
using System.Collections.Generic;
using System.Text;

namespace UserInteraction.DTO
{
    public class RatingCreateDTOModel
    {
        /// <summary>
        /// Identifier for user interaction info this rating belongs to
        /// </summary>
        public long UserInteractionInfoID { get; set; }

        /// <summary>
        /// The rating value
        /// </summary>
        public int Value { get; set; }
    }
}
