// -----------------------------------------------------------------------
// <copyright file="ExpiringPromotionReminderProcessor.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace NetSteps.QueueProcessing.Modules.ExpiringPromotionReminder
{
    using NetSteps.Encore.Core.IoC;
    using NetSteps.QueueProcessing.Modules.ExpiringPromotionReminder.Interfaces;
    using NetSteps.QueueProcessing.Modules.ModuleBase;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ExpiringPromotionReminderProcessor : QueueProcessor<PromotionExpirationInfo>
    {
        public static readonly string CProcessorName = "ExpiringPromotionReminderProcessor";

        public ExpiringPromotionReminderProcessor()
        {
            this.Name = CProcessorName;
        }

        public override void CreateQueueItems(int maxNumberToPoll)
        {
            IExpiringReminderRetriever retriever = Create.New<IExpiringReminderRetriever>();
            var accountList = retriever.RetrieveAccounts();
            foreach (var item in accountList)
            {
                this.EnqueueItem(item);
            }
        }

        public override void ProcessQueueItem(PromotionExpirationInfo item)
        {
            IExpiringReminderHandler reminderHandler = Create.New<IExpiringReminderHandler>();
            reminderHandler.HandleReminder(item);
        }
    }

    public class PromotionExpirationInfo
    {
        public int AccountID { get; set; }
        public int PromotionID { get; set; }
    }
}
