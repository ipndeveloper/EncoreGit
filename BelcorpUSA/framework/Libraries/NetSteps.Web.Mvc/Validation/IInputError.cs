using NetSteps.Encore.Core.Dto;

namespace NetSteps.Web.Mvc.Validation
{
	/// <summary>
	/// An input error.
	/// </summary>
	[DTO]
	public interface IInputError
	{
		/// <summary>
		/// The error's identity.
		/// </summary>
		int ErrorID { get; set; }
		/// <summary>
		/// Name of the user input item that failed validation.
		/// </summary>
		string InputItem { get; set; }
		/// <summary>
		/// An error message suitable for display to end users.
		/// </summary>
		string Message { get; set; }
	}
}
