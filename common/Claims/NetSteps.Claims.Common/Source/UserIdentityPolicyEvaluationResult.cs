using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Claims.Common
{
	public struct UserIdentityPolicyEvaluationResult
	{
		public readonly IEnumerable<string> Reasons;
		public readonly bool Success;

		public UserIdentityPolicyEvaluationResult(bool success, IEnumerable<string> reasons = null)
		{
			Success = success;
			Reasons = reasons;
		}
	}
}
