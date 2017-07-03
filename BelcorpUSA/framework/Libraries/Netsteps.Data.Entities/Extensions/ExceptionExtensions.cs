using System;
using NetSteps.Common.Exceptions;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Diagnostics.Utilities;

namespace NetSteps.Data.Entities.Extensions
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Helper method to find the 'real' exception by checking the base exceptions, turn the exception into a NetSteps exception, 
        /// log it to the database, and return the NetSteps exception.
        /// </summary>
        /// <param name="defaultNetStepsExceptionType">Default NetStepsExceptionType if it can't find an appropriate type.</param>
        public static NetStepsException Log(
            this Exception ex,
            Constants.NetStepsExceptionType defaultNetStepsExceptionType = Constants.NetStepsExceptionType.NetStepsException,
            int? orderID = null,
            int? accountID = null,
            string internalMessage = null)
        {
			if (ex != null)
			{
				ex.TraceException(ex);
			}

            return EntityExceptionHelper.GetAndLogNetStepsException(ex, defaultNetStepsExceptionType, orderID, accountID, internalMessage);
        }
    }
}
