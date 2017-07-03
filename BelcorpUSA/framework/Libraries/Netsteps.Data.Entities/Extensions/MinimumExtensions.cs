using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using System.Data;
using System.Data.SqlClient;

namespace NetSteps.Data.Entities.Extensions
{
    public class MinimumExtensions
    {
        public static PaginatedList<MinimumSearchData> Get(MinimumSearchParameters searchParameter)
        {
            List<MinimumSearchData> paginateResult = DataAccess.ExecWithStoreProcedureLists<MinimumSearchData>("Commissions", "ListDisbursementMinimums").ToList();

            IQueryable<MinimumSearchData> matchingItems = paginateResult.AsQueryable<MinimumSearchData>();
            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParameter);
            return matchingItems.ToPaginatedList<MinimumSearchData>(searchParameter, resultTotalCount);
        }

        public static int Save(MinimumSearchParameters searchParameters)
        {
            var result = DataAccess.ExecWithStoreProcedureListParam<int>("Commissions", "InsertDisbursementMinimum",
                new SqlParameter("DisbursementMinimumID", SqlDbType.Int) { Value = searchParameters.DisbursementMinimumID },
                new SqlParameter("MinimumAmount", SqlDbType.Money) { Value = searchParameters.MinimumAmount },
                new SqlParameter("CurrencyID", SqlDbType.Int) { Value = searchParameters.CurrencyID },
                new SqlParameter("DisbursementTypeID", SqlDbType.Int) { Value = searchParameters.DisbursementTypeID}).ToList();
            return result[0];
        }

        public static void Delete(MinimumSearchParameters searchParameters)
        {
            DataAccess.ExecWithStoreProcedureSave("Commissions", "DeleteDisbursementMinimum",
                new SqlParameter("DisbursementMinimumIDs", SqlDbType.VarChar, 8000) { Value = searchParameters.DisbursementMinimumIDs });
        }

        public static List<MinimumSearchData> ListDropDownTypes(string tableName)
        {
            List<MinimumSearchData> paginateResult = DataAccess.ExecWithStoreProcedureListParam<MinimumSearchData>("Commissions", "ListDisbursementTypes",
            new SqlParameter("TableName", SqlDbType.VarChar, 100) { Value = tableName }).ToList();
            return paginateResult;
        }

    }
}
