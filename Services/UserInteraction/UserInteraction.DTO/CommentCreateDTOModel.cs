using System;
using System.Collections.Generic;
using System.Text;

namespace UserInteraction.DTO
{
    public class CommentCreateDTOModel
    {
        /// <summary>
        /// Identifier for user interaction info this comment belongs to
        /// </summary>
        public long UserInteractionInfoID { get; set; }

        /// <summary>
        /// The comment content
        /// </summary>
        public string Content { get; set; }
    }
}
