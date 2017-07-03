using NetSteps.Commissions.Common.Models;
using NetSteps.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Services.CheckHold
{
	/// <summary>
	/// 
	/// </summary>
	public interface ICheckHoldProvider : ICache<int, ICheckHold>
	{
	}
}
