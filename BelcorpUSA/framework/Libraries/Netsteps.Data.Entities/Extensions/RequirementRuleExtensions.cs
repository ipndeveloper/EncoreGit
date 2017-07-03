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
    public class RequirementRuleExtensions
    {
        public static PaginatedList<RequirementRuleSearchData> ListRequirementRules(RequirementRuleSearchParameters searchParameter)
        {
            List<RequirementRuleSearchData> paginatedResult = DataAccess.ExecWithStoreProcedureListParam<RequirementRuleSearchData>("Commissions", "uspListRequirementRules",
                                new SqlParameter("RuleTypeID", SqlDbType.Int) { Value = searchParameter.RuleTypeID },
                                new SqlParameter("PlanID", SqlDbType.Int) { Value = searchParameter.PlanID }
                                ).ToList();

            IQueryable<RequirementRuleSearchData> matchingItems = paginatedResult.AsQueryable<RequirementRuleSearchData>();

            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParameter);

            return matchingItems.ToPaginatedList<RequirementRuleSearchData>(searchParameter, resultTotalCount);
        }

        public static Dictionary<string, string> ListRuleTypes()
        {
            return DataAccess.ExecQueryEntidadDictionary("Commissions", "uspListRuleTypes");
        }
        public static Dictionary<string, string> ListPlans()
        {
            return DataAccess.ExecQueryEntidadDictionary("Commissions", "uspListPlans");
        }


        public static RequirementRuleSearchData GetRuleByID(int id)
        {
            return DataAccess.ExecWithStoreProcedureListParam<RequirementRuleSearchData>("Commissions", "uspGetRequirementRulesByID",
                new SqlParameter("id", SqlDbType.Int) { Value = id }).FirstOrDefault<RequirementRuleSearchData>();

        }

        public static RuleTypeSearchData GetRuleTypeByID(int id)
        {
            return DataAccess.ExecWithStoreProcedureListParam<RuleTypeSearchData>("Commissions", "uspGetRuleTypeByID",
                new SqlParameter("RuleTypeID", SqlDbType.Int) { Value = id }).FirstOrDefault<RuleTypeSearchData>();

        }
        public static int InsRule(RequirementRuleSearchData pDato)
        {
            var result = DataAccess.ExecWithStoreProcedureSaveIdentity("Commissions", "uspInsRequirementRules",
                new SqlParameter("RuleTypeID", SqlDbType.Int) { Value = pDato.RuleTypeID },
                new SqlParameter("PlanID", SqlDbType.Int) { Value = pDato.PlanID },
                new SqlParameter("Value1", SqlDbType.VarChar) { Value = pDato.Value1 },
                new SqlParameter("Value2", SqlDbType.VarChar) { Value = (object)pDato.Value2 ?? DBNull.Value },
                new SqlParameter("Value3", SqlDbType.VarChar) { Value = (object)pDato.Value3 ?? DBNull.Value },
                new SqlParameter("Value4", SqlDbType.VarChar) { Value = (object)pDato.Value4 ?? DBNull.Value }
                );

            return result;
        }

        public static int UpdRule(RequirementRuleSearchData pDato)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Commissions", "uspUpdRequirementRules",
                new SqlParameter("RuleRequirementID", SqlDbType.Int) { Value = pDato.RuleRequirementID },
                new SqlParameter("RuleTypeID", SqlDbType.Int) { Value = pDato.RuleTypeID },
                new SqlParameter("PlanID", SqlDbType.Int) { Value = pDato.PlanID },
                new SqlParameter("Value1", SqlDbType.VarChar) { Value = pDato.Value1 },
                new SqlParameter("Value2", SqlDbType.VarChar) { Value = (object)pDato.Value2 ?? DBNull.Value },
                new SqlParameter("Value3", SqlDbType.VarChar) { Value = (object)pDato.Value3 ?? DBNull.Value },
                new SqlParameter("Value4", SqlDbType.VarChar) { Value = (object)pDato.Value4 ?? DBNull.Value }
                );

            return result;
        }

        /*CS:04/03/2016*/
        public static MontoMinimoWebSiteEnroll ObtenerMontoMinimoWebSiteEnroll()
        {
            return DataAccess.ExecWithStoreProcedureListParam<MontoMinimoWebSiteEnroll>("Commissions", "uspObtenerMontoMinimoWebSiteEnroll").FirstOrDefault<MontoMinimoWebSiteEnroll>();
        }
        /*CS*/
    }
}
