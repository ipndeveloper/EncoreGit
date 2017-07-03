using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Claims.Common
{
	public interface IUserIdentityPolicy
	{
		string Name
		{
			get;
			set;
		}

		string Description
		{
			get;
			set;
		}

		IEnumerable<IUserIdentityPolicyEvaluator> Evaluators
		{
			get;
			set;
		}

		UserIdentityPolicyEvaluationResult Evaluate(IUserIdentity userIdentity);
	}
}
