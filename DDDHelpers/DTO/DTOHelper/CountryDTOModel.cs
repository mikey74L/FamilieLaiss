using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DTOHelper
{
    public class CountryDTOModel
    {
        /// <summary>
        /// ID for the Country
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// German name for this country
        /// </summary>
        public string Name_German { get; set; }

        /// <summary>
        /// English name for this country
        /// </summary>
        public string Name_English { get; set; }
    }
}
