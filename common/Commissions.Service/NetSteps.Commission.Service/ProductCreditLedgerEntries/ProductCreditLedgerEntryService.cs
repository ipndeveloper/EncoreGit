using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.LedgerEntryKind;
using NetSteps.Commissions.Service.Interfaces.LedgerEntryOrigin;
using NetSteps.Commissions.Service.Interfaces.LedgerEntryReason;
using NetSteps.Commissions.Service.Interfaces.ProductCreditLedger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Commissions.Service.Models;
using NetSteps.Core.Cache;

namespace NetSteps.Commissions.Service.ProductCreditLedgerEntries
{
	public class ProductCreditLedgerEntryService : IProductCreditLedgerEntryService
	{
		private IProductCreditLedgerEntryProvider _provider;
		private ILedgerEntryKindProvider _ledgerEntryKindProvider;
		private ILedgerEntryOriginProvider _ledgerEntryOriginProvider;
		private ILedgerEntryReasonProvider _ledgerEntryReasonProvider;

		public ProductCreditLedgerEntryService(IProductCreditLedgerEntryProvider provider, ILedgerEntryKindProvider ledgerEntryKindProvider, ILedgerEntryOriginProvider ledgerEntryOriginProvider, ILedgerEntryReasonProvider ledgerEntryReasonProvider)
		{
			_provider = provider;
			_ledgerEntryKindProvider = ledgerEntryKindProvider;
			_ledgerEntryOriginProvider = ledgerEntryOriginProvider;
			_ledgerEntryReasonProvider = ledgerEntryReasonProvider;
		}


		public IEnumerable<IProductCreditLedgerEntry> GetProductCreditLedger(int accountId)
		{
			return _provider.GetProductCreditLedger(accountId);
		}

		public IProductCreditLedgerEntry GetProductCreditLedgerEntry(int ProductCreditLedgerId)
		{

			return _provider.Get(ProductCreditLedgerId);
		}

		public IProductCreditLedgerEntry AddLedgerEntry(IProductCreditLedgerEntry entry)
		{
			return _provider.AddLedgerEntry(entry);
		}

		public IProductCreditLedgerEntry UpdateLedgerEntry(IProductCreditLedgerEntry entry)
		{
			return _provider.UpdateLedgerEntry(entry);
		}

		public IProductCreditLedgerEntry AddLedgerEntry(int accountId, decimal entryAmount, DateTime effectiveDateUTC, string entryDescription, int entryReason, int entryType, int bonusType, string notes, int currencyTypeId, int userId)
		{
			var entry = new ProductCreditLedgerEntry()
			{
				AccountId = accountId,
				EntryAmount = entryAmount,
				EffectiveDate = effectiveDateUTC,
				EntryDescription = entryDescription,
				EntryReason = _ledgerEntryReasonProvider.Single(x => x.EntryReasonId == entryReason),
				EntryDate = DateTime.UtcNow,
				EntryKind = _ledgerEntryKindProvider.Single(x => x.LedgerEntryKindId == entryType),
				EntryNotes = notes,
				EntryOrigin = _ledgerEntryOriginProvider.Single(x => x.EntryOriginId == 1), // Assume GMP at the present time.
				UserId = userId,
				CurrencyTypeId = currencyTypeId
			};
			return _provider.AddLedgerEntry(entry);
		}


		public bool DeleteLedgerEntry(int accountLedgerEntryId)
		{
			return _provider.DeleteLedgerEntry(accountLedgerEntryId);
		}

	}
}
