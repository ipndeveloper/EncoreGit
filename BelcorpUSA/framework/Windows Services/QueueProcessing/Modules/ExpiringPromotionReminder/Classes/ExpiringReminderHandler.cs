// -----------------------------------------------------------------------
// <copyright file="ExpiringReminderHandler.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace NetSteps.QueueProcessing.Modules.ExpiringPromotionReminder.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NetSteps.Data.Entities;
    using NetSteps.Data.Entities.Exceptions;
    using NetSteps.Encore.Core.IoC;
    using NetSteps.QueueProcessing.Modules.ExpiringPromotionReminder.Interfaces;

    /// <summary>
    /// default implementation of IExpiringReminderHandler
    /// </summary>
    [ContainerRegister(typeof(IExpiringReminderHandler), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class ExpiringReminderHandler : IExpiringReminderHandler
    {
        static IList<DomainEventType> DomainEventTypes = null;
        static readonly string _eventName = "ExpiringPromotionNotification";

        public void HandleReminder(PromotionExpirationInfo info)
        {
            CreateEvent(info, _eventName);
        }

        private void CreateEvent(PromotionExpirationInfo info, string eventName)
        {
            try
            {
                short eventTypeID = GetEventTypeID(eventName);
                var domainEventQueueItem = DomainEventQueueItem.SetDomainEventQueueItemValues(eventTypeID,
                                                                                              accountID: info.AccountID,
                                                                                              promotionID: info.PromotionID);
                domainEventQueueItem.Save();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        private static short GetEventTypeID(string eventName)
        {
            if (DomainEventTypes == null)
            {
                DomainEventTypes = DomainEventType.LoadAll();
            }

            var item = DomainEventTypes.Where(x => x.TermName == eventName).Single();
            return item.DomainEventTypeID;
        }


    }
}
