using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;

namespace myLib.CacheStorage
{
    /// <summary>
    /// 默认的asp.net中的Cache
    /// </summary>
    class DefaultCacheAdapter : ICacheStorage
    {
        /// <summary>
        /// 当前请求上下文
        /// </summary>
        private static Cache cache = null;
        /// <summary>
        /// 构造函数
        /// </summary>
        static DefaultCacheAdapter()
        {
            cache = HttpRuntime.Cache;
        }

        #region ICacheStorage 成员
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        public void Insert(string key, object value)
        {
            cache.Insert(key, value);
        }
        /// <summary>
        /// 添加缓存（默认过期时间为20分钟）
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="expiration">绝对过期时间</param>
        public void Insert(string key, object value, DateTime expiration)
        {
            cache.Insert(key, value, null, expiration, TimeSpan.FromMinutes(20), CacheItemPriority.Normal, null);
        }
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="expiration">过期时间</param>
        public void Insert(string key, object value, TimeSpan expiration)
        {
            cache.Insert(key, value, null, Cache.NoAbsoluteExpiration, expiration, CacheItemPriority.Normal, null);
        }
        /// <summary>
        /// 获取当前缓存中key的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;
            return cache.Get(key);
        }
        /// <summary>
        /// 获取当前缓存中key的值
        /// </summary>
        /// <typeparam name="T">泛型对象</typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            object obj = this.Get(key);
            return obj == null ? default(T) : (T)obj;
        }
        /// <summary>
        /// 删除当前key的value值
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            if (Exist(key))
                cache.Remove(key);
        }
        /// <summary>
        /// 缓存是否存在key的value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exist(string key)
        {
            if (cache.Get(key) == null)
                return false;
            else
                return true;
        }
        /// <summary>
        /// 获取所有的缓存key
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetCacheKeys()
        {
            IDictionaryEnumerator ide = cache.GetEnumerator();
            while (ide.MoveNext())
            {
                yield return ide.Key.ToString();
            }
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        public void Clear()
        {
            foreach (var c in this.GetCacheKeys())
            {
                this.Remove(c);
            }
        }
        #endregion
    }
}
