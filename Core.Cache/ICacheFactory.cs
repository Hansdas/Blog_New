using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Cache
{
   public interface ICacheFactory
    {
        ICacheClient CreateClient(CacheType cacheType);
    }
}
