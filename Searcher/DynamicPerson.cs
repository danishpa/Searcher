using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Searcher
{
    public class DynamicPropertyDescriptor : PropertyDescriptor
    {
        public DynamicPropertyDescriptor(string name)
            : base(name, null)
        {
        }

        public override Type ComponentType
        {
            get { return typeof(object); }
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override Type PropertyType
        {
            get { return typeof(object); }
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override void ResetValue(object component)
        {
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
        public override object GetValue(object component)
        {
            throw new NotImplementedException();
        }
        public override void SetValue(object component, object value)
        {
            throw new NotImplementedException();
        }
    }

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

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _headers;
        }
    }

    public class DynamicList<T> : List<T>, ITypedList
        where T : DynamicObject
    {
        public DynamicList(IEnumerable<T> collection)
            : base(collection)
        {
        }

        public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
        {
            DynamicPropertyDescriptor[] dynamicDescriptors = { };

            if (this.Any())
            {
                var firstItem = this[0];

                dynamicDescriptors =
                    firstItem.GetDynamicMemberNames()
                    .Select(p => new DynamicPropertyDescriptor(p))
                    .ToArray();
            }

            return new PropertyDescriptorCollection(dynamicDescriptors);
        }

        public string GetListName(PropertyDescriptor[] listAccessors)
        {
            return null;
        }
    }
}
