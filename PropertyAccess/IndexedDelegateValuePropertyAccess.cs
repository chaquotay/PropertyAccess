using System;
using System.Reflection;

namespace PropertyAccess
{
    public class IndexedDelegateValuePropertyAccess<TTarget, TIndex, TProperty> : IIndexedPropertyReadAccess where TTarget : struct
    {
        private readonly PropertyValueGetter _getter;

        private delegate TProperty PropertyValueGetter(ref TTarget target, TIndex index);
        private delegate TProperty StaticPropertyValueGetter(TIndex index);

        public IndexedDelegateValuePropertyAccess(PropertyInfo propertyInfo)
        {
            _getter = CreateGetter(propertyInfo);
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
                    return new StaticGetWrapper(staticGetter).Get;
                }
                else
                {
                    return (PropertyValueGetter)Delegate.CreateDelegate(typeof(PropertyValueGetter), getMethod);
                }
            }
            else
            {
                return new StaticGetError("No getter implemented for property " + name).Get;
            }
        }

        private class StaticGetWrapper
        {
            private readonly StaticPropertyValueGetter _getter;

            public StaticGetWrapper(StaticPropertyValueGetter getter)
            {
                _getter = getter;
            }

            public TProperty Get(ref TTarget target, TIndex index)
            {
                return _getter.Invoke(index);
            }
        }

        private class StaticGetError
        {
            private readonly string _error;

            public StaticGetError(string error)
            {
                _error = error;
            }

            public TProperty Get(ref TTarget targe, TIndex index)
            {
                throw new NotImplementedException(_error);
            }
        }

        public TProperty GetValue(ref TTarget target, TIndex index)
        {
            return _getter.Invoke(ref target, index);
        }

        public object GetValue(object target, object index)
        {
            var temp = (TTarget) target;
            return GetValue(ref temp, (TIndex)index);
        }
    }
}