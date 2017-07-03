using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Base;
using NetSteps.Commissions.Service.Interfaces.AccountLedger;
using NetSteps.Core.Cache;
using System.Collections.Generic;
using System.Linq;

namespace NetSteps.Commissions.Service.AccountLedgerEntries
{
	public class AccountLedgerEntryProvider : CommissionsActiveMruCacheAdapter<int, IAccountLedgerEntry>, IAccountLedgerEntryProvider
	{
		private readonly IAccountLedgerEntryRepository _repository;
		private readonly ICache<int, IEnumerable<int>> _accountLedgerEntryIds;

		public AccountLedgerEntryProvider(IAccountLedgerEntryRepository repository)
		{
			_repository = repository;
			_accountLedgerEntryIds = new ActiveMruLocalMemoryCache<int, IEnumerable<int>>("AccountLedger_AccountLedgerIds", new DelegatedDemuxCacheItemResolver<int, IEnumerable<int>>(GetLedgerEntryIdsForAccountId));
		
		}
		protected override ICache<int, IAccountLedgerEntry> InitializeCache()
		{
			return new ActiveMruLocalMemoryCache<int, IAccountLedgerEntry>("AccountLedger_LedgerItems", new DelegatedDemuxCacheItemResolver<int, IAccountLedgerEntry>(GetLedgerItem));
		}

		public IAccountLedgerEntry AddLedgerEntry(IAccountLedgerEntry entry)
		{
			var added = _repository.Add(entry);
		    return (added != null) ? this.Get(added.EntryId) : null;
		}

		public IEnumerable<IAccountLedgerEntry> GetAccountLedger(int accountId)
		{
			IEnumerable<int> entryIds;
			if (_accountLedgerEntryIds.TryGet(accountId, out entryIds))
			{
				return entryIds.Select(x => Cache.Get(x));
			}

			return new List<IAccountLedgerEntry>();
		}

		public IAccountLedgerEntry UpdateLedgerEntry(IAccountLedgerEntry entry)
		{
			return _repository.Update(entry);
		}

		private bool GetLedgerEntryIdsForAccountId(int accountId, out IEnumerable<int> ledgerEntryIds)
		{
			ledgerEntryIds = _repository.GetAccountLedgerEntryIds(accountId);
			return (ledgerEntryIds != null && ledgerEntryIds.Any());
		}

		private bool GetLedgerItem(int key, out IAccountLedgerEntry value)
		{
			value = _repository.Fetch(key);
			return value != null;
		}


		public bool DeleteLedgerEntry(int accountLedgerEntryId)
		{
			return _repository.Delete(accountLedgerEntryId);
		}

	}
}
