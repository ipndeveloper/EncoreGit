using NetSteps.Commissions.Service.Interfaces.CalculationKind;
using System.Collections.Generic;
using System.Linq;

namespace NetSteps.Commissions.Service.CalculationKinds
{
	public class CalculationKindService : ICalculationKindService
	{
		protected readonly ICalculationKindProvider Provider;

		public CalculationKindService(ICalculationKindProvider provider)
		{
			Provider = provider;
		}

		public IEnumerable<Common.Models.ICalculationKind> GetCalculationKinds()
		{
			return Provider.ToArray();
		}

		public Common.Models.ICalculationKind GetCalculationKind(string calculationKindCode)
		{
			return Provider.FirstOrDefault(x => x.Code == calculationKindCode);
		}

		public Common.Models.ICalculationKind GetCalculationKind(int calculationKindId)
		{
			return Provider.FirstOrDefault(x => x.CalculationKindId == calculationKindId);
		}
	}
}