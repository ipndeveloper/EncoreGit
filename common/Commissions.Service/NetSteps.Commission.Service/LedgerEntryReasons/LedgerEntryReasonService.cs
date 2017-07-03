using NetSteps.Commissions.Service.Interfaces.LedgerEntryReason;
using System.Collections.Generic;
using System.Linq;

namespace NetSteps.Commissions.Service.LedgerEntryReasons
{
	public class LedgerEntryReasonService : ILedgerEntryReasonService
	{
		private readonly ILedgerEntryReasonProvider _provider;

		public LedgerEntryReasonService(ILedgerEntryReasonProvider provider)
		{
			_provider = provider;
		}

		public Common.Models.ILedgerEntryReason GetLedgerEntryReason(int ledgerEntryReasonId)
		{
			return _provider.SingleOrDefault(x => x.EntryReasonId == ledgerEntryReasonId);
		}

		public IEnumerable<Common.Models.ILedgerEntryReason> GetLedgerEntryReasons()
		{
			return _provider.ToArray();
		}

        public Common.Models.ILedgerEntryReason GetEntryReason(string entryReasonCode)
        {
            return _provider.SingleOrDefault(x => x.Code == entryReasonCode);
        }
    }
}
