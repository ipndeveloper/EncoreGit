using System;
using System.Data;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Exceptions
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Helper class to reduce the lines of code needed wrap code in Exception handling code with Usage Logging. - JHE
	/// Created: 3/12/2010
	/// </summary>
	public class ExceptionHandledDataAction
	{
		public static T Run<T>(ExecutionContext executionContext, Func<T> action)
		{
			try
			{
				using (new ApplicationUsageLogger(executionContext))
				{
					if (action != null)
					{
						return action();
					}

					throw EntityExceptionHelper.GetAndLogNetStepsException("Action must be set in ExceptionHandledDataAction.");
				}
			}
			catch (OptimisticConcurrencyException ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsOptimisticConcurrencyException);
			}
			catch (Exception ex)
			{
				throw ex is NetStepsException ? ex : EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsDataException);
			}
		}

		/// <summary>
		/// Helper class to reduce the lines of code needed wrap code in Exception handling code with Usage Logging. - JHE
		/// </summary>
		/// <param name="action"></param>
		public static void Run(ExecutionContext executionContext, Action action)
		{
			try
			{
				using (new ApplicationUsageLogger(executionContext))
				{
					if (action != null)
					{
						action();
					}
					else
					{
						throw EntityExceptionHelper.GetAndLogNetStepsException("Action must be set in ExceptionHandledDataAction.");
					}
				}
			}
			catch (OptimisticConcurrencyException ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsOptimisticConcurrencyException);
			}
			catch (Exception ex)
			{
				throw ex is NetStepsException ? ex : EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsDataException);
			}
		}
	}
}
