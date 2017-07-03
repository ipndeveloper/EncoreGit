using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Claims.Common
{
	public struct PasswordEvaluatorResult
	{
		public readonly bool Success;
		public readonly string Reason;

		public PasswordEvaluatorResult(bool success, string reason = null)
		{
			Success = success;
			Reason = reason;
		}
	}
}
