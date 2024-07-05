using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.DTO
{
    public class BlogEntryUpdateDTOModel
    {
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
    }
}
