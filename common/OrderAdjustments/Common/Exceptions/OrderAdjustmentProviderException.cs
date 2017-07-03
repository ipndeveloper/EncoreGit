using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.OrderAdjustments.Common.Exceptions
{
    public class OrderAdjustmentProviderException : Exception
    {
        public enum ExceptionKind
        {
            ATTEMPTED_TO_APPLY_SHIPPING_PERCENTAGE_REDUCTION_TO_NULL_SHIPPING_VALUE,
            PROPERTY_KIND_UNDEFINED,
            OPERATION_KIND_UNDEFINED,
            OPERATION_KIND_INVALID,
            ORDER_INVALID_FOR_ADJUSTMENT_APPLICATION

        }

        public ExceptionKind SpecificKind { get; private set; }
        
        public OrderAdjustmentProviderException(ExceptionKind exceptionKind, string Message) : base(Message)
        {
            SpecificKind = exceptionKind;
        }
    }
}
