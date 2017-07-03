// -----------------------------------------------------------------------
// <copyright file="ExpiringReminderRetriever.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace NetSteps.QueueProcessing.Modules.ExpiringPromotionReminder.Classes
{
    using System.Collections.Generic;
    using NetSteps.Encore.Core.IoC;
    using NetSteps.QueueProcessing.Modules.ExpiringPromotionReminder.Interfaces;

    /// <summary>
    /// default implementation of IExpiringReminderRetriever
    /// Get a list of accounts that have promotions expiring shortly
    /// </summary>
    [ContainerRegister(typeof(IExpiringReminderRetriever), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class ExpiringReminderRetriever : IExpiringReminderRetriever
    {
        public IEnumerable<PromotionExpirationInfo> RetrieveAccounts()
        {
            return new List<PromotionExpirationInfo>();
        }
    }
}
