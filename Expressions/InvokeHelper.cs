using System;
using System.Linq.Expressions;

namespace Expressions
{
    public static class InvocationHelper
    {
        public static object InvokeGenericMethodWithDynamicTypeArguments<T>(T target, Expression<Func<T, object>> expression,
            object[] methodArguments, params Type[] typeArguments)
        {
            var methodCall = (MethodCallExpression)expression.Body;

            var methodInfo = methodCall.Method;
            if (methodInfo.GetGenericArguments().Length != typeArguments.Length)
            {
                throw new ArgumentException(
                    string.Format(
                        "Parameter count mismatch. The method '{0}' has {1} type argument(s) but {2} type argument(s) were passed.",
                        methodInfo.Name,
                        methodInfo.GetGenericArguments().Length,
                        typeArguments.Length));
            }

            return methodInfo
                .GetGenericMethodDefinition()
                .MakeGenericMethod(typeArguments)
                .Invoke(target, methodArguments);
        }
        public static object InvokeGenericClassWithDynamicTypeArguments<T>(T target, Expression<Func<T, object>> expression,
            object[] methodArguments, params Type[] typeArguments)
        {
            var methodCall = (MethodCallExpression)expression.Body;

            var methodInfo = methodCall.Method;
            if (methodInfo.GetGenericArguments().Length != typeArguments.Length)
            {
                throw new ArgumentException(
                    string.Format(
                        "Parameter count mismatch. The method '{0}' has {1} type argument(s) but {2} type argument(s) were passed.",
                        methodInfo.Name,
                        methodInfo.GetGenericArguments().Length,
                        typeArguments.Length));
            }

            return methodInfo
                .GetGenericMethodDefinition()
                .MakeGenericMethod(typeArguments)
                .Invoke(target, methodArguments);
        }
    }
}
