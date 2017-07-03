using System;
using System.Linq;

namespace NetSteps.Data.Common.Context
{
	public interface IOrderStep
    {
		IOrderStepResponse Response { get; set;  }
		int CustomerAccountID { get; set; }
		string OrderStepReferenceID { get; }
    }
}
