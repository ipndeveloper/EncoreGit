using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Claims.Common
{
	public struct PasswordPolicyEvaluationResult
	{
		public readonly bool Success;
		public readonly IEnumerable<string> Reasons;

		public PasswordPolicyEvaluationResult(bool success, IEnumerable<string> reasons = null)
		{
			Success = success;
			Reasons = reasons;
		}
	}
}
