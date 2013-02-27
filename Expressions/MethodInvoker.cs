using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;


namespace Expressions
{
    public class MethodInvoker<T, TR> 
    {
        private readonly Func<T, object[], TR> _invoker;
        private readonly Action<T, object[]> _voidInvoker;
        public  MethodInfo MethodInf { get; private set; }
        private readonly T _object;

        public MethodInvoker(MethodInfo info, T obj)
        {
            _object = obj;
            MethodInf = info;
            if (MethodInf.ReturnType != typeof (void))
            {
                _invoker = GetInvoker(info);
            }
            else
            {
                _voidInvoker = GetVoidInvoker(info);
            }

        } 
        public TR Invoke(params object[] parameters)
        {
            if (_invoker != null)
            {
                return _invoker(_object, parameters);
            }
            _voidInvoker(_object, parameters);
            var obj = new object();
            return (TR)obj;
        }
        private static Func<T, object[], TR> GetInvoker(MethodInfo m)
        {
            var instanceParameter = Expression.Parameter(typeof(T), "instance");
            var parametersParameter = Expression.Parameter(typeof(object[]), "parameters");
            
            var parameterExpressions = new List<Expression>();
            var paramInfos = m.GetParameters();
            for (int i = 0; i < paramInfos.Length; i++)
            {
                BinaryExpression valueObj = Expression.ArrayIndex(
                    parametersParameter, Expression.Constant(i));
                UnaryExpression valueCast = Expression.Convert(
                    valueObj, paramInfos[i].ParameterType);

                parameterExpressions.Add(valueCast);
            }

            var methodCall = Expression.Call(instanceParameter, m, parameterExpressions);


            var lambda = Expression.Lambda<Func<T, object[], TR>>(
                methodCall, instanceParameter, parametersParameter);

            return lambda.Compile();
        } 
        private static Action<T, object[]> GetVoidInvoker(MethodInfo m)
        {
            var instance = Expression.Parameter(typeof(T), "instance");
            var parametersParameter = Expression.Parameter(typeof(object[]), "parameters");

            var parameterExpressions = new List<Expression>();
            var paramInfos = m.GetParameters();
            for (int i = 0; i < paramInfos.Length; i++)
            {
                BinaryExpression valueObj = Expression.ArrayIndex(
                    parametersParameter, Expression.Constant(i));
                UnaryExpression valueCast = Expression.Convert(
                    valueObj, paramInfos[i].ParameterType);

                parameterExpressions.Add(valueCast);
            }
            var methodCall = Expression.Call(instance, m, parameterExpressions);
            var lambda = Expression.Lambda<Action<T, object[]>>(
                       methodCall, instance, parametersParameter);

           return lambda.Compile();
        }

    }
}
