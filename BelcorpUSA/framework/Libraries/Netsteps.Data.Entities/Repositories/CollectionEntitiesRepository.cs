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

//

namespace NetSteps.Data.Entities.Repositories
{
    public class CollectionEntitiesRepository
    {

        public static PaginatedList<CollectionEntitySearchData> SearchDetails(CollectionEntitiesSearchParameter searchParameter)
        {
            List<CollectionEntitySearchData> paginatedResult = DataAccess.ExecWithStoreProcedureListParam<CollectionEntitySearchData>("Core", "upsGetMeansOfCollections",

                new SqlParameter("CompanyID", SqlDbType.Int) { Value = (object)searchParameter.CompanyID ?? DBNull.Value },
                new SqlParameter("PaymentTypeID", SqlDbType.Int) { Value = (object)searchParameter.PaymentTypeID ?? DBNull.Value },
                new SqlParameter("CollectionEntityNameOrID", SqlDbType.VarChar) { Value = searchParameter.CollectionEntityID },
                new SqlParameter("status", SqlDbType.VarChar) { Value = searchParameter.status }                
                ).ToList();

            IQueryable<CollectionEntitySearchData> matchingItems = paginatedResult.AsQueryable<CollectionEntitySearchData>();

            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParameter);

            return matchingItems.ToPaginatedList<CollectionEntitySearchData>(searchParameter, resultTotalCount);
        }



