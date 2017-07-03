using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Web.API.Base.Common
{
	public interface IInputValidator<TInput>
	{
		bool IsValid(TInput input, ValidationErrorCollector collector);
	}
}
