using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business;
using System.Linq.Expressions;
using System.Data.SqlClient;
using NetSteps.Data.Entities.Exceptions;
using System.Data;
//Modificaciones:
//@1 20151607 BR-CC-012 GYS MD: Se implemento la clase OrderPaymentsRepository para la tabla de OrderPayments
namespace NetSteps.Data.Entities.Repositories
{
      /// <summary>
    /// Repositorio para los metodos utilizados con la entidad  OrderPayments
    /// </summary>
    public  class OrderPaymentsRepository
    {
        /// <summary>
        ///  @1 Busca el registro de OrderPayments  con TicketNumber = @TicketNumber
        /// </summary>
        /// <param name="TicketNumber"></param>
        /// <returns></returns>
        public List<OrderPayments> BrowseOrderPaymentsByTicketNumber(int TicketNumber)
        {
            List<OrderPayments> result = new List<OrderPayments>();
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@TicketNumber", TicketNumber },
                                                                                          };
                SqlDataReader reader = DataAccess.GetDataReader("spGetOrderPaymentsByTicketNumber", parameters, "Core");

                if (reader.HasRows)
                {
                    result = new List<OrderPayments>();
                    while (reader.Read())
                    {
                        OrderPayments vlOrderPayments = new OrderPayments();
                     
                          vlOrderPayments.OrderPaymentID = Convert.ToInt32(reader["OrderPaymentID"]);
                          vlOrderPayments.OrderID =  Convert.ToInt32(reader["OrderID"]);
                          vlOrderPayments.OrderCustomerID = GetValueOrNull<Int32>(reader["OrderCustomerID"].ToString());
                          vlOrderPayments.PaymentTypeID =  Convert.ToInt32(reader["PaymentTypeID"]);
                          vlOrderPayments.CurrencyID =  Convert.ToInt32(reader["CurrencyID"]);
                          vlOrderPayments.OrderPaymentStatusID =  Convert.ToInt16(reader["OrderPaymentStatusID"]);
                          vlOrderPayments.CreditCardTypeID = GetValueOrNull<Int16>(reader["CreditCardTypeID"].ToString());
                          vlOrderPayments.NameOnCard = Convert.ToString(reader["NameOnCard"]);
                          vlOrderPayments.AccountNumber = Convert.ToString(reader["AccountNumber"]);
                          vlOrderPayments.BillingFirstName = Convert.ToString(reader["BillingFirstName"]);
                          vlOrderPayments.BillingLastName = Convert.ToString(reader["BillingLastName"]);
                          vlOrderPayments.BillingName = Convert.ToString(reader["BillingName"]);
                          vlOrderPayments.BillingAddress1 = Convert.ToString(reader["BillingAddress1"]);
                          vlOrderPayments.BillingAddress2 = Convert.ToString(reader["BillingAddress2"]);
                          vlOrderPayments.BillingAddress3 = Convert.ToString(reader["BillingAddress3"]);
                          vlOrderPayments.BillingCity = Convert.ToString(reader["BillingCity"]);
                          vlOrderPayments.BillingCounty = Convert.ToString(reader["BillingCounty"]);
                          vlOrderPayments.BillingState = Convert.ToString(reader["BillingState"]);
                          vlOrderPayments.BillingStateProvinceID = GetValueOrNull<Int32>(reader["BillingStateProvinceID"].ToString());
                          vlOrderPayments.BillingPostalCode = Convert.ToString(reader["BillingPostalCode"]);
                          vlOrderPayments.BillingCountryID = GetValueOrNull<Int32>(reader["BillingCountryID"].ToString());
                          vlOrderPayments.BillingPhoneNumber = Convert.ToString(reader["BillingPhoneNumber"]);
                          vlOrderPayments.IdentityNumber = Convert.ToString(reader["IdentityNumber"]);
                          vlOrderPayments.IdentityState = Convert.ToString(reader["IdentityState"]);
                          vlOrderPayments.Amount = Convert.ToDecimal(reader["Amount"]);
                          vlOrderPayments.RoutingNumber = Convert.ToString(reader["RoutingNumber"]);
                          vlOrderPayments.IsDeferred = Convert.ToBoolean(reader["IsDeferred"]);
                          vlOrderPayments.ProcessOnDateUTC = GetValueOrNull<DateTime>(reader["ProcessOnDateUTC"].ToString());
                          vlOrderPayments.ProcessedDateUTC = GetValueOrNull<DateTime>(reader["ProcessedDateUTC"].ToString());
                          vlOrderPayments.TransactionID = Convert.ToString(reader["TransactionID"]);
                          vlOrderPayments.DeferredAmount = GetValueOrNull<Int32>(reader["DeferredAmount"].ToString());
                          vlOrderPayments.DeferredTransactionID = Convert.ToString(reader["DeferredTransactionID"]);
                          vlOrderPayments.ExpirationDateUTC = GetValueOrNull<DateTime>(reader["ExpirationDateUTC"].ToString());
                          vlOrderPayments.ModifiedByUserID = GetValueOrNull<Int32>(reader["ModifiedByUserID"].ToString());
                          vlOrderPayments.Request = Convert.ToString(reader["Request"]);
                          vlOrderPayments.AccountNumberLastFour = Convert.ToString(reader["AccountNumberLastFour"]);
                          vlOrderPayments.PaymentGatewayID = GetValueOrNull<Int16>(reader["PaymentGatewayID"].ToString());
                          vlOrderPayments.SourceAccountPaymentMethodID = GetValueOrNull<Int32>(reader["SourceAccountPaymentMethodID"].ToString());
                          vlOrderPayments.BankAccountTypeID = GetValueOrNull<Int16>(reader["BankAccountTypeID"].ToString());
                          vlOrderPayments.BankName = Convert.ToString(reader["BankName"]);
                          vlOrderPayments.NachaClassType = Convert.ToString(reader["NachaClassType"]);
                          vlOrderPayments.NachaSentDate = GetValueOrNull<DateTime>(reader["NachaSentDate"].ToString());
                          vlOrderPayments.ETLNaturalKey = Convert.ToString(reader["ETLNaturalKey"]);
                          vlOrderPayments.ETLHash = Convert.ToString(reader["ETLHash"]);
                          vlOrderPayments.ETLPhase = Convert.ToString(reader["ETLPhase"]);
                          vlOrderPayments.ETLDate = GetValueOrNull<DateTime>(reader["ETLDate"].ToString());
                          vlOrderPayments.DateCreatedUTC = Convert.ToDateTime(reader["DateCreatedUTC"]);
                          vlOrderPayments.DateLastModifiedUTC = GetValueOrNull<DateTime>(reader["DateLastModifiedUTC"].ToString());
                          vlOrderPayments.BillingStreet = Convert.ToString(reader["BillingStreet"]);
                          vlOrderPayments.NegotiationLevelID = GetValueOrNull<Int32>(reader["NegotiationLevelID"].ToString());
                          vlOrderPayments.OrderExpirationStatusID = GetValueOrNull<Int32>(reader["OrderExpirationStatusID"].ToString());
                          vlOrderPayments.PaymentConfigurationID = GetValueOrNull<Int32>(reader["PaymentConfigurationID"].ToString());
                          vlOrderPayments.FineAndInterestsRulesID = GetValueOrNull<Int32>(reader["FineAndInterestsRulesID"].ToString());
                          vlOrderPayments.TicketNumber = GetValueOrNull<Int32>(reader["TicketNumber"].ToString());
                          vlOrderPayments.OriginalExpirationDate = GetValueOrNull<DateTime>(reader["OriginalExpirationDate"].ToString());
                          vlOrderPayments.CurrentExpirationDateUTC = GetValueOrNull<DateTime>(reader["CurrentExpirationDateUTC"].ToString());
                          vlOrderPayments.InitialAmount = GetValueOrNull<Int32>(reader["InitialAmount"].ToString());
                          vlOrderPayments.FinancialAmount = GetValueOrNull<Int32>(reader["FinancialAmount"].ToString());
                          vlOrderPayments.DiscountedAmount = GetValueOrNull<Int32>(reader["DiscountedAmount"].ToString());
                          vlOrderPayments.TotalAmount = GetValueOrNull<decimal>(reader["TotalAmount"].ToString());
                          vlOrderPayments.DateLastTotalAmountUTC = GetValueOrNull<DateTime>(reader["DateLastTotalAmountUTC"].ToString());
                          vlOrderPayments.Accepted = GetValueOrNull<Boolean>(reader["Accepted"].ToString());
                          vlOrderPayments.Forefit = GetValueOrNull<Boolean>(reader["Forefit"].ToString());

                          result.Add(vlOrderPayments);
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }

          public static T? GetValueOrNull<T>(string valueAsString)
              where T : struct
          {
              if (string.IsNullOrEmpty(valueAsString))
                  return null;
              return (T) Convert.ChangeType(valueAsString, typeof (T));
          }

        /// <summary>
        /// Developed By AG
        /// Req: BR-CC-015
        /// </summary>
        /// <param name="oenOrderPayment"></param>
          public void RegisterOrderPayment(OrderPaymentNegotiationData oenOrderPayment)
          {

              var tbOrderPayment = new OrderPaymentsSearchDataType();
              tbOrderPayment.Add(oenOrderPayment);

              using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
              {

                  connection.Open();
                  SqlCommand cmd = new SqlCommand();
                  cmd.Connection = connection;
                  cmd.CommandType = CommandType.StoredProcedure;
                  cmd.CommandText = "[dbo].[UpsInsOrderPayments]";
                  SqlParameter param2 = cmd.Parameters.Add("@piTypeOrderPayment", SqlDbType.Structured);
                  param2.Direction = ParameterDirection.Input;
                  param2.TypeName = "uCOR_OrderPayments";
                  param2.Value = tbOrderPayment.Count == 0 ? null : tbOrderPayment;
                  try
                  {
                      cmd.ExecuteNonQuery();
                  }
                  catch (Exception ex)
                  {
                      throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
                  }
                  


              }

          }
    }
}
