
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.OrderAdjustments.Common.Component;
using NetSteps.OrderAdjustments.Common.Model;

namespace NetSteps.OrderAdjustments.Common.Test
{
    [TestClass]
    public class OrderAdjustmentValueCalculatorTest
    {
        [TestMethod]
        public void OrderAdjustmentValueCalculator_should_calculate_order_adjustment_value_correctly_with_flatAmount()
        {
            var calculator = new OrderAdjustmentValueCalculator();
            var result = calculator.CalculateOrderAdjustmentValue(OrderAdjustmentOrderOperationKind.FlatAmount, 7, 10);
            Assert.AreEqual(7, result);
        }

        [TestMethod]
        public void OrderAdjustmentValueCalculator_should_calculate_order_adjustment_value_correctly_with_flatAmount_by_operationID()
        {
            var calculator = new OrderAdjustmentValueCalculator();
            var result = calculator.CalculateOrderAdjustmentValue((int)OrderAdjustmentOrderOperationKind.FlatAmount, 7, 10);
            Assert.AreEqual(7, result);
        }

        [TestMethod]
        public void OrderAdjustmentValueCalculator_should_calculate_order_adjustment_value_correctly_with_multiplier()
        {
            var calculator = new OrderAdjustmentValueCalculator();
            var result = calculator.CalculateOrderAdjustmentValue(OrderAdjustmentOrderOperationKind.Multiplier, .7M, 10);
            Assert.AreEqual(7, result);
        }

        [TestMethod]
        public void OrderAdjustmentValueCalculator_should_calculate_order_adjustment_value_correctly_with_multiplier_by_operationID()
        {
            var calculator = new OrderAdjustmentValueCalculator();
            var result = calculator.CalculateOrderAdjustmentValue((int)OrderAdjustmentOrderOperationKind.Multiplier, .7M, 10);
            Assert.AreEqual(7, result);
        }

        [TestMethod]
        public void OrderAdjustmentValueCalculator_should_calculate_order_line_adjustment_value_correctly_with_flatAmount()
        {
            var calculator = new OrderAdjustmentValueCalculator();
            var result = calculator.CalculateOrderLineAdjustmentValue(OrderAdjustmentOrderLineOperationKind.FlatAmount, 7, 10);
            Assert.AreEqual(7, result);
        }

        [TestMethod]
        public void OrderAdjustmentValueCalculator_should_calculate_order_line_adjustment_value_correctly_with_flatAmount_by_operationID()
        {
            var calculator = new OrderAdjustmentValueCalculator();
            var result = calculator.CalculateOrderLineAdjustmentValue((int)OrderAdjustmentOrderLineOperationKind.FlatAmount, 7, 10);
            Assert.AreEqual(7, result);
        }

        [TestMethod]
        public void OrderAdjustmentValueCalculator_should_calculate_order_line_adjustment_value_correctly_with_multiplier()
        {
            var calculator = new OrderAdjustmentValueCalculator();
            var result = calculator.CalculateOrderLineAdjustmentValue(OrderAdjustmentOrderLineOperationKind.Multiplier, .7M, 10);
            Assert.AreEqual(7, result);
        }

        [TestMethod]
        public void OrderAdjustmentValueCalculator_should_calculate_order_line_adjustment_value_correctly_with_multiplier_by_operationID()
        {
            var calculator = new OrderAdjustmentValueCalculator();
            var result = calculator.CalculateOrderLineAdjustmentValue((int)OrderAdjustmentOrderLineOperationKind.Multiplier, .7M, 10);
            Assert.AreEqual(7, result);
        }
    }
}
