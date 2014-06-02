using System.Configuration;

namespace myLib.CacheStorage
{
    /// <summary>
    /// 缓存工厂
    /// </summary>
    public class CacheFactory
    {
        /// <summary>
        /// 缓存类型
        /// </summary>
        public enum CacheType
        {
            /// <summary>
            /// 默认缓存
            /// </summary>
            DefaultCache = 0,
            /// <summary>
            /// 分布式Memcached缓存
            /// </summary>
            MemcachedCache = 1
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public static ICacheStorage CreateCacheFactory()
        {
            string cache = ConfigurationManager.AppSettings["CacheType"];
            if (CacheType.MemcachedCache.ToString() == cache)
                return new MemcachedCacheAdapter();
            else
                return new DefaultCacheAdapter();
        }
    }
}
