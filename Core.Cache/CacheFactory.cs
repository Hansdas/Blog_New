using Core.Cache.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Cache
{
    public class CacheFactory : ICacheFactory
    {
        private ICacheClient _cacheClient;
        public ICacheClient CreateClient(CacheType cacheType)
        {
            switch (cacheType)
            {
                case CacheType.Redis:
                    _cacheClient=new RedisClient();
                    break;
                case CacheType.MemoryCache:
                    break;
            }
            return _cacheClient;
        }
    }
}
