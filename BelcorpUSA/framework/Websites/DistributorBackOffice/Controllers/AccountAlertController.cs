using System;
using System.Web.Mvc;
using NetSteps.Communication.UI.Common;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;

namespace DistributorBackOffice.Controllers
{
	public class AccountAlertController : BaseController
	{
		[HttpPost]
		public ActionResult Dismiss(int id)
		{
			int accountId = 0;
			try
			{
				accountId = CurrentAccount.AccountID;
				var accountAlertUIService = Create.New<IAccountAlertUIService>();

				accountAlertUIService.Dismiss(id, accountId);

				return JsonSuccess();
			}
			catch (Exception ex)
			{
				ex.Log(accountID: accountId);
				return JsonError();
			}
		}
	}
}