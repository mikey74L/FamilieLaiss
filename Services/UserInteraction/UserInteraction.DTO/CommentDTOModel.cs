using System;
using System.Collections.Generic;
using System.Text;

namespace UserInteraction.DTO
{
    public class CommentDTOModel
    {
        /// <summary>
        /// Identifier for comment
        /// </summary>
        public long ID { get; set; }
    
        /// <summary>
        /// Identifier for user interaction info this comment belongs to
        /// </summary>
        public long UserInteractionInfoID { get; set; }

        /// <summary>
        /// Username of user that this comment belongs to
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Content for comment
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// The timestamp when this entity was created
        /// </summary>
        public DateTimeOffset CreateDate { get; set; }
    }
}
