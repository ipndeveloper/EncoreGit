
namespace NetSteps.Web.Mvc.Validation
{
	/// <summary>
	/// Validation error IDs as contants for most of the fundamental
	/// validation types.
	/// </summary>
	public enum ValidationErrorID
	{
		None = 0,
		NotDefault = -1,
		NotEmpty = -2,
		MaxLength = -3,
		MaxValue = -4,
		MinValue = -5,
		OutOfRange = -6,
		NoMatch = -7,
	}
}
