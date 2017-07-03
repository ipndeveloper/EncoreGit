using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Claims.Common
{
	public interface IPasswordPolicy
	{

		System.Collections.Generic.IEnumerable<NetSteps.Claims.Common.IPasswordPolicyEvaluator> Evaluators
		{
			get;
			set;
		}

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

		PasswordPolicyEvaluationResult Evaluate(IPassword password);
	}
}
