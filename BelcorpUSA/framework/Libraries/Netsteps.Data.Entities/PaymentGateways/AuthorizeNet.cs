using System;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using NetSteps.Addresses.Common.Models;
using NetSteps.Common.Base;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Base;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.PaymentGateways
{
	/// <summary>
	/// Authorize.Net payment gateway implementation.
	/// http://www.provue.com/authorizenet/testing/
	/// http://www.authorize.net/support/merchant/Transaction_Response/Response_Reason_Codes_and_Response_Reason_Text.htm
	/// </summary>
	internal class AuthorizeNet : BasePaymentGateway
	{
		#region Enums
		public enum ANetAuthorizeType
		{
			AUTH_CAPTURE,
			AUTH_ONLY,
			CAPTURE_ONLY,
			CREDIT,
			VOID,
			PRIOR_AUTH_CAPTURE
		}

		public enum ErrorLevel
		{
			Approved,
			Declined,
			ANetError,
			HTTPRequestError
		}
		#endregion


		#region Constructors

		public AuthorizeNet(PaymentGatewaySection paymentGatewaySection)
			: base(paymentGatewaySection)
		{

		}

		#endregion

		#region Members
		private static string _lastInvoice = string.Empty;
		private static string _lastMessage = string.Empty;

		private ANetChargeRequest _chargeRequest = new ANetChargeRequest();
		private ANetResponse _chargeResponse = new ANetResponse();

		#endregion

		// ******************** NOTE ******************** 
		// THE FULL AUTHORIZE.NET MANUAL CAN BE FOUND AT: http://www.authorize.net/support/AIM_guide.pdf
		// ********************************************** 


		// TEST CARD NUMBER   CARD TYPE
		// 370000000000002    American Express
		// 6011000000000012   Discover
		// 5424000000000015   MasterCard
		// 4007000000027      Visa    
		// 4222222222222      Force a an error - error number will be the number in the amount field

		#region Methods
		public override PaymentAuthorizationResponse Charge(OrderPayment orderPayment)
		{
			using (new ApplicationUsageLogger(new ExecutionContext(this)))
			{
				base.ResetValues();
				CurrentOrderPayment = orderPayment;

				if (!CurrentOrderPayment.ChangeTracker.ChangeTrackingEnabled)
					CurrentOrderPayment.StartTracking();

				string authorizeId;
				string authorizePin;

				if (CurrentOrderPayment.Amount <= 0)
				{
					// No amount will be charged. Tests are done with 0.01 charged.  
					// Just return true since some products have no cost.
					Result.Message = string.Empty;
					Result.Success = true;
					Result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Error;
					return Result;
					//result.Message = "Invalid amount. The amount must be greater than zero.";
					//result.Success = false;
					//return result;
				}
				// Prevents duplicate payments.
				if (!string.IsNullOrEmpty(CurrentOrderPayment.TransactionID))
				{
					Result.Message = "This transaction has already been processed.";
					Result.Success = false;
					Result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Error;
					return Result;
				}
				if (CurrentOrderPayment.DecryptedAccountNumber.IsNullOrEmpty())
				{
					Result.Message = "Error authorizing card. Credit Card number is required.";
					Result.Success = false;
					Result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Error;
					return Result;
				}

				decimal currentBalance = 0;
				ValidationResponse = ValidateCharge(ref currentBalance);
				if (!ValidationResponse.Success)
				{
					Result.Message = ValidationResponse.Message;
					Result.Success = false;
					Result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Error;
					return Result;
				}

				if (!LiveMode)
				{
					//URL: https://test.authorize.net/
					//Full Name: NetSteps LLC
					//Email: nsadmins@netsteps.com
					//Login: nsadmins@netsteps.com
					//password: KHNpjdYp3
					//API Login ID: 3Yv3D9Xbe
					//Transaction Key: 2455Sx42MfD59z5g
					//Secret Question Answer:  Simon2

					authorizeId = TestAuthorizeID();        // "3Yv3D9Xbe"; // NetSteps Test ID.
					authorizePin = TestAuthorizePin();      // "2455Sx42MfD59z5g";

					//It is necessary we force this to .01 so we don't go over limits.
					_chargeRequest.Amount = TestMinAmount();
				}
				else
				{
					authorizeId = _paymentGatewaySection.Login;
					authorizePin = _paymentGatewaySection.Password;
				}

				_chargeRequest.Testing = TestTransaction;
				_chargeRequest.LoginID = authorizeId;
				_chargeRequest.TransactionKey = authorizePin;
				_chargeRequest.AuthorizeType = ANetAuthorizeType.AUTH_CAPTURE;

				IAddress address = CurrentOrderPayment as IAddress;

				_chargeRequest.BillingFirstName = CurrentOrderPayment.BillingFirstName.RemoveDiacritics();
				_chargeRequest.BillingLastName = CurrentOrderPayment.BillingLastName.RemoveDiacritics();

				if (address != null) // I am not sure if Authorize.Net required address or not. - JHE
				{
					_chargeRequest.BillingStreetAddress = address.Address1.RemoveDiacritics();
					_chargeRequest.BillingCity = address.City.RemoveDiacritics();
					_chargeRequest.BillingState = address.State.RemoveDiacritics();
					_chargeRequest.BillingZip = address.PostalCode.RemoveDiacritics();
					if (!string.IsNullOrEmpty(address.Country))
						_chargeRequest.BillingCountry = address.Country.RemoveDiacritics();
				}
				_chargeRequest.BillingPhone = CurrentOrderPayment.BillingPhoneNumber.RemoveDiacritics();

				_chargeRequest.CardNum = CurrentOrderPayment.DecryptedAccountNumber;
				//_chargeRequest.ExpDate = CurrentOrderPayment.ExpirationDate.Value.ToExpirationString(); // TODO: I don't think the expiration format is correct. - JHE
				// Conversion from UTC was rolling the month back and causing declines
				_chargeRequest.ExpDate = CurrentOrderPayment.ExpirationDateUTC.Value.ToString("MMyyyy");
				_chargeRequest.CardCode = CurrentOrderPayment.Cvv;

				// Prevent accidental hold on account in live mode but test transaction, force to 1 penny.
				if (TestTransaction)
				{
					// simulate the “card declined” error if using the test card number that forces an error
					if (_chargeRequest.CardNum == "4222222222222")
					{
						_chargeRequest.Amount = TestFailAmount();
						_chargeRequest.CardNum = TestFailCreditCardNumber();
					}
					else
						_chargeRequest.Amount = TestMinAmount();
				}
				else
					_chargeRequest.Amount = CurrentOrderPayment.Amount;

				if (CurrentOrderPayment.OrderCustomer != null)
				{
					_chargeRequest.CompanyID = CurrentOrderPayment.OrderCustomer.OrderCustomerID.ToString();
					_chargeRequest.CompanyName = CurrentOrderPayment.OrderCustomer.FullName.RemoveDiacritics();
					_chargeRequest.BillingEmail = CurrentOrderPayment.OrderCustomer.AccountInfo.EmailAddress.RemoveDiacritics();
				}

				_chargeRequest.Description = CurrentOrderPayment.Order.OrderID + "_" + CurrentOrderPayment.OrderPaymentID + "_" +
											CurrentOrderPayment.Order.ConsultantInfo.AccountNumber;
				_chargeRequest.InvoiceNumber = CurrentOrderPayment.Order.OrderNumber;

				_chargeResponse = ChargeCard(_chargeRequest);

				SetTransactionChargeOrderPaymentResult();
				base.RecordTransaction();

				Result.Message = _chargeResponse.ResponseReasonText;

				if (_chargeResponse.ResponseReasonCode == "30") // The configuration with processor is invalid. Call Merchant Service Provider.
				{
					if (_chargeRequest.CardNum.Length == 15 && _chargeRequest.CardNum.StartsWith("3"))
						Result.Message = "We do not accept American Express.";
					else
						Result.Message = "The card type of the credit card provided is not supported. Please try a different credit card type.";
				}

				Result.Success = false;
				switch (_chargeResponse.ErrorLevel)
				{
					case ErrorLevel.Approved:
						CurrentOrderPayment.TransactionID = _chargeResponse.TransactionID;
						Result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Authorized;
						Result.Message = TermTranslatedGatewayMessage(Result.Message, _chargeRequest.CardNum.MaskString(4));
						Result.Success = true;
						break;
					case ErrorLevel.Declined:
						Result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Declined;
						if (_chargeResponse.ResponseReasonCode == "27") //AVS Decline
							Result.Message = string.Format("The billing address provided does not match the address on file for the provided credt card. Please verify your billing address or choose a different payment method (CC#: {0}).", _chargeRequest.CardNum.MaskString(4));
						else
							Result.Message = TermTranslatedGatewayMessage(Result.Message, _chargeRequest.CardNum.MaskString(4), "Please select a different payment method");

						break;
					case ErrorLevel.ANetError:
						Result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Error;
						Result.Message = TermTranslatedGatewayMessage(Result.Message, _chargeRequest.CardNum.MaskString(4));

						break;
					case ErrorLevel.HTTPRequestError:
						Result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Error;
						break;
					default:
						break;
				}

				CurrentOrderPayment.ProcessedDate = CurrentOrderPayment.ProcessOnDate = DateTime.Now;
				CurrentOrderPayment.Save();

				return Result;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="gatewayErrorMessage">Message returned from payment gateway</param>
		/// <param name="maskedCardNum">masked credit card number related to message</param>
		/// <param name="customErrorMessage">optional custom error message</param>
		/// <returns>translated message with masked cc number, or just the masked cc number if no messages</returns>
		public virtual string TermTranslatedGatewayMessage(string gatewayErrorMessage, string maskedCardNum, string customErrorMessage = null)
		{
			bool gatewayErrorMessageIsNullOrEmpty = string.IsNullOrEmpty(gatewayErrorMessage);
			bool customErrorMessageIsNullOrEmpty = string.IsNullOrEmpty(customErrorMessage);

			if (!gatewayErrorMessageIsNullOrEmpty || !customErrorMessageIsNullOrEmpty)
			{
				string message = !customErrorMessageIsNullOrEmpty
									 ? string.Format("{0} {1}", gatewayErrorMessage, customErrorMessage)
									 : gatewayErrorMessage;

				string translated = Translation.GetTerm(message.ToTitleCase().Replace(" ", ""), message);
				return string.Format("{0} (CC#: {1})", translated, maskedCardNum);
			}
			else
			{
				return string.Format("(CC#: {0})", maskedCardNum);
			}
		}

		public override PaymentAuthorizationResponse Refund(OrderPayment orderPayment, decimal amount)
		{
			Contract.Requires<ArgumentNullException>(orderPayment != null);
			Contract.Requires<ArgumentException>(orderPayment.Order != null);
			Contract.Requires<ArgumentException>(orderPayment.Order.ParentOrderID != null, "Parent order id must not be null for refunds.");

			using (new ApplicationUsageLogger(new ExecutionContext(this)))
			{


				base.ResetValues();
				CurrentOrderPayment = orderPayment;

				string authorizeId;
				string authorizePin;
				//bool success = false;
				decimal calcAmount = amount;

				if (LiveMode && string.IsNullOrEmpty(CurrentOrderPayment.TransactionID))
				{
					Result.Message = "A valid transaction Id is needed";
					Result.Success = false;
					Result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Error;
					return Result;
				}

				if (calcAmount < 0)
					calcAmount = 0 - calcAmount; // make it a positive amount for comparison
				if (calcAmount > CurrentOrderPayment.Amount)
				{
					Result.Message = "The amount being requested for refund must be less than or equal to the original settled amount.";
					Result.Success = false;
					Result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Error;
					return Result;
				}

				_chargeRequest = new ANetChargeRequest();

				if (!LiveMode)
				{
					//URL: test.authorize.net
					//Full Name: NetSteps LLC
					//Email: nsadmins@netsteps.com
					//Login: nsadmins@netsteps.com
					//password: N3tst3ps
					//API Login ID: 3Yv3D9Xbe
					//Transaction Key: 2455Sx42MfD59z5g
					//Secret Question Answer:  Simon

					authorizeId = TestAuthorizeID();    // "3Yv3D9Xbe"; // NetSteps Test ID.
					authorizePin = TestAuthorizePin();  // "2455Sx42MfD59z5g";

					//It is necessary we force this to .01 so we don't go over limits.
					_chargeRequest.Amount = TestMinAmount();
				}
				else
				{
					authorizeId = _paymentGatewaySection.Login;
					authorizePin = _paymentGatewaySection.Password;
				}

				_chargeRequest.Testing = TestTransaction;
				_chargeRequest.LoginID = authorizeId;
				_chargeRequest.TransactionKey = authorizePin;
				//_chargeRequest.AuthorizeType = AuthorizeNet.ANetAuthorizeType.VOID; // See comments below by JLS for why this is VOID instead of CREDIT

				_chargeRequest.AuthorizeType = SetAuthorizeType(CurrentOrderPayment, amount);

				_chargeRequest.TransactionID = GetTransactionID(CurrentOrderPayment); // CurrentOrderPayment.TransactionID;

				IAddress address = CurrentOrderPayment as IAddress;

				_chargeRequest.BillingFirstName = address.FirstName.RemoveDiacritics();
				_chargeRequest.BillingLastName = address.LastName.RemoveDiacritics();
				_chargeRequest.BillingStreetAddress = address.Address1.RemoveDiacritics();
				_chargeRequest.BillingCity = address.City.RemoveDiacritics();
				_chargeRequest.BillingState = address.State.RemoveDiacritics();
				_chargeRequest.BillingZip = address.PostalCode.RemoveDiacritics();
				if (!string.IsNullOrEmpty(address.Country))
					_chargeRequest.BillingCountry = address.Country.RemoveDiacritics();
				_chargeRequest.BillingPhone = CurrentOrderPayment.BillingPhoneNumber.RemoveDiacritics();

				_chargeRequest.CardNum = CurrentOrderPayment.DecryptedAccountNumber;
				//_chargeRequest.ExpDate = CurrentOrderPayment.ExpirationDate.Value.ToExpirationString();
				// Conversion from UTC was rolling the month back and causing declines
				if (CurrentOrderPayment.ExpirationDateUTC.HasValue)
				{
					_chargeRequest.ExpDate = CurrentOrderPayment.ExpirationDateUTC.Value.ToString("MMyyyy");
				}
				_chargeRequest.CardCode = CurrentOrderPayment.Cvv;

				// Prevent accidental hold on account in live mode but test transaction, force to 1 penny.
				if (TestTransaction)
					_chargeRequest.Amount = TestMinAmount();
				else
					_chargeRequest.Amount = calcAmount;

				if (CurrentOrderPayment.OrderCustomer != null)
				{
					_chargeRequest.CompanyID = CurrentOrderPayment.OrderCustomer.OrderCustomerID.ToString();
					_chargeRequest.CompanyName = CurrentOrderPayment.OrderCustomer.FullName.RemoveDiacritics();
					_chargeRequest.BillingEmail = CurrentOrderPayment.OrderCustomer.AccountInfo.EmailAddress.RemoveDiacritics();
				}

				_chargeRequest.Description = CurrentOrderPayment.Order.OrderID + "_" + CurrentOrderPayment.OrderPaymentID + "_" +
											CurrentOrderPayment.Order.ConsultantInfo.AccountNumber + "_Refund";
				_chargeRequest.InvoiceNumber = CurrentOrderPayment.Order.OrderNumber;

				// JLS 09-16-2009
				// The idea behind the refund is to first try voiding the transaction, and if that doesn't work to try crediting the transaction.
				// This is as per instructed by the Authorize.NET documentation.  
				//
				// I changed the AuthorizeType above to VOID, instead of CREDIT.  I'm adding an immediate check after processing the transaction below to see if it failed.  If so
				// I'm changing it back to CREDIT and processing it again.  All other logic/processing should remain the same;

				_chargeResponse = ChargeCard(_chargeRequest);

				if (_chargeResponse.ErrorLevel == AuthorizeNet.ErrorLevel.ANetError || _chargeResponse.ErrorLevel == AuthorizeNet.ErrorLevel.Declined)
				{
					if (_chargeRequest.AuthorizeType != ANetAuthorizeType.CREDIT)
					{
						// Reverting back to a CREDIT
						_chargeRequest.AuthorizeType = ANetAuthorizeType.CREDIT;
						_chargeResponse = ChargeCard(_chargeRequest);
					}
				}

				Result.Message = _chargeResponse.ResponseReasonText;

				_chargeRequest.Amount = calcAmount; // Restore the original amount (in case it was modified for a test transaction)
				if (_chargeRequest.Amount > 0)
					_chargeRequest.Amount = 0 - _chargeRequest.Amount; // make it a negative (credit) amount for database storage

				SetRecordTransactionRefundOrderPaymentResult();
				base.RecordTransaction();

				switch (_chargeResponse.ErrorLevel)
				{
					case ErrorLevel.Approved:
						/// Because a payment needs to submit the original transactionid we will not update the payment transactionid, it can be
						/// accessed via the payment result.  --BJC
						//orderPayment.TransactionId = chargeResponse.TransactionID;
						CurrentOrderPayment.OrderPaymentStatusID = Constants.OrderPaymentStatus.Completed.ToShort();
						Result.Success = true;
						Result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Authorized;
						break;
					case ErrorLevel.Declined:
						Result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Declined;
						break;
					case ErrorLevel.ANetError:
						Result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Error;
						break;
					case ErrorLevel.HTTPRequestError:
						Result.GatewayAuthorizationStatus = Constants.GatewayAuthorizationStatus.Error;
						Result.Success = false;
						break;
					default:
						Result.Success = false;
						break;
				}

				CurrentOrderPayment.ProcessedDate = CurrentOrderPayment.ProcessOnDate = DateTime.Now;
				CurrentOrderPayment.Save();

				return Result;
			}
		}

		public virtual ANetAuthorizeType SetAuthorizeType(OrderPayment currentOrderPayment, decimal refundAmount)
		{
			int parentOrderID = currentOrderPayment.Order.ParentOrderID.Value;

			Order order = Order.LoadOrderWithPaymentDetails(parentOrderID);

			var isVoidable =
				order.OrderPayments.Any(op => op.OrderPaymentStatusID == (short)Constants.OrderPaymentStatus.Completed
											  && op.TransactionID == currentOrderPayment.TransactionID
											  && op.AccountNumber == currentOrderPayment.AccountNumber
											  && op.Amount == refundAmount);

			return isVoidable
					   ? ANetAuthorizeType.VOID
					   : ANetAuthorizeType.CREDIT;
		}

		public virtual string GetTransactionID(OrderPayment currentOrderPayment)
		{
			return currentOrderPayment.TransactionID;
		}

		public ANetResponse ChargeCard(ANetChargeRequest chargeRequest)
		{
			using (new ApplicationUsageLogger(new ExecutionContext(this)))
			{
				string url = GetPaymentGatewayUrl(LiveMode);

				ANetResponse anetResponse = new ANetResponse();

				NameValueCollection args = new NameValueCollection();
				args.Add("x_version", "3.1");
				args.Add("x_delim_data", "True");
				args.Add("x_delim_char", "|");
				args.Add("x_relay_response", "False");

				args.Add("x_login", chargeRequest.LoginID);
				args.Add("x_tran_key", chargeRequest.TransactionKey);
				args.Add("x_amount", chargeRequest.Amount.ToString());
				args.Add("x_card_num", chargeRequest.CardNum);
				args.Add("x_exp_date", chargeRequest.ExpDate);
				args.Add("x_type", chargeRequest.AuthorizeType.ToString());
				args.Add("x_card_code", chargeRequest.CardCode);

				args.Add("x_first_name", chargeRequest.BillingFirstName);
				args.Add("x_last_name", chargeRequest.BillingLastName);
				args.Add("x_address", chargeRequest.BillingStreetAddress);
				args.Add("x_city", chargeRequest.BillingCity);
				args.Add("x_state", chargeRequest.BillingState);
				args.Add("x_zip", chargeRequest.BillingZip);
				args.Add("x_country", chargeRequest.BillingCountry);
				args.Add("x_phone", chargeRequest.BillingPhone);
				//args.Add("x_email", chargeRequest.BillingEmail.ToString()); //Don't have Anet email customers.

				args.Add("x_test_request", chargeRequest.Testing ? "TRUE" : "FALSE");
				//args.Add("x_email_customer", chargeRequest.Testing ? "FALSE" : "TRUE"); //Don't have Anet email customers.
				args.Add("x_method", "CC");

				args.Add("x_duplicate_window", "600");

				args.Add("x_cust_id", chargeRequest.CompanyID);
				args.Add("x_company", chargeRequest.CompanyName);
				args.Add("x_description", chargeRequest.Description);
				args.Add("x_invoice_num", chargeRequest.InvoiceNumber);

				switch (chargeRequest.AuthorizeType)
				{
					case ANetAuthorizeType.VOID:
					case ANetAuthorizeType.CREDIT:
						args.Add("x_trans_id", chargeRequest.TransactionID);
						break;
				}

				// loop through all the entries in the name value collection and build a delimited string to pass to a.net
				string postData = string.Empty;
				foreach (string key in args.AllKeys)
				{
					if (postData != string.Empty)
						postData += "&";

					postData += string.Format("{0}={1}", key, args[key]);
				}


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
				}
				catch (Exception e)
				{
					NetSteps.Data.Entities.Exceptions.ExceptionLogger.LogException(e, true);

					anetResponse.ErrorLevel = ErrorLevel.HTTPRequestError;
					anetResponse.ErrorMessage = e.Message;

					// It's possible that the transaction went through, even though we got an HTTP error.
					// Twice we have seen the response "Unable to read data from the transport connection...."
					// TODO: Generate an email notification and, if possible, check to see if the transaction went through.
					SendANetFailureNotification(chargeRequest, e.Message);
					return anetResponse;
				}

				try
				{
					HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
					using (StreamReader reader = new StreamReader(objResponse.GetResponseStream()))
					{
						anetResponse.RawResponseText = reader.ReadToEnd();

						String[] responseParts = anetResponse.RawResponseText.Split('|');

						// the response should be a string of delimited items. If the string doesn't contain enough delimited items 
						// then it wasn't a proper response and is treated like an error.
						if (responseParts.Length < 39)
						{
							anetResponse.ErrorLevel = ErrorLevel.HTTPRequestError;
							anetResponse.ErrorMessage = "Improperly formed response";

							return anetResponse;
						}

						anetResponse.ResponseCode = responseParts[0];
						anetResponse.ResponseSubCode = responseParts[1];
						anetResponse.ResponseReasonCode = responseParts[2];
						anetResponse.ResponseReasonText = responseParts[3];
						anetResponse.ApprovalCode = responseParts[4];
						anetResponse.AVSResult = responseParts[5]; // avs = address verification system
						anetResponse.TransactionID = responseParts[6];
						anetResponse.CardCodeResponse = responseParts[38];

						switch (anetResponse.ResponseCode)
						{
							case "1":
								anetResponse.ErrorLevel = ErrorLevel.Approved;
								break;
							case "2":
							case "4": // held for review - in our case same as decline
								anetResponse.ErrorLevel = ErrorLevel.Declined;
								break;
							case "3":
								anetResponse.ErrorLevel = ErrorLevel.ANetError;
								break;
							default:
								anetResponse.ErrorLevel = ErrorLevel.ANetError;
								break;
						}

						// Close and clean up the StreamReader
						reader.Close();
					}
				}
				catch (Exception e)
				{
					NetSteps.Data.Entities.Exceptions.ExceptionLogger.LogException(e, true);
					anetResponse.ErrorLevel = ErrorLevel.HTTPRequestError;
					anetResponse.ErrorMessage = e.Message;
					SendANetFailureNotification(chargeRequest, e.Message);

					return anetResponse;
				}

				return anetResponse;
			}
		}

		public void SendANetFailureNotification(ANetChargeRequest chargeRequest, string message)
		{
			using (new ApplicationUsageLogger(new ExecutionContext(this)))
			{
				if (chargeRequest.InvoiceNumber == _lastInvoice && message == _lastMessage)
					return; // Already sent an email about this issue

				_lastInvoice = chargeRequest.InvoiceNumber;
				_lastMessage = message;

				if (!String.IsNullOrEmpty(AlertEmailAddresses))
				{
					char[] cDelims = { ',', ';', ' ' };
					string[] recipients;
					string toList = string.Empty;
					//if (CustomConfigurationHandler.Config.General.ServerEnviroment == Common.Constants.ServerEnviroment.DeveloperMachine)
					//    toList = CustomConfigurationHandler.Config.Debug.DevEmail;
					//else
					toList = AlertEmailAddresses;
					recipients = toList.Split(cDelims, StringSplitOptions.RemoveEmptyEntries);

					string subject = "Communication Failed with Authorize.Net";
					string body = String.Format(
							@"Failure in response from Authorize.Net: <b>{0}</b><br /><br />
							Invoice Number <b>{1}</b>: A charge in the amount of {2:c} for {3} {4} was attempted to be sent to Authorize.Net.
							The response from Authorize.Net failed and did not indicate whether the charge was successful.<br/><br />
							We recommend that you contact Authorize.Net to validate whether the transaction was completed and, if so, alert them
							that the transaction will probably be resent.",
							message, chargeRequest.InvoiceNumber, chargeRequest.Amount, chargeRequest.BillingFirstName,
							chargeRequest.BillingLastName);

					System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
					foreach (string to in recipients)
					{
						msg.To.Add(to);
					}
					msg.Subject = subject;
					msg.Body = body;
					msg.IsBodyHtml = true;

					SmtpClient smtp = new SmtpClient();
					smtp.Send(msg);
				}
			}
		}

		#endregion

		#region Helper Classes
		// Acceptable formats for ExpDate
		//MMYY,
		//MM/YY,
		//MM-YY,
		//MMYYYY,
		//MM/YYYY,
		//MM-YYYY,
		//YYYY-MM-DD,
		//YYYY/MM/DD
		public class ANetChargeRequest
		{
			public string LoginID;
			public string TransactionKey;
			public bool Testing;
			public ANetAuthorizeType AuthorizeType;
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

		// Possible ResponseReasonCode
		//  6 = The credit card number is invalid. 
		//  7 = The credit card expiration date is invalid. 
		//  8 = The credit card has expired.
		// 11 = A duplicate transaction has been submitted.
		// 17 = The merchant does not accept this type of credit card.
		// 28 = The merchant does not accept this type of credit card.
		public class ANetResponse
		{
			public ErrorLevel ErrorLevel = ErrorLevel.HTTPRequestError;
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
			AuthNetRecordTransaction();
		}

		protected override void SetRecordTransactionRefundOrderPaymentResult()
		{
			AuthNetRecordTransaction();
		}

		private void AuthNetRecordTransaction()
		{
			using (new ApplicationUsageLogger(new ExecutionContext(this)))
			{
				try
				{
					//OrderPaymentResult CurrentOrderPaymentResult = new OrderPaymentResult(CurrentOrderPayment);

					//CurrentOrderPaymentResult.CurrencyID = CurrentOrderPayment.CurrencyID;
					//CurrentOrderPaymentResult.ModifiedByUserID = ApplicationContext.Instance.CurrentUserID.ToIntNullable();

					//CurrentOrderPaymentResult.DateAuthorized = DateTime.Now;
					CurrentOrderPaymentResult.DecryptedAccountNumber = CurrentOrderPayment.DecryptedAccountNumber;
					CurrentOrderPaymentResult.Amount = _chargeRequest.Amount;
					//CurrentOrderPaymentResult.OrderID = CurrentOrderPayment.OrderID;
					//if (CurrentOrderPayment.OrderCustomerID != null && CurrentOrderPayment.OrderCustomerID != 0)
					//    CurrentOrderPaymentResult.OrderCustomerID = CurrentOrderPayment.OrderCustomerID.ToInt();
					//CurrentOrderPaymentResult.OrderPaymentID = CurrentOrderPayment.OrderPaymentID;
					CurrentOrderPaymentResult.ApprovalCode = _chargeResponse.ApprovalCode;
					CurrentOrderPaymentResult.ResponseReasonCode = _chargeResponse.ResponseReasonCode;
					CurrentOrderPaymentResult.ResponseCode = _chargeResponse.ResponseCode;
					CurrentOrderPaymentResult.ResponseSubCode = _chargeResponse.ResponseSubCode;
					CurrentOrderPaymentResult.ResponseReasonText = _chargeResponse.ResponseReasonText;
					CurrentOrderPaymentResult.AuthorizeType = _chargeRequest.AuthorizeType.ToString();
					CurrentOrderPaymentResult.CardCodeResponse = _chargeResponse.CardCodeResponse;
					CurrentOrderPaymentResult.ErrorMessage = _chargeResponse.ErrorMessage;
					CurrentOrderPaymentResult.Response = _chargeResponse.RawResponseText;
					CurrentOrderPaymentResult.AVSResult = _chargeResponse.AVSResult;
					CurrentOrderPaymentResult.TransactionID = _chargeResponse.TransactionID;
					CurrentOrderPaymentResult.IsTesting = _chargeRequest.Testing;

					//var paymentGateway = SmallCollectionCache.Instance.PaymentGateways.FirstOrDefault(g => g.Name == "Authorize.Net");
					//if (paymentGateway != null)
					//CurrentOrderPaymentResult.PaymentGatewayID = paymentGateway.PaymentGatewayID; // TODO: Set this - JHE
					//if (chargeRequest.AuthorizeType == ANetAuthorizeType.CREDIT)
					//{
					//    //orderPaymentResult.UserID = orderPayment.UserId;
					//    orderPaymentResult.UserID = ApplicationContext.Instance.CurrentUser.UserID;
					//}
					//else
					//{
					//    orderPaymentResult.UserID = -1;
					//}
					//CurrentOrderPaymentResult.Save();
				}
				catch (Exception ex)
				{
					throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsPaymentGatewayException);
				}
			}
		}

		public override void ResetValues()
		{
			_chargeRequest = new ANetChargeRequest();
			_chargeResponse = new ANetResponse();

			base.ResetValues();
		}

		public override BasicResponse ValidateCharge(OrderPayment orderPayment, ref decimal currentBalance)
		{
			return BaseCardValidation(orderPayment);
		}

		public override BasicResponse ValidateRefund(OrderPayment orderPayment, ref decimal currentBalance)
		{
			return BaseCardValidation(orderPayment);
		}

		private static BasicResponse BaseCardValidation(OrderPayment orderPayment)
		{
			BasicResponse response = new BasicResponse();
			response.Success = true;

			if (response.Success && orderPayment.AccountNumber.IsNullOrEmpty())
			{
				response.Success = false;
				response.Message = "Error applying payment to order. Invalid credit card number.";
			}

			if (response.Success && orderPayment.ExpirationDate.HasValue && orderPayment.ExpirationDate.Value.LastDayOfMonth().EndOfDay() < DateTime.Now.EndOfDay())
			{
				response.Success = false;
				response.Message = "Error applying payment to order. Credit card is expired.";
			}

			if (response.Success && orderPayment.NameOnCard.IsNullOrEmpty())
			{
				response.Success = false;
				response.Message = "Error applying payment to order. 'Name on card' is required.";
			}

			var result = orderPayment.IsPaymentValidForAuthorization();
			if (!result.Success)
			{
				response.Success = false;
				response.Message = result.Message;
			}

			return response;
		}

		protected override PaymentAuthorizationResponse ChargePayment()
		{
			throw new NotImplementedException();
		}

		protected override PaymentAuthorizationResponse RefundPayment(decimal refundAmount)
		{
			throw new NotImplementedException();
		}

		public override string GetPaymentGatewayUrl(bool isLiveMode)
		{
			string url = base.GetPaymentGatewayUrl(isLiveMode);

			if (isLiveMode)
			{
				return !String.IsNullOrWhiteSpace(url)
						  ? url
						  : "https://secure.authorize.net/gateway/transact.dll";

			}

			return !String.IsNullOrWhiteSpace(url)
					   ? url
					   : "https://test.authorize.net/gateway/transact.dll";
		}

		public override string TestAuthorizeID()
		{
			string testLogin = base.TestAuthorizeID();

			return !String.IsNullOrWhiteSpace(testLogin)
					   ? testLogin
					   : "3Yv3D9Xbe";
		}

		public override string TestAuthorizePin()
		{
			string testPassword = base.TestAuthorizePin();

			return !String.IsNullOrWhiteSpace(testPassword)
					   ? testPassword
					   : "2455Sx42MfD59z5g";
		}

		public override decimal TestMinAmount()
		{
			decimal testMinAmount = base.TestMinAmount();

			return testMinAmount > Decimal.Zero ? testMinAmount : 0.01m;
		}

		public override decimal TestFailAmount()
		{
			decimal testFailAmount = base.TestFailAmount();

			return testFailAmount > Decimal.Zero ? testFailAmount : 4m;
		}

		public override string TestFailCreditCardNumber()
		{
			string testFailCreditCardNum = base.TestFailCreditCardNumber();

			return !String.IsNullOrWhiteSpace(testFailCreditCardNum)
					   ? testFailCreditCardNum
					   : "4222222222222";
		}

	}
}