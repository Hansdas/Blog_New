using Core.Common.EnumExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Core.Common.EnumExtension
{
   public class EnumConvert<T>
    {
        /// <summary>
        /// 将Enum转为Ienumable集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> AsEnumerable()
        {
            EnumQuery<T> query = new EnumQuery<T>();
            return query;
        }      
        /// <summary>
        /// 将Enum转为字典结合
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> AsDictionary()
        {
            Type @enum = typeof(T);
            Array array = Enum.GetValues(@enum);
            Dictionary<int, string> enumDic = new Dictionary<int, string>();
            foreach (var item in array)
            {
                int key = item.GetEnumValue(); ;
                enumDic.Add(key, item.ToString());
            }
            return enumDic;
        }
        /// <summary>
        /// 将Enum转为键值对的list
        /// </summary>
        /// <returns></returns>
        public static IList<KeyValueItem> AsKeyValueItem()
        {
            Type @enum = typeof(T);
            Array array = Enum.GetValues(@enum);
            IList<KeyValueItem> list = new List<KeyValueItem>();
            foreach (var item in array)
            {
                int key = item.GetEnumValue();
                KeyValueItem keyValueItem = new KeyValueItem(key.ToString(), item.ToString());
                list.Add(keyValueItem);
            }
            return list;
        }
        private class EnumQuery<T> : IEnumerable<T>
        {
            public IEnumerator<T> GetEnumerator()
            {
                IList<T> list = new List<T>();
                Array array = Enum.GetValues(typeof(T));
                foreach (var item in array)
                    list.Add((T)item);
                return list.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }

    /// <summary>
    /// 键值对的item
    /// </summary>
    public class KeyValueItem
    {
        public KeyValueItem(string key, string value)
        {
            Key = key;
            Value = value;
        }
        public string Key { get; private set; }

        public string Value { get; private set; }
    }
}
