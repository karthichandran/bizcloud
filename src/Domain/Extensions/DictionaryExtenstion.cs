using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReProServices.Domain.Extensions
{
    public static class DictionaryExtenstion
    {

        public static string ToString(this Dictionary<string, string> source, string keyValueSeparator, string sequenceSeparator)
        {
            if (source == null)
                throw new ArgumentException("Parameter source cannot be null.");

            var pairs = source.Select(x => string.Format("{0}{1}{2}", x.Key, keyValueSeparator, x.Value));

            return string.Join(sequenceSeparator, pairs);
        }
    }
}
