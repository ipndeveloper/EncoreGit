using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace NetSteps.Testing.Core
{
	public static partial class TestHelpers
	{
        public static class Serialization
        {
            public static class DataContractBinary
            {
                public static Stream Serialize(object source)
                {
                    var serializer = new DataContractSerializer(source.GetType());
                    var stream = new MemoryStream();
                    var binaryWriter = XmlDictionaryWriter.CreateBinaryWriter(stream);
                    serializer.WriteObject(binaryWriter, source);
                    binaryWriter.Flush();
                    return stream;
                }

                public static T Deserialize<T>(Stream stream)
                {
                    stream.Position = 0;
                    var serializer = new DataContractSerializer(typeof(T));
                    var binaryReader = XmlDictionaryReader.CreateBinaryReader(stream, XmlDictionaryReaderQuotas.Max);
                    return (T)serializer.ReadObject(binaryReader);
                }

                public static T Clone<T>(T source)
                {
                    return Deserialize<T>(Serialize(source));
                }
            }

            public static class DataContractXml
            {
                public static Stream Serialize(object source)
                {
                    var serializer = new DataContractSerializer(source.GetType());
                    var stream = new MemoryStream();
                    serializer.WriteObject(stream, source);
                    return stream;
                }

                public static T Deserialize<T>(Stream stream)
                {
                    stream.Position = 0;
                    var serializer = new DataContractSerializer(typeof(T));
                    return (T)serializer.ReadObject(stream);
                }

                public static T Clone<T>(T source)
                {
                    return Deserialize<T>(Serialize(source));
                }
            }

            public static class NetDataContractBinary
            {
                public static Stream Serialize(object source)
                {
                    var serializer = new NetDataContractSerializer();
                    var stream = new MemoryStream();
                    var binaryWriter = XmlDictionaryWriter.CreateBinaryWriter(stream);
                    serializer.WriteObject(binaryWriter, source);
                    binaryWriter.Flush();
                    return stream;
                }

                public static T Deserialize<T>(Stream stream)
                {
                    stream.Position = 0;
                    var serializer = new NetDataContractSerializer();
                    var binaryReader = XmlDictionaryReader.CreateBinaryReader(stream, XmlDictionaryReaderQuotas.Max);
                    return (T)serializer.ReadObject(binaryReader);
                }

                public static T Clone<T>(T source)
                {
                    return Deserialize<T>(Serialize(source));
                }
            }

            public static class NetDataContractXml
            {
                public static Stream Serialize(object source)
                {
                    var serializer = new NetDataContractSerializer();
                    var stream = new MemoryStream();
                    serializer.WriteObject(stream, source);
                    return stream;
                }

                public static T Deserialize<T>(Stream stream)
                {
                    stream.Position = 0;
                    var serializer = new NetDataContractSerializer();
                    return (T)serializer.ReadObject(stream);
                }

                public static T Clone<T>(T source)
                {
                    return Deserialize<T>(Serialize(source));
                }
            }

            public static class Binary
            {
                public static Stream Serialize(object source)
                {
                    var formatter = new BinaryFormatter();
                    var stream = new MemoryStream();
                    formatter.Serialize(stream, source);
                    return stream;
                }

                public static T Deserialize<T>(Stream stream)
                {
                    stream.Position = 0;
                    var formatter = new BinaryFormatter();
                    return (T)formatter.Deserialize(stream);
                }

                public static T Clone<T>(T source)
                {
                    return Deserialize<T>(Serialize(source));
                }
            }

            public static class Xml
            {
                public static string Serialize(object source)
                {
                    var xmlSerializer = new XmlSerializer(source.GetType());
                    using (var stringWriter = new StringWriter())
                    {
                        xmlSerializer.Serialize(stringWriter, source);
                        return stringWriter.ToString();
                    }
                }

                public static T Deserialize<T>(string xml)
                {
                    var xmlSerializer = new XmlSerializer(typeof(T));
                    using (var stringReader = new StringReader(xml))
                    {
                        return (T)xmlSerializer.Deserialize(stringReader);
                    }
                }

                public static T Clone<T>(T source)
                {
                    return Deserialize<T>(Serialize(source));
                }
            }
        }
	}
}
