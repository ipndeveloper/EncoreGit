namespace NetSteps.Agreements.Common
{
	using System.Collections.Generic;

	using NetSteps.Agreements.Common.Models;

	/// <summary>
	 /// Provides agreements and agreement versions, as well as their respective save functions
	 /// </summary>
	 public interface IAgreementsService
	 {
		  /// <summary>
		  /// Gets all agreements for the specified account, accountKind, and agreementKind
		  /// </summary>
		  /// <param name="accountId">
		  /// The account Id.
		  /// </param>
		  /// <param name="accountKindId">
		  /// The account Kind Id.
		  /// </param>
		  /// <param name="agreementKind">
		  /// The agreement Kind.
		  /// </param>
		  /// <returns>
		  /// The list of specific agreements.
		  /// </returns>
		  IEnumerable<ISpecificAgreement> GetAgreements(int accountId, int accountKindId, IAgreementKind agreementKind = null);

		  /// <summary>
		  /// Saves acceptance log entries for agreements with HasAgreed set to true
		  /// </summary>
		  /// <param name="agreements">
		  /// The agreements.
		  /// </param>
		  void SaveAcceptedAgreements(IEnumerable<ISpecificAgreement> agreements);

		  /// <summary>
		  /// Gets the agreement version info for the specified agreementKind
		  /// </summary>
		  /// <param name="agreementKind">
		  /// The agreement Kind.
		  /// </param>
		  /// <returns>
		  /// The list of agreement versions.
		  /// </returns>
		  IEnumerable<IAgreementVersion> GetAgreementVersions(IAgreementKind agreementKind = null);

		  /// <summary>
		  /// Gets the agreement version info for the specified accountKind and agreementKind
		  /// </summary>
		  /// <param name="accountKindId">
		  /// The account Kind Id.
		  /// </param>
		  /// <param name="agreementKind">
		  /// The agreement Kind.
		  /// </param>
		  /// <returns>
		  /// The list of agreement versions.
		  /// </returns>
		  IEnumerable<IAgreementVersion> GetAgreementVersions(int accountKindId, IAgreementKind agreementKind = null);

		  /// <summary>
		  /// Saves a new agreement version
		  /// </summary>
		  /// <param name="agreementVersion">
		  /// The agreement Version.
		  /// </param>
		  void SaveNewAgreementVersion(IAgreementVersion agreementVersion);
	 }
}
