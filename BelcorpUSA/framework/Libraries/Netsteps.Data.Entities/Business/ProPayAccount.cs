using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using NetSteps.Common.Configuration;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Business
{
    public partial class ProPayAccount
    {
        public string Address { get; set; }
        public int? AccountNumber { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string ErrorMessage { get; set; }
        public bool Success { get; set; }

        private ProPayAccountRequest _accountRequest = new ProPayAccountRequest();

        public PaymentGateway CurrentPaymentGateway
        {
            get
            {
                Type type = this.GetType();
                var paymentGate = new PaymentGateway();
                string fullClassWithNamespace = string.Format("{0}.{1}", type.Namespace, type.Name);
                foreach (var paymentGateway in SmallCollectionCache.Instance.PaymentGateways)
                {
                    if (paymentGateway.Namespace.StartsWith(fullClassWithNamespace))
                        paymentGate = paymentGateway;
                }
                if (paymentGate == null)
                    throw new Exception("PaymentGateway not found for in Database for this class: " + fullClassWithNamespace);
                return paymentGate;
            }
        }
        public ProPayAccount GetAccountFromProPay(string accountNumber)
        {
            var account = new ProPayAccount();
            try
            {
                using (new ApplicationUsageLogger(new ExecutionContext(this)))
                {
                    SetUpAccountRequestProperties(accountNumber);
                    var response = PingAccount(_accountRequest);
                    if (response.Success)
                    {
                        account = new ProPayAccount
                                      {
                                          AccountNumber = Convert.ToInt32(response.AccountNum),
                                          Address = response.Addr,
                                          City = response.City,
                                          State = response.State,
                                          Zip = response.Zip,
                                          Success = true
                                      };
                    }
                    else
                    {
                        account = new ProPayAccount
                                      {
                                          ErrorMessage = response.ErrorMessage,
                                          Success = false
                                      };
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return account;
        }

        private void SetUpAccountRequestProperties(string accountNumber)
        {
            bool liveMode = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.IsPaymentLiveMode);
            if (liveMode)
            {
                
                var connectionInfo = PaymentGatewaySections.Instance.FirstOrDefault();
                    _accountRequest.CertStr = connectionInfo.Login;
                    _accountRequest.AccountNum = accountNumber;
            }
            else
            {
                //Check for client specific data first
                var clientAccountNum = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.ClientPropayTestAccountNum);
                var clientAuthId = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.ClientPropayTestAuthId);
                if (!string.IsNullOrEmpty(clientAccountNum) && !string.IsNullOrEmpty(clientAuthId))
                {
                    _accountRequest.CertStr = clientAuthId;
                    _accountRequest.AccountNum = clientAccountNum;
                }
                else
                {
                    //TODO: Use netsteps default info
                    _accountRequest.CertStr = "NetStepsCertString000000000001"; 
                    _accountRequest.AccountNum = accountNumber; 
                }
            }
        }

        public ProPayAccountResponse PingAccount(ProPayAccountRequest request)
        {
            using (new ApplicationUsageLogger(new ExecutionContext(this)))
            {
                bool liveMode = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.IsPaymentLiveMode);
                string url = liveMode ? "https://epay.propay.com/api/propayapi.aspx" : "https://xmltest.propay.com/api/propayapi.aspx";

                ProPayAccountResponse response = new ProPayAccountResponse();
                string postData = request.ToXmlAccountData();

                //if the request fails, return the anetResponse with an ErrorLevel and ErrorMessage.
                string errorMessage = "";
                response = SendProPayAccountRequest(postData, url, response, request, out errorMessage);

                return response;
            }
        }

        private ProPayAccountResponse SendProPayAccountRequest(string postData, string url, ProPayAccountResponse ppResponse, ProPayAccountRequest accountRequest, out string errorMessage)
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

            GetAccountResponse(ppResponse, objRequest);

            return ppResponse;
        }

        private ProPayAccountResponse GetAccountResponse(ProPayAccountResponse ppResponse, HttpWebRequest objRequest)
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

                foreach (DataRow objRow in objDTResults.Rows.Cast<DataRow>().Where(objRow => objRow["transType"].ToString() == "13"))
                {
                    ppResponse.Status = objRow["status"].ToString();
                    if (ppResponse.Status == "00" || ppResponse.Status == "0")
                    {
                        ppResponse.Success = true;
                        ppResponse.AccountNum = objRow["accountNum"].ToString() ?? String.Empty;
                        ppResponse.Tier = objRow["tier"].ToString() ?? String.Empty;
                        ppResponse.Affiliation = objRow["affiliation"].ToString() ?? String.Empty;
                        ppResponse.Expiration = objRow["expiration"].ToString() ?? String.Empty;
                        ppResponse.AccntStatus = objRow["accntStatus"].ToString() ?? String.Empty;
                        ppResponse.Addr = objRow["addr"].ToString() ?? String.Empty;
                        ppResponse.City =  objRow["city"].ToString() ?? String.Empty;
                        ppResponse.State = objRow["state"].ToString() ?? String.Empty;
                        ppResponse.Zip = objRow["zip"].ToString() ?? String.Empty;
                        break;
                    }
                    else
                    {
                        ppResponse.Success = false;
                        ppResponse.ErrorMessage = GetAccountResponseErrorMessage(ppResponse.Status);
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


        private String GetAccountResponseErrorMessage(string statusId)
        {
            var message = String.Empty;
            switch (statusId)
            {
                case "59":
                    message = "User not Authenticated";
                    break;
                case "55":
                    message = "The email address provided does not correspond to a ProPay account.";
                    break;
                case "47":
                    message = "Invalid Account Number";
                    break;
                default:
                    message = "Invalid Account";
                    break;
            }
            return message;
        }


        #region Helper Classes
        public class ProPayAccountRequest
        {
            public string CertStr; //max length 30
            public string Class = "partner";
            public int TransType = 13;
            public string AccountNum;
            public string TerminalId = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.ClientPropayTerminalId);

            public string ToXmlAccountData()
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
                objSB.Append("  </XMLTrans>" + System.Environment.NewLine);
                objSB.Append("</XMLRequest>" + System.Environment.NewLine);

                objSB.Replace("$CertStr", this.CertStr);
                objSB.Replace("$Class", this.Class);
                objSB.Replace("$TransType", this.TransType.ToString());
                objSB.Replace("$AccountNum", this.AccountNum.ToString());
                objSB.Replace("$TerminalId", this.TerminalId);

                objSB.Append(System.Environment.NewLine);

                return objSB.ToString();
            }
        }

        public class ProPayAccountResponse
        {
            public string AccountNum;
            public string Tier = String.Empty;
            public string Affiliation = String.Empty;
            public string Expiration;
            public string AccntStatus = String.Empty;
            public string SignupDate;
            public string Addr = String.Empty;
            public string City = String.Empty;
            public string State = String.Empty;
            public string Zip = String.Empty;
            public string ErrorMessage = String.Empty;
            public string Status = String.Empty;
            public bool Success;
        }

        #endregion
    }
}
