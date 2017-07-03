using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Common.Base;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.EntityModels;

namespace NetSteps.Data.Entities.Extensions
{
    public class TitleExtensions
    {
        public static TitleSearchData GetTitleByID(int id)
        {
            return DataAccess.ExecWithStoreProcedureListParam<TitleSearchData>("Commissions", "uspListTitles",
                new SqlParameter("id", SqlDbType.Int) { Value = id }).FirstOrDefault<TitleSearchData>();

        }

        public static IEnumerable<GetTitle> GetTitles()
        {

            return DataAccess.ExecWithStoreProcedureListParam<GetTitle>("Commissions", "uspListTitles").ToList();
        }

        public static IEnumerable<TitleInformation> ListTitlesByAccount(int accountID)
        {

            return DataAccess.ExecWithStoreProcedureListParam<TitleInformation>("Commissions", "uspTitleInformationByAccount",
                new SqlParameter("AccountID", SqlDbType.Int) { Value = accountID }).ToList();
        }

        public static IEnumerable<Titles> ListTitlesCombo()
        {
            return DataAccess.ExecWithStoreProcedureListParam<Titles>("Commissions", "uspListTitlesCombo");
        }

        public static PaginatedList<TitleSearchData> ListTitles(FilterDateRangePaginatedListParameters<TitleSearchData> searchParameter)
        {
            List<TitleSearchData> paginatedResult = DataAccess.ExecWithStoreProcedureLists<TitleSearchData>("Commissions", "uspListTitles").ToList();

            IQueryable<TitleSearchData> matchingItems = paginatedResult.AsQueryable<TitleSearchData>();

            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParameter);

            return matchingItems.ToPaginatedList<TitleSearchData>(searchParameter, resultTotalCount);
        }


         
        public static Dictionary<string, string> ListTitles()
        {
            return DataAccess.ExecQueryEntidadDictionary("Commissions", "uspListAllTitles");
        }

        public static int InsTitle(TitleSearchData pTitle)
        {
            var result = DataAccess.ExecWithStoreProcedureSaveIdentity("Commissions", "uspInsTitle", 
                new SqlParameter("TitleCode", SqlDbType.VarChar) { Value = pTitle.TitleCode },
                new SqlParameter("Name", SqlDbType.VarChar) { Value = pTitle.Name },
                new SqlParameter("SortOrder", SqlDbType.Int) { Value = pTitle.SortOrder },
                new SqlParameter("ClientCode", SqlDbType.VarChar) { Value = pTitle.ClientCode },
                new SqlParameter("ClientName", SqlDbType.VarChar) { Value = pTitle.ClientName },
                new SqlParameter("TitlePhaseID", SqlDbType.Int) { Value = pTitle.TitlePhaseID }
                
                );
            return result;
        }

        public static void UpdTitle(TitleSearchData pTitle)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Commissions", "uspUpdTitle",
                    new SqlParameter("TitleID", SqlDbType.Int) { Value = pTitle.TitleID },
                    new SqlParameter("TitleCode", SqlDbType.VarChar) { Value = pTitle.TitleCode },
                    new SqlParameter("Name", SqlDbType.VarChar) { Value = pTitle.Name },
                    new SqlParameter("SortOrder", SqlDbType.Int) { Value = pTitle.SortOrder },
                    new SqlParameter("ClientCode", SqlDbType.VarChar) { Value = pTitle.ClientCode },
                    new SqlParameter("ClientName", SqlDbType.VarChar) { Value = pTitle.ClientName },
                    new SqlParameter("TitlePhaseID", SqlDbType.Int) { Value = pTitle.TitlePhaseID }
                );
        }


        public static List<RequirementTitleCalculationSearchData> ListRequirementTitleCalculations(int TitleID)
        {
            return DataAccess.ExecWithStoreProcedureListParam<RequirementTitleCalculationSearchData>("Commissions", "uspListRequirementTitleCalculations",
                new SqlParameter("TitleID", SqlDbType.Int) { Value = TitleID }).ToList();

        }

        public static List<RequirementLegSearchData> ListRequirementLegs(int TitleID)
        {
            return DataAccess.ExecWithStoreProcedureListParam<RequirementLegSearchData>("Commissions", "uspListRequirementLegs",
                new SqlParameter("TitleID", SqlDbType.Int) { Value = TitleID }).ToList();

        }

        public static Dictionary<string, string> ListCalculationTypes(bool? ReportVisibility)
        {
            string parameters = string.Empty;
            parameters += ReportVisibility == null ? "NULL" : Convert.ToInt16(ReportVisibility).ToString();

            return DataAccess.ExecQueryEntidadDictionary("Commissions", "uspGetCalculationTypes " + parameters );
        }

        public static int InsRequirementTitleCalculation(RequirementTitleCalculationSearchData pDato)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Commissions", "uspInsRequirementTitleCalculations",
                new SqlParameter("TitleID", SqlDbType.Int) { Value = pDato.TitleID },
                new SqlParameter("CalculationtypeID", SqlDbType.Int) { Value = pDato.CalculationTypeID },
                new SqlParameter("PlanID", SqlDbType.Int) { Value = pDato.PlanID },
                new SqlParameter("MinValue", SqlDbType.Decimal) { Value = pDato.MinValue },
                new SqlParameter("MaxValue", SqlDbType.Decimal) { Value = pDato.MaxValue },
                new SqlParameter("DateModified", SqlDbType.DateTime2) { Value = pDato.DateModified }
                );
            return result;
        }

        public static int InsRequirementLeg(RequirementLegSearchData pDato)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Commissions", "uspInsRequirementLegs",
                new SqlParameter("TitleID", SqlDbType.Int) { Value = pDato.TitleID },
                new SqlParameter("PlanID", SqlDbType.Int) { Value = pDato.PlanID },
                new SqlParameter("TitleRequired", SqlDbType.Int) { Value = pDato.TitleRequired },
                new SqlParameter("Generation", SqlDbType.Int) { Value = pDato.Generation },
                new SqlParameter("Level", SqlDbType.Int) { Value = pDato.Level },
                new SqlParameter("TitleQTY", SqlDbType.Decimal) { Value = pDato.TitleQTY }
                );
            return result;
        }


        public static int DelRequirementTitleCalculation(int TitleID)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Commissions", "uspDelRequirementTitle",
                new SqlParameter("TitleID", SqlDbType.Int) { Value = TitleID }
                );
            return result;
        }

        public static int DelRequirementLeg(int TitleID)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Commissions", "uspDelRequirementLeg",
                new SqlParameter("TitleID", SqlDbType.Int) { Value = TitleID }
                );
            return result;
        }
        public static Dictionary<string, string> ListTitlePhases()
        {
            return DataAccess.ExecQueryEntidadDictionary("Commissions", "UspListAllTitlePhases");
        }



        public static IEnumerable<TitleInformationByAccount> ListGetTitlesByAccount(int accountID)
        {

            return DataAccess.ExecWithStoreProcedureListParam<TitleInformationByAccount>("Commissions", "uspGetTitleInformationByAccount",
                new SqlParameter("AccountID", SqlDbType.Int) { Value = accountID }).ToList();
        }

    }
}
