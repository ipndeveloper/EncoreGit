using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Claims.Common
{
	public struct UserIdentityEvaluationResult
	{
		public readonly bool Success;
		public readonly string Reason;

		public UserIdentityEvaluationResult(bool success, string reason = null)
		{
			Success = success;
			Reason = reason;
		}
	}
}
