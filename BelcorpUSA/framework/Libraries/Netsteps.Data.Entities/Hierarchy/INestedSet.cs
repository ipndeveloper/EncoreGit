
namespace NetSteps.Data.Entities.Hierarchy
{
	public interface INestedSet
	{
		int TreeLevel { get; set; }
		int LeftAnchor { get; set; }
		int RightAnchor { get; set; }
	}
}
