using System;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;
using System.Collections.Generic;
using System.Data.SqlClient;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using System.Linq;
using System.Data;
using System.Configuration;

namespace NetSteps.Data.Entities.Repositories
{
    public class ProductQuotasRepository
    {
        /// <summary>
        /// Get All Restrictions
        /// </summary>
        /// <returns></returns>
        public static List<ProductQuotaSearchData> Search(int Active, int LanguageID, string SKUorName)
        {
            return ProductQuotasDataAccess.Search(Active, LanguageID, SKUorName);
        }

        public static List<RestrictionPerAccountType> SearchRestrictionPerAccountType()
        {
            return ProductQuotasDataAccess.SearchRestrictionPerAccountType();
        }

        public static List<RestrictionPerTitle> SearchRestrictionPerTitle()
        {
            return ProductQuotasDataAccess.SearchRestrictionPerTitle();
        }

        public static List<RestrictionsControl> SearchRestrictionsControl()
        {
            return ProductQuotasDataAccess.SearchRestrictionsControl();
        }

        public static PaginatedList<ProductQuotaSearchData> Search(FilterPaginatedListParameters<ProductQuotaSearchData> searchParameters, int Active, int LanguageID, string SKUorName)
        {
            // Apply pagination
            IQueryable<ProductQuotaSearchData> matchingItems = ProductQuotasDataAccess.Search(Active, LanguageID, SKUorName).AsQueryable<ProductQuotaSearchData>();

            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParameters);

            return matchingItems.ToPaginatedList<ProductQuotaSearchData>(searchParameters, resultTotalCount);
        }

        public static List<QuotaTypes> SearchQuotaTypes()
        {
            return ProductQuotasDataAccess.SearchQuotaTypes();
        }

