using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using System.Data.SqlClient;
using System.Data;

namespace NetSteps.Data.Entities.Extensions
{
    public class BlockingSubTypeExtensions
    {
        public static PaginatedList<BlockingSubTypeSearchData> Get(BlockingSubTypeSearchParameters searchParameter)
        {
            List<BlockingSubTypeSearchData> paginateResult = DataAccess.ExecWithStoreProcedureListParam<BlockingSubTypeSearchData>("Core","ListAccountBlockingSubTypes",
                new SqlParameter("LanguageID", SqlDbType.Int) { Value = searchParameter.LanguageID },
                new SqlParameter("BlockingTypeID", SqlDbType.Int) { Value = searchParameter.AccountBlockingTypeID },
                new SqlParameter("Enabled", SqlDbType.Int) { Value = searchParameter.Enabled}).ToList();

            IQueryable<BlockingSubTypeSearchData> matchingItems = paginateResult.AsQueryable<BlockingSubTypeSearchData>();
            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParameter);
            return matchingItems.ToPaginatedList<BlockingSubTypeSearchData>(searchParameter, resultTotalCount);
        }

        //El siguiente insertara los valores a la BD
        public static Int16 Save(BlockingSubTypeSearchParameters parameter)
        {
            var newAccountBlockingTypeID = DataAccess.ExecWithStoreProcedureListParam<Int16>("core", "InsertAccountBlockingSubTypes",
                new SqlParameter("AccountBlockingSubTypeID", SqlDbType.SmallInt) { Value = parameter.AccountBlockingSubTypeID },
                new SqlParameter("AccountBlockingTypeID", SqlDbType.SmallInt) { Value = parameter.AccountBlockingTypeID },
                new SqlParameter("Name", SqlDbType.NVarChar, 200) { Value = parameter.Name },
                new SqlParameter("Enabled", SqlDbType.Bit) { Value = parameter.Enabled },
                new SqlParameter("LanguageID", SqlDbType.Int) { Value = parameter.LanguageID },
                new SqlParameter("CadenaBlockProcess", SqlDbType.VarChar) { Value = parameter.ListaBlockingProcess }).ToList();

            return newAccountBlockingTypeID[0];
        }

        public static PaginatedList<BlockingSubTypeSearchData> ListTypeProcess(BlockingSubTypeSearchParameters searchParameter)
        {
            List<BlockingSubTypeSearchData> paginateResult = DataAccess.ExecWithStoreProcedureListParam<BlockingSubTypeSearchData>("Core",
              "ListaAccountBlockingProcess").ToList();

            IQueryable<BlockingSubTypeSearchData> matchingItems = paginateResult.AsQueryable<BlockingSubTypeSearchData>();
            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParameter);
            return matchingItems.ToPaginatedList<BlockingSubTypeSearchData>(searchParameter, resultTotalCount);
        }

        //El siguiente insertara los valores a la BD
        public static Int16 SaveSubTypeBlockProcess(Int16 AccountBlockingSubTypeID, Int16 AccountBlockingProcessID)
        {
            var newAccountBlockingTypeID = DataAccess.ExecWithStoreProcedureListParam<Int16>("core", "InsertAccountBlockingSubTypeProcess",
                new SqlParameter("AccountBlockingSubTypeID", SqlDbType.SmallInt) { Value = AccountBlockingSubTypeID },
                new SqlParameter("AccountBlockingProcessID", SqlDbType.SmallInt) { Value = AccountBlockingProcessID }).ToList();

            return newAccountBlockingTypeID[0];
        }

        public static PaginatedList<BlockingSubTypeSearchData> GetAccountSubTypeProcess(BlockingSubTypeSearchParameters searchParameter)
        {
            List<BlockingSubTypeSearchData> paginateResult = DataAccess.ExecWithStoreProcedureListParam<BlockingSubTypeSearchData>("Core",
                "ListarAccountBlockingSubTypeProcess", new SqlParameter("AccountBlockingSubTypeID", SqlDbType.SmallInt) { Value = searchParameter.AccountBlockingSubTypeID }).ToList();

            IQueryable<BlockingSubTypeSearchData> matchingItems = paginateResult.AsQueryable<BlockingSubTypeSearchData>();
            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParameter);
            return matchingItems.ToPaginatedList<BlockingSubTypeSearchData>(searchParameter, resultTotalCount);
        }




    }
}
