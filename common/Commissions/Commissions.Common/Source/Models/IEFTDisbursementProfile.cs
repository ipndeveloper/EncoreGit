
namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// EFT disbursement profile contract.
	/// </summary>
	public interface IEFTDisbursementProfile : IDisbursementProfile
	{
		/// <summary>
		/// Gets or sets the account number.
		/// </summary>
		/// <value>
		/// The account number.
		/// </value>
		string AccountNumber { get; set; }

		/// <summary>
		/// Gets or sets the name of the bank.
		/// </summary>
		/// <value>
		/// The name of the bank.
		/// </value>
		string BankName { get; set; }

        /// <summary>
        /// Gets or sets the address identifier of the bank
        /// </summary>
        int AddressId { get; set; }

		/// <summary>
		/// Gets or sets the routing
		/// </summary>
		string RoutingNumber { get; set; }

		/// <summary>
		/// Gets or sets the bank phone
		/// </summary>
		string BankPhone { get; set; }

		/// <summary>
		/// Gets or sets the bank account type id
		/// </summary>
		int BankAccountTypeId { get; set; }

	}
}
