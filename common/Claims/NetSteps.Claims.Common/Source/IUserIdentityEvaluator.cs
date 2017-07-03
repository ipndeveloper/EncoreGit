using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Claims.Common
{
	public interface IUserIdentityEvaluator
	{
		UserIdentityEvaluationResult Evaluate(IUserIdentity userIdentity);
	}
}
