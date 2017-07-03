using NetSteps.Commissions.Common;
using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Commissions.Service.Interfaces.LedgerEntryKind;
using NetSteps.Commissions.Service.Interfaces.LedgerEntryOrigin;
using NetSteps.Commissions.Service.Interfaces.LedgerEntryReason;
using NetSteps.Commissions.Service.Interfaces.AccountLedger;

namespace NetSteps.Commissions.Service
{
	public class AccountLedgerService : IAccountLedgerService
	{
		private readonly ILedgerEntryKindService _ledgerEntryKindService;
        private readonly ILedgerEntryOriginService _ledgerEntryOriginService;
        private readonly ILedgerEntryReasonService _ledgerEntryReasonService;
        private readonly IAccountLedgerEntryService _ledgerEntryService;

		public AccountLedgerService(IAccountLedgerEntryService ledgerEntryService, ILedgerEntryKindService ledgerEntryKindService, ILedgerEntryOriginService ledgerEntryOriginService, ILedgerEntryReasonService ledgerEntryReasonService)
		{
			_ledgerEntryKindService = ledgerEntryKindService;
			_ledgerEntryOriginService = ledgerEntryOriginService;
			_ledgerEntryReasonService = ledgerEntryReasonService;
			_ledgerEntryService = ledgerEntryService;
		}

		public IAccountLedgerEntry AddLedgerEntry(int accountId, decimal entryAmount, DateTime effectiveDateUTC, string entryDescription, int entryReason, int entryType, int bonusType, string notes, int currencyTypeId, int userId)
		{
			return _ledgerEntryService.AddLedgerEntry(accountId, entryAmount, effectiveDateUTC, entryDescription, entryReason, entryType, bonusType, notes, currencyTypeId, userId);
		}

		public decimal GetCurrentBalance(int accountId)
		{
			var ledger = _ledgerEntryService.GetAccountLedger(accountId);
			if (ledger != null && ledger.Any())
			{
				return ledger.First().EndingBalance.Value;
			}
			return 0M;
		}

		public ILedgerEntryKind GetEntryKind(int entryKindId)
		{
			return _ledgerEntryKindService.GetLedgerEntryKind(entryKindId);
		}

		public ILedgerEntryOrigin GetEntryOrigin(int entryOriginId)
		{
			return _ledgerEntryOriginService.GetLedgerEntryOrigin(entryOriginId);
		}

		public ILedgerEntryReason GetEntryReason(int entryReasonId)
		{
			return _ledgerEntryReasonService.GetLedgerEntryReason(entryReasonId);
		}

		public IEnumerable<IAccountLedgerEntry> RetrieveLedger(int accountId)
		{
			return _ledgerEntryService.GetAccountLedger(accountId);
		}


		public IEnumerable<ILedgerEntryOrigin> GetEntryOrigins()
		{
			return _ledgerEntryOriginService.GetLedgerEntryOrigins();
		}


		public ILedgerEntryKind GetEntryKind(string entryCode)
		{
			return _ledgerEntryKindService.GetLedgerEntryKind(entryCode);
		}


		public IEnumerable<ILedgerEntryKind> GetEntryKinds()
		{
			return _ledgerEntryKindService.GetLedgerEntryKinds();
		}

		public IEnumerable<ILedgerEntryReason> GetEntryReasons()
		{
			return _ledgerEntryReasonService.GetLedgerEntryReasons();
		}

		public IAccountLedgerEntry AddLedgerEntry(IAccountLedgerEntry ledgerEntry)
		{
			return _ledgerEntryService.AddLedgerEntry(ledgerEntry);
		}

		public IAccountLedgerEntry UpdateLedgerEntry(IAccountLedgerEntry ledgerEntry)
		{
			return _ledgerEntryService.UpdateLedgerEntry(ledgerEntry);
		}


        public decimal GetCurrentBalance(int accountId, ILedgerEntryKind entryKind)
        {
            return _ledgerEntryService.GetCurrentBalance(accountId, entryKind);
        }

        public ILedgerEntryReason GetEntryReason(string entryReasonCode)
        {
            return _ledgerEntryReasonService.GetEntryReason(entryReasonCode);
        }
    }
}
