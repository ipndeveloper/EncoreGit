using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Commissions.Common;
using System.Linq.Expressions;

namespace NetSteps.Data.Entities.Extensions
{
    public static class IProductCreditLedgerExtensions
    {
        private static readonly List<short> ActiveOrderStatuses = new List<short>
			{
				(short)Constants.OrderStatus.Pending,
				(short)Constants.OrderStatus.PendingError,
				(short)Constants.OrderStatus.PartiallyPaid,
				(short)Constants.OrderStatus.CreditCardDeclined,
				(short)Constants.OrderStatus.CreditCardDeclinedRetry
			};

        /// <summary>
        /// Gets the current balance less pending payments.
        /// </summary>
        /// <param name="productCreditLedger"></param>
        /// <param name="accountID"></param>
        /// <param name="effectiveDate"></param>
        /// <param name="entryID"></param>
        /// <param name="ignoreOrderPaymentID"></param>
        /// <returns></returns>
        public static decimal GetCurrentBalanceLessPendingPayments(this IProductCreditLedgerService productCreditLedger, int accountID, DateTime? effectiveDate = null, int? entryID = null, int? ignoreOrderPaymentID = null)
        {
            try
            {
                using (var nsContext = new NetStepsEntities())
                {
                    var pendingProductCredits = nsContext.OrderPayments
                                                .Where(x =>
                                                    x.PaymentTypeID == (int)Constants.PaymentType.ProductCredit
                                                    && x.OrderPaymentStatusID == (int)Constants.OrderPaymentStatus.Pending);

                    Expression<Func<OrderPayment, bool>> custPay = op =>
                        ((ignoreOrderPaymentID.HasValue ? op.OrderPaymentID != ignoreOrderPaymentID.Value : true) // Do not count the orderPament that is passed in.
                        && op.OrderCustomerID.HasValue
                        && op.OrderCustomer.AccountID == accountID
                        && ActiveOrderStatuses.Contains(op.Order.OrderStatusID));

                    Expression<Func<OrderPayment, bool>> partyPay = op =>
                        ((ignoreOrderPaymentID.HasValue ? op.OrderPaymentID != ignoreOrderPaymentID.Value : true) // Do not count the orderPament that is passed in.
                        && !op.OrderCustomerID.HasValue
                        && op.Order.ConsultantID == accountID
                        && ActiveOrderStatuses.Contains(op.Order.OrderStatusID));

                    Expression<Func<OrderPayment, bool>> singlePay = op =>
                        ((ignoreOrderPaymentID.HasValue ? op.OrderPaymentID != ignoreOrderPaymentID.Value : true) // Do not count the orderPament that is passed in.
                        && !op.OrderCustomerID.HasValue
                        && op.Order.OrderCustomers.Count == 1
                        && op.Order.OrderCustomers.FirstOrDefault().AccountID == accountID
                        && ActiveOrderStatuses.Contains(op.Order.OrderStatusID));

                    var payments =
                        pendingProductCredits.Where(custPay).Union(pendingProductCredits.Where(partyPay)).Union(
                            pendingProductCredits.Where(singlePay));

                    var paymentsAmount = payments.Any() ? payments.Sum(p => p.Amount) : 0M;

                    var ledgerEntries = productCreditLedger.RetrieveLedger(accountID).Where(l => 
                              (!effectiveDate.HasValue || l.EffectiveDate <= effectiveDate.Value) &&
                              (!entryID.HasValue || l.EntryId < entryID.Value));

                    var ledgerBalance = ledgerEntries.Any() ? ledgerEntries.Sum(l => l.EntryAmount) : 0M;

                    return ledgerBalance - paymentsAmount;
                }
            }
            catch(Exception excp)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(excp, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }
    }
}
