using System.Reflection;

namespace Expressions
{
    public class FieldCache
    {
        public static FieldInspector<T, R> GetInvoker<T, R>(T obj, FieldInfo info)
        {
            var keyName = string.Format("fieldInf_{0}", info.GetHashCode());
            var cached = ReflectionCache.GetItem(keyName);
            if (cached != null)
            {
                return cached as FieldInspector<T, R>;
            }
            var invoker = new FieldInspector<T, R>(info, obj);
            ReflectionCache.AddItem(keyName, invoker);
            return invoker;
        }
    }
}
