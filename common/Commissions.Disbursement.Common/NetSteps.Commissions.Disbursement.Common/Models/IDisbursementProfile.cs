
namespace NetSteps.Commissions.Disbursement.Common.Models
{
    /// <summary>
    /// base interface for disbursement profiles
    /// not intended to be directly implemented
    /// </summary>
    public interface IDisbursementProfile
    {
        /// <summary>
        /// the disbursement profile identifier
        /// </summary>
        int DisbursementProfileId { get; set; }

        /// <summary>
        /// the account id associated with the profile
        /// </summary>
        int AccountId { get; set; }

        /// <summary>
        /// the method of disbursement
        /// </summary>
        int DisbursementMethodId { get; set; }

        /// <summary>
        /// the percentage disbursed for this profile
        /// </summary>
        decimal Percentage { get; set; }

        /// <summary>
        /// is the profile enabled
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// the user id
        /// </summary>
        int UserId { get; set; }

        /// <summary>
        /// has the enrollment form been received
        /// </summary>
        bool EnrollmentFormReceived { get; set; }
    }
}
