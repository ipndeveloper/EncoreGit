using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Service.Base;
using System.Collections.Generic;

namespace NetSteps.Commissions.Service.Interfaces.Title
{
	/// <summary>
	/// 
	/// </summary>
	public interface ITitleRepository : IRepository<ITitle, int>
	{
        IEnumerable<ITitle> GetFromReportByPeriod(int periodId, int accountId);
	}
}
