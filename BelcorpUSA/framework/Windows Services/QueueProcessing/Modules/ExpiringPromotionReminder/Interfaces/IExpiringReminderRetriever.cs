using System.Collections.Generic;

namespace NetSteps.QueueProcessing.Modules.ExpiringPromotionReminder.Interfaces
{
    public interface IExpiringReminderRetriever
    {
        IEnumerable<PromotionExpirationInfo> RetrieveAccounts();
    }
}
