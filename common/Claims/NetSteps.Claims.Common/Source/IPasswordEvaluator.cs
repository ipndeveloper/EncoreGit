using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Claims.Common
{
	public interface IPasswordEvaluator
	{
		PasswordEvaluatorResult Evaluate(IPassword password);
	}
}
