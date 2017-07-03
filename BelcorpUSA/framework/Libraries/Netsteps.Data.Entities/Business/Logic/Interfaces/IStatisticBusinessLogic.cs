using System.Collections.Generic;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
	public partial interface IStatisticBusinessLogic
	{
		void SaveBatch(IStatisticRepository repository, IEnumerable<Statistic> items);
	}
}
