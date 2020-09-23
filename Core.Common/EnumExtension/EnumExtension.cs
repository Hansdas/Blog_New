using Core.Common.EnumExtension;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Core.Common.EnumExtensions
{
    public static class EnumExtension
    {
        /// <summary>
        /// 返回枚举所对应的值
        /// </summary>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static int GetEnumValue<T>(this T @enum)
        {
            return (int)@enum.GetType().GetField(@enum.ToString()).GetRawConstantValue();
        }
        /// <summary>
        /// 返回枚举名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetEnumText<T>(this T enumValue)
        {
            return Enum.GetName(typeof(T), enumValue);
        }
        /// <summary>
        /// 返回枚举所对应的附加数据
        /// </summary>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static string GetEnumAdditional<T>(this T @enum)
        {
            string value = @enum.ToString();
            FieldInfo fieldInfo = @enum.GetType().GetField(value);
            var obj = fieldInfo.GetCustomAttributes(typeof(EnumAdditionalAttribute), false).First();
            EnumAdditionalAttribute attribute = obj as EnumAdditionalAttribute;
            if (attribute == null)
                return "";
            return attribute.Additional;
        }
        /// <summary>
        /// 根据值返回枚举名字
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetEnumText<T>(this int enumValue)
        {
            return Enum.GetName(typeof(T), enumValue);
        }
    }
}
