using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Runbow.TWS.Common
{
    public class XmlSerializerHelper<T>
    {
        public T Value { get; set; }

        public void CreateTo(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ApplicationException("Missing File Path");
            }

            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(String.Empty, String.Empty);
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            XDocument xDoc = new XDocument();
            using (XmlWriter writer = xDoc.CreateWriter())
            {
                serializer.Serialize(writer, Value, ns);
            }

            xDoc.Save(path);
        }

        public void CreateToNameSpaces(string path,XmlSerializerNamespaces namespaces)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ApplicationException("Missing File Path");
            }

            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

           
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            XDocument xDoc = new XDocument();
            using (XmlWriter writer = xDoc.CreateWriter())
            {
                serializer.Serialize(writer, Value, namespaces);
            }

            xDoc.Save(path);
        }

        public void CreateToData(string path, string data)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ApplicationException("Missing File Path");
            }

            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine(data);
            }
        }

        public void Load(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ApplicationException("Missing File Path");
            }

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(String.Empty, String.Empty);
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            XDocument xDoc = XDocument.Load(path);
            using (XmlReader reader = xDoc.CreateReader())
            {
                Value = (T)serializer.Deserialize(reader);
            }
        }
    }
}