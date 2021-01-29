using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace ExpressAPI.Common
{
    //XML序列化公共处理类
    public static class XmlSerializeHelper
    {


        /// <summary>
        /// 为了序列化出来的字符串头部为utf-8，自己封装一个
        /// </summary>
        public class StringUTF8Writer : StringWriter
        {
            public override Encoding Encoding
            {
                get { return Encoding.UTF8; }
            }
        }
        /// <summary>
        /// 将实体对象转换成XML
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="obj">实体对象</param>
        public static string XmlSerialize(object o, Encoding encoding)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    XmlSerializeInternal(stream, o, encoding);


                    stream.Position = 0;
                    using (StreamReader reader = new StreamReader(stream, encoding))
                    {
                        return reader.ReadToEnd();
                    }
                }
                //    using (StringUTF8Writer sw = new StringUTF8Writer())
                //{
                //    XmlSerializer xz = new XmlSerializer(obj.GetType());

                //    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                //    ns.Add("", "");//去掉命名空间，如果要加自己在字符串拼接，不然不好处理
                //    xz.Serialize(sw, obj, ns);

                //    return sw.ToString();
                //}
             

                //using (StringWriter sw = new StringWriter())
                //{
                //    Type t = obj.GetType();
                //    XmlSerializer serializer = new XmlSerializer(obj.GetType());
                //    serializer.Serialize(sw, obj);
                //    sw.Close();
                //    return sw.ToString();
                //}
            }
            catch (Exception ex)
            {
                throw new Exception("将实体对象转换成XML异常", ex);
            }
        }

        private static void XmlSerializeInternal(Stream stream, object o, Encoding encoding)
        {
            if (o == null)
                throw new ArgumentNullException("o");
            if (encoding == null)
                throw new ArgumentNullException("encoding");


            XmlSerializer serializer = new XmlSerializer(o.GetType());

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");//去掉命名空间，如果要加自己在字符串拼接，不然不好处理
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.CheckCharacters = false;
            settings.NewLineChars = "\r\n";
            settings.Encoding = encoding;
            settings.IndentChars = " ";
            settings.OmitXmlDeclaration = true;
            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
             
                serializer.Serialize(writer, o, ns);
                writer.Close();
            }
        }

        /// <summary>
        /// 将XML转换成实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="strXML">XML</param>
        public static T DESerializer<T>(string strXML) where T : class
        {
            try
            {
                using (StringReader sr = new StringReader(strXML))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return serializer.Deserialize(sr) as T;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("将XML转换成实体对象异常", ex);
            }
        }

    }
}