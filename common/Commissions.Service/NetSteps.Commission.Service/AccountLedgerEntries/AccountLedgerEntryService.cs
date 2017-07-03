using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.AccountLedger;
using NetSteps.Commissions.Service.Interfaces.LedgerEntryKind;
using NetSteps.Commissions.Service.Interfaces.LedgerEntryOrigin;
using NetSteps.Commissions.Service.Interfaces.LedgerEntryReason;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetSteps.Commissions.Service.AccountLedgerEntries
{
    public class AccountLedgerEntryService : IAccountLedgerEntryService
    {
        private readonly IAccountLedgerEntryProvider _provider;
        private readonly ILedgerEntryKindProvider _ledgerEntryKindProvider;
        private readonly ILedgerEntryOriginProvider _ledgerEntryOriginProvider;
        private readonly ILedgerEntryReasonProvider _ledgerEntryReasonProvider;

        public AccountLedgerEntryService(IAccountLedgerEntryProvider provider, ILedgerEntryKindProvider ledgerEntryKindProvider, ILedgerEntryOriginProvider ledgerEntryOriginProvider, ILedgerEntryReasonProvider ledgerEntryReasonProvider)
        {
            _provider = provider;
            _ledgerEntryKindProvider = ledgerEntryKindProvider;
            _ledgerEntryOriginProvider = ledgerEntryOriginProvider;
            _ledgerEntryReasonProvider = ledgerEntryReasonProvider;
        }

        public IEnumerable<IAccountLedgerEntry> GetAccountLedger(int accountId)
        {
            return _provider.GetAccountLedger(accountId);
        }

        public IAccountLedgerEntry UpdateLedgerEntry(IAccountLedgerEntry entry)
        {
            return _provider.UpdateLedgerEntry(entry);
        }

        public IAccountLedgerEntry AddLedgerEntry(IAccountLedgerEntry entry)
        {
            return _provider.AddLedgerEntry(entry);
        }

        public IAccountLedgerEntry AddLedgerEntry(int accountId, decimal entryAmount, DateTime effectiveDateUTC, string entryDescription, int entryReason, int entryType, int bonusType, string notes, int currencyTypeId, int userId)
        {
            var entry = new AccountLedgerEntry
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

        public IAccountLedgerEntry GetLedgerEntry(int AccountLedgerId)
        {
            IAccountLedgerEntry entry;
            if (_provider.TryGet(AccountLedgerId, out entry))
            {
                return entry;
            }

            return null;
        }

        public decimal GetCurrentBalance(int accountId, ILedgerEntryKind entryKind)
        {
            var ledger = GetAccountLedger(accountId);
            if (ledger == null)
            {
                return 0M;
            }

            var total = ledger
                .Where(x => x.EntryKind.LedgerEntryKindId == entryKind.LedgerEntryKindId)
                .Sum(x => x.EntryAmount);
            return total;
        }
    }
}
