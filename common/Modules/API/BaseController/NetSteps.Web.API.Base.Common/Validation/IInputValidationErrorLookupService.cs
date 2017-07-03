using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Web.API.Base.Common
{
	/// <summary>
	/// Interface for looking up input error messages.
	/// </summary>
	public interface IInputValidationErrorLookupService
	{
		/// <summary>
		/// Looks up an error by its ID.
		/// </summary>
		/// <param name="errorID">the error's identity</param>
		/// <param name="ietfLanguageTag">an IETF language tag; ie. en-us, etc.</param>
		/// <param name="inputItem">name of the input item that is in error</param>
		/// <param name="formatArgs">arguments used to format the error message</param>
		/// <returns>An input error instance.</returns>
		IInputError LookupErrorByID(int errorID, string ietfLanguageTag, string inputItem, params object[] formatArgs);
	}
}
