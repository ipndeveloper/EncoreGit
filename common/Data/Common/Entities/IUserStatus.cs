namespace NetSteps.Data.Common.Entities
{
	using NetSteps.Data.Common.Locale;

	/// <summary>
	/// The UserStatus interface.
	/// </summary>
	public interface IUserStatus : ITermName
	{
		/// <summary>
		/// Gets or sets the user status id.
		/// </summary>
		short UserStatusID { get; set; }
	}
}
