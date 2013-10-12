using System;
using System.Reflection;

namespace PropertyAccess
{
    public class IndexedDelegateClassPropertyAccess<TTarget, TIndex, TProperty> : IIndexedClassPropertyAccess where TTarget : class
    {
        private readonly PropertyValueGetter _getter;
        private readonly PropertyValueSetter _setter;

        private delegate TProperty PropertyValueGetter(TTarget target, TIndex index);
        private delegate TProperty StaticPropertyValueGetter(TIndex index);

        private delegate void PropertyValueSetter(TTarget target, TIndex index, TProperty value);
        private delegate void StaticPropertyValueSetter(TIndex index, TProperty value);

        public IndexedDelegateClassPropertyAccess(PropertyInfo propertyInfo)
        {
            _getter = CreateGetter(propertyInfo);
            _setter = CreateSetter(propertyInfo);
        }

        private static PropertyValueGetter CreateGetter(PropertyInfo propertyInfo)
        {
            var name = propertyInfo.Name;

            if (propertyInfo.CanRead && propertyInfo.GetIndexParameters().Length > 0)
            {
                var getMethod = propertyInfo.GetGetMethod();
                if (getMethod.IsStatic)
                {
                    var staticGetter = (StaticPropertyValueGetter)Delegate.CreateDelegate(typeof(StaticPropertyValueGetter), getMethod);
                    return (target, index) => staticGetter.Invoke(index);
                }
                else
                {
                    return (PropertyValueGetter)Delegate.CreateDelegate(typeof(PropertyValueGetter), getMethod);
                }
            }
            else
            {
                return (target, index) =>
                           {
                               throw new NotImplementedException("No getter implemented for property " + name);
                           };
            }
        }

        private static PropertyValueSetter CreateSetter(PropertyInfo propertyInfo)
        {
            var name = propertyInfo.Name;
            if (propertyInfo.CanWrite && propertyInfo.GetIndexParameters().Length > 0)
            {
                var setMethod = propertyInfo.GetSetMethod();
                if (setMethod.IsStatic)
                {
                    var staticSetter = (StaticPropertyValueSetter)Delegate.CreateDelegate(typeof(StaticPropertyValueSetter), setMethod);
                    return (target, index, value) => staticSetter.Invoke(index, value);
                }
                else
                {
                    return (PropertyValueSetter)Delegate.CreateDelegate(typeof(PropertyValueSetter), setMethod);
                }
            }
            else
            {
                return (target, index, value) =>
                           {
                               throw new NotImplementedException("No setter implemented for property " + name);
                           };
            }
        }

        public TProperty GetValue(TTarget target, TIndex index)
        {
            return _getter.Invoke(target, index);
        }

        public object GetValue(object target, object index)
        {
            return GetValue((TTarget)target, (TIndex)index);
        }

        public void SetValue(TTarget target, TIndex index, TProperty value)
        {
            _setter.Invoke(target, index, value);
        }

        public void SetValue(object target, object index, object value)
        {
            SetValue((TTarget)target, (TIndex)index, (TProperty)value);
        }
    }
}