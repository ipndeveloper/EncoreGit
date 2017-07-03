using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.ProductCreditLedger;
using NetSteps.Commissions.Service.Base;
using NetSteps.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.ProductCreditLedgerEntries
{
	public class ProductCreditLedgerEntryProvider : CommissionsActiveMruCacheAdapter<int, IProductCreditLedgerEntry>, IProductCreditLedgerEntryProvider
	{
		private IProductCreditLedgerEntryRepository _repository;
		private ICache<int, IEnumerable<int>> _accountLedgerEntryIds;

		public ProductCreditLedgerEntryProvider(IProductCreditLedgerEntryRepository repository)
		{
			_repository = repository;
			_accountLedgerEntryIds = new ActiveMruLocalMemoryCache<int, IEnumerable<int>>("ProductCreditLedger_AccountLedgerIds", new Core.Cache.DelegatedDemuxCacheItemResolver<int, IEnumerable<int>>(GetLedgerEntryIdsForAccountId));
		}

		protected override ICache<int, IProductCreditLedgerEntry> InitializeCache()
		{
			return new ActiveMruLocalMemoryCache<int, IProductCreditLedgerEntry>("ProductCreditLedger_LedgerItems", new DelegatedDemuxCacheItemResolver<int, IProductCreditLedgerEntry>(GetLedgerItem));
		}

		public IProductCreditLedgerEntry AddLedgerEntry(IProductCreditLedgerEntry entry)
		{
			var added = _repository.Add(entry);
			return this.Get(added.EntryId);
		}

		public IEnumerable<IProductCreditLedgerEntry> GetProductCreditLedger(int accountId)
		{
			IEnumerable<int> entryIds;
			if (_accountLedgerEntryIds.TryGet(accountId, out entryIds))
			{
				return entryIds.Select(x => Cache.Get(x));
			}

			return new List<IProductCreditLedgerEntry>();
		}

		public IProductCreditLedgerEntry UpdateLedgerEntry(IProductCreditLedgerEntry entry)
		{
			return _repository.Update(entry);
		}

		private bool GetLedgerEntryIdsForAccountId(int accountId, out IEnumerable<int> ledgerEntryIds)
		{
			ledgerEntryIds = _repository.GetProductCreditLedgerEntryIds(accountId);
			return (ledgerEntryIds != null && ledgerEntryIds.Any());
		}

		private bool GetLedgerItem(int key, out IProductCreditLedgerEntry value)
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
