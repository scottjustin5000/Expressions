using System.Runtime.Caching;
using System.Threading;

namespace Expressions
{
    public static class ReflectionCache
    {
        private static readonly ObjectCache _cache = MemoryCache.Default;
        private static readonly ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();

        private static readonly CacheItemPolicy _policy = new CacheItemPolicy();

        public static void AddItem<T>(string key, T item)
        {
            _rwLock.EnterWriteLock();
            try
            {
                _cache.Set(key, item, _policy);
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }
        public static object GetItem(string key)
        {
            _rwLock.EnterReadLock();

            var item = _cache.Get(key);
            _rwLock.ExitReadLock();
            return item;
        }
        
    
    }
}
