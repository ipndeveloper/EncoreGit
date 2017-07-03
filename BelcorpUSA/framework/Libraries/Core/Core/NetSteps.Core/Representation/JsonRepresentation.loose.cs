using Newtonsoft.Json;

namespace NetSteps.Encore.Core.Representation
{	
	/// <summary>
	/// Transforms an items into a JSON representation, ignoring missing members.
	/// </summary>
	/// <typeparam name="T">item type T</typeparam>
	public class JsonTransformLoose<T> : JsonRepresentation<T>
		where T : class
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public JsonTransformLoose() : base(StaticJsonSettings.Loose) { }
	}
}
