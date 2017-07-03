
namespace NetSteps.Data.Entities.Business.Interfaces
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Interface for Entities with a IsEditable property.
	/// Created: 10-12-2010
	/// </summary>
	public interface IEditable
	{
		bool Editable { get; set; }
	}
}
