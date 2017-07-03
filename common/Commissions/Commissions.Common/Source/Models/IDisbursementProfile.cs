
namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Disbursement profile contract.
	/// </summary>
	public interface IDisbursementProfile
	{
		/// <summary>
		/// Gets or sets the disbursement profile identifier.
		/// </summary>
		/// <value>
		/// The disbursement profile identifier.
		/// </value>
		int DisbursementProfileId { get; set; }

		/// <summary>
		/// Gets the disbursement method for this disbursement.  This is formerly DisbursementType.
		/// </summary>
		/// <value>
		/// The kind of the disbursement.
		/// </value>
		DisbursementMethodKind DisbursementMethod { get; }

		/// <summary>
		/// Gets a value indicating whether this profile [is enabled].
		/// </summary>
		/// <value>
		///   <c>true</c> if [is enabled]; otherwise, <c>false</c>.
		/// </value>
		bool IsEnabled { get; set; }

		/// <summary>
		/// Gets or sets the account id
		/// </summary>
		int	AccountId { get; set; }

		/// <summary>
		/// Gets or sets the percentage
		/// </summary>
		decimal Percentage { get; set; }

		/// <summary>
		/// Gets or sets the currency id
		/// </summary>
		int CurrencyId { get; set; }

		/// <summary>
		/// Gets or sets the name on account
		/// </summary>
		string NameOnAccount { get; set; }

		/// <summary>
		/// Gets or sets if the enrollment form [has been received]
		/// </summary>
		/// <value>
		/// <c>true</c> if [has been received]; otherwise, <c>false</c>
		/// </value>
		bool EnrollmentFormReceived { get; set; }

	}
}
