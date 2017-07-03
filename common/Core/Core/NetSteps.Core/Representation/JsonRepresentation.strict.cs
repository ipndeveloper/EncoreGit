using Newtonsoft.Json;

namespace NetSteps.Encore.Core.Representation
{
	/// <summary>
	/// Transforms an items into a JSON representation, erroring on missing members.
	/// </summary>
	/// <typeparam name="T">item type T</typeparam>
	public class JsonTransformStrict<T> : JsonRepresentation<T>
		where T : class
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public JsonTransformStrict() : base(StaticJsonSettings.Strict) { }
	}
}
