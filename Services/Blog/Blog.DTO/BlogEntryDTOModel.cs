using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.DTO
{
    public class BlogEntryDTOModel
    {
        /// <summary>
        /// Identifier for blog entry
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// German header text for this Blog-Entry
        /// </summary>
        public string HeaderGerman { get; set; }

        /// <summary>
        /// English header text for this Blog-Entry
        /// </summary>
        public string HeaderEnglish { get; set; }

        /// <summary>
        /// German text for this Blog-Entry
        /// </summary>
        public string TextGerman { get; set; }

        /// <summary>
        /// English text for this Blog-Entry
        /// </summary>
        public string TextEnglish { get; set; }

        /// <summary>
        /// When was this entity created
        /// </summary>
        public DateTimeOffset CreateDate { get; set; }

        /// <summary>
        /// When was this entity last changed
        /// </summary>
        public DateTimeOffset? ChangeDate { get; set; }
    }
}
