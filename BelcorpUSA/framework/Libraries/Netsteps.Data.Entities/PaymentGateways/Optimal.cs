using System;
using System.Collections;
using NetSteps.Addresses.Common.Models;
using NetSteps.Common.Configuration;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Base;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.OptimalPaymentsWebService;

namespace NetSteps.Data.Entities.PaymentGateways
{
    /// <summary>
    /// Optimal Payment Gateway (paygea)
    /// </summary>
    internal class Optimal : BasePaymentGateway
    {

        #region Constructors

        public Optimal(PaymentGatewaySection paymentGatewaySection)
            : base(paymentGatewaySection)
        {

        }

        #endregion

        #region Methods
        /// <summary>
        /// Documentation can be found at: http://support.optimalpayments.com/docapi.asp
        /// Test card #: 4387751111011
        /// Test cvv #: 111
        /// Note: the test authentication is built into this method, and is used if the app key "IsPaymentLiveMode" is false or null
        /// </summary>
        /// <param name="orderPayment"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected override PaymentAuthorizationResponse ChargePayment()
        {
            using (new ApplicationUsageLogger(new ExecutionContext(this)))
            {
                string authorizeId;
                string authorizePin;
                string authorizeMerchantAccountNumber;

                if (!LiveMode) // NetSteps Test Authentication
                {
                    authorizeId = "test";
                    authorizePin = "test";
                    authorizeMerchantAccountNumber = "89999105";
                }
                else
                {
                    authorizeId = _paymentGatewaySection.Login;
                    authorizePin = _paymentGatewaySection.Pin;
                    authorizeMerchantAccountNumber = _paymentGatewaySection.MerchantAccountNumber;
                }

                // Prepare the call to the Direct Debit Web Service
                CCAuthRequestV1 ccRequest = new CCAuthRequestV1(); // CCPaymentRequestV1 ccRequest = new CCPaymentRequestV1();
                ccRequest.amount = CurrentOrderPayment.Amount.ToString("0.00");

                MerchantAccountV1 merchantAccount = new MerchantAccountV1();
                merchantAccount.accountNum = authorizeMerchantAccountNumber;
                merchantAccount.storeID = authorizeId;
                merchantAccount.storePwd = authorizePin;
                ccRequest.merchantAccount = merchantAccount;
                ccRequest.merchantRefNum = string.Format("Ref-{0}", authorizeMerchantAccountNumber.Substring(0, 5));

                CardV1 card = new CardV1();
                card.cardNum = CurrentOrderPayment.DecryptedAccountNumber; // "4387751111011";
                CardExpiryV1 expiry = new CardExpiryV1();
                expiry.month = CurrentOrderPayment.ExpirationDate.Value.Month;
                expiry.year = CurrentOrderPayment.ExpirationDate.Value.Year;

                card.cardExpiry = expiry;
                //card.cardType = CardTypeV1.VI;
                card.cardTypeSpecified = false; // true;
                card.cvdIndicator = (string.IsNullOrEmpty(CurrentOrderPayment.Cvv) ? 0 : 1); //1;
                card.cvdIndicatorSpecified = true;
                card.cvd = CurrentOrderPayment.Cvv; //"111";
                ccRequest.card = card;

                IAddress address = CurrentOrderPayment as IAddress;

                BillingDetailsV1 billingDetails = new BillingDetailsV1();
                billingDetails.firstName = CurrentOrderPayment.BillingFirstName;
                billingDetails.lastName = CurrentOrderPayment.BillingLastName;
                billingDetails.street = address.Address1;
                billingDetails.city = address.City;
                #region State enum parsing
                //TODO: ? for countries besides US?
                //Note: this state (item) field is optional, so we if we have any errors parsing to the enum, just don't put anything
                try
                {
                    Hashtable htStates = NetSteps.Common.Constants.States;
                    ArrayList keys = new ArrayList(htStates.Keys);
                    ArrayList values = new ArrayList(htStates.Values);
                    for (int i = 0; i < htStates.Count; i++)
                    {
                        if (keys[i].ToString().ToLower() == CurrentOrderPayment.BillingState.ToLower() ||
                            values[i].ToString().ToLower() == CurrentOrderPayment.BillingState.ToLower())
                        {
                            billingDetails.Item = Enum.Parse(typeof(StateV1), keys[i].ToString());
                            break;
                        }
                    }

                    //try to do a raw conversion from the state to the enum
                    if (billingDetails.Item == null)
                        billingDetails.Item = Enum.Parse(typeof(StateV1), CurrentOrderPayment.BillingState, true);
                }
                catch
                {
                    billingDetails.Item = null;
                }
                #endregion
                #region Country enum parsing
                switch (address.Country.ToLower())
                {
                    case "mx":
                    case "mexico":
                        billingDetails.country = CountryV1.MX;
                        break;
                    case "il":
                    case "israel":
                        billingDetails.country = CountryV1.IL;
                        break;
                    case "us":
                    case "united states":
                    case "united states of america":
                    case "america":
                    case "usa":
                    default:
                        billingDetails.country = CountryV1.US;
                        break;
                }
                #endregion
                billingDetails.countrySpecified = true;
                billingDetails.zip = CurrentOrderPayment.BillingPostalCode;
                billingDetails.phone = CurrentOrderPayment.BillingPhoneNumber;
                billingDetails.cardPayMethod = CardPayMethodV1.WEB;
                billingDetails.cardPayMethodSpecified = true;
                ccRequest.billingDetails = billingDetails;

                /* -- Optional parameters from example
                ccAuthRequest.previousCustomer = true;
                ccAuthRequest.previousCustomerSpecified = true;
                ccAuthRequest.customerIP = "127.0.0.1";
                ccAuthRequest.productType = ProductTypeV1.M; //M = Both Digital and Physical(e.g., software downloaded followed by media shipment)
                ccAuthRequest.productTypeSpecified = true;
                */

                //Perform the Web Services call to process the charge
                CreditCardServiceV1 ccService = new CreditCardServiceV1();

                if (LiveMode)
                    ccService.Url = "https://webservices.optimalpayments.com/creditcardWS/CreditCardService/v1"; //Removed wsdl from url
                else
                    ccService.Url = "https://webservices.test.optimalpayments.com/creditcardWS/CreditCardService/v1";

                CCTxnResponseV1 ccResponse = ccService.ccPurchase(ccRequest);

                OptimalRecordTransaction(ccRequest, ccResponse);
                base.RecordTransaction();

                if (ccResponse.decision == DecisionV1.ACCEPTED)
                {
                    Result.Message = string.Empty;
                    Result.Success = true;
                    Result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Authorized;
                    return Result;
                }
                else
                {
                    Result.Message = ccResponse.description;
                    Result.Success = false;

                    if (ccResponse.decision == DecisionV1.DECLINED)
                        Result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Declined;
                    else
                        Result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Error;

                    return Result;
                }
            }
        }

        private void OptimalRecordTransaction(CCAuthRequestV1 chargeRequest, CCTxnResponseV1 response)
        {
            CurrentOrderPaymentResult.DecryptedAccountNumber = CurrentOrderPayment.DecryptedAccountNumber;
            CurrentOrderPaymentResult.Amount = Convert.ToDecimal(chargeRequest.amount);
            CurrentOrderPaymentResult.ApprovalCode = response.code.ToString();
            CurrentOrderPaymentResult.CardCodeResponse = response.cvdResponse.ToString();
            CurrentOrderPaymentResult.ErrorMessage = string.Empty;
            CurrentOrderPaymentResult.Response = response.description;
            CurrentOrderPaymentResult.AVSResult = response.decision.ToString();
            CurrentOrderPaymentResult.TransactionID = response.requestId.ToString();
        }

        protected override PaymentAuthorizationResponse RefundPayment(decimal ammount)
        {
            throw new NotImplementedException();
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