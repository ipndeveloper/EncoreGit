using System;
using System.Threading;

namespace NetSteps.EventProcessing.Service
{
	public class EventHandlerProcess<T>
	{
		public EventHandlerProcess(Action preExecution, Action<T> execution, T executionParameter, Action postExecution)
		{
			PreExecution = preExecution;
			Execution = execution;
			ExecutionParameter = executionParameter;
			PostExecution = postExecution;
		}

		public Action PostExecution { get; private set; }
		public Action<T> Execution { get; private set; }
		public T ExecutionParameter { get; private set; }
		public Action PreExecution { get; private set; }

		public void QueueExecutionAndPostExecution()
		{
			ThreadPool.QueueUserWorkItem(threadExecutionPostExecution, ExecutionParameter);
		}

		private void threadExecutionPostExecution(object executionParameter)
		{
			Execution.Invoke((T)executionParameter);
			PostExecution.Invoke();
		}
	}
}
