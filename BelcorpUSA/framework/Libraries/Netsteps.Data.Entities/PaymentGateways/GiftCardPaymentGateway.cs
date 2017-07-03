using System;
using NetSteps.Common.Base;
using NetSteps.Common.Configuration;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities.Base;
using NetSteps.Encore.Core.IoC;
using NetSteps.GiftCards.Common;

namespace NetSteps.Data.Entities.PaymentGateways
{
    public class GiftCardPaymentGateway : BasePaymentGateway
    {

        #region Constructors

        public GiftCardPaymentGateway(PaymentGatewaySection paymentGatewaySection)
            : base(paymentGatewaySection)
        {

        }

        #endregion

        protected override PaymentAuthorizationResponse ChargePayment()
        {
            if (this.Result.Success)
            {
                var gcService = Create.New<IGiftCardService>();

                var gc = gcService.FindByCodeAndCurrency(
                    this.CurrentOrderPayment.DecryptedAccountNumber, this.CurrentOrderPayment.Order.CurrencyID);
                gc.Balance -= this.CurrentOrderPayment.Amount;
                gcService.Update(gc);
                Result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Authorized;
                Result.Message = "Authorized";

                // save GiftCardID onto OP.TransactionID so it's easier to join to OP for ledger purposes
                this.CurrentOrderPayment.TransactionID = gc.GiftCardID.ToString();
            }

            return this.Result;
        }

        public override BasicResponse ValidateCharge(OrderPayment orderPayment, ref decimal currentBalance)
        {
            var gcService = Create.New<IGiftCardService>();

            var gc = gcService.FindByCodeAndCurrency(orderPayment.DecryptedAccountNumber, orderPayment.Order.CurrencyID);
            currentBalance = gcService.GetBalanceWithPendingPayments(orderPayment.DecryptedAccountNumber, orderPayment.Order) ?? 0m;

            if (gc == null)
                return new BasicResponse()
                {
                    Success = false,
                    Message = Translation.GetTerm("GiftCardNotFound", "There was no gift card found with that code")
                };
            else if (orderPayment.Amount <= 0m)
                return new BasicResponse()
                {
                    Success = false,
                    Message = Translation.GetTerm("GiftCardZeroAmount", "Cannot apply a 0 amount to a gift card")
                };
            else if (currentBalance < 0m)
                return new BasicResponse()
                {
                    Success = false,
                    Message = Translation.GetTerm("GiftCardInsufficientFunds", "There were insufficient funds on gift card: {0}", orderPayment.DecryptedAccountNumber ?? string.Empty)
                };
            else if (gc.ExpirationDate != null && gc.ExpirationDate < DateTime.UtcNow)
                return new BasicResponse()
                {
                    Success = false,
                    Message = Translation.GetTerm("GiftCardInvalidExirationDate", "The following gift card has expired: {0}", orderPayment.DecryptedAccountNumber ?? string.Empty)
                };
            else
                return new BasicResponse() { Success = true };
        }

        protected override void SetTransactionChargeOrderPaymentResult()
        {
            //Nothing Use Defaults.
        }

        protected override PaymentAuthorizationResponse RefundPayment(decimal ammount)
        {
            throw new NotImplementedException();
        }

        public override BasicResponse ValidateRefund(OrderPayment orderPayment, ref decimal currentBalance)
        {
            throw new NotImplementedException();
        }

        protected override void SetRecordTransactionRefundOrderPaymentResult()
        {
            throw new NotImplementedException();
        }
    }
}