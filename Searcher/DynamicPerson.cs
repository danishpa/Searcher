using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using System.ComponentModel;

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

            result = _fields[index];
            return true;
        }

        public int Search(string searchTerm)
        {
            return _fields.Where(property => property.Contains(searchTerm)).Count();
        }
        
    }
}
