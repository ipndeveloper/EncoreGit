
namespace NetSteps.Foundation.Common
{
	/// <summary>
	/// Represents the execution context of the application
	/// </summary>
	public interface IExecutionContext
	{
		/// <summary>
		/// Get or set the current user
		/// </summary>
		object CurrentUser { get; set; }
		/// <summary>
		/// Get or set the current culture information string (i.e. en-us, de-de, etc.)
		/// </summary>
		string CurrentCultureInfo { get; set; }
	}
}
