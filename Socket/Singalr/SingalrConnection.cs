using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Core.Socket.Singalr
{
   internal class SingalrConnection
    {
        /// <summary>
        /// 连接对象集合
        /// </summary>
        public static ConcurrentDictionary<string, string> ConnectionMaps = new ConcurrentDictionary<string, string>();
        /// <summary>
        /// 添加连接对象
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="value"></param>
        public static void SetConnectionMaps(string connectionId, string value)
        {
            ConnectionMaps.AddOrUpdate(connectionId, value, (string s, string y) => value);
        }
        /// <summary>
        /// 删除对象集合
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            if (ConnectionMaps.ContainsKey(key))
                ConnectionMaps.TryRemove(key, out string value);
        }
        /// <summary>
        /// 获取连接
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<string> GetConnectionIds(string value)
        {
            List<string> connectionIds = new List<string>();
            foreach (KeyValuePair<string, string> item in ConnectionMaps)
            {
                if (item.Value == value)
                    connectionIds.Add(item.Key);
            }
            return connectionIds;
        }

    }
}
