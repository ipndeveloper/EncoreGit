using System;
using NetSteps.Addresses.Common.Models;
using NetSteps.Common.Configuration;
using NetSteps.Data.Entities.Base;
using OpenECHO;

namespace NetSteps.Data.Entities.PaymentGateways
{
    // TODO: This is a rush job. Refactor later.
    internal class Echo : BasePaymentGateway
    {

        #region Constructors

        public Echo(PaymentGatewaySection paymentGatewaySection)
            : base(paymentGatewaySection)
        {

        }

        #endregion

        #region Methods
        protected override PaymentAuthorizationResponse ChargePayment()
        {
            string echoId;
            string echoPin;
            //bool success = false;

            if (CurrentOrderPayment.Amount <= 0)
            {
                Result.Message = "Invalid amount. The amount must be greater than 0.";
                Result.Success = false;
                Result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Error;
                return Result;
            }
            // Prevents duplicate payments.
            if (!string.IsNullOrEmpty(CurrentOrderPayment.TransactionID))
            {
                Result.Message = "This transaction has already been processes.";
                Result.Success = false;
                Result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Error;
                return Result;
            }

            echoId = _paymentGatewaySection.Login;
            echoPin = _paymentGatewaySection.Password;

            OrderPaymentResult orderPaymentResult = new OrderPaymentResult(CurrentOrderPayment);
            OpenECHONet openECHO = new OpenECHONet();

            // Set values for ECHO object.
            if (!LiveMode)
            {
                echoId = "123>4686561"; //NetSteps Test ID.
                echoPin = "50436257";
            }

            orderPaymentResult.IsTesting = !LiveMode; //Opposite of live mode.

            openECHO.merchant_echo_id = echoId;
            openECHO.merchant_pin = echoPin;
            openECHO.merchant_email = "brianh@netsteps.com";
            openECHO.order_type = "S";
            //openECHO.transaction_type = "EV"; //TODO: May need to process in 2 trans av then ds.
            openECHO.transaction_type = "AV";
            openECHO.billing_ip_address = "166.70.112.143";

            IAddress address = CurrentOrderPayment as IAddress;

            openECHO.merchant_trace_nbr = CurrentOrderPayment.Order.OrderID.ToString() + "_" + CurrentOrderPayment.OrderPaymentID.ToString() + "_" + CurrentOrderPayment.Order.ConsultantInfo.AccountNumber.ToString();
            openECHO.billing_first_name = CurrentOrderPayment.BillingFirstName;
            openECHO.billing_last_name = CurrentOrderPayment.BillingLastName;
            openECHO.billing_address1 = address.Address1;
            openECHO.billing_city = address.City;
            openECHO.billing_state = address.State;
            openECHO.billing_zip = address.PostalCode;
            openECHO.billing_phone = CurrentOrderPayment.BillingPhoneNumber;
            openECHO.cnp_security = CurrentOrderPayment.Cvv;

            openECHO.cc_number = CurrentOrderPayment.DecryptedAccountNumber; // Encryption.DecryptTripleDES(orderPayment.EncryptedAccountNumber, Constants.key, orderPayment.Order.OrderNumber);
            // This is to avoid accidental processing of real credit cards on the test system.
            if (!LiveMode)
                openECHO.cc_number = "4005550000000019"; //Test Visa Number

            string[] date = CurrentOrderPayment.ExpirationDate.ToString().Split('/');
            openECHO.ccexp_month = date[1];
            openECHO.ccexp_year = date[0];
            openECHO.grand_total = CurrentOrderPayment.Amount.ToString("0.00");

            // If AV (Auth with AVS) then DS (Deposit Funds)
            if (openECHO.Submit())
            {
                //If Address verify meets min requiremnts then proccess deposit.
                //If CVV2 Code is good then process regardless of AVS.
                //See https://wwws.echo-inc.com/ISPGuide-Response.asp#AVSReturnCode
                if (openECHO.avs_result == "X" || openECHO.avs_result == "Y" || openECHO.avs_result == "A"
                    || openECHO.avs_result == "B" || openECHO.avs_result == "D" || openECHO.avs_result == "I"
                    || openECHO.avs_result == "M" || openECHO.avs_result == "S"
                    || openECHO.security_result == "M")
                {
                    openECHO.transaction_type = "DS";

                    if (openECHO.Submit())
                    {
                        Result.Message = "success"; // openECHO.authorization
                        CurrentOrderPayment.TransactionID = openECHO.order_number;
                        Result.Success = true;
                        Result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Authorized;
                    }
                    else
                    {
                        Result.Message = "declined"; // openECHO.EchoDeclineCode
                        Result.Success = false;
                        Result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Declined;
                    }
                }
                else
                {
                    Result.Message = "declined"; // openECHO.EchoDeclineCode
                    Result.Success = false;
                    Result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Declined;
                }

            }
            else
            {
                Result.Message = "declined"; // openECHO.EchoDeclineCode
                Result.Success = false;
                Result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Declined;
            }


            orderPaymentResult.DateAuthorized = DateTime.Now;
            orderPaymentResult.DecryptedAccountNumber = CurrentOrderPayment.DecryptedAccountNumber;
            orderPaymentResult.Amount = CurrentOrderPayment.Amount;
            orderPaymentResult.OrderID = CurrentOrderPayment.Order.OrderID;
            if (CurrentOrderPayment.OrderCustomer != null)
                orderPaymentResult.OrderCustomerID = CurrentOrderPayment.OrderCustomer.OrderCustomerID;
            orderPaymentResult.OrderPaymentID = CurrentOrderPayment.OrderPaymentID;
            orderPaymentResult.ApprovalCode = Result.Message.ToUpper();

            orderPaymentResult.ResponseCode = openECHO.status;
            orderPaymentResult.ResponseReasonText = openECHO.echoType1;
            orderPaymentResult.AuthorizeType = openECHO.transaction_type;
            orderPaymentResult.ErrorMessage = openECHO.error;
            orderPaymentResult.Response = openECHO.Response;
            orderPaymentResult.AVSResult = openECHO.avs_result;
            orderPaymentResult.TransactionID = openECHO.order_number;
            //orderPaymentResult.Insert();
            orderPaymentResult.Save();

            return Result;
        }

