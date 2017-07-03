using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace NetSteps.Common.Serialization
{
    public static class XMLSerializationHelper
    {
        public static string XmlSerialize(object obj)
        {
            if (null != obj)
            {
                // Assuming obj is an instance of an object
                XmlSerializer ser = new XmlSerializer(obj.GetType());
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                System.IO.StringWriter writer = new System.IO.StringWriter(sb);
                ser.Serialize(writer, obj);
                return sb.ToString();
            }
            return string.Empty;
        }

        public static object XmlDeserialize(Type objType, string xmlDoc)
        {
            if (xmlDoc != null && objType != null)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlDoc);
                //Assuming doc is an XML document containing a serialized object and objType is a System.Type set to the type of the object.
                XmlNodeReader reader = new XmlNodeReader(doc.DocumentElement);
                XmlSerializer ser = new XmlSerializer(objType);
                return ser.Deserialize(reader);
            }
            return null;
        }
    }
}
