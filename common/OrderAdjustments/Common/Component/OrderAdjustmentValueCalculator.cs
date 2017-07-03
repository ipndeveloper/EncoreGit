using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.OrderAdjustments.Common.Model;

namespace NetSteps.OrderAdjustments.Common.Component
{
    public class OrderAdjustmentValueCalculator : IOrderAdjustmentValueCalculator
    {


        public decimal CalculateOrderAdjustmentValue(OrderAdjustmentOrderOperationKind operation, decimal adjustmentOperand, decimal originalValue)
        {
            switch (operation)
            {
                case OrderAdjustmentOrderOperationKind.FlatAmount:
                    return adjustmentOperand;
                case OrderAdjustmentOrderOperationKind.Multiplier:
                    return originalValue * adjustmentOperand;
                default:
                    throw new InvalidOperationException("OrderAdjustmentValueCalculator was passed an invalid order operation kind.");
            }
        }

        public decimal CalculateOrderLineAdjustmentValue(OrderAdjustmentOrderLineOperationKind operation, decimal adjustmentOperand, decimal originalValue)
        {
            switch (operation)
            {
                case OrderAdjustmentOrderLineOperationKind.AddedItem:
                    return originalValue;
                case OrderAdjustmentOrderLineOperationKind.Multiplier:
                    return originalValue * adjustmentOperand;
                case OrderAdjustmentOrderLineOperationKind.FlatAmount:
                    return adjustmentOperand;
                default:
                    throw new InvalidOperationException("OrderAdjustmentValueCalculator was passed an invalid order line operation kind.");
            }
        }

        public decimal CalculateOrderAdjustmentValue(int operationKindID, decimal adjustmentOperand, decimal originalValue)
        {
            return CalculateOrderAdjustmentValue((OrderAdjustmentOrderOperationKind)operationKindID, adjustmentOperand, originalValue);
        }

        public decimal CalculateOrderLineAdjustmentValue(int operationKindID, decimal adjustmentOperand, decimal originalValue)
        {
            return CalculateOrderLineAdjustmentValue((OrderAdjustmentOrderLineOperationKind)operationKindID, adjustmentOperand, originalValue);
        }
    }
}
