using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business;
using System.Data.SqlClient;
using NetSteps.Data.Entities.Exceptions;


namespace NetSteps.Data.Entities
{
    public class SponsorshipDataAcces
    {
        //Obtener Lista de RequirementTypes para opcion Vlaid Documets
        //Developed by Kelvin Lopez C. - CSTI

        public static List<SponsorshipSearchData> Search( int CountryID)
        {
            List<SponsorshipSearchData> result = null;
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("upsGetIDTypes",
                            new Dictionary<string, object>() 
                                    { 
                                        { "@CountryID", CountryID }
                                    }
                                    , "Core");

                if (reader.HasRows)
                {
                    result = new List<SponsorshipSearchData>();
                    while (reader.Read())
                    {
                        SponsorshipSearchData requerimetTypes = new SponsorshipSearchData();
                        requerimetTypes.RestrictionDocumentID = Convert.ToInt32(reader["RestrictionDocumentID"]);
                        requerimetTypes.IDTypeID = Convert.ToInt32(reader["IDTypeID"]);
                        requerimetTypes.Name = Convert.ToString(reader["Name"]);
                        requerimetTypes.TermName = Convert.ToString(reader["TermName"]);                    
                        result.Add(requerimetTypes);
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }

        /*
        public static List<SponsorshipSearchData> Search()
        {
            List<SponsorshipSearchData> result = null;
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("upsGetRequirementTypes", null, "Core");

                if (reader.HasRows)
                {
                    result = new List<SponsorshipSearchData>();
                    while (reader.Read())
                    {
                        SponsorshipSearchData requerimetTypes = new SponsorshipSearchData();
                        requerimetTypes.RequirementTypeID = Convert.ToInt32(reader["RequirementTypeID"]);
                        requerimetTypes.Name = Convert.ToString(reader["Name"]);
                        requerimetTypes.TermName = Convert.ToString(reader["TermName"]);
                        requerimetTypes.LogicalOperator = Convert.ToString(reader["LogicalOperator"]);
                        requerimetTypes.Order = Convert.ToInt32(reader["Order"]);
                        result.Add(requerimetTypes);
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }
        */

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<SponsorDataAccountStatus> spGetRulePerStatus()
        {
            List<SponsorDataAccountStatus> result = null;
            try
            {
                SqlDataReader readerac = DataAccess.GetDataReader("spGetRulePerStatus", null, "Core");

                if (readerac.HasRows)
                {
                    result = new List<SponsorDataAccountStatus>();
                    while (readerac.Read())
                    {
                        SponsorDataAccountStatus AccountStatutes = new SponsorDataAccountStatus();
                        AccountStatutes.AccountStatusID = Convert.ToInt32(readerac["ActivityStatusID"]);
                        result.Add(AccountStatutes);
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }       
        public static List<SponsorshipSearchData> spGetRulesPerDocuments()
        {
            List<SponsorshipSearchData> result = null;
            try
            {
                SqlDataReader readerac = DataAccess.GetDataReader("spGetRulesPerDocuments", null, "Core");

                if (readerac.HasRows)
                {
                    result = new List<SponsorshipSearchData>();
                    while (readerac.Read())
                    {
                        SponsorshipSearchData RulesPerDocuments = new SponsorshipSearchData();
                        RulesPerDocuments.RequirementTypeID = Convert.ToInt32(readerac["RequirementTypeID"]);
                        RulesPerDocuments.LogicalOperator = Convert.ToString(readerac["LogicalOperator"]);
                        RulesPerDocuments.Order = Convert.ToInt32(readerac["Order"]);
                        result.Add(RulesPerDocuments);
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }

        public static List<SponsorDataTitleType> spGetTitlesPaids()
        {
            List<SponsorDataTitleType> result = new List<SponsorDataTitleType>();
            try
            {
                SqlDataReader readerac = DataAccess.GetDataReader("spGetTitlesPaids", null, "Core");

                if (readerac.HasRows)
                {
                    result = new List<SponsorDataTitleType>();
                    while (readerac.Read())
                    {
                        SponsorDataTitleType TitlesPaids = new SponsorDataTitleType();
                        TitlesPaids.TitleID = Convert.ToInt32(readerac["TitleID"]);
                        result.Add(TitlesPaids);
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }

        public static List<SponsorDataTitleType> spGetTitlesRecognizeds()
        {
            List<SponsorDataTitleType> result = new List<SponsorDataTitleType>();
            try
            {
                SqlDataReader readerac = DataAccess.GetDataReader("spGetTitlesRecognizeds", null, "Core");

                if (readerac.HasRows)
                {
                    result = new List<SponsorDataTitleType>();
                    while (readerac.Read())
                    {
                        SponsorDataTitleType TitlesRecognizeds = new SponsorDataTitleType();
                        TitlesRecognizeds.TitleID = Convert.ToInt32(readerac["TitleID"]);
                        result.Add(TitlesRecognizeds);
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }

        public static Dictionary<string, string> spDeleteRulePerStatus()
        {
            //var result = DataAccess.ExecWithStoreProcedureSave("Core", "spDeleteRulePerStatus", null);         
            try
            {
                return DataAccess.ExecQueryEntidadDictionary("Core", "spDeleteRulePerStatus");        
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            } 
        }
        public static Dictionary<string, string> spDeleteRulesPerDocuments()
        {
            //var result = DataAccess.ExecWithStoreProcedureSave("Core", "spDeleteRulesPerDocuments", null);
            try
            {
                return DataAccess.ExecQueryEntidadDictionary("Core", "spDeleteRulesPerDocuments");
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            } 
        }
        public static Dictionary<string, string> spDeleteRulePerTitle()
        {
            //var result = DataAccess.ExecWithStoreProcedureSave("Core", "spDeleteRulePerTitle", null);
            try
            {
                return DataAccess.ExecQueryEntidadDictionary("Core", "spDeleteRulePerTitle");
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            } 
        }

        //Obtencion de datos de AccountStatuses para opcion RestrictAccountStatuses
        //Developed by Kelvin Lopez C. - CSTI
        public static List<SponsorDataAccountStatus> SearchAccounts(int MarketID)
        {
            List<SponsorDataAccountStatus> result = null;
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>(){
                {"@MarketID", MarketID}              
                };

                SqlDataReader readerac = DataAccess.GetDataReader("upsGetAccountStatuses", parameters, "Core");

                if (readerac.HasRows)
                {
                    result = new List<SponsorDataAccountStatus>();
                    while (readerac.Read())
                    {
                        SponsorDataAccountStatus AccountStatutes = new SponsorDataAccountStatus();
                        AccountStatutes.AccountStatusID = Convert.ToInt32(readerac["AccountStatusID"]);
                        AccountStatutes.Name = Convert.ToString(readerac["Name"]);

                        result.Add(AccountStatutes);
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }

        //Obtencion de datos de TitlesType para opcion Restrisct Per Titles?
        //Developed by Kelvin Lopez C. - CSTI
        public static List<SponsorDataTitleType> SearchTitles()
        {
            List<SponsorDataTitleType> result = null;
            try
            {
                SqlDataReader readerac = DataAccess.GetDataReader("upsGetTitles", null, "Commissions");

                if (readerac.HasRows)
                {
                    result = new List<SponsorDataTitleType>();
                    while (readerac.Read())
                    {
                        SponsorDataTitleType Titles = new SponsorDataTitleType();
                        Titles.TitleID = Convert.ToInt32(readerac["TitleID"]);
                        Titles.Name = Convert.ToString(readerac["Name"]);

                        result.Add(Titles);
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }

        // Insert @RulePerStatuses
        public static int Insert(SponsorDataAccountStatus accountstatus)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                { "@AccountStatusID", accountstatus.AccountStatusID } };

                SqlCommand cmd = DataAccess.GetCommand("upsRulePerStatus", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }


        // Insert @RulesPerDocuments:
        public static int InsertDoc(SponsorshipSearchData RulesPerDocuments)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>(){
                {"@RequirementTypeID", RulesPerDocuments.RequirementTypeID},
                {"@LogicalOperator", RulesPerDocuments.LogicalOperator},
                {"@Order", RulesPerDocuments.Order}
                };

                SqlCommand cmd = DataAccess.GetCommand("upsRulesPerDocuments", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        // Insert @RulePerTitles:
        public static int InsertTitles(SponsorDataTitleType RulePerTitles)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>(){
                {"@TitleID", RulePerTitles.TitleID},
                {"@TitleTypeID", RulePerTitles.TitleTypeID}
                };

                SqlCommand cmd = DataAccess.GetCommand("upsRulePerTitle", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }


    }
}
