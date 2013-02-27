using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Expressions
{
   internal static class ObjectInvokerCache<TArg1, TArg2, TArg3, TArg4, TArg5, TR>
    {
        private static readonly Dictionary<Type, Func<TArg1, TArg2, TArg3,TArg4,TArg5, TR>> _creationMethods =
            new Dictionary<Type, Func<TArg1, TArg2, TArg3, TArg4, TArg5, TR>>();
 
        public static TR CreateInstanceOf(Type type, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5)
        {
            CacheInstanceCreationMethodIfRequired(type);
 
            return _creationMethods[type].Invoke(arg1, arg2, arg3, arg4, arg5);
        }
      
        private static void CacheInstanceCreationMethodIfRequired(Type type)
        {
            var argumentTypes = new[] { typeof(TArg1), typeof(TArg2) };

            Type[] constructorArgumentTypes = argumentTypes.Where(t => t != typeof(Ignored)).ToArray();

            var constructor = type.GetConstructor(
                BindingFlags.Instance | BindingFlags.Public,
                null,
                CallingConventions.HasThis,
                constructorArgumentTypes,
                new ParameterModifier[0]);

            var lamdaParameterExpressions = new[]
            {
                Expression.Parameter(typeof(TArg1), "param1"),
                Expression.Parameter(typeof(TArg2), "param2"),
                Expression.Parameter(typeof(TArg3), "param3"),
                Expression.Parameter(typeof(TArg4), "param4"),
                Expression.Parameter(typeof(TArg5), "param5")
            };

            var constructorParameterExpressions = lamdaParameterExpressions
                .Take(constructorArgumentTypes.Length)
                .ToArray();

            var constructorCallExpression = Expression.New(constructor, constructorParameterExpressions);

            var constructorCallingLambda = Expression
                .Lambda<Func<TArg1, TArg2, TArg3, TArg4, TArg5, TR>>(constructorCallExpression, lamdaParameterExpressions)
                .Compile();
 
            _creationMethods[type] = constructorCallingLambda;
        }
    }
    public class Ignored
    {
    }
}
