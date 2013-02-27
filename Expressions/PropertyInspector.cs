using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Expressions
{
    public class PropertyInspector<T, TR>
    {

        private readonly Func<T, TR> _getter;
        private readonly Action<T, TR> _setter; 
       
        private readonly T _object;

        public PropertyInfo PropertyInf { get; private set; }

        public PropertyInspector(PropertyInfo prop, T obj)
        {
            PropertyInf = prop;
            _object = obj;
            _getter = Getter();
            _setter = Setter();
            
        }
        public TR GetValue()
        {
            return _getter(_object);
        }
        public void SetValue(TR val)
        {
            _setter(_object, val);
        }
        public Action<T, TR> Setter()
        {
                var instance = Expression.Parameter(typeof (T), "instance");
                var value = Expression.Parameter(typeof (TR), "value");

                return Expression.Lambda<Action<T, TR>>(Expression.Call(instance, PropertyInf.GetSetMethod(true), value),
                                                        new ParameterExpression[] {instance, value}).Compile();
        }
  
        private Func<T,TR> Getter()
        {
            var instance = Expression.Parameter(typeof(T), "instance");
            var propertyAccess = Expression.Property(instance,PropertyInf);
            var lambda = Expression.Lambda<Func<T, TR>>(propertyAccess, instance);
            return lambda.Compile();
        }


    }
}
