using NetSteps.Core.Dto;

namespace NetSteps.Commissions.Disbursement.Common.Models
{
    /// <summary>
    /// disbursement profile for propay
    /// </summary>
    [DTO]
    public interface IPropayDisbursementProfile : IDisbursementProfile
    {
        /// <summary>
        /// the propay account number
        /// </summary>
        string PropayAccountNumber { get; set; }
    }
}
