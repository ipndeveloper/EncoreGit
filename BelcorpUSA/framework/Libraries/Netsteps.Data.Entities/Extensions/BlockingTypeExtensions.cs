using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System;

namespace NetSteps.Data.Entities.Extensions
{
    public class BlockingTypeExtensions
    {
        public static PaginatedList<BlockingTypeSearchData> Get(BlockingTypeSearchParameters searchParameter)
        {
            List<BlockingTypeSearchData> paginateResult = DataAccess.ExecWithStoreProcedureListParam<BlockingTypeSearchData>("Core",
                "ListAccountBlockingTypes", new SqlParameter("LanguageID", SqlDbType.Int) { Value = searchParameter.LanguageID }).ToList();

            IQueryable<BlockingTypeSearchData> matchingItems = paginateResult.AsQueryable<BlockingTypeSearchData>();
            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParameter);
            return matchingItems.ToPaginatedList<BlockingTypeSearchData>(searchParameter, resultTotalCount);
        }
        // El siguiente insertara los valores a la BD
        public static Int16 Save(BlockingTypeSearchParameters parameter)
        {
            var newAccountBlockingTypeID = DataAccess.ExecWithStoreProcedureListParam<Int16>("core", "InsertAccountBlockingTypes",
                new SqlParameter("AccountBlockingTypeID", SqlDbType.SmallInt) { Value = parameter.AccountBlockingTypeID },
                new SqlParameter("Name", SqlDbType.NVarChar, 200) { Value = parameter.Name },
                new SqlParameter("Enabled", SqlDbType.Bit) { Value = parameter.Enabled },
                new SqlParameter("LanguageID", SqlDbType.Int) { Value = parameter.LanguageID }).ToList();

            return newAccountBlockingTypeID[0];
        }

        //public static string GetAccountIsLocked(BlockingTypeSearchParameters parameter)
        //{
        //    var newAccount = DataAccess.ExecWithStoreProcedureListParam<string>("core", "ObtenerAccountIsLocked",
        //        new SqlParameter("AccountID", SqlDbType.VarChar) { Value = parameter.AccountID },
        //         new SqlParameter("LanguageID", SqlDbType.Int) { Value = parameter.LanguageID }).ToList();

        //    return newAccount[0];
        //}

        public static AccountBlockingSearchData GetAccountIsLocked(BlockingTypeSearchParameters parameter)
        {
            AccountBlockingSearchData result = DataAccess.ExecWithStoreProcedureListParam<AccountBlockingSearchData>("core", "ObtenerAccountIsLocked",
                new SqlParameter("AccountID", SqlDbType.VarChar) { Value = parameter.AccountID },
                 new SqlParameter("LanguageID", SqlDbType.Int) { Value = parameter.LanguageID }).ToList().First();

            return result;
        }

        public static PaginatedList<BlockingTypeSearchData> GetAccountBlockingHistory(BlockingTypeSearchParameters searchParameter)
        {
            List<BlockingTypeSearchData> paginateResult = DataAccess.ExecWithStoreProcedureListParam<BlockingTypeSearchData>("Core",
                "ListaAccountBlockingHistory", new SqlParameter("AccountID", SqlDbType.VarChar) { Value = searchParameter.AccountID },
                 new SqlParameter("LanguageID", SqlDbType.Int) { Value = searchParameter.LanguageID }).ToList();

            IQueryable<BlockingTypeSearchData> matchingItems = paginateResult.AsQueryable<BlockingTypeSearchData>();
            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParameter);
            return matchingItems.ToPaginatedList<BlockingTypeSearchData>(searchParameter, resultTotalCount);
        }

        // El siguiente insertara los valores a la BD Para el Historial de Bloqueos
        public static int SaveAccountBlockingHistory(BlockingTypeSearchParameters parameter)
        {
            var newAccountBlockingHistoryID = DataAccess.ExecWithStoreProcedureListParam<int>("core", "InsertAccountBlockingHistoryChangeHistory",
                new SqlParameter("AccountID", SqlDbType.Int) { Value = parameter.AccountID },
                new SqlParameter("AccountBlockingTypeID", SqlDbType.SmallInt) { Value = parameter.AccountBlockingTypeID == null ? (object)DBNull.Value : parameter.AccountBlockingTypeID },
                new SqlParameter("AccountBlockingSubTypeID", SqlDbType.SmallInt) { Value = parameter.AccountBlockingSubTypeID == null ? (object)DBNull.Value : parameter.AccountBlockingSubTypeID },
                new SqlParameter("Reasons", SqlDbType.NVarChar) { Value = parameter.Reasons },
                 new SqlParameter("CreateByUserID", SqlDbType.Int) { Value = parameter.CreateByUserID },
                  new SqlParameter("DateCreatedUTC", SqlDbType.DateTime) { Value = parameter.DateCreatedUTC },
                   new SqlParameter("IsLocked", SqlDbType.Bit) { Value = parameter.IsLocked }).ToList();

            return newAccountBlockingHistoryID[0];
        }
    }
}