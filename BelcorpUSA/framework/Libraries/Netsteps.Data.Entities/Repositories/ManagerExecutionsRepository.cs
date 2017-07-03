using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Repositories.Interfaces;

namespace NetSteps.Data.Entities.Repositories
{
    public class ManagerExecutionsRepository : IManagerExecutionsRepository
    {
        /// <summary>
        /// Returns the subprocesses
        /// </summary>
        /// <param name="LanguageID">current language</param>
        /// <returns>A generic list of SubProcessStatusSearchData class</returns>
        public List<SubProcessStatusSearchData> ListSubProcessStatus(int LanguageID, int status, int page, int pageSize, string column, string order)
        {
            var result = new List<SubProcessStatusSearchData>();
            try
            {
                object[] parameters = { new SqlParameter("@LanguageID", SqlDbType.Int) { Value = LanguageID } ,
                                          new SqlParameter("@StatusID", SqlDbType.Int) { Value = status },
                                        new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize },
                                        new SqlParameter("@PageNumber", SqlDbType.Int) { Value = page },
                                        new SqlParameter("@Colum", SqlDbType.VarChar) { Value = column },
                                        new SqlParameter("@Order", SqlDbType.VarChar) { Value = order }};

                using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCommission))
                {
                    result = DbContext.Database.SqlQuery<SubProcessStatusSearchData>
                            (GenerateQueryString("spListSubProcesses", parameters), parameters).ToList();
                }

                return result;
            }
            catch (Exception) { throw new Exception("An error has occurred, please try again."); }
        }

        /// <summary>
        /// Get an especific failed subprocess 
        /// </summary>
        /// <param name="MonthlyClosureDetailLogID">subprocess's id</param>
        /// <returns>retorns a FailedSubProcessSearchData object</returns>
        public FailedSubProcessSearchData GetFailedSubProcess(int LanguageID, int MonthlyClosureDetailLogID)
        {
            var result = new FailedSubProcessSearchData();
            try
            {
                object[] parameters = { new SqlParameter("@MonthlyClosureDetailLogID", SqlDbType.Int) { Value = MonthlyClosureDetailLogID },
                                       new SqlParameter("@LanguageId", SqlDbType.Int) { Value = LanguageID }};

                using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCommission))
                {
                    result = DbContext.Database.SqlQuery<FailedSubProcessSearchData>(GenerateQueryString("spGetFailedSubprocess", parameters), parameters).First();
                }

                return result;
            }
            catch (Exception) { throw new Exception("An error has occurred, please try again."); }
        }

        /// <summary>
        /// Get an especific failed subprocess 
        /// </summary>
        /// <param name="MonthlyClosureDetailLogID">subprocess's id</param>
        /// <returns>retorns a FailedSubProcessPersonalIndicatorSearchData object</returns>
        public FailedSubProcessPersonalIndicatorSearchData GetFailedSubProcess_PI(int LanguageID, int PersonalIndicatorDetailLogID)
        {
            var result = new FailedSubProcessPersonalIndicatorSearchData();
            try
            {
                object[] parameters = { new SqlParameter("@PersonalIndicatorDetailLogID", SqlDbType.Int) { Value = PersonalIndicatorDetailLogID },
                                       new SqlParameter("@LanguageId", SqlDbType.Int) { Value = LanguageID }};

                using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCommission))
                {
                    result = DbContext.Database.SqlQuery<FailedSubProcessPersonalIndicatorSearchData>(GenerateQueryString("spGetFailedSubprocess_PersonalIndicator", parameters), parameters).First();
                }

                return result;
            }
            catch (Exception) { throw new Exception("An error has occurred, please try again."); }
        }

        /// <summary>
        /// Gets periodId and planID to repocess
        /// </summary>
        /// <param name="MonthlyClosureLogID"></param>
        /// <returns>returns a PeriodPlanSearchData class</returns>
        public PeriodPlanSearchData GetPlanAndPeriod(int MonthlyClosureLogID)
        {
            var result = new PeriodPlanSearchData();
            try
            {
                object[] parameters = { new SqlParameter("@MonthlyClosureLogID", SqlDbType.Int) { Value = MonthlyClosureLogID } };

                using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCommission))
                {
                    result = DbContext.Database.SqlQuery<PeriodPlanSearchData>(GenerateQueryString("spGetPlanAndPeriod", parameters), parameters).First();
                }

                return result;
            }
            catch (Exception) { throw new Exception("An error has occurred, please try again."); }
        }

        /// <summary>
        /// Add @ as pref of parameters
        /// </summary>
        /// <param name="query">Query or store procedure</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>string format Query @parameter ...</returns>
        private string GenerateQueryString(string query, params object[] parameters)
        {
            if (!query.Contains("@") && parameters != null)
            {
                var parameterNames = from p in parameters select ((System.Data.SqlClient.SqlParameter)p).ParameterName;
                query = string.Format("{0} {1}", query, string.Join(", ", parameterNames));
            }

            return query;
        }

        /// <summary>
        /// Returns the subprocesses
        /// </summary>
        /// <param name="LanguageID">current language</param>
        /// <returns>A generic list of MainProcessesDetail class</returns>
        public List<MainProcessesDetailSearchData> ListMainProcessesDetail(int LanguageID, int status, int page, int pageSize, string column, string order)
        {
            var result = new List<MainProcessesDetailSearchData>();
            try
            {
                object[] parameters = { new SqlParameter("@LanguageID", SqlDbType.Int) { Value = LanguageID } ,
                                          new SqlParameter("@StatusID", SqlDbType.Int) { Value = status },
                                        new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize },
                                        new SqlParameter("@PageNumber", SqlDbType.Int) { Value = page },
                                        new SqlParameter("@Colum", SqlDbType.VarChar) { Value = column },
                                        new SqlParameter("@Order", SqlDbType.VarChar) { Value = order }};

                using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCommission))
                {
                    result = DbContext.Database.SqlQuery<MainProcessesDetailSearchData>
                            (GenerateQueryString("spListMainProcessesDetail", parameters), parameters).ToList();
                }

                return result;
            }
            catch (Exception) { throw new Exception("An error has occurred, please try again."); }
        }

        /// <summary>
        /// Gets OrderID and OrderStatusID to repocess
        /// </summary>
        /// <param name="PersonalIndicatorLogID"></param>
        /// <returns>returns a OrderOrderStatusSearchData class</returns>
        public OrderOrderStatusSearchData GetOderAndOrderStatus(int PersonalIndicatorLogID)
        {
            var result = new OrderOrderStatusSearchData();
            try
            {
                object[] parameters = { new SqlParameter("@PersonalIndicatorLogID", SqlDbType.Int) { Value = PersonalIndicatorLogID } };

                using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCommission))
                {
                    result = DbContext.Database.SqlQuery<OrderOrderStatusSearchData>(GenerateQueryString("spGetOrderAndOrderStatus", parameters), parameters).First();
                }

                return result;
            }
            catch (Exception) { throw new Exception("An error has occurred, please try again."); }
        }
    }
}
