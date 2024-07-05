using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DTOHelper
{
    public class SecurityQuestionDTOModel
    {
        /// <summary>
        /// ID for the security question
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// German name for this security question
        /// </summary>
        public string Name_German { get; set; }

        /// <summary>
        /// English name for this security question
        /// </summary>
        public string Name_English { get; set; }
    }
}
