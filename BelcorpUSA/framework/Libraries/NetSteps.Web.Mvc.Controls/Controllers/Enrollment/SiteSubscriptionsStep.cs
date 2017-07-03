using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Enrollment.Common.Models.Config;
using NetSteps.Enrollment.Common.Models.Context;
using NetSteps.Web.Mvc.Controls.Infrastructure;

namespace NetSteps.Web.Mvc.Controls.Controllers.Enrollment
{
    public class SiteSubscriptionsStep : BaseEnrollmentStep
    {
        private static int _baseSiteId;
        private static IEnumerable<AutoshipSchedule> _autoshipSchedules;
        protected readonly static object _lock = new object();
        private static IEnumerable<int> _autoshipScheduleIDs = new List<int>();

        static SiteSubscriptionsStep()
        {
            LoadConfigValues();

            EnrollmentConfigHandler.ConfigUpdated -= new EventHandler(EnrollmentConfigHandler_ConfigUpdated);
            EnrollmentConfigHandler.ConfigUpdated += new EventHandler(EnrollmentConfigHandler_ConfigUpdated);
        }
        private static void EnrollmentConfigHandler_ConfigUpdated(object sender, EventArgs e)
        {
            LoadConfigValues();
        }
        private static void LoadConfigValues()
        {
            lock (_lock)
            {

                IEnumerable<dynamic> siteSubscriptionProperties = EnrollmentConfigHandler.GetProperties("SiteSubscriptions");
                _baseSiteId = int.Parse((string)siteSubscriptionProperties.First(p => p.Name == "BaseSiteID"));
                _autoshipScheduleIDs = ((XElement)siteSubscriptionProperties.First(p => p.Name == "AutoshipSchedules")).Elements("AutoshipSchedule").Select(a => a.Attribute<int>("ID"));
                _autoshipSchedules = ((XElement)siteSubscriptionProperties.First(p => p.Name == "AutoshipSchedules")).Elements("AutoshipSchedule").Select(a => SmallCollectionCache.Instance.AutoshipSchedules.GetById(a.Attribute<int>("ID")));
            }
        }

        public virtual ActionResult Index()
        {
            _controller.ViewData["Schedules"] = _autoshipSchedules;
            _controller.ViewData["IsSkippable"] = this.IsSkippable;
            _controller.ViewData["StepCounter"] = _enrollmentContext.StepCounter;

            SetDomains();

            return PartialView();
        }

        private void SetDomains()
        {
            int marketID = SmallCollectionCache.Instance.Countries.GetById(_enrollmentContext.CountryID).MarketID;

            Site baseSite = Site.LoadBaseSiteForNewPWS(marketID);

            var domains = baseSite != null ? baseSite.GetDomains() : new List<string>();

            _controller.ViewData["Domains"] = domains.ToArray();
        }

        public virtual ActionResult SubmitStep(List<string> urls, int? autoshipScheduleId)
        {
            try
            {
                if (!IsSiteUrlTaken(urls))
                {
                    int scheduleID = _autoshipScheduleIDs.First();

                    // create and charge the new autoship - sets status to paid
                    AutoshipOrder newAutoship =
                        AutoshipOrder.GenerateTemplateFromSchedule(
                        scheduleID,
                        EnrollingAccount,
                        _enrollmentContext.MarketID);                   

                    SaveSite(newAutoship, urls);

                    return NextStep();
                }
                return JsonError(Translation.GetTerm("SiteUrlIsNotAvailable", "The url is not available."));
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return JsonError(exception.PublicMessage);
            }
        }

        protected virtual bool IsSiteUrlAvailable(Account account, string subdomain)
        {
            Site site = null;
            if (account != null && account.AccountID > 0)
            {
                site = Site.LoadByAccountID(account.AccountID).FirstOrDefault();
            }

            bool isSiteUrlAvailable = site == null
                ? SiteUrl.IsAvailable(subdomain)
                : SiteUrl.IsAvailable(site.SiteID, subdomain);


            return isSiteUrlAvailable;
        }

        protected virtual bool IsSiteUrlTaken(List<string> urls)
        {
            Account account = EnrollingAccount;

            return urls.Any(u => !IsSiteUrlAvailable(account, u));
        }

        public virtual void SaveSite(AutoshipOrder newAutoship, List<string> urls)
        {
            var newAccount = EnrollingAccount;
            Order order = new Order();

            if (newAutoship != null && newAutoship.Order != null)
            {
                order = newAutoship.Order;
                order.OrderTypeID = AutoshipOrder.GetDefaultAutoshipTemplateOrderTypeID(newAccount.AccountTypeID);
                order.SiteID = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.NSCoreSiteID);

                order.SetConsultantID(newAccount);
                newAutoship.UpdateAutoshipAccount(newAccount);

                newAutoship.Save();


                int marketID = _enrollmentContext.MarketID;

                Site pws = Site.LoadByAccountID(newAccount.AccountID).FirstOrDefault();
                if (pws == null)
                {
                    // Create PWS
                    var baseSite = Site.LoadBaseSiteForNewPWS(marketID);
                    baseSite.CreateChildSite(
                        newAccount,
                        marketID,
                        newAutoship.AutoshipOrderID,
                        urls: urls,
                        saveNewSite: true
                     );
                }

                _enrollmentContext.SiteSubscriptionAutoshipOrder = newAutoship;
            }
        }

        public virtual ActionResult SkipStep()
        {
            return NextStep();
        }

        public SiteSubscriptionsStep(IEnrollmentStepConfig stepConfig, Controller controller, IEnrollmentContext enrollmentContext)
            : base(stepConfig, controller, enrollmentContext) { }
    }
}
