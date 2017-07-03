using System;
using NetSteps.Common.Configuration;
using NetSteps.Data.Entities.Base;

namespace NetSteps.Data.Entities.PaymentGateways
{
    /// <summary>
    /// EFT payment gateway implementation.
    /// </summary>
    internal class EFTPaymentGateway : BasePaymentGateway
    {

        #region Constructors

        public EFTPaymentGateway(PaymentGatewaySection paymentGatewaySection)
            : base(paymentGatewaySection)
        {

        }

        #endregion

        protected override PaymentAuthorizationResponse ChargePayment()
        {
            Result.Success = false;

            if (CurrentOrderPayment.Amount <= 0)
            {
                Result.Message = string.Empty;
                Result.Success = false;
                Result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Error;
                return Result;
            }
            else
            {
                Result.Message = "Authorized";
                Result.Success = true;
                Result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Authorized;
            }

            return Result;
        }

        protected override PaymentAuthorizationResponse RefundPayment(decimal ammount)
        {
            throw new NotImplementedException();
        }

        protected override void SetTransactionChargeOrderPaymentResult()
        {
            CashRecordTransaction();
        }

        protected override void SetRecordTransactionRefundOrderPaymentResult()
        {
            throw new NotImplementedException();
        }

        protected void CashRecordTransaction()
        {
            CurrentOrderPaymentResult.DecryptedAccountNumber = CurrentOrderPayment.DecryptedAccountNumber;
            CurrentOrderPaymentResult.ApprovalCode = string.Empty;
            CurrentOrderPaymentResult.ResponseReasonCode = string.Empty;
            CurrentOrderPaymentResult.ResponseCode = string.Empty;
            CurrentOrderPaymentResult.ResponseSubCode = string.Empty;
            CurrentOrderPaymentResult.ResponseReasonText = string.Empty;
            CurrentOrderPaymentResult.AuthorizeType = string.Empty;
            CurrentOrderPaymentResult.CardCodeResponse = string.Empty;
            CurrentOrderPaymentResult.ErrorMessage = string.Empty;
            CurrentOrderPaymentResult.Response = string.Empty;
            CurrentOrderPaymentResult.AVSResult = string.Empty;
            CurrentOrderPaymentResult.TransactionID = string.Empty;
        }
    }
}