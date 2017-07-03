namespace NetSteps.QueueProcessing.Modules.AutoshipReminder
{
    using System;
    using System.Collections.Generic;

    using NetSteps.Data.Entities;
    using NetSteps.Data.Entities.Exceptions;
    using NetSteps.QueueProcessing.Modules.ModuleBase;

    public class AutoshipReminderQueueProcessor : QueueProcessor<IEnumerable<AutoshipOrder>>
    {
        public static readonly string CProcessorName = "AutoshipReminderQueueProcessor";

        private int _isZero;
        private const int ZERO = 0;
        private readonly Object _locker = new Object();

        public AutoshipReminderQueueProcessor()
        {
            this.Name = CProcessorName;
        }

        public override void CreateQueueItems(int maxNumberToPoll)
        {
            try
            {
                lock (this._locker)
                {
                    if (this._isZero == ZERO)
                    {
                        this._isZero += 1;

                        this.Logger.Info("AutoshipReminderQueueProcessor - CreateQueueItems");

                        var autoshipReminders = AutoshipOrder.QueueAutoshipReminders();

                        if (autoshipReminders.Count > 0)
                            this.EnqueueItem(autoshipReminders);
                        else
                            this._isZero = ZERO;

                        this.Logger.Info("AutoshipReminderQueueProcessor - Enqueued {0} Items", autoshipReminders.Count);
                    }
                }

            }
            catch (Exception ex)
            {
                this._isZero = ZERO;
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }

        }

        public override void ProcessQueueItem(IEnumerable<AutoshipOrder> items)
        {
            AutoshipOrder.SendAutoshipReminderAutoResponder(items);
            this._isZero = ZERO;
        }
    }
}
