using System;
using System.Threading;
using NetSteps.Common.Extensions;

namespace NetSteps.Common.Threading
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Some helper methods when dealing with threads/threading.
	/// Created: 02-14-2011
	/// </summary>
	public class ThreadHelpers
	{
		// http://stackoverflow.com/questions/299198/implement-c-generic-timeout
		public static void RunWithTimeout(Action action, TimeSpan timeout)
		{
			try
			{
				Thread threadToKill = null;
				Action wrappedAction = () =>
				{
					threadToKill = Thread.CurrentThread;
					action();
				};
				IAsyncResult result = wrappedAction.BeginInvoke(null, null);
				if (result.AsyncWaitHandle.WaitOne(timeout.TotalMilliseconds.ToInt()))
				{
					wrappedAction.EndInvoke(result);
				}
				else
				{
					threadToKill.Abort();
					throw new TimeoutException();
				}
			}
			catch (TimeoutException)
			{

			}
		}
	}
}
