using System;
using System.Collections.Generic;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
    public partial class Newsletter
    {
        //public List<string> GetEmailTemplateTokenNames()
        //{
        //    return BusinessLogic.GetEmailTemplateTokenNames();
        //}

        //public static List<Newsletter> LoadHistoricalNewslettersForConsultant(Account account)
        //{
        //    try
        //    {
        //        return BusinessLogic.LoadHistoricalNewslettersForConsultant(Repository, account);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
        //    }
        //}

        public static List<Newsletter> LoadAll()
        {
            try
            {
                return BusinessLogic.LoadAll(Repository);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public bool IsSent { get { return DateSentUTC != null; } }

        //public string Preview()
        //{
        //    return BusinessLogic.GeneratePreviewHtml(this);
        //}

        //public string ConsultantPreview(Account consultantAccount)
        //{
        //    return BusinessLogic.GeneratePreviewHtml(this, consultantAccount);
        //}

        /*
        public List<EmailTemplateToken> GetFakeConsultantTokens()
        {
            return BusinessLogic.GetFakeConsultantTokens();
        }

        public List<EmailTemplateToken> GetFakeContactTokens()
        {
            return BusinessLogic.GetFakeContactTokens();
        }
        */

        /*
        public static Newsletter Load(int newsletterId)
        {
            try
            {
                return BusinessLogic.Load(Repository, newsletterId);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        */

        public List<FakeNewsletterStatistic> FakeNewsletterStatistics
        {
            get
            {
                Account joesmith = new Account() { FirstName = "Joe", LastName = "Smith", EmailAddress = "joesmith@hotmail.com" };
                Account pamtrudy = new Account() { FirstName = "Pam", LastName = "Trudy", EmailAddress = "pamtrudy@yahoo.com" };
                Account toddlillywhite = new Account() { FirstName = "Todd", LastName = "Lillywhite", EmailAddress = "toddlillywhite@aol.com" };
                Account haroldcrick = new Account() { FirstName = "Harold", LastName = "Crick", EmailAddress = "haroldcrick@hotmail.com" };
                Account sarahlee = new Account() { FirstName = "Sarah", LastName = "Lee", EmailAddress = "sarahlee@test.com" };

                return new List<FakeNewsletterStatistic>()
                           {
                               new FakeNewsletterStatistic() { Type = NewsletterStatisticType.Open, TimestampUTC = this.ScheduledSendDateUTC.AddDays(2).AddMinutes(67), Account = joesmith },
                               new FakeNewsletterStatistic() { Type = NewsletterStatisticType.Open, TimestampUTC = this.ScheduledSendDateUTC.AddDays(5).AddMinutes(412), Account = pamtrudy },
                               new FakeNewsletterStatistic() { Type = NewsletterStatisticType.Open, TimestampUTC = this.ScheduledSendDateUTC.AddDays(7).AddMinutes(370), Account = toddlillywhite },
                               new FakeNewsletterStatistic() { Type = NewsletterStatisticType.Bounce, TimestampUTC = this.ScheduledSendDateUTC.AddDays(3).AddMinutes(185), Account = sarahlee },
                               new FakeNewsletterStatistic() { Type = NewsletterStatisticType.Bounce, TimestampUTC = this.ScheduledSendDateUTC.AddDays(1).AddMinutes(12), Account = haroldcrick },
                               new FakeNewsletterStatistic() { Type = NewsletterStatisticType.LinkClick, TimestampUTC = this.ScheduledSendDateUTC.AddDays(4).AddMinutes(84), Account = pamtrudy, Attributes = new List<FakeNewsletterStatisticAttribute>() { new FakeNewsletterStatisticAttribute() { Type = "LinkName", Value = "Buy" }  } },
                               new FakeNewsletterStatistic() { Type = NewsletterStatisticType.LinkClick, TimestampUTC = this.ScheduledSendDateUTC.AddDays(4).AddMinutes(84), Account = haroldcrick, Attributes = new List<FakeNewsletterStatisticAttribute>() { new FakeNewsletterStatisticAttribute() { Type = "LinkName", Value = "Buy" } } },
                               new FakeNewsletterStatistic() { Type = NewsletterStatisticType.LinkClick, TimestampUTC = this.ScheduledSendDateUTC.AddDays(4).AddMinutes(84), Account = toddlillywhite, Attributes = new List<FakeNewsletterStatisticAttribute>() { new FakeNewsletterStatisticAttribute() { Type = "LinkName", Value = "Join" } } }
                           };
            }
        }
    }

    public enum NewsletterStatisticType { Sent, Open, Bounce, LinkClick };
    public class FakeNewsletterStatistic
    {
        public Account Account { get; set; }
        public NewsletterStatisticType Type { get; set; }
        public DateTime TimestampUTC { get; set; }
        public List<FakeNewsletterStatisticAttribute> Attributes { get; set; }
    }

    public class FakeNewsletterStatisticAttribute
    {
        public string Type { get; set; }
        public string Value { get; set; }

        public FakeNewsletterStatisticAttribute()
        {
            Type = "";
            Value = "";
        }
    }
}
