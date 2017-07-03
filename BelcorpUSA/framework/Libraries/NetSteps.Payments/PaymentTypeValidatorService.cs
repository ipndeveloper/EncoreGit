using System.Collections.Generic;
using System.Linq;

using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Interfaces;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Payments
{
	[ContainerRegister(typeof(IPaymentTypeValidator), RegistrationBehaviors.Default)]
	public class PaymentTypeValidatorService : IPaymentTypeValidator
	{
		#region IPaymentTypeValidator
		public BasicResponse IsValidPayment(Order order, IPayment payment, decimal amount)
		{
			OrderTotals remainingTotals = GetCurrentBestCaseTotals(order);

			if (remainingTotals.Subtotal < 0)
			{
				return new BasicResponse { Success = false, Message = Translation.GetTerm("CurrentPaymentsHaveReducedSubtotalBelowZero", "Current payments have reduced the subtotal below zero") };
			}

			if (remainingTotals.ShippingHandling < 0)
			{
				return new BasicResponse { Success = false, Message = Translation.GetTerm("CurrentPaymentsHaveReducedShippingHandlingBelowZero", "Current payments have reduced the shipping/handling below zero") };
			}

			if (remainingTotals.Tax < 0)
			{
				return new BasicResponse { Success = false, Message = Translation.GetTerm("CurrentPaymentsHaveReducedTaxBelowZero", "Current payments have reduced the tax below zero") };
			}

			if (remainingTotals.Subtotal > 0 && remainingTotals.Unapplied < (remainingTotals.Tax + remainingTotals.ShippingHandling + remainingTotals.Subtotal))
			{
				return new BasicResponse { Success = true };
			}

			if (payment.CanPayForTax)
			{
				amount -= remainingTotals.Tax;
			}

			if (payment.CanPayForShippingAndHandling)
			{
				amount -= remainingTotals.ShippingHandling.GetRoundedNumber();
			}

			amount += remainingTotals.Unapplied;
			amount -= remainingTotals.Subtotal;

			if (amount <= 0)
				return new BasicResponse { Success = true };

			return new BasicResponse { Success = false, Message = Translation.GetTerm("NewPaymentCannotFitOnOrderBecauseOfOtherPaymentRestrictions", "New payment cannot fit on order because of other payment restrictions") };
		}

		public decimal DetermineNewPaymentAmount(Order order, IPayment payment, decimal amount)
		{
			OrderTotals remainingTotals = GetCurrentBestCaseTotals(order);

			decimal canPayForAmount = 0;
            //if (payment.CanPayForTax)
            //{
            //    // Shift tax into the can pay
            //    canPayForAmount += remainingTotals.Tax;
            //    remainingTotals.Tax = 0;
            //}

            //if (payment.CanPayForShippingAndHandling)
            //{
            //    // Shift shipping/handling into the can pay
            //    canPayForAmount += remainingTotals.ShippingHandling;
            //    remainingTotals.ShippingHandling = 0;
            //}
            canPayForAmount += remainingTotals.Tax;
            remainingTotals.Tax = 0;

            canPayForAmount += remainingTotals.ShippingHandling;
            remainingTotals.ShippingHandling = 0;

			ApplyUnpaidToTaxShippingHandling(ref remainingTotals);
			canPayForAmount += remainingTotals.Subtotal;
			canPayForAmount -= remainingTotals.Unapplied;

			return canPayForAmount < amount ? canPayForAmount : amount;
		}

		private void ApplyUnpaidToTaxShippingHandling(ref OrderTotals remainingTotals)
		{
			if (remainingTotals.Unapplied > 0 && remainingTotals.Tax > 0)
			{
				if (remainingTotals.Unapplied >= remainingTotals.Tax)
				{
					remainingTotals.Unapplied -= remainingTotals.Tax;
					remainingTotals.Tax = 0;
				}
				else
				{
					remainingTotals.Tax -= remainingTotals.Unapplied;
					remainingTotals.Unapplied = 0;
				}

			}

			if (remainingTotals.Unapplied > 0 && remainingTotals.ShippingHandling > 0)
			{
				if (remainingTotals.Unapplied >= remainingTotals.ShippingHandling)
				{
					remainingTotals.Unapplied -= remainingTotals.ShippingHandling;
					remainingTotals.ShippingHandling = 0;
				}
				else
				{
					remainingTotals.ShippingHandling -= remainingTotals.Unapplied;
					remainingTotals.Unapplied = 0;
				}
			}

		}

		#endregion

		#region Calculate Totals
		private OrderTotals GetCurrentBestCaseTotals(Order order)
		{
			List<OrderPayment> orderPayments = order.OrderPayments
				.Where(op => op.OrderPaymentStatusID == (int)Constants.OrderPaymentStatus.Pending || op.OrderPaymentStatusID == (int)Constants.OrderPaymentStatus.Completed).ToList();

			decimal subtotalOnly = orderPayments.Where(op => !op.CanPayForTax && !op.CanPayForShippingAndHandling).Sum(op => op.Amount);
			decimal taxAndSubtotal = orderPayments.Where(op => op.CanPayForTax && !op.CanPayForShippingAndHandling).Sum(op => op.Amount);
			decimal shippingAndSubtotal = orderPayments.Where(op => !op.CanPayForTax && op.CanPayForShippingAndHandling).Sum(op => op.Amount);
			decimal anyPayment = orderPayments.Where(op => op.CanPayForTax && op.CanPayForShippingAndHandling).Sum(op => op.Amount);

			// Tax can include a fraction of a cent. At some point we need to have tax rounded in a sane location.
			// Shipping and handling is the same situation.
			var orderTotals = new OrderTotals(
				order.Subtotal ?? 0,
				order.TaxAmountTotalOverride != null ? order.TaxAmountTotalOverride.Value.GetRoundedNumber() : order.TaxAmountTotal != null ? order.TaxAmountTotal.GetRoundedNumber() : 0,
				(order.ShippingTotal ?? 0).GetRoundedNumber() + (order.HandlingTotal ?? 0).GetRoundedNumber()
				);

			orderTotals.Subtotal -= subtotalOnly;

			if (taxAndSubtotal > orderTotals.Tax)
			{
				taxAndSubtotal -= orderTotals.Tax;
				orderTotals.Tax = 0;
				orderTotals.Subtotal -= taxAndSubtotal;
			}
			else
			{
				orderTotals.Tax -= taxAndSubtotal;
			}

			if (shippingAndSubtotal > orderTotals.ShippingHandling)
			{
				shippingAndSubtotal -= orderTotals.ShippingHandling;
				orderTotals.ShippingHandling = 0;
				orderTotals.Subtotal -= shippingAndSubtotal;
			}
			else
			{
				orderTotals.ShippingHandling -= shippingAndSubtotal;
			}

			orderTotals.Unapplied = anyPayment;

			return orderTotals;
		}
		#endregion

		/// <summary>
		/// A simple data structure used to store the temporary totals of an order
		/// </summary>
		private struct OrderTotals
		{
			/// <summary>
			/// Remaining subtotal in the order
			/// </summary>
			public decimal Subtotal;

			/// <summary>
			/// Remaining tax in the order
			/// </summary>
			public decimal Tax;

			/// <summary>
			/// Remaining ShippingHandling in the order
			/// </summary>
			public decimal ShippingHandling;

			/// <summary>
			/// This is the total of payments that can go to any field
			/// </summary>
			public decimal Unapplied;

			/// <summary>
			/// Initializes a new instance of the OrderTotals struct
			/// </summary>
			/// <param name="subtotal">Subtotal of the order</param>
			/// <param name="tax">Tax of the order</param>
			/// <param name="shippingHandling">Shipping/Handling of the order</param>
			public OrderTotals(decimal subtotal, decimal tax, decimal shippingHandling)
				: this()
			{
				Subtotal = subtotal;
				Tax = tax;
				ShippingHandling = shippingHandling;
				Unapplied = 0;
			}
		}
	}
}
