using System.Linq;
using NetSteps.Common.DataFaker;
using NetSteps.Common.Extensions;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities
{
    using System;

    using NetSteps.Data.Entities.Generated;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Data;
    using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
    using NetSteps.Data.Entities.Dto;
    using NetSteps.Data.Entities.Business.Logic;
    using NetSteps.Data.Entities.Business;
    using NetSteps.Data.Common.Context;
    using NetSteps.Data.Entities.Context;
    using NetSteps.Data.Entities.Exceptions;

    /// <summary>
    /// Author: John Egbert
    /// Description: Account Extensions
    /// Created: 09-22-2010
    /// </summary>
    public static class AccountExtensions
    {
        public static Name ToDataFakerNameObject(this Account account)
        {
            var name = new Name()
            {
                FirstName = account.FirstName,
                MiddleName = account.MiddleName,
                LastName = account.LastName,
                IsMale = account.GenderID == ConstantsGenerated.Gender.Male.ToInt()
            };

            return name;
        }

        public static void RemoveAllOrdersFromObjectGraph(this Account account, bool onlyRemoveUnsavedItems = true)
        {
            if (account != null)
            {
                account.StopEntityTracking();
                if (account.Orders != null)
                    account.Orders.Clear();

                if (account.AutoshipOrders != null)
                    account.AutoshipOrders.Clear();
                account.StartEntityTracking();
            }
        }

        public static bool HasPWS(this Account account)
        {
            string url = String.Empty;
            return HasPWS(account, ref url);
        }

        //INI - GR_Encore-07
        public static IActivityInfo GetActivityInfoForCurrentUser(int accountID)
        {
            int periodID = DataAccess.ExecWithFunctionScalar("Commissions", "dbo.sfnGetPeriodForDateByPlan ",
                new SqlParameter("inDate", SqlDbType.DateTime2) { Value = DateTime.UtcNow },
                new SqlParameter("PlanID", SqlDbType.Int) { Value = DBNull.Value });
            var activity = ActivityBusinessLogic.Instance.GetByFilters(accountID, periodID);
            if (activity != null)
            {
                return new ActivityInfo
                {
                    AccountConsistencyStatusID = activity.AccountConsistencyStatusID,
                    AccountID = activity.AccountID,
                    ActivityStatusID = activity.ActivityStatusID,
                    HasContinuity = activity.HasContinuity,
                    PeriodID = activity.PeriodID
                };
            }
            else
            {
                return null;

                //throw EntityExceptionHelper.GetAndLogNetStepsException("Activity Info is not initialized in a current period", Constants.NetStepsExceptionType.NetStepsBusinessException);
            }

        }
        //FIN - GR_Encore-07

        public static bool HasPWS(this Account account, ref string PWSUrl)
        {
            string url = String.Empty;

            var sites = Site.LoadByAccountID(account.AccountID);
            if (sites.Count > 0)
            {
                var defaultSite = sites.FirstOrDefault(s => s.SiteStatusID == (int)ConstantsGenerated.SiteStatus.Active);
                if (defaultSite != null)
                {
                    //Get the localhost url if we are on dev boxes - DES
                    var defaultUrl = defaultSite.SiteUrls.FirstOrDefault(su => su.IsPrimaryUrl);
                    if (defaultUrl == null && defaultSite.SiteUrls.Count > 0)
                    {
                        defaultUrl = defaultSite.SiteUrls.FirstOrDefault();
                    }
                    if (defaultUrl != null)
                    {
                        PWSUrl = defaultUrl.Url;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the best address for an account
        /// </summary>
        /// <param name="account">
        /// The account.
        /// </param>
        /// <returns>
        /// returns an address object
        /// </returns>
        public static Address GetBestAccountAddress(this Account account)
        {
            var addresses = account.Addresses.ToList();
            if (!addresses.Any())
            {
                return null;
            }

            if (addresses.Any(ad => ad.AddressTypeID == (short)ConstantsGenerated.AddressType.Main))
            {
                return addresses.FirstOrDefault(ad => ad.AddressTypeID == (int)ConstantsGenerated.AddressType.Main);
            }

            if (addresses.Any(ad => ad.AddressTypeID == (int)ConstantsGenerated.AddressType.Shipping))
            {
                if (addresses.Any(ad => ad.AddressTypeID == (int)ConstantsGenerated.AddressType.Shipping && ad.IsDefault))
                {
                    return addresses.FirstOrDefault(ad => ad.AddressTypeID == (int)ConstantsGenerated.AddressType.Shipping && ad.IsDefault);
                }
                return addresses.FirstOrDefault(ad => ad.AddressTypeID == (int)ConstantsGenerated.AddressType.Shipping);
            }
            return null;
        }

        public static NetSteps.Commissions.Common.Models.IAccount ConvertToCommissionsAccount(this Account a)
        {
            var c = Create.New<NetSteps.Commissions.Common.Models.IAccount>();
            c.AccountId = a.AccountID;
            c.AccountKindId = a.AccountTypeID;
            c.AccountNumber = a.AccountNumber;
            c.AccountStatusId = a.AccountStatusID;
            c.BirthdayUtc = a.BirthdayUTC;
            c.EmailAddress = a.EmailAddress;
            c.EnrollerId = a.EnrollerID;
            c.EnrollmentDateUtc = a.EnrollmentDateUTC;
            c.EntityName = a.EntityName;
            c.FirstName = a.FirstName;
            c.IsEntity = a.IsEntity;
            c.LastName = a.LastName;
            c.MiddleName = a.MiddleName;
            c.SponsorId = a.SponsorID;
            c.TerminatedDateUtc = a.TerminatedDateUTC;

            var address = a.Addresses.FirstOrDefault(x => x.AddressTypeID == 1);
            c.CountryId = (address != null) ? address.CountryID : 0;

            return c;
        }

        public class AccountUser
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }

        public static Dictionary<string, string> ListAccountUser(string query)
        {
            List<AccountUser> InventoryMovementTypesResult = DataAccess.ExecWithStoreProcedureListParam<AccountUser>("Core", "uspListAccount",
                new SqlParameter("Account", SqlDbType.VarChar) { Value = query }).ToList();

            Dictionary<string, string> wareAccountUserResultDic = new Dictionary<string, string>();
            wareAccountUserResultDic.Add("Select a Type", "0");
            foreach (var item in InventoryMovementTypesResult)
            {
                wareAccountUserResultDic.Add(item.Name, Convert.ToString(item.ID));
            }
            return wareAccountUserResultDic;
        }

        public static decimal ListCreditLedgerLisXAccount(int AccountID)
        {
            decimal periodResult = DataAccess.ExecWithStoreProcedureSaveIdentity("Commissions", "uspCreditLedgerLisXAccount",
                new SqlParameter("AccountID", SqlDbType.Int) { Value = AccountID });
            return periodResult;
        }


        /// <summary>
        /// Create by FHP
        /// </summary>
        /// <returns>List Id, Name</returns>
        public static Dictionary<string, string> GetAccountTypes()
        {
            List<UtilSearchData.Select> AccountTypesResultDicResult = DataAccess.ExecWithStoreProcedureLists<UtilSearchData.Select>("Core", "uspGetAccountTypes").ToList();
            Dictionary<string, string> AccountTypesResultDic = new Dictionary<string, string>();
            foreach (var item in AccountTypesResultDicResult)
            {
                AccountTypesResultDic.Add(Convert.ToString(item.Id), item.Name);
            }
            return AccountTypesResultDic;
        }

        public static string CanEditAccountType(int UserID)
        {
            string result = DataAccess.ExecWithStoreProcedureScalarType<string>("Core", "upsCanEditAccountType",
                new SqlParameter("UserID", SqlDbType.Int) { Value = UserID }
             );
            return result;
        }

        /// <summary>
        /// Create by KTC
        /// </summary>
        /// <returns>Int AccountStatusID</returns>
        public static int UpdateAccountStatusByReEntryRules(int AccountID)
        {
            int statusID = DataAccess.ExecWithStoreProcedureSaveIdentity("Core", "upsReEntryRules",
                new SqlParameter("AccountID", SqlDbType.Int) { Value = AccountID }

             );
            return statusID;
        }

        /// <summary>
        /// Create by KTC
        /// </summary>
        /// <returns>nothing</returns>
        public static void UpdateAccountsCommission(int AccountID)
        {
            int statusID = DataAccess.ExecWithStoreProcedureSave("Core", "SPCallAccountsComm ",
                new SqlParameter("AccountID", SqlDbType.Int) { Value = AccountID }

             );
        }

        public static void AccountPropertyTypesIns(bool acceptShareWithNetwork, bool acceptEmailFrom, int accountID)
        {
            DataAccess.ExecWithStoreProcedureSave("Core", "USPAccountPropertyTypesIns",
                new SqlParameter("AcceptShareWithNetwork", SqlDbType.Bit) { Value = acceptShareWithNetwork },
                new SqlParameter("AcceptEmailFrom", SqlDbType.Bit) { Value = acceptEmailFrom },
                new SqlParameter("AccountID", SqlDbType.Int) { Value = accountID }
             );
        }
        /// <summary>
        /// Create by JQP
        /// </summary>
        /// <returns>nothing</returns>
        public static void UnBlockMailAccount(int AccountID, string email)
        {
            int statusID = DataAccess.ExecWithStoreProcedureSave("Core", "usp_UnBlockMailAccount ",
                new SqlParameter("AccountID", SqlDbType.Int) { Value = AccountID },
                new SqlParameter("email", SqlDbType.VarChar) { Value = email }
             );
        }

        public static Dictionary<string, string> ValidarExistenciaCPF(string CPF, int AccountID = 0)
        {


            Dictionary<string, string> dcResultado = new Dictionary<string, string>();
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
            {
                connection.Open();
                var parameters = new SqlParameter[] {
                                    new SqlParameter("@CPF", CPF ),
                                    new SqlParameter("@AccountID", AccountID )
                };
                SqlDataReader dataReader = DataAccess.queryDatabase("[dbo].[SpValidarExistenciaCPF]", connection, parameters);
                while (dataReader.Read())
                {
                    for (int indice = 0; indice < dataReader.FieldCount; indice++)
                    {
                        dcResultado[dataReader.GetName(indice)] = dataReader[dataReader.GetName(indice)].ToString();
                    }
                }
                return dcResultado;
            }

        }
        public static Dictionary<string, string> ValidarExistenciaPIS(string PIS, int AccountID = 0)
        {

            Dictionary<string, string> dcResultado = new Dictionary<string, string>();
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
            {
                connection.Open();
                var parameters = new SqlParameter[] {
                                    new SqlParameter("@PIS", PIS ),
                                     new SqlParameter("@AccountID", AccountID )
                };
                SqlDataReader dataReader = DataAccess.queryDatabase("[dbo].[SpValidarExistenciaPIS]", connection, parameters);
                while (dataReader.Read())
                {
                    for (int indice = 0; indice < dataReader.FieldCount; indice++)
                    {
                        dcResultado[dataReader.GetName(indice)] = dataReader[dataReader.GetName(indice)].ToString();
                    }
                }
                return dcResultado;
            }

        }
        public static Dictionary<string, string> ValidarExistenciaRG(string RG, int AccountID = 0)
        {
            Dictionary<string, string> dcResultado = new Dictionary<string, string>();
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
            {
                connection.Open();
                var parameters = new SqlParameter[] {
                                    new SqlParameter("@RG", RG ),
                                    new SqlParameter("@AccountID", AccountID )
                };
                SqlDataReader dataReader = DataAccess.queryDatabase("[dbo].[SpValidarExistenciaRG]", connection, parameters);
                while (dataReader.Read())
                {
                    for (int indice = 0; indice < dataReader.FieldCount; indice++)
                    {
                        dcResultado[dataReader.GetName(indice)] = dataReader[dataReader.GetName(indice)].ToString();
                    }
                }
                return dcResultado;
            }

        }

        public static void UpdateDisbursementProfileBank(int DisbursementProfileID, int BankID)
        {
            int statusID = DataAccess.ExecWithStoreProcedureSave("Commissions", "UpdateDisbursementProfileBank ",
                new SqlParameter("DisbursementProfileID", SqlDbType.Int) { Value = DisbursementProfileID },
                new SqlParameter("BankID", SqlDbType.Int) { Value = BankID }
             );
        }

        public static int GetValidSSN(string SNN)
        {
            return DataAccess.ExecWithStoreProcedureScalarType<int>(ConnectionStrings.BelcorpCore, "spGetValidSSN",
                new SqlParameter("SSN", SqlDbType.VarChar) { Value = SNN });
        }

        public static int GetBanksID(int DisbursementProfileID)
        {
            return DataAccess.ExecWithStoreProcedureScalarType<int>(ConnectionStrings.BelcorpCommission, "GetBanksIDbyDisbursementProfile",
                new SqlParameter("DisbursementProfileID", SqlDbType.Int) { Value = DisbursementProfileID });
        }

        public static List<MinimumSearchData> GetDisbursementProfileIDs(int AccountID)
        {
            List<MinimumSearchData> list = DataAccess.ExecWithStoreProcedureListParam<MinimumSearchData>("Commissions", "GetDisbursementProfileByAccount",
            new SqlParameter("AccountID", SqlDbType.Int) { Value = AccountID }).ToList();
            return list;
        }

        public static List<DisbursementProfilesSearchData> GetFullDisbursementProfileByAccount(int AccountID)
        {
            List<DisbursementProfilesSearchData> list = DataAccess.ExecWithStoreProcedureListParam<DisbursementProfilesSearchData>("Commissions", "GetFullDisbursementProfileByAccount",
            new SqlParameter("AccountID", SqlDbType.Int) { Value = AccountID }).ToList();
            return list;
        }

        public static List<BankSearchData> GetBanksComissions()
        {
            List<BankSearchData> list = DataAccess.ExecWithStoreProcedureLists<BankSearchData>("Commissions", "uspGetBanks"
            ).ToList();
            return list;
        }


        public static void UpdateDateAccount(int AccountID, string Date, string campo)
        {
            DataAccess.ExecWithStoreProcedureSave("Core", "upsUpdateDateAccount",
                   new SqlParameter("AccountID", SqlDbType.Int) { Value = AccountID },
                   new SqlParameter("Date", SqlDbType.Int) { Value = Date },
                   new SqlParameter("Campo", SqlDbType.Int) { Value = campo }
               );
        }

        //CSTI(CS):05/03/2016
        private static AccountEnrollDisbursementProfile AccountEnrollDisbursementProfile;

        public static AccountEnrollDisbursementProfile GetAccountEnrollDisbursementProfile(this Account account)
        {
            return AccountEnrollDisbursementProfile;
        }

        public static void SetAccountEnrollDisbursementProfile(this Account account, AccountEnrollDisbursementProfile pAccountEnrollDisbursementProfile)
        {
            AccountEnrollDisbursementProfile = pAccountEnrollDisbursementProfile;
        }
    }
}
