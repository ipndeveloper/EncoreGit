using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace NetSteps.Common.Serialization
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Clones an object by using the <see cref="BinaryFormatter" />.
    /// The object to be cloned must be serializable.
    /// Created: 01-20-2009
    /// </summary>
    public static class ObjectCloner
    {
        public static object Clone(object obj)
        {
            using (MemoryStream buffer = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(buffer, obj);
                buffer.Position = 0;
                object temp = formatter.Deserialize(buffer);
                return temp;
            }
        }

        /// <summary>
        /// http://blog.vascooliveira.com/how-to-duplicate-entity-framework-objects/ - JHE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T CloneViaDataContractSerialization<T>(T obj)
        {
            DataContractSerializer dcSer = new DataContractSerializer(obj.GetType());
            MemoryStream memoryStream = new MemoryStream();
            dcSer.WriteObject(memoryStream, obj);
            memoryStream.Position = 0;
            T newObject = (T)dcSer.ReadObject(memoryStream);
            return newObject;
        }

    }
}