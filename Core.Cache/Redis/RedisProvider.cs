
using Core.CPlatform;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Core.Cache.Redis
{
    public class RedisProvider
    {
        private static ConcurrentDictionary<string, ConnectionMultiplexer> connectionDic = new ConcurrentDictionary<string, ConnectionMultiplexer>();
        private static object _lock = new object();
        public static IDatabase Instance
        {
            get
            {
                if (_database == null)
                {
                    lock (_lock)
                    {
                        if (_database == null)
                        {
                            _database = GetConnection().GetDatabase(_defaultDB);
                        }
                    }
                }
                return _database;
            }
        }
        private static IDatabase _database;
        private static int _defaultDB;
        private static ConnectionMultiplexer GetConnection()
        {

            RedisModel model = ConfigureProvider.BuildModel<RedisModel>("Redis");
            string connStr = string.Format("{0}:{1}", model.Host, model.Port);
            _defaultDB = model.DefaultDB;
            ConfigurationOptions options = new ConfigurationOptions()
            {
                EndPoints = { { connStr } },
                DefaultDatabase = _defaultDB,
                //ServiceName = connStr,
                Password = model.Password,
                ReconnectRetryPolicy = new ExponentialRetry(5000),
            };
            options.ClientName = model.InstanceName;
            return connectionDic.GetOrAdd(connStr, s => ConnectionMultiplexer.Connect(options));
        }
    }
}
