using System;
using System.Diagnostics.Contracts;
using NetSteps.Commissions.Common;
using NetSteps.Common.Base;
using NetSteps.Common.Configuration;
using NetSteps.Common.Globalization;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Base;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.PaymentGateways
{
    internal class ProductCreditPaymentGateway : BasePaymentGateway
    {
        #region Constructors

        public ProductCreditPaymentGateway(PaymentGatewaySection paymentGatewaySection)
            : base(paymentGatewaySection)
        {

        }

        #endregion

        protected override PaymentAuthorizationResponse ChargePayment()
        {
			var currentOrderPayment = CurrentOrderPayment;
			if (currentOrderPayment.Order == null && currentOrderPayment.OrderID > 0)
			{
				currentOrderPayment.Order = Order.Load(currentOrderPayment.OrderID);
			}

			if (currentOrderPayment.Order == null)
			{
				Result.Success = false;
			}
		

            if (Result.Success)
            {
				Result.Success = ApplyProductCredit(0, currentOrderPayment);
            }

            if (Result.Success)
            {
                Result.Message = "Authorized";
                Result.Success = true;
                Result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Authorized;
            }
            else
            {
                //Result.Message = errorMessage;
                Result.Success = false;
                Result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Declined;
                return Result;
            }

            return Result;
        }

        public int GetAccountID(Order order, OrderPayment orderPayment = null)
        {
			Contract.Requires<ArgumentNullException>(order != null);
            if (orderPayment == null)
            {
                if (order.OrderCustomers != null && order.OrderCustomers.Count == 1)
				{
                    return order.OrderCustomers[0].AccountID;
				}
					
					return order.ConsultantID;
            }
			
				if (orderPayment.OrderCustomer != null)
			{
					return orderPayment.OrderCustomer.AccountID;
			}
					if (order.OrderTypeID == (int)Constants.OrderType.PartyOrder) // Only allowing host credit to be redeemed on party orders for now - JHE
			{
						return order.ConsultantID;
			}
						if (order.OrderCustomers != null && order.OrderCustomers.Count == 1)
			{
							return order.OrderCustomers[0].AccountID;
					}

			return order.ConsultantID;
        }

        public override BasicResponse ValidateCharge(OrderPayment orderPayment, ref decimal currentBalance)
        {
            using (new ApplicationUsageLogger(new ExecutionContext(this)))
            {
                try
                {
					var response = new BasicResponse { Success = true };

                    currentBalance = Create.New<IProductCreditLedgerService>().GetCurrentBalanceLessPendingPayments(GetAccountID(orderPayment.Order, orderPayment), null, null, orderPayment.OrderPaymentID);
					if (orderPayment.Amount > currentBalance)
                    {
                        response.Success = false;
                        response.Message = Translation.GetTerm("NotEnoughProductCredit", "Not Enough Product Credit");
                    }

					return response;
                }
                catch (Exception ex)
                {
                    throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsPaymentGatewayException);
                }
            }
        }

		private bool ApplyProductCredit(decimal currentBalance, OrderPayment currentOrderPayment)
        {
			Contract.Requires<ArgumentNullException>(currentOrderPayment != null);
			Contract.Requires<ArgumentException>(currentOrderPayment.Order != null);

			//If both are included, original check to see if amount is over grand total already takes care of this
            ////Add a new entry to subtract the amount of the payment - DES
            //ledgerEntry.EntryDate = DateTime.Now;
            //ledgerEntry.EntryOriginID = SmallCollectionCache.Instance.LedgerEntryOrigins.GetByCode("OE").EntryOriginID; //Order Entry
            ////TODO: This line could cause problems with international parties, so figure out to do currency exchanges possibly - DES


			var accountId = GetAccountID(currentOrderPayment.Order, currentOrderPayment);
            var consultant = Account.LoadFull(accountId);

            var mainAddress = consultant.Addresses.GetDefaultByTypeID(Constants.AddressType.Main) ?? new Address();
            var currency = SmallCollectionCache.Instance.Currencies.GetById(SmallCollectionCache.Instance.Countries.GetById(mainAddress.CountryID).CurrencyID);
            
            var amount = Math.Abs(currentOrderPayment.Amount) * -1;
            var effectiveDate = DateTime.Now;
            int? bonusTypeId = null;
            var userId = ApplicationContext.Instance.CurrentUserID;

            const string entryDescription = "Product Credit";
            const string notes = "Product Credit";

            var service = Create.New<IProductCreditLedgerService>();
            var entryReasonId = service.GetEntryReason("CR").EntryReasonId;
            var ledgerEntryTypeId = service.GetEntryKind("GA").LedgerEntryKindId;
            
		    service.AddLedgerEntry(
		        accountId
		        , amount
		        , effectiveDate
		        , entryDescription
		        , entryReasonId
		        , ledgerEntryTypeId
		        , bonusTypeId ?? 0
		        , notes
                , currency.CurrencyID
		        , userId);

            return true;
        }

        protected override PaymentAuthorizationResponse RefundPayment(decimal ammount)
        {
            throw new NotImplementedException();
        }

        protected override void SetTransactionChargeOrderPaymentResult()
        {
            //Nothing Use Defaults.
        }

        protected override void SetRecordTransactionRefundOrderPaymentResult()
        {
            throw new NotImplementedException();
        }
    }
}