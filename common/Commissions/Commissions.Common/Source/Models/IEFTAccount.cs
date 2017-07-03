namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// EFT Account model
	/// </summary>
	public interface IEFTAccount
	{
		/// <summary>
		/// Gets or sets the disbursement profile identifier.
		/// </summary>
		/// <value>
		/// The disbursement profile identifier.
		/// </value>
		int DisbursementProfileId { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		string Name { get; set; }

		/// <summary>
		/// Gets or sets the routing number.
		/// </summary>
		/// <value>
		/// The routing number.
		/// </value>
		string RoutingNumber { get; set; }

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
		/// Gets or sets the bank phone.
		/// </summary>
		/// <value>
		/// The bank phone.
		/// </value>
		string BankPhone { get; set; }

		/// <summary>
		/// Gets or sets the bank address1.
		/// </summary>
		/// <value>
		/// The bank address1.
		/// </value>
		string BankAddress1 { get; set; }

		/// <summary>
		/// Gets or sets the bank address2.
		/// </summary>
		/// <value>
		/// The bank address2.
		/// </value>
		string BankAddress2 { get; set; }

		/// <summary>
		/// Gets or sets the bank address3.
		/// </summary>
		/// <value>
		/// The bank address3.
		/// </value>
		string BankAddress3 { get; set; }

		/// <summary>
		/// Gets or sets the bank city.
		/// </summary>
		/// <value>
		/// The bank city.
		/// </value>
		string BankCity { get; set; }

		/// <summary>
		/// Gets or sets the state of the bank.
		/// </summary>
		/// <value>
		/// The state of the bank.
		/// </value>
		string BankState { get; set; }

		/// <summary>
		/// Gets or sets the bank zip.
		/// </summary>
		/// <value>
		/// The bank zip.
		/// </value>
		string BankZip { get; set; }
		/// <summary>
		/// Gets or sets the bank country.
		/// </summary>
		/// <value>
		/// The bank country.
		/// </value>
		string BankCountry { get; set; }
		/// <summary>
		/// Gets or sets the bank county.
		/// </summary>
		/// <value>
		/// The bank county.
		/// </value>
		string BankCounty { get; set; }
		/// <summary>
		/// Gets or sets the type of the account.
		/// </summary>
		/// <value>
		/// The type of the account.
		/// </value>
		BankAccountKind AccountType { get; set; }

		/// <summary>
		/// Gets or sets the percent to deposit.
		/// </summary>
		/// <value>
		/// The percent to deposit.
		/// </value>
		int PercentToDeposit { get; set; }

        /// <summary>
        /// Gets of sets whether this account is enabled
        /// </summary>
        bool IsEnabled { get; set; }
	}
}
