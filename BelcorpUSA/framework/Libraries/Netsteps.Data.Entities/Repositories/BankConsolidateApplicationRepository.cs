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
    public class BankConsolidateApplicationRepository
    {

        public static PaginatedList<BankConsolidateApplicationSearchData> SearchBankConsolidateApplication(BankConsolidateApplicationSearchParameter searchParameter)
        {
            List<BankConsolidateApplicationSearchData> paginatedResult = DataAccess.ExecWithStoreProcedureListParam<BankConsolidateApplicationSearchData>("Core", "uspGetBankConsolidateApplication",

                new SqlParameter("BankId", SqlDbType.Int) { Value = (object)searchParameter.Bankid ?? DBNull.Value },
                new SqlParameter("BankConsolidateDatePro", SqlDbType.VarChar) { Value = (object)searchParameter.BankConsolidateDatePro ?? DBNull.Value }
                 ).ToList();

            IQueryable<BankConsolidateApplicationSearchData> matchingItems = paginatedResult.AsQueryable<BankConsolidateApplicationSearchData>();

            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParameter);

            return matchingItems.ToPaginatedList<BankConsolidateApplicationSearchData>(searchParameter, resultTotalCount);
        }

        public static void InsertBankPayments(BankPaymentsSearchParameter param)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Core", "uspInsertBankPayments",
                new SqlParameter("TicketNumber", SqlDbType.Int) { Value = param.TicketNumber },
                new SqlParameter("DateReceivedBank_cad", SqlDbType.VarChar) { Value = param.DateReceivedBank },
                new SqlParameter("Amount", SqlDbType.VarChar) { Value = param.Amount },
                new SqlParameter("FileSequence", SqlDbType.Int) { Value = param.FileSequence },

                 new SqlParameter("BankName", SqlDbType.VarChar) { Value = param.BankName },
                new SqlParameter("FileNameBank", SqlDbType.VarChar) { Value = param.FileNameBank },
                new SqlParameter("logSequence", SqlDbType.Int) { Value = param.logSequence },
                new SqlParameter("BankId", SqlDbType.Int) { Value = param.Bankid }
             
                );
        }

        //Aplicación del recaudo
        public static void ImplementationCollection(BankPaymentsSearchParameter param)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Core", "uspImplementationCollection",
                new SqlParameter("TipoCredito", SqlDbType.VarChar) { Value = param.TipoCredito },
                new SqlParameter("BankId", SqlDbType.Int) { Value = param.Bankid },
                new SqlParameter("FileSequence", SqlDbType.Int) { Value = param.FileSequence },
                new SqlParameter("UserID", SqlDbType.Int) { Value = param.UserID },
                new SqlParameter("NombreArchivo", SqlDbType.VarChar) { Value = param.FileNameBank },
                new SqlParameter("FechaArchivo", SqlDbType.VarChar) { Value = param.FileDate },
                new SqlParameter("BankName", SqlDbType.VarChar) { Value = param.BankName }
              );
        }
        
        public static PaginatedList<BankPayments> SearchBankPayments(BankPaymentsSearchParameter searchParameter)
        {
            //List<BankConsolidateApplicationSearchData> paginatedResult = DataAccess.ExecWithStoreProcedureListParam<BankConsolidateApplicationSearchData>("Core", "uspGetBankConsolidateApplication",
             BankPaymentsRepository rp=  new BankPaymentsRepository();
             var paginatedResult = rp.BrowseBankPayments().FindAll(
                    x => x.AccountCode == (searchParameter.AccountCode.HasValue ? searchParameter.AccountCode : x.AccountCode) &&
                    (x.BankID == (searchParameter.Bankid.HasValue ? searchParameter.Bankid : x.BankID)) &&
                    (x.FileSequence == (searchParameter.FileSequence.HasValue? searchParameter.FileSequence : x.FileSequence)) &&
                    (x.TicketNumber == (searchParameter.TicketNumber.HasValue ? searchParameter.TicketNumber : x.TicketNumber))     
                    );

            IQueryable<BankPayments> matchingItems = paginatedResult.AsQueryable<BankPayments>();
            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParameter);
            return matchingItems.ToPaginatedList<BankPayments>(searchParameter, resultTotalCount);

        }

    }
}
