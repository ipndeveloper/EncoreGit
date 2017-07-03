
namespace NetSteps.Encore.Core.Representation
{
	/// <summary>
	/// Transforms source item into a it's JSON representation
	/// </summary>
	/// <typeparam name="T">item type T</typeparam>
	public interface IJsonRepresentation<T> : IRepresentation<T, string>
	{
	}
}
