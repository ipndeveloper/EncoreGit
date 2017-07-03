using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.OrderAdjustments.Common.Model;

namespace NetSteps.OrderAdjustments.Common
{
    public interface IOrderAdjustmentValueCalculator
    {
        decimal CalculateOrderAdjustmentValue(OrderAdjustmentOrderOperationKind operation, decimal adjustmentOperand, decimal originalValue);
        decimal CalculateOrderLineAdjustmentValue(OrderAdjustmentOrderLineOperationKind operation, decimal adjustmentOperand, decimal orignalValue);
        decimal CalculateOrderAdjustmentValue(int operationKindID, decimal adjustmentOperand, decimal originalValue);
        decimal CalculateOrderLineAdjustmentValue(int operationKindID, decimal adjustmentOperand, decimal orignalValue);
    }
}
