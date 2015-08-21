using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.ComponentModel;

namespace Dynamics
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
