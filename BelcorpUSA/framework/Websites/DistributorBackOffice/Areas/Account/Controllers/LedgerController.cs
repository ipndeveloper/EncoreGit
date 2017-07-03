using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Commissions.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Commissions.Common.Models;

namespace DistributorBackOffice.Areas.Account.Controllers
{
	public class LedgerController : BaseAccountsController
	{
		private IProductCreditLedgerService _productCreditLedgerService = Create.New<IProductCreditLedgerService>();
		private ICommissionsService _commissionsService = Create.New<ICommissionsService>();

		#region Product Credit Ledger
		[FunctionFilter("Accounts-Product Credit Ledger", "~/Account", Constants.SiteType.BackOffice)]
		public virtual ActionResult ProductCredit()
		{
			return View("ProductCredit");
		}

		[OutputCache(CacheProfile = "PagedGridData")]
		[FunctionFilter("Accounts-Product Credit Ledger", "~/Account", Constants.SiteType.BackOffice)]
		public virtual ActionResult GetProductCredit(int page, int pageSize)
		{
			try
			{
				var accountLedgers = _productCreditLedgerService.RetrieveLedger(CurrentAccount.AccountID);
				if (accountLedgers != null && accountLedgers.Any())
				{
					var entries = accountLedgers.OrderByDescending(le => le.EffectiveDate).ThenByDescending(le => le.EntryId);
					StringBuilder builder = new StringBuilder();

					int count = 0;
					foreach (var entry in entries.Skip(page * pageSize).Take(pageSize))
					{
						var currency = SmallCollectionCache.Instance.Currencies.FirstOrDefault(c => c.CurrencyID == entry.CurrencyTypeId);
						IBonusKind bonusKind = null;
						if (entry.BonusTypeId.GetValueOrDefault() > 0)
						{
							bonusKind = _commissionsService.GetBonusKind(entry.BonusTypeId.Value);
						}
						builder.Append("<tr class=\"").Append(count % 2 == 1 ? "Alt" : String.Empty).Append(entry.EntryAmount < 0 ? " Negative" : " Positive").Append("\">")
							.AppendCell(entry.EntryDescription)
							.AppendCell(Translation.GetTerm(entry.EntryReason.TermName))
							.AppendCell(Translation.GetTerm(entry.EntryKind.TermName))
							.AppendCell(entry.EffectiveDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
							.AppendCell(bonusKind != null ? Translation.GetTerm(bonusKind.TermName) : String.Empty)
							.AppendCell(entry.EntryAmount.ToString("C", currency.Culture))
							.AppendCell(entry.EndingBalance.GetValueOrDefault().ToString("C", currency.Culture))
							.Append("</td></tr>");
						++count;
					}
					return Json(new { totalPages = Math.Ceiling(entries.Count() / (double)pageSize), page = builder.ToString() });
				}
				return Json(new { totalPages = 0, page = "<tr><td colspan=\"7\">" + Translation.GetTerm("NoLedgerEntries", "No ledger entries.") + "</td></tr>" });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}
		#endregion
	}
}
