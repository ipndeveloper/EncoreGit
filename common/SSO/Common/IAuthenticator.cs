using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.SSO.Common
{
	/// <summary>
	/// Encodes and decodes url parameters
	/// </summary>
	public interface IAuthenticator
	{
		/// <summary>
		/// Encodes DecodedText with provided key ?? defaultKey and provided salt
		/// </summary>
		/// <param name="model"></param>
		/// <param name="exceptionHandler">Code executed when an exception is thrown</param>
		void Encode(ISingleSignOnModel model, Action<Exception> exceptionHandler);

		/// <summary>
		/// Decodes EncodedText with provided key ?? defaultKey and provided salt
		/// </summary>
		/// <param name="model"></param>
		/// <param name="exceptionHandler">Code executed when an exception is thrown</param>
		void Decode(ISingleSignOnModel model, Action<Exception> exceptionHandler);

		/// <summary>
		/// Encodes DecodedText with provided key ?? defaultKey and provided salt
		/// </summary>
		/// <param name="model"></param>
		/// <param name="exceptionHandler">Code executed when an exception is thrown</param>
		void Encode(ISingleSignOnTimeStampedModel model, Action<Exception> exceptionHandler);

		/// <summary>
		/// Decodes EncodedText with provided key ?? defaultKey and provided salt
		/// </summary>
		/// <param name="model"></param>
		/// <param name="exceptionHandler">Code executed when an exception is thrown</param>
		void Decode(ISingleSignOnTimeStampedModel model, Action<Exception> exceptionHandler);

	}
}
