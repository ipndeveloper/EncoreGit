using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using System.Configuration;

//@01 20150717 BR-CC-003 G&S LIB: Se crea la clase con sus respectivos métodos


namespace NetSteps.Data.Entities.Repositories
{
    public class PaymentMethodsRepository
    {

        public static PaginatedList<PaymentMethodsSearchData> SearchDetails(PaymentMethodsSearchParameters searchParameter)
        {
            List<PaymentMethodsSearchData> paginatedResult = DataAccess.ExecWithStoreProcedureListParam<PaymentMethodsSearchData>("Core", "uspListPaymentMethods",

                new SqlParameter("FineAndInterestRulesID", SqlDbType.Int) { Value = (object)searchParameter.RuleID ?? DBNull.Value },
                new SqlParameter("orderStatusID", SqlDbType.Int) { Value = (object)searchParameter.OrderStatusID ?? DBNull.Value },
                new SqlParameter("collectionEntityID", SqlDbType.VarChar) { Value = searchParameter.CollectionEntityID },
                new SqlParameter("daysForPayment", SqlDbType.VarChar) { Value = searchParameter.DaysPayment },
                new SqlParameter("toleranceValue", SqlDbType.VarChar) { Value = searchParameter.tolerance  },
                new SqlParameter("AccountTypeRestrictionId", SqlDbType.Int) { Value = (object)searchParameter.AccountTypeId ?? DBNull.Value },
                new SqlParameter("OrderTypeRestrictionId", SqlDbType.Int) { Value = (object)searchParameter.OrderType ?? DBNull.Value },
                new SqlParameter("state", SqlDbType.VarChar) { Value = searchParameter.state },
                new SqlParameter("city", SqlDbType.VarChar) { Value = searchParameter.city },
                new SqlParameter("county", SqlDbType.VarChar) { Value = searchParameter.county  }                
                ).ToList();

            IQueryable<PaymentMethodsSearchData> matchingItems = paginatedResult.AsQueryable<PaymentMethodsSearchData>();

            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParameter);

            return matchingItems.ToPaginatedList<PaymentMethodsSearchData>(searchParameter, resultTotalCount);
        }

        public static  PaymentConfigurations EditPaymentConfigurations( int ID )
        {
            return DataAccess.ExecWithStoreProcedureListParam<PaymentConfigurations>(ConnectionStrings.BelcorpCore, "uspGetPaymentConfiguration",
                    new SqlParameter("paymentConfigurationID", SqlDbType.Int) { Value = ID },
                    new SqlParameter("secc", SqlDbType.Int) { Value = 0 }
                   ).ToList().FirstOrDefault(); 

        }


        public static List<PaymentConfigurationPerAccountSearchData> EditPaymentConfigurationPerAccount(int ID)
        {
            return DataAccess.ExecWithStoreProcedureListParam<PaymentConfigurationPerAccountSearchData>(ConnectionStrings.BelcorpCore, "uspGetPaymentConfiguration",
                    new SqlParameter("paymentConfigurationID", SqlDbType.Int) { Value = ID },
                    new SqlParameter("secc", SqlDbType.Int) { Value = 1 }
                   ).ToList();

        }

        public static string GetTDescPaymentConfiguationByOrderPayment(int OrderPaymentID)
        {
            return DataAccess.ExecWithStoreProcedureScalarType<string>(ConnectionStrings.BelcorpCore, "uspGetPaymentConfiguration",
                    new SqlParameter("paymentConfigurationID", SqlDbType.Int) { Value = OrderPaymentID },
                     new SqlParameter("secc", SqlDbType.Int) { Value = 4 }
                   );
        }

        public static bool IsTargetCredit(int CollectionEntityID)
        {
            return DataAccess.ExecWithStoreProcedureScalarType<bool>(ConnectionStrings.BelcorpCore, "uspIsTargetCredit",
                    new SqlParameter("CollectionEntityID", SqlDbType.Int) { Value = CollectionEntityID }
                   );
        }

