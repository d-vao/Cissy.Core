using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Cissy.Configuration;
using Cissy.Caching.MemoryCache;
using Cissy.Caching.WeakCaching;

namespace Cissy.Caching
{
    public static class CachConfigHelper
    {
        /// <summary>
        /// 注入本地内存缓存服务
        /// </summary>
        /// <param name="cissyConfigBuilder"></param>
        /// <param name="MemoryCacheOptions"></param>
        /// <returns></returns>
        public static CissyConfigBuilder AddLocalMemoryCache(this CissyConfigBuilder cissyConfigBuilder, Microsoft.Extensions.Caching.Memory.MemoryCacheOptions MemoryCacheOptions = default)
        {
            LocalMemoryCache cache = new LocalMemoryCache();
            cache.Init(MemoryCacheOptions);

            cissyConfigBuilder.ServiceCollection.AddSingleton(typeof(ILocalMemoryCache), cache);
            return cissyConfigBuilder;
        }
        /// <summary>
        /// 注入本地弱引用缓存服务
        /// </summary>
        /// <param name="cissyConfigBuilder"></param>
        /// <returns></returns>
        public static CissyConfigBuilder AddWeakCache(this CissyConfigBuilder cissyConfigBuilder)
        {
            WeakCaching.WeakStorage weakstorage = new WeakCaching.WeakStorage();
            cissyConfigBuilder.ServiceCollection.AddSingleton(typeof(IWeakCache), weakstorage);
            return cissyConfigBuilder;
        }
    }
}
