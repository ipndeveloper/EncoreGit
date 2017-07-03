
namespace NetSteps.Common.Interfaces
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Interface to provide A generic replacement for the ListValue table and objects.
	/// Created: 05-04-2010
	/// </summary>
	public interface IListValue
	{
		int ID { get; set; }
		string Title { get; set; }

		void Save();
	}
}
