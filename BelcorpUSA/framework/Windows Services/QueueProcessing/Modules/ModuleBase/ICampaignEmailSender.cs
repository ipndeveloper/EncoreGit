// -----------------------------------------------------------------------
// <copyright file="ICampaignEmailSender.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace NetSteps.QueueProcessing.Modules.ModuleBase
{

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICampaignEmailSender
    {
        bool SendEmail();

        /// <summary>
        /// Method to send the email for more than one email id
        /// currently it is used to send out the emails for party guests
        /// </summary>
        /// <returns></returns>
        bool SendEmails();

    }
}
