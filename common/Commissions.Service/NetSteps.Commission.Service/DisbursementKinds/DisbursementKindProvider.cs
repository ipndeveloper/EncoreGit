using System.Linq;
using NetSteps.Commissions.Service.Interfaces.DisbursementKinds;
using NetSteps.Core.Cache;
using System.Collections.Generic;

namespace NetSteps.Commissions.Service.DisbursementKinds
{
	public class DisbursementKindProvider : ActiveLocalMemoryCachedListBase<IDisbursementKind>, IDisbursementKindProvider
	{
		protected readonly IDisbursementKindRepository Repository;
		public DisbursementKindProvider(IDisbursementKindRepository repository)
		{
			Repository = repository;
		}

		protected override IList<IDisbursementKind> PerformRefresh()
		{
			return Repository.FetchAll();
		}

		public IDisbursementKind GetDisbursementKind(Common.Models.DisbursementMethodKind method)
		{
			return this.FirstOrDefault(x => x.DisbursementKindId == (int)method);
		}

        public string GetDisbursementMethodCode(int disbursementMethodId)
        {
            var method = this.FirstOrDefault(x => x.DisbursementKindId == disbursementMethodId);
            return method != null ? method.Code : null;
        }
    }
}
