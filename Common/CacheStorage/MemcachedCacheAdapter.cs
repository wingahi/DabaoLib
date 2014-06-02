using Enyim.Caching;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;

namespace myLib.CacheStorage
{
    /// <summary>
    /// 分布式Memcached缓存
    /// </summary>
    class MemcachedCacheAdapter : ICacheStorage
    {
        private MemcachedClient cache;

        public MemcachedCacheAdapter()
        {
            cache = new MemcachedClient();
            IList<string> keys = new List<string>();
            cache.Store(StoreMode.Add, "keys", keys);
        }

        #region ICacheStorage 成员
        /// <summary>
        /// 插入缓存
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        public void Insert(string key, object value)
        {
            cache.Store(StoreMode.Set, key, value);
        }
        /// <summary>
        /// 插入缓存
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="expiration">绝对过期时间</param>
        public void Insert(string key, object value, DateTime expiration)
        {
            cache.Store(StoreMode.Set, key, value, expiration);
            Updatekeys(key);
        }
        /// <summary>
        /// 插入缓存
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="expiration">过期时间</param>
        public void Insert(string key, object value, TimeSpan expiration)
        {
            cache.Store(StoreMode.Set, key, value, expiration);
            Updatekeys(key);
        }
        /// <summary>
        /// 根据key获取value
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public object Get(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;
            return cache.Get(key);
        }
        /// <summary>
        /// 根据key获取value
        /// </summary>
        /// <typeparam name="T">T对象</typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            object obj = this.Get(key);
            return obj == null ? default(T) : (T)obj;
        }
        /// <summary>
        /// 删除key的缓存的值
        /// </summary>
        /// <param name="key">key</param>
        public void Remove(string key)
        {
            if (this.Exist(key))
                cache.Remove(key);
        }
        /// <summary>
        /// 检验key是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exist(string key)
        {
            if (cache.Get(key) != null)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 获取所有的key
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetCacheKeys()
        {
            return cache.Get("keys") as IEnumerable<string>;
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
        /// <summary>
        /// 更新key
        /// </summary>
        /// <param name="key">key</param>
        private void Updatekeys(string key)
        {
            IList<string> keys = new List<string>();

            if (cache.Get("keys") != null)
                keys = cache.Get("keys") as IList<string>;

            if (!keys.Contains(key.ToLower()))
                keys.Add(key);
            cache.Store(StoreMode.Set, "keys", keys);
        }
        #endregion
    }
}
