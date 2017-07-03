using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Mail;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Commissions.Common;

namespace DistributorBackOffice.Areas.Communication.Helpers
{
	public interface IDwsEmailHelper
	{
		ObservableCollection<MailMessageRecipient> AddMailMessageRecipients(int currentAccountID, List<int> accountIDs);

		List<MailMessageRecipient> AddDownlineEmailRecipient(List<int> accountIDs);
	}

	[ContainerRegister(typeof(IDwsEmailHelper), RegistrationBehaviors.Default)]
	public class DwsEmailHelper : IDwsEmailHelper
	{
        private readonly Downline _currentPeriodDownline;

		public DwsEmailHelper(Downline currentPeriodDownline)
		{
			_currentPeriodDownline = currentPeriodDownline;
		}

		public DwsEmailHelper()
			: this(CurrentPeriodDownline())
		{
		}

		public ObservableCollection<MailMessageRecipient> AddMailMessageRecipients(int currentAccountID, List<int> accountIDs)
		{
			var recipients = new ObservableCollection<MailMessageRecipient>();

			foreach (int accountID in accountIDs)
			{
				if (currentAccountID == accountID)
					continue;

				var parentNodeDetails = _currentPeriodDownline.Lookup[accountID];

				string firstName = ((string)parentNodeDetails.FirstName).ToJavaScriptStringEncode();
				string lastName = ((string)parentNodeDetails.LastName).ToJavaScriptStringEncode();

				recipients.Add(NewMailMessageRecipient(firstName, lastName, accountID));
			}

			return recipients;
		}

		public List<MailMessageRecipient> AddDownlineEmailRecipient(List<int> accountIDs)
		{
			string entireDownline = Translation.GetTerm("NumberOfDownlineAccounts", "Number of Downline Accounts");
			string downlineCount = string.Format("({0})", accountIDs.Count);

			var downlineEmailRecipient = new List<MailMessageRecipient> { NewMailMessageRecipient(entireDownline, downlineCount, 0) };

			return downlineEmailRecipient;
		}

		private static Downline CurrentPeriodDownline()
		{
			var _commissionsService = Create.New<ICommissionsService>();

		    var downline = DownlineCache.GetDownline(_commissionsService.GetCurrentPeriod().PeriodId);

			return downline;
		}

		private static MailMessageRecipient NewMailMessageRecipient(string firstName, string lastName, int accountID)
		{
			return new MailMessageRecipient()
			{
				Name = string.Format("{0} {1}", firstName, lastName).Trim(),
				MailMessageRecipientType = Constants.MailMessageRecipientType.Individual,
				Internal = true,
				AccountID = accountID,
			};
		}
	}
}