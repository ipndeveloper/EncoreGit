using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Interfaces.Title
{
	/// <summary>
	/// 
	/// </summary>
	public interface ITitleProvider : IEnumerable<ITitle>
	{
        IEnumerable<ITitle> GetFromReportByPeriod(int periodId, int accountId);
	}
}
