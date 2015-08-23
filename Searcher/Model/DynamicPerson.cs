using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;

namespace Searcher.Model
{
    public class DynamicPerson : DynamicObject, ISearchable, ICloneable
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

            // If there aren't enough fields for all headers, return Empty string and still succeed
            result = string.Empty;
            if (index < _fields.Length)
            {
                result = _fields[index];
            }

            return true;
        }

        public int Search(string searchTerm)
        {
            return _fields.Where(property => property.FuzzyContains(searchTerm)).Count();
        }
        
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _headers;
        }

        public object Clone()
        {
            return new DynamicPerson(
                (string[])_headers.Clone(),
                (string[])_fields.Clone());
        }
    }
}