        public static bool IsTargetCreditByPaymentConfiguration(int PaymentConfigurationID)
        {
            return DataAccess.ExecWithStoreProcedureScalarType<bool>(ConnectionStrings.BelcorpCore, "uspIsTargetCreditByPaymentConfiguration",
                    new SqlParameter("PaymentConfigurationID", SqlDbType.Int) { Value = PaymentConfigurationID }
                   );
        }
        public static int GetNumberCuotasByPaymentConfigurationID(int PaymentConfigurationID)
        {
            return DataAccess.ExecWithStoreProcedureScalarType<int>(ConnectionStrings.BelcorpCore, "uspGetNumberCuotasByPaymentConfigurationID",
                    new SqlParameter("PaymentConfigurationID", SqlDbType.Int) { Value = PaymentConfigurationID }
                   );
        }

        public static List<PaymentConfigurationPerOrderTypesSearchData> EditPaymentConfigurationPerOrderTypes(int ID)
        {
            return DataAccess.ExecWithStoreProcedureListParam<PaymentConfigurationPerOrderTypesSearchData>(ConnectionStrings.BelcorpCore, "uspGetPaymentConfiguration",
                    new SqlParameter("paymentConfigurationID", SqlDbType.Int) { Value = ID },
                    new SqlParameter("secc", SqlDbType.Int) { Value = 2 }
                   ).ToList();

        }

        public static List<ZonesData> EditPaymentConfigurationGeoScope(int ID)
        {
            return DataAccess.ExecWithStoreProcedureListParam<ZonesData>(ConnectionStrings.BelcorpCore, "uspGetPaymentConfiguration",
                    new SqlParameter("paymentConfigurationID", SqlDbType.Int) { Value = ID },
                    new SqlParameter("secc", SqlDbType.Int) { Value = 3 }
                   ).ToList();

        }

