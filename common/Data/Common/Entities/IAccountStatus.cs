namespace NetSteps.Data.Common.Entities
{
	using NetSteps.Data.Common.Locale;

	/// <summary>
	/// The AccountStatus interface.
	/// </summary>
	public interface IAccountStatus : ITermName
	{
		/// <summary>
		/// Gets or sets the account status id.
		/// </summary>
		short AccountStatusID { get; set; }
	}
}
