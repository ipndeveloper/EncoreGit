
namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Profile of a check disbursement.
	/// </summary>
	public interface ICheckDisbursementProfile : IDisbursementProfile
	{
		/// <summary>
		/// Gets the name on the check.
		/// </summary>
		/// <value>
		/// The name on check.
		/// </value>
		string NameOnCheck { get; set; }

		/// <summary>
		/// Gets the address identifier.
		/// </summary>
		/// <value>
		/// The address identifier.
		/// </value>
		int AddressId { get; set; }
	}
}
