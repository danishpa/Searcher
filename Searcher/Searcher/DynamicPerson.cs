using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;

namespace Searcher
{
    public class DynamicPerson : DynamicObject, ISearchable
    {
        private string[] _headers;
        private string[] _fields;

        public DynamicPerson(string[] headers, string[] fields)
        {
            _headers = new string[headers.Length];
            headers.CopyTo(_headers, 0);

            _fields = new string[fields.Length];
            fields.CopyTo(_fields, 0);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            int index = 0;

            result = null;

            index = Array.IndexOf(_headers, binder.Name);
            if (index < 0 || index >= _headers.Length)
            {
                return false;
            }

            if (index >= _fields.Length)
            {
                // Not enough fields for all headers
                result = string.Empty;
            }
            else
            {
                result = _fields[index];
            }

            return true;
        }

        public int Search(string searchTerm)
        {
            return _fields.Where(property => property.Contains(searchTerm)).Count();
        }
        
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _headers;
        }
    }
}
