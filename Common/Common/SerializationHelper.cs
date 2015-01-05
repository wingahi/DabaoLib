using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace myLib.Common
{
    /// <summary>
    /// 序列化助手类
    /// </summary>
    public class SerializationHelper
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SerializationHelper() { }

        #region Newtonsoft.Json 封装
        /// <summary>
        /// 序列化为JSON字符串
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string ToJson(object entity)
        {
            var converter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            var serializer = new JsonSerializer();
            serializer.Converters.Add(converter);

            var sb = new StringBuilder();
            serializer.Serialize(new JsonTextWriter(new StringWriter(sb)), entity);
            return sb.ToString();
        }
        /// <summary>
        /// 反序列化JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T FromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        /// <summary>
        /// 序列化为XML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToXml<T>(T obj)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, obj);
                stream.Position = 0;
                var reader = new StreamReader(stream);
                var xml = reader.ReadToEnd();
                reader.Close();
                return xml;
            }
        }
        /// <summary>
        /// 反序列化XML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T FromXml<T>(string xml)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var stream = new MemoryStream())
            {
                var writer = new StreamWriter(stream);
                writer.Write(xml);
                writer.Flush();
                stream.Position = 0;
                var instance = (T)serializer.Deserialize(stream);
                writer.Close();
                return instance;
            }
        } 
        #endregion

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="filename">文件路径</param>
        /// <returns></returns>
        public static object Load(Type type, string filename)
        {
            FileStream fs = null;
            try
            {
                // open the stream...
                fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var serializer = new XmlSerializer(type);
                return serializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }
        
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="filename">文件路径</param>
        public static void Save(object obj, string filename)
        {
            FileStream fs = null;
            // serialize it...
            try
            {
                fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                var serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(fs, obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

    }
}
