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
    public class AccountCreditRepository    
    {

        public  PaginatedList<AccountCreditSearchData> SearchAccountCredit(AccountSearchParameters searchParameter)
        {

            string FecIni =""; string FecFin ="";

            if (searchParameter.StartDate.HasValue)
                FecIni = searchParameter.StartDate.ToShortDateString();


            if (searchParameter.EndDate.HasValue)
                FecFin = searchParameter.EndDate.ToShortDateString();

            List<AccountCreditSearchData> paginatedResult = DataAccess.ExecWithStoreProcedureListParam<AccountCreditSearchData>("Core", "upsListAccountCredits",

                new SqlParameter("AccountID", SqlDbType.Int) { Value = (object)searchParameter.AccountID ?? DBNull.Value },
                new SqlParameter("AccountStatusID", SqlDbType.Int) { Value = (object)searchParameter.AccountStatusID ?? DBNull.Value },
                new SqlParameter("AccountTypes", SqlDbType.Int) { Value = (object)searchParameter.AccountTypeID ?? DBNull.Value },
                new SqlParameter("StateProvinceID", SqlDbType.Int) { Value = (object)searchParameter.StateProvinceID ?? DBNull.Value },

                new SqlParameter("City", SqlDbType.VarChar) { Value = (object)searchParameter.City ?? DBNull.Value  },
                new SqlParameter("PostalCode", SqlDbType.VarChar) { Value = (object)searchParameter.PostalCode ?? DBNull.Value },
                new SqlParameter("Email", SqlDbType.VarChar) { Value = (object)searchParameter.Email ?? DBNull.Value },
                new SqlParameter("CountryID", SqlDbType.Int) { Value = (object)searchParameter.CountryID ?? DBNull.Value },
                new SqlParameter("SiteUrl", SqlDbType.VarChar) { Value = (object)searchParameter.SiteUrl ?? DBNull.Value  },
                new SqlParameter("PhoneNumber", SqlDbType.VarChar) { Value = (object)searchParameter.PhoneNumber ?? DBNull.Value },
                new SqlParameter("SponsorID", SqlDbType.Int) { Value = (object)searchParameter.SponsorID ?? DBNull.Value },

                new SqlParameter("StartDate", SqlDbType.VarChar) { Value = (object)FecIni },
                new SqlParameter("EndDate", SqlDbType.VarChar) { Value = (object)FecFin },
                 new SqlParameter("TitleID", SqlDbType.Int) { Value = (object)searchParameter.TitleID ?? DBNull.Value } 
                ).ToList();

    


            IQueryable<AccountCreditSearchData> matchingItems = paginatedResult.AsQueryable<AccountCreditSearchData>();

            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParameter);

            return matchingItems.ToPaginatedList<AccountCreditSearchData>(searchParameter, resultTotalCount);
        }


        public static List<AccountCreditSearchData> GetAccountCreditLog(int AccountID)
        {
            List<AccountCreditSearchData> result = new List<AccountCreditSearchData>();
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@AccountID", AccountID } 
                                       
                };

                SqlDataReader reader = DataAccess.GetDataReader("uspGetAccountCreditLog", parameters, "Core");

                if (reader.HasRows)
                {
                    result = new List<AccountCreditSearchData>();
                    while (reader.Read())
                    {
                        result.Add(new AccountCreditSearchData()
                        {
                            FullName = Convert.ToString(reader["UserName"]),
                            AccountType = Convert.ToString(reader["AccountCreditLogNameType"]),
                            AccountCreditUti = Convert.ToString(reader["AccountCreditLogAmout"]),
                           AccountCreditFec = Convert.ToString(reader["AccountCreditLogDate"])                        
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
