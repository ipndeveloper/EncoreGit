using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Common;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;

namespace nsCore.Areas.Accounts.Models.Ledger
{
	public class LedgerEntryFormModel
	{
		readonly ICommissionsService _commissionsService = Create.New<ICommissionsService>();
		readonly IProductCreditLedgerService _ledgerService = Create.New<IProductCreditLedgerService>();

		public IEnumerable<ILedgerEntryReason> LedgerEntryReasons
		{
			get
			{
				return this._ledgerService.GetEntryReasons();
			}
		}

		public IEnumerable<ILedgerEntryKind> LedgerEntryKinds
		{
			get
			{
				return this._ledgerService.GetEntryKinds();
			}
		}

		public IEnumerable<IBonusKind> BonusKinds
		{
			get
			{
				return this._commissionsService.GetBonusKinds();
			}
		}

	    public decimal GetCurrentBalance(bool isProductCredit, int accountId)
	    {
	        return !isProductCredit
	            ? _ledgerService.GetCurrentBalance(accountId)
	            : _ledgerService.GetCurrentBalanceLessPendingPayments(accountId);
	    }
	}
}