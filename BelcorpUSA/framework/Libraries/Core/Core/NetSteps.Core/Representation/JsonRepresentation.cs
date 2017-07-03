using System;
using System.Diagnostics.Contracts;

using Newtonsoft.Json;

namespace NetSteps.Encore.Core.Representation
{
	internal static class StaticJsonSettings
	{
		internal readonly static JsonSerializerSettings Strict = new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Error, ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
		internal readonly static JsonSerializerSettings Loose = new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Ignore, ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
	}

	/// <summary>
	/// Transforms source item into a it's JSON representation
	/// </summary>
	/// <typeparam name="T">item type T</typeparam>
	public class JsonRepresentation<T> : RepresentationBase<T, string>, IJsonRepresentation<T>
	{
		readonly JsonSerializerSettings _settings;
		
		/// <summary>
		/// Creates a new instance with serializer settings given.
		/// </summary>
		/// <param name="settings">serializer settings</param>
		public JsonRepresentation(JsonSerializerSettings settings)
		{
			Contract.Requires<ArgumentNullException>(settings != null);
			_settings = settings;
		}

		/// <summary>
		/// Produces a JSON representation from an item.
		/// </summary>
		/// <param name="item">the item</param>
		/// <returns>JSON representation of the item</returns>
		public override string TransformItem(T item)
		{
			return JsonConvert.SerializeObject(item, Formatting.None, _settings);
		}

		/// <summary>
		/// Restores an item from it's JSON representation
		/// </summary>
		/// <param name="json">the item's JSON representation</param>
		/// <returns>the restored item</returns>
		public override T RestoreItem(string json)
		{
			return JsonConvert.DeserializeObject<T>(json, _settings);
		}
	}
}
