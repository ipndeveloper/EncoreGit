using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Common.Base;


namespace NetSteps.Data.Entities.Extensions
{
    public class DisbursementReportExtensions
    {

        public static PaginatedList<DisbursementReportSearchData> Search(DisbursementReportSearchParameters searchParameter, bool GetAll)
        {
            object RowsCount;
            List<DisbursementReportSearchData> paginatedResult = DataAccess.ExecWithStoreProcedureListParam<DisbursementReportSearchData>("Commissions", "uspListDisbursementReport", out RowsCount,
                new SqlParameter("GetAll", SqlDbType.Bit) { Value = (object)GetAll ?? DBNull.Value },
                new SqlParameter("AccountID", SqlDbType.Int) { Value = (object)searchParameter.AccountID ?? DBNull.Value },
                new SqlParameter("PeriodID", SqlDbType.Int) { Value = searchParameter.PeriodID },
                new SqlParameter("DisbursementStatusID", SqlDbType.Int) { Value = searchParameter.DisbursementStatusID },
                new SqlParameter("PageSize", SqlDbType.Int) { Value = searchParameter.PageSize },
                new SqlParameter("PageNumber", SqlDbType.Int) { Value = searchParameter.PageIndex },
                new SqlParameter("Colum", SqlDbType.VarChar) { Value = searchParameter.OrderBy },
                new SqlParameter("Order", SqlDbType.VarChar) { Value = searchParameter.Order },
                new SqlParameter("RowsCount", SqlDbType.Int) { Value = 0, Direction = ParameterDirection.Output }
                ).ToList();

            IQueryable<DisbursementReportSearchData> matchingItems = paginatedResult.AsQueryable<DisbursementReportSearchData>();

            var resultTotalCount = (int)RowsCount;

            return matchingItems.ToPaginatedList<DisbursementReportSearchData>(searchParameter, resultTotalCount);
        }

        public static Dictionary<int, string> ListDisbursementStatuses()
        {
            List<DisbursementReportStatus> StatusResult = DataAccess.ExecWithStoreProcedureLists<DisbursementReportStatus>("Commissions", "uspListDisbursementStatuses").ToList();
            Dictionary<int, string> StatusResultDic = new Dictionary<int, string>();
            foreach (var item in StatusResult)
            {
                StatusResultDic.Add(item.StatusID, Convert.ToString(item.Name));
            }
            return StatusResultDic;
        }

    }
}
