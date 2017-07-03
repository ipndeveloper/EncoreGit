using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business;
using System.Linq.Expressions;
using System.Data.SqlClient;
using NetSteps.Data.Entities.Exceptions;

//Modificaciones:
//@1 20151607 BR-CC-012 GYS MD: Se implemento la clase PaymentConfigurationsRepository para la tabla de PaymentConfigurations
namespace NetSteps.Data.Entities.Repositories
{
    /// <summary>
    /// Repositorio para los metodos utilizados con la entidad  PaymentConfigurations
    /// </summary>
    public class PaymentConfigurationsRepository 
    {
        /// <summary>
        /// @1 Busca el registro de PaymentConfigurations  con PaymentConfigurationID = @PaymentConfigurationID
        /// </summary>
        /// <param name="PaymentConfigurationID"></param>
        /// <returns></returns>
        public PaymentConfigurations BrowsePaymentConfigurationByPaymentConfigurationID(int? PaymentConfigurationID)
        {
           PaymentConfigurations result = new PaymentConfigurations();
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@PaymentConfigurationID", PaymentConfigurationID },
                                                                                          };
                SqlDataReader reader = DataAccess.GetDataReader("spGetPaymentConfigurationByPaymentConfigurationID", parameters, "Core");

                if (reader.HasRows)
                {
                    result = new PaymentConfigurations();
                    if (reader.Read())
                    {
                        return (new PaymentConfigurations()
                        {
                            PaymentConfigurationID = Convert.ToInt32(reader["PaymentConfigurationID"]),
                            CollectionEntityID = Convert.ToInt32(reader["CollectionEntityID"]),
                            OrderStatusID = Convert.ToInt16(reader["OrderStatusID"]),
                            DaysForPayment = Convert.ToInt32(reader["DaysForPayment"]),
                            FineAndInterestRulesID = Convert.ToInt32(reader["FineAndInterestRulesID"]),
                            TolerancePercentage = GetValueOrNull<double>(reader["TolerancePercentage"].ToString()),
                            ToleranceValue = GetValueOrNull<int>(reader["ToleranceValue"].ToString()),
                            PaymentExceeded = GetValueOrNull<bool>(reader["PaymentExceeded"].ToString()),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }

        public PaymentConfigurations GetPaymentConfigurationByOrderID(int OrderID)
        {
            try
            {
                PaymentConfigurations configuration = null;

                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[GetPaymentConfigurationByOrderID]";
                    cmd.Parameters.AddWithValue("@OrderID", OrderID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                configuration = new PaymentConfigurations();

                                #region [Assign Values]
                                configuration.PaymentConfigurationID = Convert.ToInt32(reader["PaymentConfigurationID"]);
                                configuration.CollectionEntityID = Convert.ToInt32(reader["CollectionEntityID"]);
                                configuration.OrderStatusID = Convert.ToInt16(reader["OrderStatusID"]);
                                configuration.DaysForPayment = Convert.ToInt32(reader["DaysForPayment"]);
                                configuration.FineAndInterestRulesID = Convert.ToInt32(reader["FineAndInterestRulesID"]);
                                configuration.TolerancePercentage = Convert.ToDouble(reader["TolerancePercentage"]);
                                configuration.ToleranceValue = Convert.ToInt32(reader["ToleranceValue"]);
                                configuration.PaymentExceeded = Convert.ToBoolean(reader["PaymentExceeded"]);
                                configuration.Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : string.Empty;
                                configuration.NumberCuotas = Convert.ToInt32(reader["NumberCuotas"]);
                                configuration.NumberDayVal = Convert.ToInt32(reader["NumberDayVal"]);
                                configuration.PaymentCredit = reader["PaymentCredit"] != DBNull.Value ? reader["PaymentCredit"].ToString() : string.Empty;
                                #endregion
                            }
                        }
                    }
                }

                return configuration;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static T? GetValueOrNull<T>(string valueAsString)
         where T : struct
        {
            if (string.IsNullOrEmpty(valueAsString))
                return null;
            return (T)Convert.ChangeType(valueAsString, typeof(T));
        }
    }
}