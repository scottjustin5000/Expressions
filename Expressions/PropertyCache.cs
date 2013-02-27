using System.Reflection;

namespace Expressions
{
   public class PropertyCache
    {
       public static PropertyInspector<T, R> GetInvoker<T, R>(T obj, PropertyInfo info)
       {
           var keyName = string.Format("propertyInf_{0}", info.GetHashCode());
           var cached = ReflectionCache.GetItem(keyName);
           if (cached != null)
           {
               return cached as PropertyInspector<T, R>;
           }
           var invoker = new PropertyInspector<T, R>(info, obj);
           ReflectionCache.AddItem(keyName, invoker);
           return invoker;
       }
    }
}
