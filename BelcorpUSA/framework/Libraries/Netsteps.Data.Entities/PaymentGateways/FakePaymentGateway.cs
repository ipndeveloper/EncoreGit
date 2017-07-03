using System;
using NetSteps.Addresses.Common.Models;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Base;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.PaymentGateways
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Used for testing.
    /// Created: 04-16-2010
    /// </summary>
    internal class FakePaymentGateway : BasePaymentGateway
    {
        #region Constructors

        public FakePaymentGateway(PaymentGatewaySection paymentGatewaySection)
            : base(paymentGatewaySection)
        {

        }

        #endregion

        #region Methods
        protected override PaymentAuthorizationResponse ChargePayment()
        {
            using (new ApplicationUsageLogger(new ExecutionContext(this)))
            {
                PaymentAuthorizationResponse result = new PaymentAuthorizationResponse();

                bool liveMode = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.IsPaymentLiveMode);
                bool testTransaction = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.IsPaymentTestTransaction);
                string authorizeId;
                string authorizePin;
                //bool success = false;

                if (CurrentOrderPayment.Amount <= 0)
                {
                    // No amount will be charged. Tests are done with 0.01 charged.  
                    // Just return true since some products have no cost.
                    result.Message = string.Empty;
                    result.Success = true;
                    result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Error;
                    return result;
                    //result.Message = "Invalid amount. The amount must be greater than zero.";
                    //result.Success = false;
                    //return result;
                }
                // Prevents duplicate payments.
                if (!string.IsNullOrEmpty(CurrentOrderPayment.TransactionID))
                {
                    result.Message = "This transaction has already been processes.";
                    result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Error;
                    return result;
                }

                GatewayChargeRequest chargeRequest = new GatewayChargeRequest();

                if (!liveMode)
                {
                    authorizeId = "6zz6m5N4Et"; //NetSteps Test ID.
                    authorizePin = "9V9wUv6Yd92t27t5";
                    //chargeRequest.Amount = 0.01m;
                }
                else
                {
                    authorizeId = _paymentGatewaySection.Login;
                    authorizePin = _paymentGatewaySection.Password;
                }

                chargeRequest.Testing = testTransaction;
                chargeRequest.LoginID = authorizeId;
                chargeRequest.TransactionKey = authorizePin;
                chargeRequest.AuthorizeType = NetSteps.Data.Entities.PaymentGateways.AuthorizeNet.ANetAuthorizeType.AUTH_CAPTURE;

                IAddress address = CurrentOrderPayment as IAddress;

                chargeRequest.BillingFirstName = CurrentOrderPayment.BillingFirstName.RemoveDiacritics();
                chargeRequest.BillingLastName = CurrentOrderPayment.BillingLastName.RemoveDiacritics();
                chargeRequest.BillingStreetAddress = address.Address1;
                chargeRequest.BillingCity = address.City.RemoveDiacritics();
                chargeRequest.BillingState = address.State.RemoveDiacritics();
                chargeRequest.BillingZip = address.PostalCode;
                if (!string.IsNullOrEmpty(address.Country))
                    chargeRequest.BillingCountry = address.Country.RemoveDiacritics();
                chargeRequest.BillingPhone = CurrentOrderPayment.BillingPhoneNumber;

                chargeRequest.CardNum = CurrentOrderPayment.DecryptedAccountNumber; // Encryption.DecryptTripleDES(orderPayment.EncryptedAccountNumber, Constants.key, Constants.salt);
                chargeRequest.ExpDate = CurrentOrderPayment.ExpirationDate.Value.ToExpirationString(); // TODO: I don't think the expiration format is correct. - JHE
                chargeRequest.CardCode = CurrentOrderPayment.Cvv;

                // Prevent accidental hold on account in live mode but test transaction, force to 1 penny.
                if (testTransaction)
                    chargeRequest.Amount = 0.01m;
                else
                    chargeRequest.Amount = CurrentOrderPayment.Amount;

                if (CurrentOrderPayment.OrderCustomer != null)
                {
                    chargeRequest.CompanyID = CurrentOrderPayment.OrderCustomer.OrderCustomerID.ToString();
                    chargeRequest.CompanyName = CurrentOrderPayment.OrderCustomer.FullName.RemoveDiacritics();
                    chargeRequest.BillingEmail = CurrentOrderPayment.OrderCustomer.AccountInfo.EmailAddress;
                }

                chargeRequest.Description = CurrentOrderPayment.Order.OrderID + "_" + CurrentOrderPayment.OrderPaymentID + "_" +
                                            CurrentOrderPayment.Order.ConsultantInfo.AccountNumber;
                chargeRequest.InvoiceNumber = CurrentOrderPayment.Order.OrderNumber;

                GatewayResponse chargeResponse = ChargeCard(chargeRequest);

                FakeRecordTransaction(chargeRequest, chargeResponse);
                base.RecordTransaction();

                result.Message = chargeResponse.ResponseReasonText;

                switch (chargeResponse.ErrorLevel)
                {
                    case NetSteps.Data.Entities.PaymentGateways.AuthorizeNet.ErrorLevel.Approved:
                        CurrentOrderPayment.TransactionID = chargeResponse.TransactionID;
                        result.Success = true;
                        result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Authorized;
                        break;
                    case NetSteps.Data.Entities.PaymentGateways.AuthorizeNet.ErrorLevel.Declined:
                        result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Declined;
                        break;
                    case NetSteps.Data.Entities.PaymentGateways.AuthorizeNet.ErrorLevel.ANetError:
                        result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Error;
                        break;
                    case NetSteps.Data.Entities.PaymentGateways.AuthorizeNet.ErrorLevel.HTTPRequestError:
                        result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Error;
                        result.Success = false;
                        break;
                    default:
                        result.Success = false;
                        break;
                }

                CurrentOrderPayment.ProcessedDate = CurrentOrderPayment.ProcessOnDate = DateTime.Now;
                CurrentOrderPayment.Save();

                return result;
            }
        }

        protected override PaymentAuthorizationResponse RefundPayment(decimal amount)
        {
            using (new ApplicationUsageLogger(new ExecutionContext(this)))
            {
                PaymentAuthorizationResponse result = new PaymentAuthorizationResponse();

                bool liveMode = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.IsPaymentLiveMode);
                bool testTransaction = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.IsPaymentTestTransaction);
                string authorizeId;
                string authorizePin;
                //bool success = false;
                decimal calcAmount = amount;

                if (liveMode && string.IsNullOrEmpty(CurrentOrderPayment.TransactionID))
                {
                    result.Message = "A valid transaction Id is needed";
                    result.Success = false;
                    result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Error;
                    return result;
                }

                if (calcAmount < 0)
                    calcAmount = 0 - calcAmount; // make it a positive amount for comparison
                if (calcAmount > CurrentOrderPayment.Amount)
                {
                    result.Message = "The amount being requested for refund must be less than or equal to the original settled amount.";
                    result.Success = false;
                    result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Error;
                    return result;
                }

                GatewayChargeRequest chargeRequest = new GatewayChargeRequest();

                //var connectionInfo = PaymentGatewaySection.Instance.FirstOrDefault(p => p.Namespace == CurrentPaymentGateway.Namespace);

                authorizeId = _paymentGatewaySection.Login;
                authorizePin = _paymentGatewaySection.Password;
                if (!liveMode)
                {
                    authorizeId = "6zz6m5N4Et"; // NetSteps Test ID.
                    authorizePin = "9V9wUv6Yd92t27t5";
                    //chargeRequest.Amount = 0.01m;
                }

                chargeRequest.Testing = testTransaction;
                chargeRequest.LoginID = authorizeId;
                chargeRequest.TransactionKey = authorizePin;
                chargeRequest.AuthorizeType = AuthorizeNet.ANetAuthorizeType.VOID; // See comments below by JLS for why this is VOID instead of CREDIT
                chargeRequest.TransactionID = CurrentOrderPayment.TransactionID;

                IAddress address = CurrentOrderPayment as IAddress;

                chargeRequest.BillingFirstName = address.FirstName;
                chargeRequest.BillingLastName = address.LastName;
                chargeRequest.BillingStreetAddress = address.Address1;
                chargeRequest.BillingCity = address.City;
                chargeRequest.BillingState = address.State;
                chargeRequest.BillingZip = address.PostalCode;
                if (!string.IsNullOrEmpty(address.Country))
                    chargeRequest.BillingCountry = address.Country;
                chargeRequest.BillingPhone = CurrentOrderPayment.BillingPhoneNumber;

                chargeRequest.CardNum = CurrentOrderPayment.DecryptedAccountNumber; // Encryption.DecryptTripleDES(orderPayment.EncryptedAccountNumber, Constants.key, Constants.salt);
                chargeRequest.ExpDate = CurrentOrderPayment.ExpirationDate.Value.ToExpirationString();
                chargeRequest.CardCode = CurrentOrderPayment.Cvv;

                //Prevent accidental hold on account in live mode but test transaction, force to 1 penny.
                if (testTransaction)
                    chargeRequest.Amount = 0.01m;
                else
                    chargeRequest.Amount = calcAmount;

                if (CurrentOrderPayment.OrderCustomer != null)
                {
                    chargeRequest.CompanyID = CurrentOrderPayment.OrderCustomer.OrderCustomerID.ToString();
                    chargeRequest.CompanyName = CurrentOrderPayment.OrderCustomer.FullName;
                    chargeRequest.BillingEmail = CurrentOrderPayment.OrderCustomer.AccountInfo.EmailAddress;
                }

                chargeRequest.Description = CurrentOrderPayment.Order.OrderID + "_" + CurrentOrderPayment.OrderPaymentID + "_" +
                                            CurrentOrderPayment.Order.ConsultantInfo.AccountNumber + "_Refund";
                chargeRequest.InvoiceNumber = CurrentOrderPayment.Order.OrderNumber;

                // JLS 09-16-2009
                // The idea behind the refund is to first try voiding the transaction, and if that doesn't work to try crediting the transaction.
                // This is as per instructed by the Authorize.NET documentation.  
                //
                // I changed the AuthorizeType above to VOID, instead of CREDIT.  I'm adding an immediate check after processing the transaction below to see if it failed.  If so
                // I'm changing it back to CREDIT and processing it again.  All other logic/processing should remain the same;

                GatewayResponse chargeResponse = ChargeCard(chargeRequest);

                if (chargeResponse.ErrorLevel == AuthorizeNet.ErrorLevel.ANetError || chargeResponse.ErrorLevel == AuthorizeNet.ErrorLevel.Declined)
                {
                    // Reverting back to a CREDIT
                    chargeRequest.AuthorizeType = NetSteps.Data.Entities.PaymentGateways.AuthorizeNet.ANetAuthorizeType.CREDIT;
                    chargeResponse = ChargeCard(chargeRequest);
                }

                result.Message = chargeResponse.ResponseReasonText;

                chargeRequest.Amount = calcAmount; // Restore the original amount (in case it was modified for a test transaction)
                if (chargeRequest.Amount > 0)
                    chargeRequest.Amount = 0 - chargeRequest.Amount; // make it a negative (credit) amount for database storage

                FakeRecordTransaction(chargeRequest, chargeResponse);
                base.RecordTransaction();

                switch (chargeResponse.ErrorLevel)
                {
                    case NetSteps.Data.Entities.PaymentGateways.AuthorizeNet.ErrorLevel.Approved:
                        /// Because a payment needs to submit the original transactionid we will not update the payment transactionid, it can be
                        /// accessed via the payment result.  --BJC
                        //orderPayment.TransactionId = chargeResponse.TransactionID;
                        CurrentOrderPayment.OrderPaymentStatusID = Constants.OrderPaymentStatus.Completed.ToShort();
                        result.Success = true;
                        result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Authorized;
                        break;
                    case NetSteps.Data.Entities.PaymentGateways.AuthorizeNet.ErrorLevel.Declined:
                        result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Declined;
                        break;
                    case NetSteps.Data.Entities.PaymentGateways.AuthorizeNet.ErrorLevel.ANetError:
                        result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Error;
                        break;
                    case NetSteps.Data.Entities.PaymentGateways.AuthorizeNet.ErrorLevel.HTTPRequestError:
                        result.Success = false;
                        result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Error;
                        break;
                }

                CurrentOrderPayment.ProcessedDate = CurrentOrderPayment.ProcessOnDate = DateTime.Now;
                CurrentOrderPayment.Save();

                return result;
            }
        }

        public PaymentAuthorizationResponse ChargeCheck(OrderPayment orderPayment)
        {
            throw new NotImplementedException("Charging checks is not yet implemented for Authorized.Net");
        }

        public GatewayResponse ChargeCard(GatewayChargeRequest chargeRequest)
        {
            using (new ApplicationUsageLogger(new ExecutionContext(this)))
            {
                GatewayResponse anetResponse = new GatewayResponse();

                try
                {
                    //anetResponse.ResponseCode = responseParts[0];
                    //anetResponse.ResponseSubCode = responseParts[1];
                    //anetResponse.ResponseReasonCode = responseParts[2];
                    //anetResponse.ResponseReasonText = responseParts[3];
                    //anetResponse.ApprovalCode = responseParts[4];
                    //anetResponse.AVSResult = responseParts[5]; // avs = address verification system
                    //anetResponse.TransactionID = responseParts[6];
                    //anetResponse.CardCodeResponse = responseParts[38];

                    anetResponse.ErrorLevel = NetSteps.Data.Entities.PaymentGateways.AuthorizeNet.ErrorLevel.Approved;
                }
                catch (Exception ex)
                {
                    EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsDataException);
                    return anetResponse;
                }
                return anetResponse;
            }
        }

        public void FakeRecordTransaction(GatewayChargeRequest chargeRequest, GatewayResponse response)
        {
            using (new ApplicationUsageLogger(new ExecutionContext(this)))
            {
                try
                {
                    CurrentOrderPaymentResult.DecryptedAccountNumber = CurrentOrderPayment.DecryptedAccountNumber;
                    CurrentOrderPaymentResult.Amount = chargeRequest.Amount;
                    CurrentOrderPaymentResult.ApprovalCode = response.ApprovalCode;
                    CurrentOrderPaymentResult.ResponseReasonCode = response.ResponseReasonCode;
                    CurrentOrderPaymentResult.ResponseCode = response.ResponseCode;
                    CurrentOrderPaymentResult.ResponseSubCode = response.ResponseSubCode;
                    CurrentOrderPaymentResult.ResponseReasonText = response.ResponseReasonText;
                    CurrentOrderPaymentResult.AuthorizeType = chargeRequest.AuthorizeType.ToString();
                    CurrentOrderPaymentResult.CardCodeResponse = response.CardCodeResponse;
                    CurrentOrderPaymentResult.ErrorMessage = response.ErrorMessage;
                    CurrentOrderPaymentResult.Response = response.RawResponseText;
                    CurrentOrderPaymentResult.AVSResult = response.AVSResult;
                    CurrentOrderPaymentResult.TransactionID = response.TransactionID;
                    CurrentOrderPaymentResult.IsTesting = chargeRequest.Testing;
                }
                catch (Exception ex)
                {
                    throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsPaymentGatewayException);
                }
            }
        }
        #endregion

        #region Helper Classes
        public class GatewayChargeRequest
        {
            public string LoginID;
            public string TransactionKey;
            public bool Testing;
            public NetSteps.Data.Entities.PaymentGateways.AuthorizeNet.ANetAuthorizeType AuthorizeType;
            public string CardNum;
            public string ExpDate;
            public decimal Amount;
            public string CardCode = string.Empty; // CVV2
            public string BillingFirstName;
            public string BillingLastName;
            public string BillingStreetAddress; // Required if using AVS
            public string BillingCity;
            public string BillingState;
            public string BillingZip;
            public string BillingCountry = "USA";
            public string BillingPhone;
            public string BillingEmail;

            public string CompanyID; // max length 20
            public string CompanyName; // max length 50
            public string Description; // max length 255
            public string InvoiceNumber; // max length 255

            public string TransactionID;
        }

        public class GatewayResponse
        {
            public NetSteps.Data.Entities.PaymentGateways.AuthorizeNet.ErrorLevel ErrorLevel = NetSteps.Data.Entities.PaymentGateways.AuthorizeNet.ErrorLevel.HTTPRequestError;
            public string ErrorMessage = string.Empty;

            public string ResponseCode = string.Empty; // 1 accepted, 2 declined, 3 error, 4 being held for review
            public string ResponseSubCode = string.Empty;
            public string ResponseReasonCode = string.Empty; // used when response code = 3: 
            public string ResponseReasonText = string.Empty;
            public string AVSResult = string.Empty;
            public string CardCodeResponse = string.Empty;
            public string ApprovalCode = string.Empty;
            public string TransactionID = string.Empty;
            public string RawResponseText = string.Empty;
        }
        #endregion

        protected override void SetTransactionChargeOrderPaymentResult()
        {
            throw new NotImplementedException();
        }

        protected override void SetRecordTransactionRefundOrderPaymentResult()
        {
            throw new NotImplementedException();
        }
    }
}