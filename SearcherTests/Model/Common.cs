using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Searcher;

namespace Searcher.Tests
{
    public static class Common
    {
        public static DynamicPerson CreateDynamicPerson(params string[] fields)
        {
            return new DynamicPerson(fields.Select(f => string.Format("Header_{0}", f)).ToArray(), fields);
        }

        public static DynamicPerson CreateDynamicPerson(string[] headers, string[] fields)
        {
            return new DynamicPerson(headers, fields);
        }
    }
}
