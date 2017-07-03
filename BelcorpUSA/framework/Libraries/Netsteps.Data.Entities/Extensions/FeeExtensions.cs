using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using System.Data.SqlClient;
using System.Data;

namespace NetSteps.Data.Entities.Extensions
{
    public class FeeExtensions
    {
        public static PaginatedList<FeeSearchData> Get(FeeSearchParameters searchParameter)
        {
            List<FeeSearchData> paginateResult = DataAccess.ExecWithStoreProcedureLists<FeeSearchData>("Commissions", "ListDisbursementFees").ToList();

            IQueryable<FeeSearchData> matchingItems = paginateResult.AsQueryable<FeeSearchData>();
            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParameter);
            return matchingItems.ToPaginatedList<FeeSearchData>(searchParameter, resultTotalCount);
        }

        public static int Save(FeeSearchParameters searchParameters)
        {
            var result = DataAccess.ExecWithStoreProcedureListParam<int>("Commissions", "InsertDisbursementFee",
                new SqlParameter("DisbursementFeeID", SqlDbType.Int) { Value = searchParameters.DisbursementFeeID },
                new SqlParameter("DisbursementFeeTypeID", SqlDbType.Int) { Value = searchParameters.DisbursementFeeTypeID },
                new SqlParameter("DisbursementTypeID", SqlDbType.Int) { Value = searchParameters.DisbursementTypeID },
                new SqlParameter("CurrencyID", SqlDbType.Int) { Value = searchParameters.CurrencyID },
                new SqlParameter("Amount", SqlDbType.Float) { Value = searchParameters.Amount }).ToList();
            return result[0];
        }

        public static void Delete(FeeSearchParameters searchParameters)
        {
            DataAccess.ExecWithStoreProcedureSave("Commissions", "DeleteDisbursementFee",
                new SqlParameter("DisbursementFeeIDs", SqlDbType.VarChar, 8000) { Value = searchParameters.DisbursementFeeIDs });
        }
    }
}
