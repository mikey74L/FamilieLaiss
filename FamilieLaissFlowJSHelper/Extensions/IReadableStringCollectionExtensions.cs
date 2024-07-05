using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FamilieLaissFlowJsHelper.Extensions
{
    public static class IReadableStringCollectionExtensions
    {
        [DebuggerStepThrough]
        public static NameValueCollection AsNameValueCollection(this IDictionary<string, StringValues> collection)
        {
            NameValueCollection values = new NameValueCollection();
            foreach (KeyValuePair<string, StringValues> pair in collection)
            {
                string introduced3 = pair.Key;
                values.Add(introduced3, Enumerable.First<string>(pair.Value));
            }
            return values;
        }

        [DebuggerStepThrough]
        public static NameValueCollection AsNameValueCollection(this IEnumerable<KeyValuePair<string, StringValues>> collection)
        {
            NameValueCollection values = new NameValueCollection();
            foreach (KeyValuePair<string, StringValues> pair in collection)
            {
                string introduced3 = pair.Key;
                values.Add(introduced3, Enumerable.First<string>(pair.Value));
            }
            return values;
        }
    }
}
