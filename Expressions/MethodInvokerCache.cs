using System.Reflection;

namespace Expressions
{
   public static class MethodInvokerCache 
   {
       public static MethodInvoker<T, R> GetInvoker<T, R>(T obj, MethodInfo info)
       {
           var keyName = string.Format("methodInf_{0}", info.GetHashCode());
           var cached = ReflectionCache.GetItem(keyName);
           if (cached != null)
           {
               return cached as MethodInvoker<T, R>;
           }
           var invoker = new MethodInvoker<T, R>(info, obj);
           ReflectionCache.AddItem(keyName, invoker);
           return invoker;
       }
   }
}
