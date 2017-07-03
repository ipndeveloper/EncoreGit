using System;
using NetSteps.Common.Base;
using NetSteps.Common.Configuration;
using NetSteps.Common.Globalization;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Base;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Commissions.Common;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.PaymentGateways
{
	internal class EnrollmentCreditPaymentGateway : BasePaymentGateway
	{
		#region Constructors

		public EnrollmentCreditPaymentGateway(PaymentGatewaySection paymentGatewaySection)
			: base(paymentGatewaySection)
		{

		}

		#endregion

		protected override PaymentAuthorizationResponse ChargePayment()
		{
			decimal currentBalance = 0;
			string errorMessage = string.Empty;

			if (Result.Success)
			{
				Result.Success = ApplyEnrollmentCredit(currentBalance);
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
			if (orderPayment == null)
			{
				if (order.OrderCustomers != null && order.OrderCustomers.Count == 1)
					return order.OrderCustomers[0].AccountID;
				else
					return order.ConsultantID;
			}
			else
			{
				if (orderPayment.OrderCustomer != null)
					return orderPayment.OrderCustomer.AccountID;
				else
				{
					if (order.OrderTypeID == (int)Constants.OrderType.PartyOrder) // Only allowing host credit to be redeemed on party orders for now - JHE
						return order.ConsultantID;
					else
					{
						if (order.OrderCustomers != null && order.OrderCustomers.Count == 1)
							return order.OrderCustomers[0].AccountID;
						else
							return order.ConsultantID;
					}
				}
			}
		}

		public override BasicResponse ValidateCharge(OrderPayment orderPayment, ref decimal currentBalance)
		{
			BasicResponse response = new BasicResponse();

			using (new ApplicationUsageLogger(new ExecutionContext(this)))
			{
				try
				{
					response.Success = true;

				    var service = Create.New<IProductCreditLedgerService>();
				    var ledgerEntryKind = service.GetEntryKind("EC");
				    var accountId = GetAccountID(orderPayment.Order, orderPayment);
				    currentBalance = service.GetCurrentBalance(accountId, ledgerEntryKind);

					if (response.Success && orderPayment.Amount > currentBalance)
					{
						response.Success = false;
						response.Message = Translation.GetTerm("NotEnoughEnrollmentCredit", "Not Enough Enrollment Credit");
					}
				}
				catch (Exception ex)
				{
					throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsPaymentGatewayException);
				}
			}

			return response;
		}

		private bool ApplyEnrollmentCredit(decimal currentBalance)
		{
			////Add a new entry to subtract the amount of the payment - DES
			//ledgerEntry.EntryDate = DateTime.Now;
			//ledgerEntry.EntryOriginID = SmallCollectionCache.Instance.LedgerEntryOrigins.GetByCode("OE").EntryOriginID; //Order Entry
			////TODO: This line could cause problems with international parties, so figure out to do currency exchanges possibly - DES

			var accountId = GetAccountID(CurrentOrderPayment.Order, CurrentOrderPayment);
			var enrollee = Account.LoadFull(accountId);

			var mainAddress = enrollee.Addresses.GetDefaultByTypeID(Constants.AddressType.Main) ?? new Address();
            var currency = SmallCollectionCache.Instance.Currencies.GetById(SmallCollectionCache.Instance.Countries.GetById(mainAddress.CountryID).CurrencyID);
            
            var amount = Math.Abs(CurrentOrderPayment.Amount) * -1;
			var effectiveDate = DateTime.Now;
			int? bonusTypeId = null;
			var userId = ApplicationContext.Instance.CurrentUserID;

            const string entryDescription = "Enrollment Product Credit";
            const string notes = "Enrollment Product Credit";

            var service = Create.New<IProductCreditLedgerService>();
            var entryReasonId = service.GetEntryReason("PCC").EntryReasonId;
            var ledgerEntryTypeId = service.GetEntryKind("EC").LedgerEntryKindId;

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