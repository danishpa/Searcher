using System;
using System.ComponentModel;


namespace Searcher.Model.Dynamics
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
}
