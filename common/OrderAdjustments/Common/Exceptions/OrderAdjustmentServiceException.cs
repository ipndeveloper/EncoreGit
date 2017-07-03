using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.OrderAdjustments.Common.Exceptions
{
    public class OrderAdjustmentServiceException : Exception
    {
        public enum ExceptionKind
        {
            CANNOT_RETRIEVE_ADJUSTMENTS_FOR_ORDER_FAILED_VALIDATION
        }

        public ExceptionKind SpecificKind { get; private set; }

        public OrderAdjustmentServiceException(ExceptionKind exceptionKind, string Message)
            : base(Message)
        {
            SpecificKind = exceptionKind;
        }
    }
}
