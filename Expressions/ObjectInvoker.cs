using System;

namespace Expressions
{
    public class ObjectInvoker<T>
    {
        private Type _type;
      public ObjectInvoker (Type type)
      {
          _type = type;
      } 
         public T GetObject()
         {
             return GetObject<Ignored>(null);
         }
        public T GetObject<TArg1>(TArg1 arg1)
        {
            return GetObject<TArg1,Ignored>(arg1,null);
        }
        public T GetObject<TArg1, TArg2>(TArg1 arg1, TArg2 arg2)
        {
           return GetObject<TArg1, TArg2, Ignored>(arg1,arg2, null);
        }
        public T GetObject<TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3)
        {
            return GetObject<TArg1, TArg2,TArg3, Ignored>(arg1, arg2, arg3, null);
        }
        public T GetObject<TArg1, TArg2, TArg3, TArg4>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4)
        {
            return GetObject<TArg1, TArg2, TArg3,TArg4, Ignored>(arg1, arg2, arg3,arg4, null);
        }
    
        public T GetObject<TArg1, TArg2, TArg3, TArg4, TArg5>(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5)
        {
            return 
            ObjectInvokerCache<TArg1, TArg2, TArg3, TArg4,TArg5,T>
                .CreateInstanceOf(_type, arg1, arg2, arg3, arg4,arg5);
 
        }
    }
}