        public static List<CollectionEntitySearchData> BrowseCollectionEntities()
        {
            List<CollectionEntitySearchData> result = new List<CollectionEntitySearchData>();
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("spGetCollectionEntities", null, "Core");

                if (reader.HasRows)
                {
                    result = new List<CollectionEntitySearchData>();
                    while (reader.Read())
                    {
                        result.Add(new CollectionEntitySearchData()
                        {
                            CollectionEntityID = Convert.ToInt32(reader["CollectionEntityID"]),
                            CollectionEntityName = Convert.ToString(reader["CollectionEntityName"])

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


        public static List<ScopeLevelSearchData> SearchPaymentsZones(int scopeLevelID)
        {
            List<ScopeLevelSearchData> result = new List<ScopeLevelSearchData>();
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@scopeLevelID", scopeLevelID } };
                SqlDataReader reader = DataAccess.GetDataReader("spGetPaymentZones", parameters, "Core");

                if (reader.HasRows)
                {
                    //result = new List<RoutesData>();
                    while (reader.Read())
                    {
                        result.Add(new ScopeLevelSearchData()
                        {
                            scopeLevelID = Convert.ToInt32(reader["ScopeLevelID"]),
                            Name = Convert.ToString(reader["Name"])
                            
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

        public static int InsertPaymentsZones(PaymentsZonesData Zone, int PaymentConfigurationID)
        {
            try
            {                

                //Insert Zone
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@PaymentConfigurationID", PaymentConfigurationID },
                                                                                            { "@Value", Zone.Value },
                                                                                            { "@Except", Zone.Except },
                                                                                            { "@Name", Zone.Name }};
                //

                SqlCommand cmd = DataAccess.GetCommand("spInsertPaymentsZone", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }


        public static void DeletePaymentConfiguration(int ID)
        {
            try
            {

                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@paymentConfigurationID", ID }};
                //

                SqlCommand cmd = DataAccess.GetCommand("uspDeletePaymentConfiguration", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static int InsertPaymentsConfiguration(int CollectionEntityID, List<int> RestrictOrdList, List<int> RestrictAccList,
            string toleranceValue, string tolerancePercentage, string daysForPayment, int orderStatus, int rulesID,string Description,int? NumberCuotas,
            int? daysVal, string paymentCredit)
        {
            try
            {
                DataTable dtRestrictAccList = new DataTable();
                dtRestrictAccList.Columns.Add("ItemIndex");
                dtRestrictAccList.Columns.Add("ItemID");

                for (int i = 0; i < RestrictAccList.Count(); i++)
                {
                    dtRestrictAccList.Rows.Add(i + 1, RestrictAccList[i]);
                }

                DataTable dtRestrictOrdList = new DataTable();
                dtRestrictOrdList.Columns.Add("ItemIndex");
                dtRestrictOrdList.Columns.Add("ItemID");

                for (int i = 0; i < RestrictOrdList.Count(); i++)
                {
                    dtRestrictOrdList.Rows.Add(i + 1, RestrictOrdList[i]);
                }

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[spInsertPaymentsConfiguration02]";
                    cmd.Parameters.AddWithValue("@PaymentConfigurationID", -1);
                    cmd.Parameters.AddWithValue("@CollectionEntityID", CollectionEntityID);
                    cmd.Parameters.AddWithValue("@FineAndInterestRulesID", rulesID);
                    cmd.Parameters.AddWithValue("@orderStatusID", orderStatus);
                    cmd.Parameters.AddWithValue("@daysForPayment", daysForPayment);
                    cmd.Parameters.AddWithValue("@tolerancePercentage", tolerancePercentage);
                    cmd.Parameters.AddWithValue("@toleranceValue", toleranceValue);
                    cmd.Parameters.AddWithValue("@AccountTypeRestrictionList", dtRestrictAccList).SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@OrderTypeRestrictionList", dtRestrictOrdList).SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@Description", Description);
                    cmd.Parameters.AddWithValue("@DaysVal", daysVal);
                    cmd.Parameters.AddWithValue("@PaymentCredit", paymentCredit);
                    cmd.Parameters["@PaymentConfigurationID"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    return Convert.ToInt32(cmd.Parameters["@PaymentConfigurationID"].Value); ;
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        

        public static void UpdatePaymentsConfiguration(int CollectionEntityID, List<int> RestrictOrdList, List<int> RestrictAccList,
           string toleranceValue, string tolerancePercentage, string daysForPayment, int orderStatus, int rulesID, int paymentConfigurationID, string Description, int? NumberCuotas,
            int? daysVal, string paymentCredit)
        {
            try
            {
                DataTable dtRestrictAccList = new DataTable();
                dtRestrictAccList.Columns.Add("ItemIndex");
                dtRestrictAccList.Columns.Add("ItemID");

                for (int i = 0; i < RestrictAccList.Count(); i++)
                {
                    dtRestrictAccList.Rows.Add(i + 1, RestrictAccList[i]);
                }

                DataTable dtRestrictOrdList = new DataTable();
                dtRestrictOrdList.Columns.Add("ItemIndex");
                dtRestrictOrdList.Columns.Add("ItemID");

                for (int i = 0; i < RestrictOrdList.Count(); i++)
                {
                    dtRestrictOrdList.Rows.Add(i + 1, RestrictOrdList[i]);
                }

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[spUpdatePaymentsConfiguration]";
                    cmd.Parameters.AddWithValue("@PaymentConfigurationID", paymentConfigurationID);
                    cmd.Parameters.AddWithValue("@CollectionEntityID", CollectionEntityID);
                    cmd.Parameters.AddWithValue("@FineAndInterestRulesID", rulesID);
                    cmd.Parameters.AddWithValue("@orderStatusID", orderStatus);
                    cmd.Parameters.AddWithValue("@daysForPayment", daysForPayment);
                    cmd.Parameters.AddWithValue("@tolerancePercentage", tolerancePercentage);
                    cmd.Parameters.AddWithValue("@toleranceValue", toleranceValue);
                    cmd.Parameters.AddWithValue("@AccountTypeRestrictionList", dtRestrictAccList).SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@OrderTypeRestrictionList", dtRestrictOrdList).SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@Description", Description);
                    cmd.Parameters.AddWithValue("@NumberCuotas", NumberCuotas);
                    cmd.Parameters.AddWithValue("@DaysVal", daysVal);
                    cmd.Parameters.AddWithValue("@PaymentCredit", paymentCredit);
                    cmd.ExecuteNonQuery();

                    
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        /// <summary>
        /// Developed By KLC - CSTI
        /// Get States From TaxCache
        /// </summary>
        /// <returns></returns>
        public static List<StateProvincesData> getStates()
        {
            List<StateProvincesData> result = new List<StateProvincesData>();
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("getStateTaxCache", null, "Core");

                if (reader.HasRows)
                {
                    //result = new List<RoutesData>();
                    while (reader.Read())
                    {
                        result.Add(new StateProvincesData()
                        {
                            Name = Convert.ToString(reader["State"])
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

    }

}