        public static void Delete(List<int> items)
        {
            using (SqlConnection connection = new SqlConnection(DataAccess.GetConnectionString("Core")))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction("DeleteRestrictionsTransaction");

                try
                {
                    foreach (var item in items) ProductQuotasDataAccess.Delete(item, connection, transaction);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
                }
            }
        }

        public static void ChangeQuotaStatus(List<int> items, bool Active)
        {
            using (SqlConnection connection = new SqlConnection(DataAccess.GetConnectionString("Core")))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction("DeleteRestrictionsTransaction");

                try
                {
                    foreach (var item in items) ProductQuotasDataAccess.ChangeQuotaStatus(item, Active, connection, transaction);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
                }
            }
        }

        public static bool UpdateRestrictionState(int restrictionID, bool state, bool hasAccount, string[] accountIDs)
        {
            bool rpta = false;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction("SaveAccountGroupTran");

                try
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[uspUpdateRestriction]", connection, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@pRestrictionID", SqlDbType.Int, -1).Value = restrictionID;
                        cmd.Parameters.Add("@pIsActive", SqlDbType.Bit, 1).Value = state;
                        cmd.ExecuteNonQuery();
                        //rpta = cmd.ExecuteNonQuery() > 0;
                    }

                    //*********************************************************
                    if (hasAccount)
                    {
                        using (SqlCommand cmd1 = new SqlCommand("[dbo].[upsDeleteRestrictionPerAccountGroup]", connection, transaction))
                        {
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.Parameters.Add("@RestrictionID", SqlDbType.Int, -1).Value = restrictionID;
                            cmd1.ExecuteNonQuery();
                            //rpta = cmd.ExecuteNonQuery() > 0;
                        }


                    }//elimina el grupo de cuenta para registrar las nuevas
                    if (!accountIDs.IsNull())
                    {
                        foreach (var accountID in accountIDs) ProductQuotasDataAccess.SaveRestrictionPerAccountGroup(Convert.ToInt32(accountID), restrictionID, connection, transaction);
                    }

                    rpta = true;
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);

                }
            }

            return rpta;
        }

        public static int Save(ProductQuotaSearchData restriction,
                               string[] paidAsTitleIDs,
                               string[] recognizedTitleIDs,
                               string[] accountTypeIDs,
                               string[] accountIDs)
        {
            int restrictionID = 0;
            using (SqlConnection connection = new SqlConnection(DataAccess.GetConnectionString("Core")))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction("SaveRestAccountTypesTran");

                try
                {
                    // Save restriction
                    restrictionID = ProductQuotasDataAccess.SaveRestriction(restriction, connection, transaction);
                    ProductQuotasDataAccess.SaveRestrictionsPerProduct(restrictionID, restriction.ProductID, restriction.Quantity, connection, transaction);

                    // Save restriction per titles
                    if (!paidAsTitleIDs.IsNull())
                    {
                        foreach (var titleID in paidAsTitleIDs) ProductQuotasDataAccess.SaveRestrictionPerTitles(restrictionID, titleID, 1, connection, transaction);
                    }

                    if (!recognizedTitleIDs.IsNull())
                    {
                        foreach (var titleID in recognizedTitleIDs) ProductQuotasDataAccess.SaveRestrictionPerTitles(restrictionID, titleID, 2, connection, transaction);
                    }

                    // Save restriction per account types
                    if (!accountTypeIDs.IsNull())
                    {
                        foreach (var accountTypeID in accountTypeIDs) ProductQuotasDataAccess.SaveRestrictionPerAccountTypes(Convert.ToInt32(accountTypeID), restrictionID, connection, transaction);
                    }

                    if (!accountIDs.IsNull())
                    {
                        foreach (var accountID in accountIDs) ProductQuotasDataAccess.SaveRestrictionPerAccountGroup(Convert.ToInt32(accountID), restrictionID, connection, transaction);
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
                }
            }

            return restrictionID;
        }

        public static int Save(ProductQuotaSearchData restriction)
        {
            int restrictionID = 0;
            using (SqlConnection connection = new SqlConnection(DataAccess.GetConnectionString("Core")))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction("SaveRestPersonsTran");

                try
                {
                    // Save restriction
                    restrictionID = ProductQuotasDataAccess.SaveRestriction(restriction, connection, transaction);
                    ProductQuotasDataAccess.SaveRestrictionsPerProduct(restrictionID, restriction.ProductID, restriction.Quantity, connection, transaction);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
                }
            }

            return restrictionID;
        }

        public static bool ProductIsRestricted(int productId, int quantity, int accountId, int accountTypeId)
        {
            bool isRestricted = true;
            SqlDataReader reader = DataAccess.GetDataReader("IsThereRestriction", new Dictionary<string, object>() { { "@ProductID", productId },
            { "@AccountID", accountId },{ "@QuantityOrdered", quantity }}, "Core");

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    isRestricted = Convert.ToBoolean(reader["Result"]);
                    break;
                }
            }
            return isRestricted;
        }

        public static List<ListProducts> getRestrictionxAccountProduct(int RestrictionID, int PeriodID, int AccountID)
        {
            List<ListProducts> resultado = null;

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("PeriodID", PeriodID);
            parameters.Add("AccountID", AccountID);
            parameters.Add("RestrictionID", RestrictionID);

            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("getRestrictionxAccountProduct", parameters, "core");

                if (reader.HasRows)
                {
                    resultado = new List<ListProducts>();
                    while (reader.Read())
                    {
                        resultado.Add(new ListProducts()
                        {
                            ProductID = Convert.ToInt32(reader["ProductID"]),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            QuantityRes = Convert.ToInt32(reader["QuantityRes"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return resultado;
        }

        public static List<ProductsRestriction> getProductRestriction(int vaccountID, int vperiodID)
        {
            List<ProductsRestriction> result = null;

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("accountID", vaccountID);
            parameters.Add("periodID", vperiodID);

            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("ListProductxRestriction", parameters, "core");

                if (reader.HasRows)
                {
                    result = new List<ProductsRestriction>();
                    while (reader.Read())
                    {
                        result.Add(new ProductsRestriction()
                        {
                            RestrictionID = Convert.ToInt32(reader["RestrictionID"]),
                            AccountTypeID = Convert.ToInt32(reader["AccountTypeID"]),
                            TitleID = Convert.ToInt32(reader["TitleID"]),
                            TitleTypeID = Convert.ToInt32(reader["TitleTypeID"]),
                            TituloPago = Convert.ToInt32(reader["TituloPago"]),
                            TituloCarrera = Convert.ToInt32(reader["TituloCarrera"])
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

        public static ProductQuotaEntity LoadFullQuotaByID(int restrictionID, int LanguageID)
        {
            var quota = new ProductQuotaEntity()
            {
                RestrictionID = restrictionID,
                PaidAsTitlesIDs = new List<int>(),
                RecognizedTitlesIDs = new List<int>(),
                AccountTypeIDs = new List<int>()
            };

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[uspLoadRestrictionById]", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@pRestrictionID", SqlDbType.Int, -1).Value = restrictionID;
                        cmd.Parameters.Add("@LanguageID", SqlDbType.Int).Value = LanguageID;

                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            #region Load Restriction / Product
                            if (reader.Read())
                            {
                                quota.Name = reader["Name"].ToString();
                                //quota.StartDate = Convert.ToDateTime(reader["StartDate"]);
                                //quota.EndDate = reader["EndDate"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["EndDate"]);
                                quota.RestrictionType = Convert.ToInt32(reader["RestrictionType"]);
                                quota.Active = Convert.ToBoolean(reader["Active"]);

                                quota.ProductID = Convert.ToInt32(reader["ProductID"]);
                                quota.ProductSKU = reader["SKU"].ToString();
                                quota.ProductName = reader["ProductName"].ToString();
                                quota.Quantity = Convert.ToInt32(reader["Quantity"]);
                                quota.StartPeriodID = Convert.ToInt32(reader["StartPeriodID"]);
                                quota.EndPeriodID = Convert.ToInt32(reader["EndPeriodID"]);
                            }
                            #endregion

                            #region LoadLists
                            reader.NextResult();

                            while (reader.Read())
                            {
                                quota.PaidAsTitlesIDs.Add(Convert.ToInt32(reader["TitleID"]));
                            }

                            reader.NextResult();

                            while (reader.Read())
                            {
                                quota.RecognizedTitlesIDs.Add(Convert.ToInt32(reader["TitleID"]));
                            }

                            reader.NextResult();

                            while (reader.Read())
                            {
                                quota.AccountTypeIDs.Add(Convert.ToInt32(reader["AccountTypeID"]));
                            }

                            reader.NextResult();

                            while (reader.Read())
                            {
                                quota.AccountIDs.Add(string.Format("{0};{1}", Convert.ToInt32(reader["AccountID"]), Convert.ToString(reader["Name"])));
                            }
                            #endregion
                        }
                    }
                }
            }
            catch (Exception)
            {
                quota = new ProductQuotaEntity();
            }

            return quota;
        }

    }

    #region Data Access

    class ProductQuotasDataAccess
    {

        public static List<ProductQuotaSearchData> Search(int Active, int LanguageID, string SKUorName)
        {
            List<ProductQuotaSearchData> result = null;
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Active", Active }, { "@LanguageID", LanguageID }, { "@SKUorName", SKUorName } };
                SqlDataReader reader = DataAccess.GetDataReader("upsGetProductQuotas", parameters, "Core");
                result = new List<ProductQuotaSearchData>();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductQuotaSearchData productQuota = new ProductQuotaSearchData();
                        productQuota.RestrictionID = Convert.ToInt32(reader["RestrictionID"]);
                        productQuota.Name = Convert.ToString(reader["Name"]);
                        productQuota.ProductID = Convert.ToInt32(reader["ProductID"]);
                        productQuota.ProductName = Convert.ToString(reader["ProductName"]);
                        productQuota.StartPeriodID = Convert.ToInt32(reader["StartPeriodID"]);
                        productQuota.EndPeriodID = Convert.ToInt32(reader["EndPeriodID"]);
                        productQuota.SKU = Convert.ToString(reader["SKU"]);
                        productQuota.Active = Convert.ToBoolean(reader["Active"]);
                        result.Add(productQuota);
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }

        public static List<RestrictionPerAccountType> SearchRestrictionPerAccountType()
        {
            List<RestrictionPerAccountType> result = null;
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("upsGetRestrictionPerAccountType", null, "Core");

                if (reader.HasRows)
                {
                    result = new List<RestrictionPerAccountType>();
                    while (reader.Read())
                    {
                        result.Add(new RestrictionPerAccountType()
                        {
                            RestrictionAccountID = Convert.ToInt32(reader["RestrictionAccountID"]),
                            RestrictionID = Convert.ToInt32(reader["RestrictionID"]),
                            AccountTypeID = Convert.ToInt32(reader["AccountTypeID"])
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

        public static List<RestrictionPerTitle> SearchRestrictionPerTitle()
        {
            List<RestrictionPerTitle> result = null;
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("upsGetRestrictionPerTitle", null, "Core");

                if (reader.HasRows)
                {
                    result = new List<RestrictionPerTitle>();
                    while (reader.Read())
                    {
                        result.Add(new RestrictionPerTitle()
                        {
                            RestrictionTitleID = Convert.ToInt32(reader["RestrictionTitleID"]),
                            RestrictionID = Convert.ToInt32(reader["RestrictionID"]),
                            TitleID = Convert.ToInt32(reader["TitleID"]),
                            TitleTypeID = Convert.ToInt32(reader["TitleTypeID"])
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

        public static List<RestrictionsControl> SearchRestrictionsControl()
        {
            List<RestrictionsControl> result = null;
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("upsGetRestrictionsControl", null, "Core");

                if (reader.HasRows)
                {
                    result = new List<RestrictionsControl>();
                    while (reader.Read())
                    {
                        result.Add(new RestrictionsControl()
                        {
                            RestrictionID = Convert.ToInt32(reader["RestrictionID"]),
                            AccountID = Convert.IsDBNull(reader["AccountID"]) ? 0 : Convert.ToInt32(reader["AccountID"]),
                            Quantity = Convert.ToInt32(reader["Quantity"])
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

        public static List<QuotaTypes> SearchQuotaTypes()
        {
            List<QuotaTypes> result = null;
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("upsGetQuotaTypes", null, "Core");

                if (reader.HasRows)
                {
                    result = new List<QuotaTypes>();
                    while (reader.Read())
                    {
                        result.Add(new QuotaTypes()
                        {
                            QuotaTypeID = Convert.ToInt32(reader["QuotaTypeID"]),
                            Name = Convert.ToString(reader["Name"]),
                            TermName = Convert.ToString(reader["TermName"])

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

        public static void Delete(int restrictionID, SqlConnection connection, SqlTransaction transaction)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@RestrictionID", restrictionID } };

                SqlCommand cmd = DataAccess.GetCommand("upsDelRestriction", parameters, connection, transaction) as SqlCommand;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void ChangeQuotaStatus(int restrictionID, bool Active, SqlConnection connection, SqlTransaction transaction)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@RestrictionID", restrictionID }, { "@Active", Active } };

                SqlCommand cmd = DataAccess.GetCommand("upsChangeQuotaStatus", parameters, connection, transaction) as SqlCommand;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static int SaveRestriction(ProductQuotaSearchData restriction, SqlConnection connection, SqlTransaction transaction)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@Active", restriction.Active },
                                                                                            //{ "@StartDate", restriction.StartDate },
                                                                                            //{ "@EndDate", restriction.EndDate },
                                                                                            { "@Name", restriction.Name },
                                                                                            { "@TermName", restriction.TermName } ,
                                                                                            { "@RestrictionType", restriction.RestricionType },
                                                                                            { "@StartPeriodID", restriction.StartPeriodID  },
                                                                                            { "@EndPeriodID", restriction.EndPeriodID  }};
                // wv: Se incluye el periodo en Restriction 20160427

                SqlCommand cmd = DataAccess.GetCommand("upsInsRestriction", parameters, connection, transaction) as SqlCommand;
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void SaveRestrictionPerTitles(int restrictionID, string titleID, int titleTypeID, SqlConnection connection, SqlTransaction transaction)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@RestrictionID", restrictionID },
                                                                                            { "@TitleID", titleID },
                                                                                            { "@TitleTypeID", titleTypeID }};

                SqlCommand cmd = DataAccess.GetCommand("upsInsRestrictionPerTitle", parameters, connection, transaction) as SqlCommand;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void SaveRestrictionPerAccountTypes(int accountTypeID, int restrictionID, SqlConnection connection, SqlTransaction transaction)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@RestrictionID", restrictionID },
                                                                                            { "@AccountTypeID", accountTypeID }};

                SqlCommand cmd = DataAccess.GetCommand("upsInsRestrictionPerAccountType", parameters, connection, transaction) as SqlCommand;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void SaveRestrictionPerPersons(int? accountID, int restrictionID, int quantity, SqlConnection connection, SqlTransaction transaction)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@RestrictionID", restrictionID },
                                                                                            { "@AccountID", accountID },
                                                                                            { "@Quantity", quantity }};

                SqlCommand cmd = DataAccess.GetCommand("upsInsRestrictionPerPerson", parameters, connection, transaction) as SqlCommand;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void SaveRestrictionPerAccountGroup(int accountID, int restrictionID, SqlConnection connection, SqlTransaction transaction)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@RestrictionID", restrictionID },
                                                                                            { "@AccountID", accountID }};

                SqlCommand cmd = DataAccess.GetCommand("upsInsRestrictionPerAccountGroup", parameters, connection, transaction) as SqlCommand;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void SaveRestrictionsPerProduct(int restrictionID, int productID, int quantity, SqlConnection connection, SqlTransaction transaction)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@RestrictionID", restrictionID },
                                                                                            { "@ProductID", productID },
                                                                                            { "@Quantity", quantity }};

                SqlCommand cmd = DataAccess.GetCommand("uspInsRestrictionsPerProduct", parameters, connection, transaction) as SqlCommand;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static int QuantityProductPerQuota(int productId)
        {
            int quantity = DataAccess.ExecWithStoreProcedureSaveIdentity("Core", "upsRestrictionsMoreProducts",
                          new SqlParameter("ProductID", SqlDbType.Int) { Value = productId }
                          );
            return quantity;
        }

        public static int RestrictionType(int productId)
        {
            int restrictionType = DataAccess.ExecWithStoreProcedureSaveIdentity("Core", "upsListRestrictionsType",
                          new SqlParameter("ProductID", SqlDbType.Int) { Value = productId }
                          );
            return restrictionType;
        }

        public static int CountOrdersPerAccounts(int restrictionID, int accountID)
        {
            int countOrdersPerAccounts = DataAccess.ExecWithStoreProcedureSaveIdentity("Core", "upsCountOrdersPerAccounts",
                          new SqlParameter("RestrictionID", SqlDbType.Int) { Value = restrictionID },
                          new SqlParameter("AccountID", SqlDbType.Int) { Value = accountID }
                          );
            return countOrdersPerAccounts;
        }

    }

    #endregion
}
