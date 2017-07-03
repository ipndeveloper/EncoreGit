using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using NetSteps.Addresses.Common.Models;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Base;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.PaymentGateways
{
    /// <summary>
    /// ProPay payment gateway implementation.
    /// </summary>
    internal class ProPayPaymentGateway : BasePaymentGateway
    {
        #region --- Members ---

        public string AuthorizeId { get; private set; }
        public string AuthorizePin { get; private set; }
        public string AuthorizeAccountNum { get; private set; }
	    private bool _solidTestTransaction = false;

        private ProPayChargeRequest _chargeRequest;
        private ProPayResponse _chargeResponse = new ProPayResponse();

        #endregion

        #region --- Constructors ---

        public ProPayPaymentGateway(PaymentGatewaySection paymentGatewaySection)
            : base(paymentGatewaySection)
        {

        }

        public ProPayPaymentGateway(bool liveMode, bool testTransaction, ProPayChargeRequest chargeRequest)
            : base(liveMode, testTransaction)
        {
            this._chargeRequest = chargeRequest;
        }

        #endregion

        #region --- Methods ---
        public override PaymentAuthorizationResponse Charge(OrderPayment orderPayment)
        {
            _chargeRequest = new ProPayChargeRequest();
            var response = new PaymentAuthorizationResponse();
            using (new ApplicationUsageLogger(new ExecutionContext(this)))
            {
                base.ResetValues();
                CurrentOrderPayment = orderPayment;

                if (!CurrentOrderPayment.ChangeTracker.ChangeTrackingEnabled)
                    CurrentOrderPayment.StartTracking();

                if (ValidateOrderPayment(response))
                {
                    _chargeResponse = ChargeCard(_chargeRequest);
                    response.Success = _chargeResponse.Success;
                    response.GatewayAuthorizationStatus = _chargeResponse.Success ? Constants.GatewayAuthorizationStatus.Authorized :
                    Constants.GatewayAuthorizationStatus.Declined;

                    if (response.Success)
                    {
                        SetTransactionChargeOrderPaymentResult();
                        base.RecordTransaction();
                        CurrentOrderPayment.TransactionID = CurrentOrderPaymentResult.TransactionID;
                        CurrentOrderPayment.ProcessedDate = CurrentOrderPayment.ProcessOnDate = DateTime.Now;
                        CurrentOrderPayment.Save();
                    }
                    else
                    {
                        response.Message = string.Format("{0} (CC#: {1})", _chargeResponse.ErrorMessage, _chargeRequest.CardNum.MaskString(4));
                    }
                }
            }
            return response;
        }

        public bool ValidateOrderPayment(PaymentAuthorizationResponse response)
        {
            var result = CheckPaymentAmountIsPositive(CurrentOrderPayment, response);
            if (!result)
                return false;

            CheckForDuplicateTransaction(CurrentOrderPayment, response);
            if (!string.IsNullOrEmpty(response.Message))
                return false;

            CheckNullorEmptyCreditCardNumber(CurrentOrderPayment, response);
            if (!string.IsNullOrEmpty(response.Message))
                return false;

            CheckLiveMode();
            DoChargeRequest();
            SetUpChargeRequestProperties(04);
            var address = CurrentOrderPayment as IAddress;
            SetAddressForm(address);

            return true;
        }

        /// <summary>
        /// Sets up the charge request for processing.
        /// </summary>
        private void DoChargeRequest()
        {
            _chargeRequest.CardNum = CurrentOrderPayment.DecryptedAccountNumber;
            _chargeRequest.ExpDate = CurrentOrderPayment.ExpirationDate.HasValue ? PropayExpDateString(CurrentOrderPayment.ExpirationDate.Value) : "0213";
            _chargeRequest.CardCode = CurrentOrderPayment.Cvv;
            DoChargeRequestAmount();
            //DoChargeRequestForCustomer(payment);
            _chargeRequest.InvNum = CurrentOrderPayment.Order != null ? CurrentOrderPayment.Order.OrderNumber : "";
        }

        /// <summary>
        /// Sets the amount of the charge request.
        /// </summary>
        private void DoChargeRequestAmount()
        {
            //Propay wants int - 100 = 1.00 so remove the decimal 
            _chargeRequest.Amount = _solidTestTransaction ? 100 : Convert.ToInt32(CurrentOrderPayment.Amount.ToString(".00").Replace(".", ""));
        }

        /// <summary>
        /// Sets up the charge request parameters for use in ValidateOrderPayment.
        /// </summary>
        private void SetUpChargeRequestProperties(int transTypeId)
        {
			_chargeRequest.TerminalId = GetPaymentGatewayTerminalId(LiveMode) ?? ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.ClientPropayTerminalId);
            _chargeRequest.Testing = _solidTestTransaction;
            _chargeRequest.TransType = transTypeId; //04 for credit card charge, 07 for refund
            _chargeRequest.CertStr = AuthorizeId;
            _chargeRequest.AccountNum = Convert.ToInt32(AuthorizeAccountNum);
        }

        /// <summary>
        /// Sets up the address form section of the charge request.
        /// </summary>
        private void SetAddressForm(IAddress address)
        {
            if (address != null)
            {
                _chargeRequest.Addr = address.Address1.RemoveDiacritics();
                _chargeRequest.Addr2 = address.Address2.RemoveDiacritics();
                _chargeRequest.Addr3 = address.Address3.RemoveDiacritics();
                _chargeRequest.City = address.City.RemoveDiacritics();
                _chargeRequest.State = address.State.RemoveDiacritics();
                _chargeRequest.Zip = address.PostalCode.RemoveDiacritics();
            }
        }

		public string GetUrl()
			{
				bool liveMode = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.IsPaymentLiveMode);

				var liveUrl = _paymentGatewaySection.LiveUrl;
				if (string.IsNullOrWhiteSpace(liveUrl))
					liveUrl = "https://epay.propay.com/api/propayapi.aspx";

				var testUrl = _paymentGatewaySection.TestUrl;
				if (string.IsNullOrWhiteSpace(testUrl))
					testUrl = "https://xmltest.propay.com/api/propayapi.aspx";

			return liveMode ? liveUrl : testUrl;
		}

		public ProPayResponse ChargeCard(ProPayChargeRequest chargeRequest)
		{
			using (new ApplicationUsageLogger(new ExecutionContext(this)))
			{
				var url = GetUrl();
                ProPayResponse response = new ProPayResponse();
                string postData = chargeRequest.ToXmlCreditCardData();

                //if the request fails, return the anetResponse with an ErrorLevel and ErrorMessage.
                string errorMessage = "";
                response = SendCreditCardPostRequest(postData, url, response, chargeRequest, out errorMessage);

                return response;
            }
        }

        private ProPayResponse SendCreditCardPostRequest(string postData, string url, ProPayResponse ppResponse, ProPayChargeRequest chargeRequest, out string errorMessage)
        {
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
            objRequest.Method = "POST";
            objRequest.ContentLength = postData.Length;
            objRequest.ContentType = "application/x-www-form-urlencoded";
            StreamWriter writer = null;

            try
            {
                writer = new StreamWriter(objRequest.GetRequestStream());
                writer.Write(postData);
                writer.Close();
                errorMessage = "";
            }
            catch (Exception e)
            {
                NetSteps.Data.Entities.Exceptions.ExceptionLogger.LogException(e, true);
                errorMessage = e.Message;
            }

            GetCreditCardResponse(ppResponse, objRequest);

            return ppResponse;
        }

        private ProPayResponse GetCreditCardResponse(ProPayResponse ppResponse, HttpWebRequest objRequest)
        {
            try
            {
                HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                var objResponseStream = objResponse.GetResponseStream();
                var objDS = new DataSet();
                var objXMLReader = new XmlTextReader(objResponseStream);
                objDS.ReadXml(objXMLReader);
                //Load response stream into XMLReader 
                var objDTResults = new DataTable();
                foreach (DataTable dt in objDS.Tables.Cast<DataTable>().Where(dt => dt.Columns.Contains("transType")))
                {
                    objDTResults = dt;
                    break;
                }

                 foreach (DataRow objRow in objDTResults.Rows.Cast<DataRow>().Where(objRow => objRow["transType"].ToString() == "04" || objRow["transType"].ToString() == "4"))
                {
                    ppResponse.Status = objRow["status"].ToString();

                    if (ppResponse.Status == "00" || ppResponse.Status == "0")
                    {
                        ppResponse.Success = true;
                        ppResponse.TransNum = objRow["transNum"] as string ?? String.Empty;
                        ppResponse.AuthCode = objRow["authCode"] as string ?? String.Empty;
                        ppResponse.AvsResult = objRow["AVS"] as string ?? String.Empty;
                        ppResponse.ResponseCode = objRow["ResponseCode"] as string ?? String.Empty;
                        break;
                        //return true;
                    }
                    else
                    {
                        ppResponse.Success = false;
                        ppResponse.ResponseCode = String.Empty;
                        if (ppResponse.Status == "58")
                            ppResponse.ResponseCode = objRow["ResponseCode"] != null ? objRow["ResponseCode"] as string : String.Empty;
                        ppResponse.ErrorMessage = GetCreditCardResponseErrorMessage(ppResponse.Status);
                        if (ppResponse.Status == "58" && !String.IsNullOrEmpty(ppResponse.ResponseCode))
                        ppResponse.ErrorMessage = ppResponse.ErrorMessage + " " +
                                                  GetCreditCardDeclinedResponseErrorMessage(ppResponse.ResponseCode);

                    }
                }

                // Close and clean up the StreamReader
                objXMLReader.Close();
            }
            catch (Exception e)
            {
                NetSteps.Data.Entities.Exceptions.ExceptionLogger.LogException(e, true);
                ppResponse.ErrorMessage = e.Message;
            }
            return ppResponse;
        }

        private String GetCreditCardResponseErrorMessage(string statusId)
        {
            var message = String.Empty;
            switch (statusId)
            {
				case "28":
            		message = "Invalid Billing Address";
            		break;
				case "29":
            		message = "Invalid Apartment Number";
            		break;
				case "30":
            		message = "Invalid City";
            		break;
				case "31":
            		message = "Invalid State";
            		break;
				case "32":
            		message = "Invalid Billing Zip Code";
            		break;
                case "48":
                    message = "Invalid Credit Card Number";
                    break;
                case "49":
                    message = "Invalid Expiration Date";
                    break;
                case "58":
                    message = "Credit Card has been declined.";
                    break;
                case "59":
                    message = "Invalid Transaction"; //User not authenticated. Probably a change in IP address that was not reported to Propay
                    break;
                case "61":
                    message = "Invalid Transaction"; //Propay account in question is not allowed to process transactions this large. 
                    break;
                case "62":
                    message = "Invalid Transaction"; //The propay account's mothly transaction volume limit has been exceeded.
                    break;
                case "81":
                    message = "Invalid Transaction"; //The propay account has expired and the merchant needs to renew it. 
                    break;
            }
            return message;
        }

        private String GetCreditCardDeclinedResponseErrorMessage(string responseCode)
        {
            var message = String.Empty;
            switch (responseCode)
            {
                case "14":
                    message = "Invalid credit card number as reported by issuing bank";
                    break;
                case "19":
                    message = "Credit card issuer’s bank timed out. Please attempt this transaction again";
                    break;
                case "17":
                    message = "Card limit exceeded";
                    break;
                case "51":
                    message = "Insufficient funds";
                    break;
                case "54":
                    message = "Expired card";
                    break;
                case "15":
                    message = "Invalid credit card number. Credit card networks cannot locate this card’s issuing bank";
                    break;
                default:
                    message = "";
                    break;
            }
            return message;
        }

        #endregion

        #region --- Refund ---

        public override PaymentAuthorizationResponse Refund(OrderPayment orderPayment, decimal amount)
        {
            _chargeRequest = new ProPayChargeRequest();
            var response = new PaymentAuthorizationResponse();
            using (new ApplicationUsageLogger(new ExecutionContext(this)))
            {
                base.ResetValues();
                CurrentOrderPayment = orderPayment;

                if (!CurrentOrderPayment.ChangeTracker.ChangeTrackingEnabled)
                    CurrentOrderPayment.StartTracking();

                _chargeRequest.TransNum = CurrentOrderPayment.TransactionID;
                _chargeRequest.Amount = Convert.ToInt32(amount.ToString().Replace(".", ""));
                if (ValidateRefund(response, amount))
                {
                    var ppResponse = RefundCard(_chargeRequest);
                    response.Success = ppResponse.Success;
                    response.GatewayAuthorizationStatus = ppResponse.Success ? Constants.GatewayAuthorizationStatus.Authorized :
                    Constants.GatewayAuthorizationStatus.Declined;

                    if (response.Success)
                    {
                        if (amount > 0)
                            amount = 0 - amount; // make it a negative (credit) amount for database storage
                        SetRecordTransactionRefundOrderPaymentResult();
                        base.RecordTransaction();
                    }
                    else
                    {
                        response.Message = string.Format("{0} (CC#: {1})", ppResponse.ErrorMessage, _chargeRequest.CardNum.MaskString(4));
                    }

                    //orderPayment.ProcessedDate = orderPayment.ProcessOnDate = DateTime.Now;
                    //orderPayment.Save();
                }


            }
            return response;
        }

        public bool ValidateRefund(PaymentAuthorizationResponse response, decimal amount)
        {
            ChangeNegativeCalcAmountToPositive(amount);
            CheckForValidTransactionIDForRefund(CurrentOrderPayment, response);
            if (!string.IsNullOrEmpty(response.Message))
                return false;
            CheckForValidRequestedRefundAmount(CurrentOrderPayment, response, amount);
            if (!string.IsNullOrEmpty(response.Message))
                return false;

            CheckLiveMode();
            SetUpChargeRequestProperties(07);
            return true;
        }

        public ProPayResponse RefundCard(ProPayChargeRequest chargeRequest)
        {
            using (new ApplicationUsageLogger(new ExecutionContext(this)))
            {
				string url = GetUrl();

                ProPayResponse response = new ProPayResponse();
                string postData = chargeRequest.ToXmlRefundData();

                //if the request fails, return the anetResponse with an ErrorLevel and ErrorMessage.
                string errorMessage = "";
                response = SendRefundPostRequest(postData, url, response, chargeRequest, out errorMessage);

                return response;
            }
        }

        private ProPayResponse SendRefundPostRequest(string postData, string url, ProPayResponse ppResponse, ProPayChargeRequest chargeRequest, out string errorMessage)
        {
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
            objRequest.Method = "POST";
            objRequest.ContentLength = postData.Length;
            objRequest.ContentType = "application/x-www-form-urlencoded";
            StreamWriter writer = null;

            try
            {
                writer = new StreamWriter(objRequest.GetRequestStream());
                writer.Write(postData);
                writer.Close();
                errorMessage = "";
            }
            catch (Exception e)
            {
                NetSteps.Data.Entities.Exceptions.ExceptionLogger.LogException(e, true);
                errorMessage = e.Message;
            }

            GetRefundResponse(ppResponse, objRequest);

            return ppResponse;
        }

        private ProPayResponse GetRefundResponse(ProPayResponse ppResponse, HttpWebRequest objRequest)
        {
            try
            {
                HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                var objResponseStream = objResponse.GetResponseStream();
                var objDS = new DataSet();
                var objXMLReader = new XmlTextReader(objResponseStream);
                objDS.ReadXml(objXMLReader);
                //Load response stream into XMLReader 
                var objDTResults = new DataTable();
                foreach (DataTable dt in objDS.Tables.Cast<DataTable>().Where(dt => dt.Columns.Contains("transType")))
                {
                    objDTResults = dt;
                    break;
                }

                foreach (DataRow objRow in objDTResults.Rows.Cast<DataRow>().Where(objRow => objRow["transType"].ToString() == "07" || objRow["transType"].ToString() == "7"))
                {
                    ppResponse.Status = objRow["status"].ToString();

                    if (ppResponse.Status == "00" || ppResponse.Status == "0")
                    {
                        ppResponse.Success = true;
                        ppResponse.TransNum = objRow["transNum"].ToString() ?? String.Empty;
                        break;
                        //return true;
                    }
                    else
                    {
                        ppResponse.Success = false;
                        ppResponse.ErrorMessage = "Refund failed";
                    }
                }

                // Close and clean up the StreamReader
                objXMLReader.Close();
            }
            catch (Exception e)
            {
                NetSteps.Data.Entities.Exceptions.ExceptionLogger.LogException(e, true);
                ppResponse.ErrorMessage = e.Message;
            }
            return ppResponse;
        }
        #endregion

        #region  --- internal methods ---
        /// <summary>
        /// Checks to determine whether or not the payment amount entered is positive.
        /// </summary>
        /// <remarks>
        /// Refactored from AuthorizeNet.
        /// Came from the ChargeCreditCard method.
        /// </remarks>
        internal bool CheckPaymentAmountIsPositive(OrderPayment payment, PaymentAuthorizationResponse response)
        {
            if (payment.Amount <= 0)
            {
                // No amount will be charged.
                // Just return true since some products have no cost.
                response.Message = string.Empty;
                response.Success = true;
                response.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Error;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks for and stops duplicate transactions.
        /// </summary>
        /// /// <remarks>
        /// Refactored from AuthorizeNet.
        /// Came from the ChargeCreditCard method.
        /// </remarks>
        internal void CheckForDuplicateTransaction(OrderPayment payment, PaymentAuthorizationResponse response)
        {
            if (!string.IsNullOrEmpty(payment.TransactionID))
            {
                response.Message = "This transaction has already been processed.";
                response.Success = false;
                response.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Error;
            }
        }

        /// <summary>
        /// Checks for null or empty credit card numbers and stops authorization at this point.
        /// </summary>
        /// <remarks>
        /// Refactored from AuthorizeNet.
        /// Came from the ChargeCreditCard method.
        /// </remarks>
        internal void CheckNullorEmptyCreditCardNumber(OrderPayment payment, PaymentAuthorizationResponse response)
        {
            if (String.IsNullOrEmpty(payment.DecryptedAccountNumber))
            {
                response.Message = "Error authorizing card. Credit Card number is required.";
                response.Success = false;
                response.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Error;
            }
        }

        /// <summary>
        /// Checks for the live mode of a transaction.
        /// If the live mode is false it will set AuthorizeId
        /// and AuthorizePin to default test strings.
        /// </summary>
        /// <remarks>
        /// Refactored from AuthorizeNet.
        /// Came from the ChargeCreditCard method.
        internal void CheckLiveMode()
        {
            if (LiveMode)
            {
                AuthorizeId = _paymentGatewaySection.Login;
                AuthorizeAccountNum = _paymentGatewaySection.MerchantAccountNumber;
            }
            else
            {
                //Check for client specific data first
                var clientAccountNum = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.ClientPropayTestAccountNum);
                var clientAuthId = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.ClientPropayTestAuthId);
                if (!string.IsNullOrEmpty(clientAccountNum) && !string.IsNullOrEmpty(clientAuthId))
                {
                    AuthorizeId = clientAuthId;
                    AuthorizePin = "";
                    AuthorizeAccountNum = clientAccountNum;
                }
                else
                {
                //This is just for savvi testing
                    //TODO: Use netsteps default info
                    AuthorizeId = "NetStepsCertString000000000001";
                AuthorizePin = "";
                    AuthorizeAccountNum = "1036411";
                }
            }
        }

        /// <summary>
        /// Checks for a valid transaction Id.
        /// </summary>
        /// <remarks>
        /// If the transaction Id is not null then the refund process can continue.
        /// </remarks>
        internal PaymentAuthorizationResponse CheckForValidTransactionIDForRefund(OrderPayment payment, PaymentAuthorizationResponse response)
        {
            if (string.IsNullOrEmpty(payment.TransactionID))
            {
                response.Message = "A valid transaction Id is needed";
                response.Success = false;
                response.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Error;
            }
            return response;
        }


        internal void CheckForValidRequestedRefundAmount(OrderPayment payment, PaymentAuthorizationResponse response, decimal calcAmount)
        {
            if (calcAmount > payment.Amount)
            {
                response.Message = "The amount being requested for refund must be less than or equal to the original settled amount.";
                response.Success = false;
                response.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Error;
            }
        }

        internal decimal ChangeNegativeCalcAmountToPositive(decimal calcAmount)
        {
            if (calcAmount < 0)
                calcAmount = 0 - calcAmount; // make it a positive amount for comparison
            return calcAmount;
        }
        #endregion

        #region --- Helper Classes ---
        // Acceptable formats for ExpDate
        //MMYY,
        //MM/YY,
        //MM-YY,
        //MMYYYY,
        //MM/YYYY,
        //MM-YYYY
        public class ProPayChargeRequest
        {
            public string CertStr; //max length 30
            public string Class = "partner";
            public int TransType;
            public int Amount;
            public string Addr; //max length 100
            public string Addr2; //max length 20 - apt num
            public string Addr3; //max length 100
            public string City; //max length 30
            public string State; //2 char code "UT"
            public string Country; //omit unless told otherwise
            public string Zip; //min 3, max 9
            public int AccountNum; //Assigned by propay
            public string SourceEmail; //Omit unless told otherwise
            public string ExpDate;
            public string CardNum;
            public string CardCode = string.Empty; // CVV2
            public string TransNum;
            public string InvNum; //max length 50
            public string CardholderName; //max length 100
            public bool Testing;
			public string TerminalId;
			public string Comment1; //max length 120

            public string ToXmlCreditCardData()
            {
                ;
                StringBuilder objSB = new StringBuilder();
                //Setup request XML
                objSB.Append("<?xml version=\"1.0\"?>" + System.Environment.NewLine);
                objSB.Append("<!DOCTYPE Request.dtd>" + System.Environment.NewLine);
                objSB.Append("<XMLRequest>" + System.Environment.NewLine);
                objSB.Append("  <certStr>$CertStr</certStr>" + System.Environment.NewLine);
                if (!string.IsNullOrEmpty(this.TerminalId))
                    objSB.Append(" <termid>$TerminalId</termid>" + System.Environment.NewLine);
                objSB.Append("  <class>$Class</class>" + System.Environment.NewLine);
                objSB.Append("  <XMLTrans>" + System.Environment.NewLine);
                objSB.Append("    <transType>$TransType</transType>" + System.Environment.NewLine);
                objSB.Append("    <accountNum>$AccountNum</accountNum>" + System.Environment.NewLine);
                objSB.Append("     <amount>$Amount</amount>" + System.Environment.NewLine);
                objSB.Append("     <ccNum>$ccNum</ccNum>" + System.Environment.NewLine);
				objSB.Append("     <expDate>$expDate</expDate>" + System.Environment.NewLine);
				objSB.Append("     <invNum>$invNum</invNum>" + System.Environment.NewLine);
				objSB.Append("     <comment1>$Comment1</comment1>" + System.Environment.NewLine);
				if (!string.IsNullOrWhiteSpace(this.Addr))
					objSB.Append("     <addr>$addr</addr>" + System.Environment.NewLine);
				if (!string.IsNullOrWhiteSpace(this.Addr2))
					objSB.Append("     <addr2>$addr2</addr2>" + System.Environment.NewLine);
				if (!string.IsNullOrWhiteSpace(this.Addr3))
					objSB.Append("     <addr3>$addr3</addr3>" + System.Environment.NewLine);
				if (!string.IsNullOrWhiteSpace(this.City))
					objSB.Append("     <city>$city</city>" + System.Environment.NewLine);
				if (!string.IsNullOrWhiteSpace(this.State))
					objSB.Append("     <state>$state</state>" + System.Environment.NewLine);
				if (!string.IsNullOrWhiteSpace(this.Zip))
					objSB.Append("     <zip>$zip</zip>" + System.Environment.NewLine);
                objSB.Append("  </XMLTrans>" + System.Environment.NewLine);
                objSB.Append("</XMLRequest>" + System.Environment.NewLine);

                objSB.Replace("$CertStr", this.CertStr);
                objSB.Replace("$Class", this.Class);
                objSB.Replace("$TransType", this.TransType.ToString());
                objSB.Replace("$AccountNum", this.AccountNum.ToString());
                objSB.Replace("$Amount", this.Amount.ToString());
                objSB.Replace("$ccNum", this.CardNum);
				objSB.Replace("$expDate", this.ExpDate);
				objSB.Replace("$invNum", this.InvNum);
				objSB.Replace("$addr", this.Addr);
				objSB.Replace("$addr2", this.Addr2);
				objSB.Replace("$addr3", this.Addr3);
				objSB.Replace("$city", this.City);
				objSB.Replace("$state", this.State);
				objSB.Replace("$zip", this.Zip);
                objSB.Replace("$TerminalId", this.TerminalId);
				objSB.Replace("$Comment1", this.Comment1);
                //objSB.Remove(0, objSB.ToString().Length);

                objSB.Append(System.Environment.NewLine);

                return objSB.ToString();
            }

            public string ToXmlRefundData()
            {
                StringBuilder objSB = new StringBuilder();
                //Setup request XML
                objSB.Append("<?xml version=\"1.0\"?>" + System.Environment.NewLine);
                objSB.Append("<!DOCTYPE Request.dtd>" + System.Environment.NewLine);
                objSB.Append("<XMLRequest>" + System.Environment.NewLine);
                objSB.Append("  <certStr>$CertStr</certStr>" + System.Environment.NewLine);
                if (!string.IsNullOrEmpty(this.TerminalId))
                    objSB.Append(" <termid>$TerminalId</termid>" + System.Environment.NewLine);
                objSB.Append("  <class>$Class</class>" + System.Environment.NewLine);
                objSB.Append("  <XMLTrans>" + System.Environment.NewLine);
                objSB.Append("    <transType>$TransType</transType>" + System.Environment.NewLine);
                objSB.Append("    <accountNum>$AccountNum</accountNum>" + System.Environment.NewLine);
                objSB.Append("     <transNum>$transNum</transNum>" + System.Environment.NewLine);
                objSB.Append("     <amount>$Amount</amount>" + System.Environment.NewLine);
                objSB.Append("     <invNum>$invNum</invNum>" + System.Environment.NewLine);
                objSB.Append("  </XMLTrans>" + System.Environment.NewLine);
                objSB.Append("</XMLRequest>" + System.Environment.NewLine);

                objSB.Replace("$CertStr", this.CertStr);
                objSB.Replace("$Class", this.Class);
                objSB.Replace("$TransType", this.TransType.ToString());
                objSB.Replace("$AccountNum", this.AccountNum.ToString());
                objSB.Replace("$Amount", this.Amount.ToString());
                objSB.Replace("$transNum", this.TransNum);
                objSB.Replace("$invNum", this.InvNum);
                objSB.Replace("$TerminalId", this.TerminalId);

                objSB.Append(System.Environment.NewLine);

                return objSB.ToString();
            }
        }

        // Possible ResponseCode
        // 00 - Success
        // 01 - Transaction blocked by issuer
        // 04 - Pick up card and deny transaction
        // 05 - Problem with account
        // 12 - Invalid transaction
        // 14 - Invalid card number
        // 82 - Invalid CVV2
        //48 – Invalid credit card number (You will not need to document this if you build your own validation and MOD10 checking before sending data to ProPay.)  
        //58 – Decline (Include an appropriate decline message to the cardholder based on <responseCode> which is documented separately from <status> 
        //59 – User not authenticated (This is important because some day you may change your IP address and you probably will not remember, at that point, to notify ProPay.  You may want to consider sending yourself some Kind of notification whenever you get this response.)
        //61 – Indicates that the ProPay account in question is not allowed to process transactions this large.  The merchant should call ProPay Customer Service.
        //62 – Indicates that the ProPay account’s monthly transaction volume limit has been exceeded.  The merchant should call ProPay Customer Service.
        //81 – The ProPay account has expired and the merchant needs to renew it.

        public class ProPayResponse
        {
            public string ErrorMessage = string.Empty;
            public string Status = string.Empty;
            public string ResponseCode = string.Empty; // 1 accepted, 2 declined, 3 error, 4 being held for review
            public string TransNum = string.Empty; //Propay transaction identifier
            public string AuthCode = string.Empty; //max lenght 10, usually 5 characters long
            public string Resp = string.Empty; //Text of the responseCode.
            public string AvsResult = string.Empty;
            public string RawResponseText = string.Empty;
            public string CardCodeResponse = string.Empty;
            public bool Success;
        }

        protected string PropayExpDateString(DateTime date)
        {
            return date.ToString("MMyy");
        }
        #endregion

        protected override void SetTransactionChargeOrderPaymentResult()
        {
            PropayRecordTransaction();
        }

        protected override void SetRecordTransactionRefundOrderPaymentResult()
        {
            PropayRecordTransaction();
        }

        public void PropayRecordTransaction()
        {
            CurrentOrderPaymentResult.DecryptedAccountNumber = CurrentOrderPayment.DecryptedAccountNumber;
            CurrentOrderPaymentResult.ApprovalCode = _chargeResponse.AuthCode;
            CurrentOrderPaymentResult.ResponseReasonCode = _chargeResponse.Resp;
            CurrentOrderPaymentResult.ResponseCode = _chargeResponse.ResponseCode;
            CurrentOrderPaymentResult.ResponseReasonText = _chargeResponse.Resp;
            CurrentOrderPaymentResult.CardCodeResponse = _chargeResponse.CardCodeResponse;
            CurrentOrderPaymentResult.ErrorMessage = _chargeResponse.ErrorMessage;
            CurrentOrderPaymentResult.Response = _chargeResponse.RawResponseText;
            CurrentOrderPaymentResult.AVSResult = _chargeResponse.AvsResult;
            CurrentOrderPaymentResult.TransactionID = _chargeResponse.TransNum;
            //CurrentOrderPaymentResult.PaymentGatewayID = _paymentGatewaySection.PaymentGatewayID.ToShortNullable();
        }

        public override void ResetValues()
        {
            _chargeRequest = new ProPayChargeRequest();
            _chargeResponse = new ProPayResponse();

            base.ResetValues();
        }

        protected override PaymentAuthorizationResponse ChargePayment()
        {
            throw new NotImplementedException();
        }

        protected override PaymentAuthorizationResponse RefundPayment(decimal refundAmount)
        {
            throw new NotImplementedException();
        }
    }
}
 