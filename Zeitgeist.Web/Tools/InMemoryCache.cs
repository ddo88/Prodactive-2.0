using System;
using System.Dynamic;
using System.Runtime.Caching;  

namespace Zeitgeist.Web.Tools
{
    /// 
    /// 

    public class InMemoryCache: ICacheService
    {
        public T GetOrSet<T>(string cacheKey, Func<T> getItemCallback) where T : class
        {
            T item = MemoryCache.Default.Get(cacheKey) as T;
            if (item == null)
            {
                item = getItemCallback();
                MemoryCache.Default.Add(cacheKey, item, DateTime.Now.AddMinutes(10));
            }
            return item;
        }

        public void DeleteCache(string cacheKey)
        {
            MemoryCache.Default.Remove(cacheKey);
        }
    }

    interface ICacheService
    {
        T GetOrSet<T>(string cacheKey, Func<T> getItemCallback) where T : class;
        void DeleteCache(string cacheKey);
    }
}