namespace NetSteps.SSO.Common
{
	using System;

	using NetSteps.Encore.Core.Dto;

	/// <summary>
	/// Model used to encode and decode url parameters
	/// </summary>
	[DTO]
	public interface ISingleSignOnTimeStampedModel
	{
		/// <summary>
		/// Decoded text, text that is going to be encoded
		/// </summary>
		string DecodedText { get; set; }

		/// <summary>
		/// Timestamp for the SSO token
		/// </summary>
		DateTime TimeStamp { get; set; }

		/// <summary>
		/// Encoded text, text that is going to be decoded
		/// </summary>
		string EncodedText { get; set; }
	}
}