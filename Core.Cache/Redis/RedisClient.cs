using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cache.Redis
{
    public class RedisClient : ICacheClient
    {
        /// <summary>
        /// 过期时间
        /// </summary>
        private static TimeSpan ExpireTime = new TimeSpan(24, 0, 0);

        #region String操作
        public void StringSet<T>(string key, T t, TimeSpan? expiry = null)
        {
            string json = JsonConvert.SerializeObject(t);
            RedisProvider.Instance.StringSet(key,json,expiry);
        }
        public string StringGet(string key)
        {
            return RedisProvider.Instance.StringGet(key);
        }
        public T StringGet<T>(string key)
        {
            string value = StringGet(key);
            if (string.IsNullOrEmpty(value))
                return default(T);
            return JsonConvert.DeserializeObject<T>(value);
        }
        public List<T> StringGetList<T>(string[] keys)
        {
            RedisKey[] redisKeys = new RedisKey[keys.Length];
            for (int i = 0; i < keys.Length; i++)
                redisKeys[i] = keys[i];            
            RedisValue[] redisValues=  RedisProvider.Instance.StringGet(redisKeys);
            List<T> list = new List<T>();
            for(int i = 0; i < redisValues.Length; i++)
            {
                string json = redisValues[i];
                T t = JsonConvert.DeserializeObject<T>(json);
                list.Add(t);
            }
            return list;
        }
        #endregion

        #region 集合（Set）操作
        public void AddSet(string key, string value)
        {
            RedisProvider.Instance.SetAdd(key, value);
        }
        public string[] GetMembers(string key)
        {
            string[] members = RedisProvider.Instance.SetMembers(key).ToStringArray();
            return members;
        }

        public bool SetRemove(string key, string value)
        {
            return RedisProvider.Instance.SetRemove(key, value);
        }

        #endregion
        public void Remove(string key)
        {
            RedisProvider.Instance.KeyDelete(key);
        }
        public void BatchRemove(string[] keys)
        {
            var tran = RedisProvider.Instance.CreateTransaction();
            foreach (var item in keys)
                tran.KeyDeleteAsync(item);
            tran.Execute();
        }
        #region list操作
        public async Task<long> AddListTop<T>(string key, T t)
        {
            string value = JsonConvert.SerializeObject(t);
            return await AddListTop(key, value);
        }

        public async Task<long> AddListTop(string key, string value)
        {
            return await RedisProvider.Instance.ListLeftPushAsync(key, value);
        }
        public async Task<long> AddListTail<T>(string key, T t)
        {
            if (t == null)
                throw new ArgumentException("对象t为null");
            string value = JsonConvert.SerializeObject(t);
            return await AddListTail(key, value);
        }

        public async Task<long> AddListTail(string key, string value)
        {
            return await RedisProvider.Instance.ListRightPushAsync(key, value);
        }
        public async Task ListInsert<T>(string key, int index, T t)
        {
            if (t == null)
                throw new ArgumentException("对象t为null");
            string value = JsonConvert.SerializeObject(t);
            await RedisProvider.Instance.ListSetByIndexAsync(key, index, value);
        }
        public async Task<long> ListLenght(string key)
        {
            return await RedisProvider.Instance.ListLengthAsync(key);
        }

        public async Task listPop(string key)
        {
            await RedisProvider.Instance.ListRightPopAsync(key);
        }

        public async Task<List<T>> ListRange<T>(string key, int startindex, int endIndex)
        {
            var rediusValue = await RedisProvider.Instance.ListRangeAsync(key, startindex, endIndex);
            List<T> list = new List<T>();
            foreach (var item in rediusValue)
            {
                list.Add(JsonConvert.DeserializeObject<T>(item));
            }
            return list;
        }
        #endregion
    }
}
