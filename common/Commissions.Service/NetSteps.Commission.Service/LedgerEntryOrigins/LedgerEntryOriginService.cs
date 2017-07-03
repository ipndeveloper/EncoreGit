using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Interfaces.LedgerEntryOrigin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.LedgerEntryOrigins
{
	public class LedgerEntryOriginService : ILedgerEntryOriginService
	{
		private ILedgerEntryOriginProvider _provider;

		public LedgerEntryOriginService(ILedgerEntryOriginProvider provider)
		{
			_provider = provider;
		}

		public ILedgerEntryOrigin GetLedgerEntryOrigin(int ledgerEntryOriginId)
		{
			return _provider.SingleOrDefault(x => x.EntryOriginId == ledgerEntryOriginId);
		}

		public IEnumerable<ILedgerEntryOrigin> GetLedgerEntryOrigins()
		{
			return _provider.ToArray();
		}
	}
}
