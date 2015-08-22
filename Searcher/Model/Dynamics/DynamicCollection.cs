using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Searcher.Model.Dynamics
{
    public class DynamicCollection<T> : ObservableCollection<T>, ITypedList
        where T : DynamicObject
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
