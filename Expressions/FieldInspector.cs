using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Expressions
{
    public class FieldInspector<T,TR>
    {
        private readonly Func<T, TR> _getter;
        private readonly T _object;

        public FieldInfo FieldInfo { get; private set; }
       
        public FieldInspector(FieldInfo fieldInfo, T obj)
        {
            FieldInfo = fieldInfo;
            _object = obj;
            _getter = Getter(fieldInfo);

        }
        public TR GetValue(T instance)
        {
            return _getter(instance);
        }
        private static Func<T, TR> Getter(FieldInfo fieldInfo)
        {
            var instance = Expression.Parameter(typeof(object), "instance");

            var instanceCast = fieldInfo.IsStatic ? null :
                Expression.Convert(instance, fieldInfo.ReflectedType);

            var fieldValue = Expression.Field(instanceCast, fieldInfo);


            var lambda = Expression.Lambda<Func<T, TR>>(fieldValue, instance);

            return lambda.Compile();
        } 

    }
}
