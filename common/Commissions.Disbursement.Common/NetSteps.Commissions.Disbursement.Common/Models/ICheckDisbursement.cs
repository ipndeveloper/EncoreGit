using NetSteps.Core.Dto;

namespace NetSteps.Commissions.Disbursement.Common.Models
{
    /// <summary>
    /// disbursement profile for checks
    /// </summary>
    [DTO]
    public interface ICheckDisbursement : IDisbursementProfile
    {
        /// <summary>
        /// the name for the check
        /// </summary>
        string NameOnAccount { get; set; }

        /// <summary>
        /// address identifier
        /// </summary>
        int AddressId { get; set; }
    }
}
