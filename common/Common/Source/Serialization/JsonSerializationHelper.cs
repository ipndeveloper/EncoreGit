using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;

namespace NetSteps.Common.Serialization
{
	public static class JsonSerializationHelper
	{
		public static T Deserialize<T>(string input, IEnumerable<Type> knownTypes = null)
		{
			return Deserialize<T>(Encoding.Default.GetBytes(input), knownTypes);
		}
		public static T Deserialize<T>(byte[] bytes, IEnumerable<Type> knownTypes = null)
		{
			if (bytes == null) return default(T);

			Stream stream = new MemoryStream(bytes);
			return Deserialize<T>(stream, knownTypes);
		}
		public static T Deserialize<T>(Stream stream, IEnumerable<Type> knownTypes = null)
		{
			if (stream == null) return default(T);

			DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T), knownTypes);
			T result = (T)serializer.ReadObject(stream);
			return result;
		}

		public static string Serialize(object obj, IEnumerable<Type> knownTypes = null)
		{
			if (obj == null) return null;

			string result;
			using (MemoryStream stream = new MemoryStream())
			{
				Serialize(obj, stream, knownTypes);
				result = Encoding.Default.GetString(stream.ToArray(), 0, (int)stream.Length);
			}
			return result;
		}
		public static void Serialize(object obj, Stream stream, IEnumerable<Type> knownTypes = null)
		{
			if (obj == null) return;

			DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType(), knownTypes);
			serializer.WriteObject(stream, obj);
		}
	}
}
