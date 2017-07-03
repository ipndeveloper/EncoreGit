using System;

namespace NetSteps.EventProcessing.Common
{
	public interface IExceptionLogger
	{
		void LogException(Exception ex);
	}
}
