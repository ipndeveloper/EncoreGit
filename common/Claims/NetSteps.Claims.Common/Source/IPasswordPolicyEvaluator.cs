using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Claims.Common
{
	public interface IPasswordPolicyEvaluator
	{
		int Index
		{
			get;
			set;
		}

		string Name
		{
			get;
			set;
		}

		IPasswordEvaluator GetEvaluator();
	}
}
