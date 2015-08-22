using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Searcher.Model.Dynamics
{
    public class DynamicCollection<T> : ObservableCollection<T>, ITypedList, ICloneable
        where T : DynamicObject, ICloneable
    {
        private string TypedListName;

        public DynamicCollection(IEnumerable<T> collection)
            : base(collection)
        {
        }

        public DynamicCollection(IEnumerable<T> collection, string name)
            : this(collection)
        {
            TypedListName = name;
        }

        public object Clone()
        {
            return new DynamicCollection<T>(this.Select(item => (T)item.Clone()));
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
            return TypedListName;
        }
    }
}
