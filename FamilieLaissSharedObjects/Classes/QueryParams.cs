using System;
using System.Collections.Generic;
using System.Text;

namespace FamilieLaissSharedObjects.Classes
{
    public class QueryParams
    {
        public string WhereClause { get; set; }

        public string IncludeNav { get; set; }

        public string OrderBy { get; set; }

        public int? Take { get; set; }

        public int? Skip { get; set; }
    }
}
