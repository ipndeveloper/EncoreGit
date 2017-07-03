using System;
using NetSteps.Common.Exceptions;

namespace NetSteps.Data.Entities.Exceptions
{
	public class PartyOrderMinimumAmountException : NetStepsException
	{
		public PartyOrderMinimumAmountException()
			: base("Minimum party amount has not been met.")
		{
		}

		public PartyOrderMinimumAmountException(Exception exception)
			: base("Minimum party amount has not been met.", exception)
		{
		}
	}
}
