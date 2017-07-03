using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.LedgerEntryKind;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.LedgerEntryKinds
{
	public class LedgerEntryKindService : ILedgerEntryKindService
	{
		private ILedgerEntryKindProvider _provider;

		public LedgerEntryKindService(ILedgerEntryKindProvider provider)
		{
			_provider = provider;
		}

		public ILedgerEntryKind GetLedgerEntryKind(int ledgerEntryKindId)
		{
			return _provider.SingleOrDefault(x => x.LedgerEntryKindId == ledgerEntryKindId);
		}

		public IEnumerable<ILedgerEntryKind> GetLedgerEntryKinds()
		{
			return _provider.ToArray();
		}

		public ILedgerEntryKind GetLedgerEntryKind(string ledgerEntryKindCode)
		{
			return _provider.SingleOrDefault(x => x.Code == ledgerEntryKindCode);
		}
	}
}
