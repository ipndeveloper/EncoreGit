// -----------------------------------------------------------------------
// <copyright file="ExpiringPromotionReminderConfiguration.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace NetSteps.QueueProcessing.Modules.ExpiringPromotionReminder.Configuration
{
    using System.Configuration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ExpiringPromotionReminderConfiguration : ConfigurationElement
    {
        const string PropertyName_DaysBeforeExpiration = "daysBeforeExpiration";
        [ConfigurationProperty(PropertyName_DaysBeforeExpiration, IsKey = true, IsRequired = true)]
        public int DaysBeforeExpiration
        {
            get { return (int)this[PropertyName_DaysBeforeExpiration]; }
            set { this[PropertyName_DaysBeforeExpiration] = value; }
        }
    }
}