        //public PaymentAuthorizationResponse ChargeCheck(OrderPayment orderPayment)
        //{
        //    PaymentAuthorizationResponse result = new PaymentAuthorizationResponse();

        //    bool liveMode = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.IsPaymentLiveMode);
        //    string echoId;
        //    string echoPin;
        //    //bool success = false;

        //    if (orderPayment.Amount <= 0)
        //    {
        //        result.Message = "Invalid amount. The amount must be greater than 0.";
        //        result.Success = false;
        //        result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Error;
        //        return result;
        //    }
        //    //Prevents duplicate payments.
        //    if (!string.IsNullOrEmpty(orderPayment.TransactionID))
        //    {
        //        result.Message = "This transaction has already been processes.";
        //        result.Success = false;
        //        result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Error;
        //        return result;
        //    }

        //    var connectionInfo = PaymentGatewaySection.Instance.FirstOrDefault(p => p.Namespace == CurrentPaymentGateway.Namespace);

        //    echoId = connectionInfo.Login;
        //    echoPin = connectionInfo.Password;

        //    OrderPaymentResult orderPaymentResult = new OrderPaymentResult(orderPayment);
        //    OpenECHONet openECHO = new OpenECHONet();

        //    //Set values for ECHO object.
        //    if (!liveMode)
        //    {
        //        echoId = "123>4686561"; //NetSteps Test ID.
        //        echoPin = "50436257";
        //    }

        //    orderPaymentResult.IsTesting = !liveMode; // Opposite of live mode.