        public static List<CompanySearchData> BrowseCompanies()
        {
            List<CompanySearchData> result = new List<CompanySearchData>();
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("uspGetCompanies", null, "Core");

                if (reader.HasRows)
                {
                    result = new List<CompanySearchData>();
                    while (reader.Read())
                    {
                        result.Add(new CompanySearchData()
                        {
                            CompanyID = Convert.ToInt32(reader["CompanyID"]),
                            CompanyName = Convert.ToString(reader["CompanyName"])

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

        public static List<BankSearchData> BrowseBanks()
        {
            List<BankSearchData> result = new List<BankSearchData>();
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("uspGetBanks", null, "Core");

                if (reader.HasRows)
                {
                    result = new List<BankSearchData>();
                    while (reader.Read())
                    {
                        result.Add(new BankSearchData()
                        {
                            BankID = Convert.ToInt32(reader["BankID"]),
                            BankName = Convert.ToString(reader["BankName"]),
                            BankCode = Convert.ToInt32(reader["BankCode"]),
                            TermName = Convert.ToString(reader["TermName"]),
                            SortIndex = Convert.ToInt32(reader["SortIndex"]),
                            Active = Convert.ToBoolean(reader["Active"]),
                            MarketID = Convert.ToInt32(reader["MarketID"])

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

        public static List<CollectionEntitiesData> BrowseCollectionEntities()
        {
            List<CollectionEntitiesData> result = new List<CollectionEntitiesData>();
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("uspGetCollections", null, "Core");

                if (reader.HasRows)
                {
                    result = new List<CollectionEntitiesData>();
                    while (reader.Read())
                    {
                        result.Add(new CollectionEntitiesData()
                        {
                            CollectionEntityID = Convert.ToInt32(reader["CollectionEntityID"]),
                            PaymentTypeID = Convert.ToInt32(reader["PaymentTypeID"]),
                            PaymentGatewayID =  Convert.ToInt16(reader["PaymentGatewayID"]),
                            BankID =  Convert.ToInt32(reader["BankID"]),
                            BankAgencie = Convert.ToString(reader["BankAgencie"]),
                            BankAccountNumber = Convert.ToInt32(reader["BankAccountNumber"]),
                            BankAccountType =  Convert.ToInt32(reader["BankAccountType"]),
                            CompanyID =  Convert.ToInt32(reader["CompanyID"]),
                            CollectionTypePerBankID =  Convert.ToInt32(reader["CollectionTypePerBankID"]),
                            CollectionDocumentTypePerBankID =  Convert.ToInt32(reader["CollectionDocumentTypePerBankID"]),
                            CollectionAgreement = Convert.ToString(reader["CollectionAgreement"]),
                            CollectionEntityName = Convert.ToString(reader["CollectionEntityName"]),
                            Active = Convert.ToInt32(reader["Active"]),
                            FileNameBankCollection = Convert.ToString(reader["FileNameBankCollection"]),
                            InitialPositionName = Convert.ToInt32(reader["InitialPositionName"]),
                            FinalPositionName = Convert.ToInt32(reader["FinalPositionName"]),
                            CodeDetail = Convert.ToInt32(reader["CodeDetail"])
                       
                            

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

        public static List<CollectionTypesPerBankSearchData> BrowseCollectionTypesPerBank()
        {
            List<CollectionTypesPerBankSearchData> result = new List<CollectionTypesPerBankSearchData>();
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("uspGetCollectionTypesPerBank", null, "Core");

                if (reader.HasRows)
                {
                    result = new List<CollectionTypesPerBankSearchData>();
                    while (reader.Read())
                    {
                        result.Add(new CollectionTypesPerBankSearchData()
                        {
                            CollectionTypesPerBankID = Convert.ToInt32(reader["CollectionTypesPerBankID"]),
                            BankID = Convert.ToInt32(reader["BankID"]),
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


        public static List<CollectionDocumentsPerBankSearchData> BrowseCollectionDocumentsPerBank()
        {
            List<CollectionDocumentsPerBankSearchData> result = new List<CollectionDocumentsPerBankSearchData>();
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("uspGetCollectionDocumentsPerBank", null, "Core");

                if (reader.HasRows)
                {
                    result = new List<CollectionDocumentsPerBankSearchData>();
                    while (reader.Read())
                    {
                        result.Add(new CollectionDocumentsPerBankSearchData()
                        {
                            CollectionDocumentsPerBankID = Convert.ToInt32(reader["CollectionDocumentsPerBankID"]),
                            BankID = Convert.ToInt32(reader["BankID"]),
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

        public static int SaveCollectionEntityCreditCard(int collectionId, int paymentTypeID, int collectionEntityID,
                                int location, string collectionEntityName, int chkStatus)
        {
            try
            {

                //Insert Zone
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@collectionId", collectionId },
                                                                                            { "@paymentTypeID", paymentTypeID },
                                                                                            { "@paymentGatewayID", collectionEntityID },
                                                                                            { "@location", location },
                                                                                            { "@collectionEntityName", collectionEntityName },
                                                                                            { "@chkStatus", chkStatus }};
                //

                SqlCommand cmd = new SqlCommand();
                if (collectionId == 0)
                    cmd = DataAccess.GetCommand("spInsertCollectionEntitiesCreditCard", parameters, "Core") as SqlCommand;
                else
                    cmd = DataAccess.GetCommand("spUpdateCollectionEntitiesCreditCard", parameters, "Core") as SqlCommand;

                cmd.Connection.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static int SaveCollectionEntityPaymentTicket(int collectionId, int paymentTypeID,  int collectionEntityID,
	                             string bankAgencie, string  bankAccountNumber, int accountType, int  location,int  collectionType,  int collectionDocument,
                                 string collectionAgreement, string collectionEntityName,int chkStatus,
               string  fileNameBankCollection,int initialPositionName,int finalPositionName,int codeDetail)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@collectionId", collectionId },
                                                                                            { "@paymentTypeID", paymentTypeID },
                                                                                            { "@bankID", collectionEntityID },
                                                                                            { "@bankAgencie", Convert.ToInt32(bankAgencie) },
                                                                                            { "@bankAccountNumber", Convert.ToInt32(bankAccountNumber) },                                                                                            
                                                                                            { "@accountType", accountType },
                                                                                            { "@location", location },
                                                                                            { "@collectionType", collectionType },
                                                                                            { "@collectionDocument", collectionDocument },
                                                                                            { "@collectionAgreement", Convert.ToInt32(collectionAgreement) },
                                                                                            { "@collectionEntityName", collectionEntityName  },
                     
                                                                                            { "@fileNameBankCollection", fileNameBankCollection},
                                                                                            { "@initialPositionName", initialPositionName  },
                                                                                            { "@finalPositionName", finalPositionName },
                                                                                             { "@codeDetail", codeDetail } ,
                                                                                             { "@chkStatus", chkStatus }
                
                };
                //
                SqlCommand cmd = new SqlCommand();
                if (collectionId == 0)
                    cmd = DataAccess.GetCommand("spInsertCollectionEntitiesPaymentTicket", parameters, "Core") as SqlCommand;
                else
                    cmd = DataAccess.GetCommand("spUpdateCollectionEntitiesPaymentTicket", parameters, "Core") as SqlCommand;

                cmd.Connection.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void ClearCollectionZones(int CollectionEntityID)
        {
            try
            {

                //Insert Zone
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@CollectionEntityID", CollectionEntityID }
                                                                                            };
                //

                SqlCommand cmd = DataAccess.GetCommand("spDeleteCollectionZone", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();
                cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        public static int InsertCollectionZones(CollectionZonesData Zone, int CollectionEntityID)
        {
            try
            {

                //Insert Zone
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@CollectionEntityID", CollectionEntityID },
                                                                                            { "@Value", Zone.Value },
                                                                                            { "@Except", Zone.Except },
                                                                                            { "@Name", Zone.Name }};
                //

                SqlCommand cmd = DataAccess.GetCommand("spInsertCollectionZone", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<CollectionZonesData> BrowseCollectionZones(int CollectionEntityID)
        {
            List<CollectionZonesData> result = new List<CollectionZonesData>();
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@CollectionEntityID", CollectionEntityID } };
                SqlDataReader reader = DataAccess.GetDataReader("uspGetCollectionZones", parameters, "Core");

                if (reader.HasRows)
                {
                    result = new List<CollectionZonesData>();
                    while (reader.Read())
                    {
                        result.Add(new CollectionZonesData()
                        {
                            ScopeLevelID = Convert.ToInt32(reader["scopeLevelID"]),
                            Value = Convert.ToString(reader["AreaLevel"]),
                            Name = Convert.ToString(reader["Name"]),
                            Except = Convert.ToBoolean(reader["Except"])
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

        public static int? GetPaymentTicketID()
        {

            int? result = 0;
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("uspGetPaymentTicketType", null, "Core");

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        result = Convert.ToInt32(reader["PaymentTypeID"]);

                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }

        public static List<PaymentTypeSearchData> BrowsePaymentTypes()
        {
            List<PaymentTypeSearchData> result = new List<PaymentTypeSearchData>();
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("uspGetPaymentTypes", null, "Core");

                if (reader.HasRows)
                {
                    result = new List<PaymentTypeSearchData>();
                    while (reader.Read())
                    {
                        result.Add(new PaymentTypeSearchData()
                        {
                            PaymentTypeID = Convert.ToInt32(reader["PaymentTypeID"]),
                            Name = Convert.ToString(reader["Name"]),
                            Active = Convert.ToBoolean(reader["Active"])

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
