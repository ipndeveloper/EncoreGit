namespace NetSteps.Data.Common.Entities
{
	/// <summary>
	/// The AccountStatusChangeReason interface.
	/// </summary>
	public interface IAccountStatusChangeReason
	{
		/// <summary>
		/// Gets or sets the account status change reason id.
		/// </summary>
		short AccountStatusChangeReasonID { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		string Name { get; set; }
	}
}
