using System.Linq;
using System.Web.Mvc;
using NetSteps.Common.Configuration;
using NetSteps.Data.Entities;
using NetSteps.Enrollment.Common.Models.Config;
using NetSteps.Enrollment.Common.Models.Context;

namespace NetSteps.Web.Mvc.Controls.Controllers.Enrollment
{
    public class ConfirmationStep : BaseEnrollmentStep
    {
        public virtual ActionResult Index()
        {
            RefreshEnrollingAccount();

            var account = EnrollingAccount;

            account.Activate(true);

            AddUserRole(account);

            account.Save();

            UserSiteWidget.SetCustomUserWidgetVisibility(account);

			OnEnrollmentComplete(account, InitialOrder);
			
            SendEnrollmentCompletedEmails();

            _controller.ViewData["FullName"] = account.FullName;
            _controller.ViewData["AccountNumber"] = account.AccountNumber;

            _enrollmentContext.Initialize(_enrollmentContext.EnrollmentConfig, _enrollmentContext.CountryID, _enrollmentContext.LanguageID, ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.NSCoreSiteID));
            _enrollmentContext.EnrollingAccount = account;
            return PartialView();
        }

        private void AddUserRole(Account account)
        {
            var accountTypeRole = Role.LoadAll()
				 .FirstOrDefault(x => x.AccountTypes.Any(at => at.AccountTypeID == _enrollmentContext.AccountTypeID));
			if (accountTypeRole != null && !account.User.Roles.Select(r => r.RoleID).Contains(accountTypeRole.RoleID))
			{
				account.User.Roles.Add(accountTypeRole);
			}
			
        }

        private void RefreshEnrollingAccount()
        {
            EnrollingAccount = Account.LoadFull(EnrollingAccount.AccountID);
        }

        private void SendEnrollmentCompletedEmails()
        {
            if (InitialOrder != null)
            {
                // Send Email to the new account holder with order start kit info
                DomainEventQueueItem.AddEnrollmentCompletedEventToQueue(InitialOrder.OrderID, EnrollingAccount.AccountID);

                // If a distributor enrolled then send an email to the enroller notifying her of the enrollment.
                if (_enrollmentContext.AccountTypeID == (short)Constants.AccountType.Distributor)
                    DomainEventQueueItem.AddDistributorJoinsDownlineEventToQueue(InitialOrder.OrderID, EnrollingAccount.AccountID);
            }
            else
            {
                DomainEventQueueItem.AddEnrollmentCompletedEventToQueue(EnrollingAccount.AccountID, EnrollingAccount.AccountTypeID);
            }
        }

        public ConfirmationStep(IEnrollmentStepConfig stepConfig, Controller controller, IEnrollmentContext enrollmentContext)
            : base(stepConfig, controller, enrollmentContext) { }
    }
}