using System;
using System.Reflection;

namespace Chaquotay.PropertyAccess
{
    public class ReflectionPropertyAccess : IPropertyAccess
    {
        private readonly PropertyInfo _propertyInfo;

        public ReflectionPropertyAccess(Type target, string name)
        {
            _propertyInfo = target.GetProperty(name);
        }

        public object GetValue(object target)
        {
            return _propertyInfo.GetValue(target, null);
        }

        public void SetValue(object target, object value)
        {
            _propertyInfo.SetValue(target, value, null);
        }
    }
}