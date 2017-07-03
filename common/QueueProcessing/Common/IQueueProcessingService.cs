using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.QueueProcessing.Common
{
	public interface IQueueProcessingService
	{
		void StartQueues();
		void StopQueues();
	}
}
