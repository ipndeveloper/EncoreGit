// -----------------------------------------------------------------------
// <copyright file="IExpiringReminderHandler.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace NetSteps.QueueProcessing.Modules.ExpiringPromotionReminder.Interfaces
{

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IExpiringReminderHandler
    {
        void HandleReminder(PromotionExpirationInfo account);
    }
}
