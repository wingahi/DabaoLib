using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace myLib.Security
{
    /// <summary>
    /// 加盐加密
    /// </summary>
    public class SaltHash
    {
        #region 加盐加密

        /// <summary>
        /// 生成用于加盐加密的盐值
        /// </summary>
        /// <returns></returns>
        public static string GenerateSalt()
        {
            var data = new byte[0x10];
            new RNGCryptoServiceProvider().GetBytes(data);
            return Convert.ToBase64String(data);
        }

        private const string PasswordHashAlgorithmName = "SHA1";

        /// <summary>
        /// 加盐加密
        /// </summary>
        /// <param name="password">明文密码</param>
        /// <param name="salt">明文盐值</param>
        /// <returns>加密后的密码</returns>
        public static string EncodePassword(string password, string salt)
        {
            var src = Encoding.Unicode.GetBytes(password);
            var saltbuf = Convert.FromBase64String(salt);
            var dst = new byte[saltbuf.Length + src.Length];
            byte[] inArray = null;
            Buffer.BlockCopy(saltbuf, 0, dst, 0, saltbuf.Length);
            Buffer.BlockCopy(src, 0, dst, saltbuf.Length, src.Length);

            var algorithm = HashAlgorithm.Create(PasswordHashAlgorithmName);
            inArray = algorithm.ComputeHash(dst);

            return Convert.ToBase64String(inArray);
        }

        #endregion
    }
}
