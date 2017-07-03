using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Promotions.Common.Model
{
	public interface IPromotionState
	{
		bool IsValid { get; }
		IEnumerable<string> ConstructionErrors { get; }
		void AddConstructionError(string component, string message);
	}
}
