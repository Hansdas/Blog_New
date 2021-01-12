using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Core.Common
{
    public class EncrypUtil
    {
        public static string MD5Encry(string strs)
        {
            if (string.IsNullOrEmpty(strs))
                return "";
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.Default.GetBytes(strs);//将要加密的字符串转换为字节数组
            byte[] encryptdata = md5.ComputeHash(bytes);//将字符串加密后也转换为字符数组
            return Convert.ToBase64String(encryptdata);//将加密后的字节数组转换为加密字符串
        }
        public static string Get_SHA1(string strSource)
        {
            string strResult = "";
            //Create 
            SHA1 md5 = SHA1.Create();
            //注意编码UTF8、UTF7、Unicode等的选择 
            byte[] bytResult = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(strSource));
            //字节类型的数组转换为字符串 
            for (int i = 0; i < bytResult.Length; i++)
            {
                //16进制转换 
                strResult = strResult + bytResult[i].ToString("X");
            }
            return strResult.ToLower();
        }
    }
}
