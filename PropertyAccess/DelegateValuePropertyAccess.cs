using System;
using System.Reflection;

namespace PropertyAccess
{
    public class DelegateValuePropertyAccess<TTarget, TProperty> : IValuePropertyAccess where TTarget : struct
    {
        private readonly PropertyValueGetter _getter;

        private delegate TProperty PropertyValueGetter(ref TTarget target);
        private delegate TProperty StaticPropertyValueGetter();

        private delegate void StaticPropertyValueSetter(TProperty value);

        public DelegateValuePropertyAccess(PropertyInfo propertyInfo)
        {
            _getter = CreateGetter(propertyInfo);
        }

        private static PropertyValueGetter CreateGetter(PropertyInfo propertyInfo)
        {
            var name = propertyInfo.Name;

            if (propertyInfo.CanRead && propertyInfo.GetIndexParameters().Length == 0)
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

            public TProperty Get(ref TTarget target)
            {
                return _getter.Invoke();
            }
        }

        private class StaticGetError
        {
            private readonly string _error;

            public StaticGetError(string error)
            {
                _error = error;
            }

            public TProperty Get(ref TTarget target)
            {
                throw new NotImplementedException(_error);
            }
        }


        public TProperty GetValue(TTarget target)
        {
            return _getter.Invoke(ref target);
        }

        public object GetValue(object target)
        {
            return GetValue((TTarget)target);
        }
    }
}