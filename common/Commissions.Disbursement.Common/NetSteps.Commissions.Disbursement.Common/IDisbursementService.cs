using NetSteps.Commissions.Disbursement.Common.Models;
using System.Collections.Generic;

namespace NetSteps.Commissions.Disbursement.Common
{
    public interface IDisbursementService
    {
        /// <summary>
        /// Gets the maximum disbursement profiles.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns></returns>
        int GetMaximumDisbursementProfiles(IDisbursementMethod method);

        /// <summary>
        /// Gets the disbursement profiles by account identifier.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns></returns>
        IEnumerable<IDisbursementProfile> GetDisbursementProfilesByAccountId(int accountId);

        /// <summary>
        /// Gets the type of the disbursement profiles by account and.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="disbursementMethod">The disbursement method.</param>
        /// <returns></returns>
        IEnumerable<IDisbursementProfile> GetDisbursementProfilesByAccountAndDisbursementMethod(int accountId, IDisbursementMethod disbursementMethod);

        /// <summary>
        /// Gets the profile count by account and disbursement method.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="disbursementMethod">The disbursement method.</param>
        /// <returns></returns>
        int GetDisbursementProfileCountByAccountAndDisbursementMethod(int accountId, IDisbursementMethod disbursementMethod);

        /// <summary>
        /// Gets the disbursement method code.
        /// </summary>
        /// <param name="disbursementMethodId">The disbursement method identifier.</param>
        /// <returns></returns>
        string GetDisbursementMethodCode(int disbursementMethodId);

        /// <summary>
        /// Saves the disbursement profile.
        /// </summary>
        /// <param name="profile">The profile identifier.</param>
        /// <returns></returns>
        bool SaveDisbursementProfile(IDisbursementProfile profile);

    }
}
