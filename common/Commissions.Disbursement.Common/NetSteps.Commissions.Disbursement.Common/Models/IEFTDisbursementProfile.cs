using NetSteps.Core.Dto;

namespace NetSteps.Commissions.Disbursement.Common.Models
{
    /// <summary>
    /// disbursement profile for eft
    /// </summary>
    [DTO]
    public interface IEFTDisbursementProfile : IDisbursementProfile
    {
        /// <summary>
        /// the bank account number
        /// </summary>
        string BankAccountNumber { get; set; }

        /// <summary>
        /// the bank routing number
        /// </summary>
        string BankRoutingNumber { get; set; }

        /// <summary>
        /// the bank name
        /// </summary>
        string BankName { get; set; }

        /// <summary>
        /// the bank phone number
        /// </summary>
        string BankPhone { get; set; }

        /// <summary>
        /// the kind of bank account
        /// </summary>
        int BankAccountKindId { get; set; }
    }
}
