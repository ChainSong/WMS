using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;

namespace Runbow.ImportPrice
{
   public class XMLHelper
    {
        /// <summary>         
        /// 使用反射把List<T>转换成XmlDocument         
        /// </summary>        
        /// <returns></returns>         
        public static XmlDocument ListToXML<T>(string XmlName, IList<T> IL)
        {
            try
            {
                XmlDocument XMLdoc = new XmlDocument();

                //建立XML的定义声明                 

                XmlDeclaration XMLdec = XMLdoc.CreateXmlDeclaration("1.0", "utf-8", null);

                XMLdoc.AppendChild(XMLdec);

                XmlElement Root = XMLdoc.CreateElement(XmlName);

                PropertyInfo[] PropertyInfos = typeof(T).GetProperties();

                foreach (T item in IL)
                {

                    XmlElement ChildNode = XMLdoc.CreateElement(typeof(T).Name);

                    foreach (PropertyInfo pro in PropertyInfos)
                    {

                        if (pro != null)
                        {

                            string KeyName = pro.Name;

                            string KeyValue = string.Empty;

                            if (pro.GetValue(item, null) != null)
                            {

                                KeyValue = pro.GetValue(item, null).ToString();

                            }

                            ChildNode.SetAttribute(KeyName, KeyValue);

                            ChildNode.InnerText = KeyValue;

                        }

                    }

                    Root.AppendChild(ChildNode);

                }

                XMLdoc.AppendChild(Root);

                return XMLdoc;

            }

            catch (Exception ex)
            {

                //LogHelper.LogDebug("List<T>生成XML失败：" + ex.Message);                 

                return null;

            }

        }




    }
}
