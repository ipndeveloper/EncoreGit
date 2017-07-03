using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NetSteps.Common.Serialization
{
	public class BinarySerializationHelper
	{
		public static byte[] Serialize(object obj)
		{
			if (obj != null)
			{
				BinaryFormatter serializer = new BinaryFormatter();
				MemoryStream stream = new MemoryStream();
				serializer.Serialize(stream, obj);
				stream.Position = 0;
				var bytes = new byte[stream.Length];
				stream.Read(bytes, 0, bytes.Length);
				stream.Close();
				return bytes;
			}
			return new byte[0];
		}

		public static object Deserialize(byte[] bytes)
		{
			if (bytes != null && bytes.Length > 0)
			{
				BinaryFormatter serializer = new BinaryFormatter();
				MemoryStream stream = new MemoryStream();
				stream.Write(bytes, 0, bytes.Length);
				stream.Position = 0;
				return serializer.Deserialize(stream);
			}
			return null;
		}

		public static T Deserialize<T>(byte[] bytes) where T : class
		{
			var obj = Deserialize(bytes);
			return obj == null ? (T)null : (T)obj;
		}
	}
}
