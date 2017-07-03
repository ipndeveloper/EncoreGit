namespace NetSteps.Agreements.Common
{
	using System;

	using NetSteps.Encore.Core.Dto;

	/// <summary>
	/// Information required to display and accept an agreement
	/// </summary>
	[DTO]
	public interface IAgreement
	{
		/// <summary>
		/// Gets the account number for which to find agreements that have not been accepted
		/// </summary>
		int AccountId { get; set; }

		/// <summary>
		/// Gets the account kind for the accountId specified
		/// </summary>
		int AccountKindId { get; set; }

		/// <summary>
		/// Gets the version info for this agreement
		/// </summary>
		IAgreementVersion AgreementVersion { get; set; }

		/// <summary>
		/// Gets the date this version of the agreement was accepted by the user
		/// </summary>
		DateTime? DateAcceptedUtc { get; set; }

		/// <summary>
		/// Gets whether the user has agreed
		/// </summary>
		bool HasAgreed { get; set; }
	}
}
