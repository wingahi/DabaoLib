using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace myLib.Security
{
    /// <summary>
    /// Base64 加密解密类
    /// </summary>
    public class Base64
    {
        /// <summary>
        /// BASE64编码字符表，即各编码的表示字符
        /// </summary>
        private static readonly char[] BASE64_ENCODE_TABLE = 
            {
                'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V',
                'W','X','Y','Z','a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r',
                's','t','u','v','w','x','y','z','0','1','2','3','4','5','6','7','8','9','+','/','='
            };

        /// <summary>
        /// BASE64字符编码表，根据各编码字符的ASSII码值找到对应的BASE64编码
        /// </summary>
        private static readonly byte[] BASE64_DECODE_TABLE =
            {
                255,255,255,255,255,255,255,255,//000-007
                255,255,255,255,255,255,255,255,//008-015
                255,255,255,255,255,255,255,255,//017-023
                255,255,255,255,255,255,255,255,//024-031
                255,255,255,255,255,255,255,255,//032-039
                255,255,255,062,255,255,255,063,//040-047
                052,053,054,055,056,057,058,059,//048-055
                060,061,255,255,255,064,255,255,//056-063
                061,000,001,002,003,004,005,006,//064-071
                007,008,009,010,011,012,013,014,//072-079
                015,016,017,018,019,020,021,022,//080-087
                023,024,025,255,255,255,255,255,//088-095
                255,026,027,028,029,030,031,032,//096-103
                033,034,035,036,037,038,039,040,//104-111
                041,042,043,044,045,046,047,048,//112-119
                049,050,051,255,255,255,255,255,//120-127
                255,255,255,255,255,255,255,255,//128-135
                255,255,255,255,255,255,255,255,//136-143
                255,255,255,255,255,255,255,255,//144-151
                255,255,255,255,255,255,255,255,//152-159
                255,255,255,255,255,255,255,255,//160-167
                255,255,255,255,255,255,255,255,//168-175
                255,255,255,255,255,255,255,255,//176-183
                255,255,255,255,255,255,255,255,//184-191
                255,255,255,255,255,255,255,255,//192-199
                255,255,255,255,255,255,255,255,//200-207
                255,255,255,255,255,255,255,255,//208-215
                255,255,255,255,255,255,255,255,//216-223
                255,255,255,255,255,255,255,255,//224-231
                255,255,255,255,255,255,255,255,//232-239
                255,255,255,255,255,255,255,255,//240-247
                255,255,255,255,255,255,255,255,//248-255
            };

        private static byte[] ToBase64(byte[] data)
        {
            List<byte> Result = new List<byte>();
            using (MemoryStream ms = new MemoryStream(data))
            {
                ms.Position = 0;
                byte[] buffer = new byte[3];
                int count;
                count = ms.Read(buffer, 0, 3);
                byte val = 0;
                UInt32 shiftValue = 0;
                while (count > 0)
                {
                    switch (count)
                    {
                        case 1: buffer[1] = 0;
                            buffer[2] = 0;
                            break;
                        case 2: buffer[2] = 0; break;
                        default: break;
                    }
                    shiftValue = buffer[0];
                    val = (byte)(shiftValue >> 2);
                    Result.Add(val);
                    shiftValue &= 3;
                    val = (byte)(shiftValue << 4);
                    shiftValue = buffer[1];
                    val += (byte)(shiftValue >> 4);
                    Result.Add(val);
                    shiftValue &= 15;
                    val = (byte)(shiftValue << 2);
                    shiftValue = buffer[2];
                    val += (byte)(shiftValue >> 6);
                    Result.Add(val);
                    val = (byte)(shiftValue & 63);
                    Result.Add(val);
                    count = ms.Read(buffer, 0, 3);
                }
            }
            return Result.ToArray();
        }

        /// <summary>
        /// 将字节流（数组）转换为BASE64字符串
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <returns>转换后的BASE64字符串</returns>
        public static string ToBase64String(byte[] data)
        {
            byte[] base64ByteArray = ToBase64(data);
            int lenNull = data.Length % 3;
            if (lenNull == 1)
            {
                base64ByteArray[base64ByteArray.Length - 2] = 64;
                base64ByteArray[base64ByteArray.Length - 1] = 64;
            }
            else if (lenNull == 2)
            {
                base64ByteArray[base64ByteArray.Length - 1] = 64;
            }
            StringBuilder Result = new StringBuilder(string.Empty);
            foreach (byte i in base64ByteArray)
            {
                Result.Append(BASE64_ENCODE_TABLE[i]);
            }
            return Result.ToString();
        }

        /// <summary>
        /// 将BASE64字符串转换为相应的字节流（数组）
        /// </summary>
        /// <param name="base64String">BASE64字符串</param>
        /// <returns>转换后的字节流（数组）</returns>
        public static byte[] FromBase64String(string base64String)
        {
            List<byte> Result = new List<byte>();
            StringReader reader = new StringReader(base64String);
            int count = 0;
            char[] buffer = new char[4];
            byte val;
            UInt32 bs64;
            count = reader.Read(buffer, 0, 4);
            while (count > 0)
            {
                bs64 = BASE64_DECODE_TABLE[(byte)buffer[0]];
                val = (byte)(bs64 << 2);
                bs64 = BASE64_DECODE_TABLE[(byte)buffer[1]];
                val += (byte)(bs64 >> 4);
                Result.Add(val);
                if (BASE64_DECODE_TABLE[(byte)buffer[2]] == 64)
                {
                    break;
                }
                bs64 &= 15;
                val = (byte)(bs64 << 4);
                bs64 = BASE64_DECODE_TABLE[(byte)buffer[2]];
                val += (byte)(bs64 >> 2);
                Result.Add(val);
                if (BASE64_DECODE_TABLE[(byte)buffer[3]] == 64)
                {
                    break;
                }
                bs64 &= 3;
                bs64 <<= 6;
                val = (byte)(bs64 + BASE64_DECODE_TABLE[(byte)buffer[3]]);
                Result.Add(val);
                count = reader.Read(buffer, 0, 4);
            }
            return Result.ToArray();
        }
    }
}
