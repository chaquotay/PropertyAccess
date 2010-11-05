using System;
using System.Reflection;

namespace Chaquotay.PropertyAccess
{
    /// <summary>
    /// Inspired by http://msmvps.com/blogs/jon_skeet/archive/2008/08/09/making-reflection-fly-and-exploring-delegates.aspx
    /// </summary>
    /// <typeparam name="TTarget"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    public class DelegatePropertyAccess<TTarget, TProperty> : IPropertyAccess
    {
        private readonly PropertyValueGetter _getter;
        private readonly PropertyValueSetter _setter;

        private delegate TProperty PropertyValueGetter(TTarget target);
        private delegate TProperty StaticPropertyValueGetter();

        private delegate void PropertyValueSetter(TTarget target, TProperty value);
        private delegate void StaticPropertyValueSetter(TProperty value);

        public DelegatePropertyAccess(PropertyInfo propertyInfo)
        {
            _getter = CreateGetter(propertyInfo);
            _setter = CreateSetter(propertyInfo);
        }

        private static PropertyValueGetter CreateGetter(PropertyInfo propertyInfo)
        {
            var name = propertyInfo.Name;

            if (propertyInfo.CanRead)
            {
                var getMethod = propertyInfo.GetGetMethod();
                if (getMethod.IsStatic)
                {
                    var staticGetter = (StaticPropertyValueGetter)Delegate.CreateDelegate(typeof(StaticPropertyValueGetter), getMethod);
                    return target => staticGetter.Invoke();
                }
                else
                {
                    return (PropertyValueGetter)Delegate.CreateDelegate(typeof(PropertyValueGetter), getMethod);
                }
            }
            else
            {
                return target =>
                           {
                               throw new NotImplementedException("No getter implemented for property " + name);
                           };
            }
        }

        private static PropertyValueSetter CreateSetter(PropertyInfo propertyInfo)
        {
            var name = propertyInfo.Name;
            if (propertyInfo.CanWrite)
            {
                var setMethod = propertyInfo.GetSetMethod();
                if (setMethod.IsStatic)
                {
                    var staticSetter = (StaticPropertyValueSetter)Delegate.CreateDelegate(typeof(StaticPropertyValueSetter), setMethod);
                    return (target, value) => staticSetter.Invoke(value);
                }
                else
                {
                    return (PropertyValueSetter)Delegate.CreateDelegate(typeof(PropertyValueSetter), setMethod);
                }
            }
            else
            {
                return (target, value) =>
                           {
                               throw new NotImplementedException("No setter implemented for property " + name);
                           };
            }
        }

        public TProperty GetValue(TTarget target)
        {
            return _getter.Invoke(target);
        }

        public object GetValue(object target)
        {
            return GetValue((TTarget)target);
        }

        public void SetValue(TTarget target, TProperty value)
        {
            _setter.Invoke(target, value);
        }

        public void SetValue(object target, object value)
        {
            SetValue((TTarget)target, (TProperty)value);
        }
    }
}