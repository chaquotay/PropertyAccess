using System;
using System.Linq;
using System.Reflection;

namespace PropertyAccess
{
    public class PropertyAccessFactory
    {
        public static IPropertyReadAccess CreateRead(Type targetType, string name)
        {
            var propertyInfo = targetType.GetProperty(name);

            if(targetType.IsValueType)
            {
                return CreateForValue(propertyInfo);
            } else
            {
                return CreateForClass(propertyInfo);
            }
        }

        public static IClassPropertyAccess CreateForClass(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                return null;

            var targetType = propertyInfo.DeclaringType;

            if (!targetType.IsValueType)
            {
                var type = typeof(DelegateClassPropertyAccess<,>).MakeGenericType(targetType, propertyInfo.PropertyType);
                return (IClassPropertyAccess)Activator.CreateInstance(type, propertyInfo);
            }

            return null;
        }

        public static IClassPropertyAccess CreateForClass(Type type, string name)
        {
            return CreateForClass(type.GetProperty(name));
        }

        public static IValuePropertyAccess CreateForValue(Type type, string name)
        {
            return CreateForValue(type.GetProperty(name));
        }

        public static IValuePropertyAccess CreateForValue(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                return null;

            var targetType = propertyInfo.DeclaringType;

            if (targetType.IsValueType)
            {
                // Value types have to be handled differently, because the corresponding delegate types
                // have to provide their first argument ('this') as a ref.
                // see: http://stackoverflow.com/questions/4326736/how-can-i-create-an-open-delegate-from-a-structs-instance-method
                // see: http://stackoverflow.com/questions/1212346/uncurrying-an-instance-method-in-net/1212396#1212396

                var type = typeof(DelegateValuePropertyAccess<,>).MakeGenericType(targetType, propertyInfo.PropertyType);
                return (IValuePropertyAccess)Activator.CreateInstance(type, propertyInfo);
            }

            return null;
        }


        public static DelegateClassPropertyAccess<TTarget, TResult> CreateForClass<TTarget, TResult>(string name) where TTarget : class
        {
            var propertyInfo = typeof(TTarget).GetProperty(name);
            return new DelegateClassPropertyAccess<TTarget, TResult>(propertyInfo);
        }

        public static DelegateValuePropertyAccess<TTarget, TResult> CreateForValue<TTarget, TResult>(string name) where TTarget : struct
        {
            var propertyInfo = typeof(TTarget).GetProperty(name);
            return new DelegateValuePropertyAccess<TTarget, TResult>(propertyInfo);
        }

        public static IIndexedClassPropertyAccess CreateClassIndexed(Type targetType, Type indexType, string name)
        {
            var properties = (from property in targetType.GetProperties()
                let indexerParameters = property.GetIndexParameters()
                where indexerParameters.Length == 1 && indexerParameters[0].ParameterType == indexType
                select property).ToList();

            if (properties.Count != 1)
                return null;

            var propertyInfo = properties.Single();
            var type = typeof(IndexedDelegateClassPropertyAccess<,,>).MakeGenericType(targetType, indexType, propertyInfo.PropertyType);
            return (IIndexedClassPropertyAccess)Activator.CreateInstance(type, propertyInfo);
        }

        public static IndexedDelegateClassPropertyAccess<TTarget, TIndex, TResult> CreateClassIndexed<TTarget, TIndex, TResult>(string name) where TTarget : class
        {
            var propertyInfo = typeof(TTarget).GetProperty(name, typeof(TResult), new []{ typeof(TIndex) });
            return new IndexedDelegateClassPropertyAccess<TTarget, TIndex, TResult>(propertyInfo);
        }

        public static IIndexedPropertyReadAccess CreateValueIndexed(Type targetType, Type indexType, string name)
        {
            var propertyInfo = targetType.GetProperty(name);
            if (propertyInfo == null)
                return null;

            var type = typeof(IndexedDelegateValuePropertyAccess<,,>).MakeGenericType(targetType, indexType, propertyInfo.PropertyType);
            return (IIndexedPropertyReadAccess)Activator.CreateInstance(type, propertyInfo);
        }

        public static IndexedDelegateValuePropertyAccess<TTarget, TIndex, TResult> CreateValueIndexed<TTarget, TIndex, TResult>(string name) where TTarget : struct
        {
            var propertyInfo = typeof(TTarget).GetProperty(name);
            return new IndexedDelegateValuePropertyAccess<TTarget, TIndex, TResult>(propertyInfo);
        }
    }
}