        //    openECHO.merchant_echo_id = echoId;
        //    openECHO.merchant_pin = echoPin;
        //    openECHO.merchant_email = "brianh@netsteps.com";
        //    openECHO.order_type = "S";
        //    openECHO.transaction_type = "DH"; //"DD" is with verification.
        //    openECHO.billing_ip_address = "166.70.112.143";

        //    IAddress address = orderPayment as IAddress;

        //    openECHO.merchant_trace_nbr = orderPayment.Order.OrderID.ToString() + "_" + orderPayment.OrderPaymentID.ToString() + "_" + orderPayment.Order.ConsultantInfo.AccountNumber.ToString();
        //    openECHO.ec_account_type = "PC";
        //    openECHO.ec_payment_type = "WEB";
        //    openECHO.billing_phone = orderPayment.BillingPhoneNumber;
        //    openECHO.ec_payee = "Personal Preference Inc.";
        //    openECHO.ec_first_name = orderPayment.BillingFirstName;
        //    openECHO.ec_last_name = orderPayment.BillingLastName;
        //    openECHO.ec_address1 = address.Address1;
        //    openECHO.ec_city = address.City;
        //    openECHO.ec_state = address.State;
        //    openECHO.ec_zip = address.PostalCode;
        //    openECHO.ec_id_number = orderPayment.IdentityNumber;
        //    if (orderPayment.IdentityState.Length == 2)
        //    {
        //        openECHO.ec_id_state = orderPayment.IdentityState;
        //        openECHO.ec_id_type = "DL";
        //    }
        //    else
        //    {
        //        openECHO.ec_id_type = "GN";
        //    }
        //    openECHO.ec_serial_number = "0";
        //    //openECHO.ec_account = orderPayment.AccountNumber;
        //    openECHO.ec_account = orderPayment.DecryptedAccountNumber; // Encryption.DecryptTripleDES(orderPayment.EncryptedAccountNumber, Constants.key, orderPayment.Order.OrderNumber);
        //    openECHO.ec_rt = orderPayment.RoutingNumber;
        //    //This is to avoid accidental processing of real accounts on the test system.
        //    if (!liveMode)
        //    {
        //        openECHO.ec_account = "24413815"; // Test Acct Number;
        //        openECHO.ec_rt = "490000018"; // Test Routing Number
        //    }

        //    openECHO.grand_total = orderPayment.Amount.ToString("0.00");

        //    if (openECHO.Submit())
        //    {
        //        result.Message = "success"; // openECHO.authorization
        //        orderPayment.TransactionID = openECHO.order_number;
        //        result.Success = true;
        //        result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Authorized;
        //    }
        //    else
        //    {
        //        result.Message = "declined"; // openECHO.EchoDeclineCode
        //        result.Success = false;
        //        result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Declined;
        //    }

        //    orderPaymentResult.DateAuthorized = DateTime.Now;
        //    orderPaymentResult.DecryptedAccountNumber = orderPayment.DecryptedAccountNumber;
        //    orderPaymentResult.Amount = orderPayment.Amount;
        //    orderPaymentResult.OrderID = orderPayment.Order.OrderID;
        //    if (orderPayment.OrderCustomer != null)
        //        orderPaymentResult.OrderCustomerID = orderPayment.OrderCustomer.OrderCustomerID;
        //    orderPaymentResult.OrderPaymentID = orderPayment.OrderPaymentID;
        //    orderPaymentResult.ApprovalCode = result.Message.ToUpper();

        //    orderPaymentResult.ResponseCode = openECHO.status;
        //    orderPaymentResult.ResponseReasonText = openECHO.echoType1;
        //    orderPaymentResult.AuthorizeType = openECHO.transaction_type;
        //    orderPaymentResult.ErrorMessage = openECHO.error;
        //    orderPaymentResult.Response = openECHO.Response;
        //    orderPaymentResult.AVSResult = openECHO.avs_result;
        //    orderPaymentResult.TransactionID = openECHO.order_number;
        //    //orderPaymentResult.Insert();
        //    orderPaymentResult.Save();

        //    return result;
        //}

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
