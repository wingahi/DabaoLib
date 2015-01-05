using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace myLib.Security
{
    /// <summary>
    /// MD5加密类
    /// </summary>
    public class Md5Encrypt
    {
        #region MD5加密
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static string Md5(string pwd)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            var data = Encoding.Default.GetBytes(pwd);
            var md5Data = md5.ComputeHash(data);
            md5.Clear();
            var str = "";
            for (var i = 0; i < md5Data.Length; i++)
            {
                str += md5Data[i].ToString("x").PadLeft(2, '0');

            }
            return str;
        }
        #endregion
    }
}
