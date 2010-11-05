using System;

namespace Chaquotay.PropertyAccess
{
    internal class PropertyAccessFactory
    {
        public static IPropertyAccess Create(Type targetType, string name)
        {
            var propertyInfo = targetType.GetProperty(name);
            var type = typeof(DelegatePropertyAccess<,>).MakeGenericType(targetType, propertyInfo.PropertyType);
            return (IPropertyAccess)Activator.CreateInstance(type, propertyInfo);
        }

        public static DelegatePropertyAccess<TTarget, TResult> Create<TTarget, TResult>(string name)
        {
            var propertyInfo = typeof (TTarget).GetProperty(name);
            return new DelegatePropertyAccess<TTarget, TResult>(propertyInfo);
        }
    }
}