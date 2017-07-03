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
    public class ManagmentLedgerExtension

    {
        public static PaginatedList<ManagmentLedgerSearchData> Search(ManagmentLedgerSearchParameters searchParameter, bool GetAll)
        {
            object RowsCount;
            List<ManagmentLedgerSearchData> paginatedResult = DataAccess.ExecWithStoreProcedureListParam<ManagmentLedgerSearchData>("Commissions", "uspListManagmentLedger", out RowsCount,
                                new SqlParameter("GetAll", SqlDbType.Bit) { Value = (object)GetAll ?? DBNull.Value },
                                new SqlParameter("AccountID", SqlDbType.Int) { Value = (object)searchParameter.AccountID ?? DBNull.Value },
                                new SqlParameter("PeriodID", SqlDbType.Int) { Value = searchParameter.PeriodID},
                                new SqlParameter("BonusTypeID", SqlDbType.Int) { Value = searchParameter.BonusTypeID},
                                new SqlParameter("StartDate", SqlDbType.Date) { Value = (object)searchParameter.StartDate ?? DBNull.Value },
                                new SqlParameter("EndDate", SqlDbType.Date) { Value = (object)searchParameter.EndDate ?? DBNull.Value },
                                new SqlParameter("EntryAmount", SqlDbType.Decimal) { Value = (object)searchParameter.EntryAmount ?? DBNull.Value },
                                new SqlParameter("EntryReasonID", SqlDbType.Int) { Value = searchParameter.EntryReasonID },
                                new SqlParameter("EntryOriginID", SqlDbType.Int) { Value = searchParameter.EntryOriginID},
                                new SqlParameter("EntryTypeID", SqlDbType.Int) { Value = searchParameter.EntryTypeID},
                                new SqlParameter("PageSize", SqlDbType.Int) { Value = searchParameter.PageSize},
                                new SqlParameter("PageNumber", SqlDbType.Int) { Value = searchParameter.PageIndex },
                                new SqlParameter("Colum", SqlDbType.VarChar) { Value = searchParameter.OrderBy },
                                new SqlParameter("Order", SqlDbType.VarChar) { Value = searchParameter.Order },
                                new SqlParameter("RowsCount", SqlDbType.Int) { Value = 0, Direction = ParameterDirection.Output }
                ).ToList();

            IQueryable<ManagmentLedgerSearchData> matchingItems = paginatedResult.AsQueryable<ManagmentLedgerSearchData>();

            var resultTotalCount = (int)RowsCount;
            return matchingItems.ToPaginatedList<ManagmentLedgerSearchData>(searchParameter, resultTotalCount);
        }

        public static Dictionary<int, string> ListBonusTypesML()
        {
            List<BonusTypes> BonusTypesResult = DataAccess.ExecWithStoreProcedureLists<BonusTypes>(ConnectionStrings.BelcorpCommission, "uspListBonusTypesML").ToList();
            Dictionary<int, string> BonusTypesResultDic = new Dictionary<int, string>();
            foreach (var item in BonusTypesResult)
            {
                BonusTypesResultDic.Add(item.BonusTypeID, Convert.ToString(item.TermName));
            }
            return BonusTypesResultDic;
        }

        public static Dictionary<int, string> ListLedgerEntryOriginsML()
        {
            List<EntryOrigins> EntryOriginsResult = DataAccess.ExecWithStoreProcedureLists<EntryOrigins>(ConnectionStrings.BelcorpCommission, "uspListLedgerEntryOriginsML").ToList();
            Dictionary<int, string> EntryOriginsResultDic = new Dictionary<int, string>();
            foreach (var item in EntryOriginsResult)
            {
                EntryOriginsResultDic.Add(item.EntryOriginID, Convert.ToString(item.Termname));
            }
            return EntryOriginsResultDic;
        }

        public static Dictionary<int, string> ListLedgerEntryReasonsML()
        {
            List<EntryReasons> EntryReasonsResult = DataAccess.ExecWithStoreProcedureLists<EntryReasons>(ConnectionStrings.BelcorpCommission, "uspListLedgerEntryReasonsML").ToList();
            Dictionary<int, string> EntryReasonsResultDic = new Dictionary<int, string>();
            foreach (var item in EntryReasonsResult)
            {
                EntryReasonsResultDic.Add(item.EntryReasonID, Convert.ToString(item.Termname));
            }
            return EntryReasonsResultDic;
        }

        public static Dictionary<int, string> ListLedgerEntryTypesML()
        {
            List<EntryTypes> EntryTypesResult = DataAccess.ExecWithStoreProcedureLists<EntryTypes>(ConnectionStrings.BelcorpCommission, "uspListLedgerEntryTypesML").ToList();
            Dictionary<int, string> EntryTypesResultDic = new Dictionary<int, string>();
            foreach (var item in EntryTypesResult)
            {
                EntryTypesResultDic.Add(item.EntryTypeID, Convert.ToString(item.Termname));
            }
            return EntryTypesResultDic;
        }



    }
}
