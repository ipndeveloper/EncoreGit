using System;
using System.Web.Mvc;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Controllers;
using NetSteps.Commissions.Common;
using NetSteps.Encore.Core.IoC;

namespace nsCore.Areas.Accounts.Controllers
{
	public abstract class BaseAccountsController : BaseController
	{
		private ICommissionsService _comSvc;
		protected ICommissionsService CommissionsService
		{
			get
			{
				if (_comSvc == null)
				{
					_comSvc = Create.New<ICommissionsService>();
				}
				return _comSvc;
			}
		}

		protected virtual ActionResult CheckForAccount(string id, object model = null, bool forceLoadAccount = false)
		{
			bool wasAccountLoaded = false;

			if (!string.IsNullOrEmpty(id))
			{
				if (IsCurrentAccountNull() || IsCurrentAccountDifferent(id))
				{
					ResetCoreContextAccount(id);
					wasAccountLoaded = true;
				}

				ForceLoadAccount(id, wasAccountLoaded, forceLoadAccount);
			}

			if (IsCurrentAccountNull())
				return Redirect("~/Accounts");

			return View(model);
		}
		private static bool IsCurrentAccountNull()
		{
			return CoreContext.CurrentAccount.IsNull();
		}

		private static bool IsCurrentAccountDifferent(string id)
		{
			return !String.Equals(CoreContext.CurrentAccount.AccountNumber, id);
		}

		private static void ForceLoadAccount(string id, bool isAlreadyLoaded, bool forceLoad)
		{
			if (!isAlreadyLoaded && forceLoad)
				ResetCoreContextAccount(id);

		}

		private static void ResetCoreContextAccount(string id)
		{
			CoreContext.CurrentAccount = Account.LoadForSessionByAccountNumber(id);
		}

		protected override void SetViewData()
		{
			var account = CurrentAccount;
			if (account == null)
			{
				Redirect("~/Accounts");
				return;
			}

			base.SetViewData();
		}

		protected override Account CurrentAccount
		{
			get
			{
				var account = base.CurrentAccount;
				if (!String.IsNullOrEmpty(AccountNum)
				&& (ForceAccountLoad || (account == null || !String.Equals(account.AccountNumber, AccountNum, StringComparison.InvariantCulture))))
				{
					base.CurrentAccount = account = Account.LoadForSessionByAccountNumber(AccountNum);
				}
				return account;
			}
			set
			{
				base.CurrentAccount = value;
			}
		}

		/// <summary>
		/// The current working account number.
		/// </summary>
		public string AccountNum { get; protected set; }

		/// <summary>
		/// Indicates whether the account should be forcibly loaded if it doesn't match
		/// account currently associated with the session/action.
		/// </summary>
		public bool ForceAccountLoad { get; protected set; }
	}
}
