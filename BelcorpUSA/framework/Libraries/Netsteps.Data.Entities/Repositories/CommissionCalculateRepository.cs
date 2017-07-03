namespace NetSteps.Data.Entities.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using NetSteps.Data.Entities.Dto;
    using NetSteps.Data.Entities.Repositories.Interfaces;

    /// <summary>
    /// Implementation of the ICommissionCalculateRepository Inteface
    /// </summary>
    public partial class CommissionCalculateRepository : ICommissionCalculateRepository
    {
        /// <summary>
        /// Calculate Commission By Personal Volumen
        /// </summary>
        /// <param name="accountsPicked">Accounts picked</param>
        /// <param name="bonusTypeId">Bonus Type Id</param>
        /// <param name="pediodId">Period Id</param>
        public void CommissionByPersonalVolumen(IEnumerable<AccountPerformanceDataDto> accountsPicked, int bonusTypeId, int periodId)
        {
            int rowAffected = 0;
            var tmpTable = AccountsPickedToTable(accountsPicked);

            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCommission))
            {
                object[] parameters = { new SqlParameter("@Cuentasseleccionadas", SqlDbType.Structured) { Value = tmpTable, TypeName = CommissionSelectedTableType },
                                        new SqlParameter("@BonusTypeID", SqlDbType.Int) { Value = bonusTypeId },
                                        new SqlParameter("@PeriodID", SqlDbType.Int) { Value = periodId } };
                var result = context.Database.SqlQuery<int>(GenerateQueryString(CommissionPersonalVolumenSP, parameters), parameters);

                rowAffected = result.First();
            }           
        }

        /// <summary>
        /// Calculate Commission By Level
        /// </summary>
        /// <param name="accountsPicked">Accounts picked</param>
        /// <param name="level">Commision Level</param>
        /// <param name="bonusTypeId">Bonus Type Id</param>
        /// <param name="periodId">Period Id</param>
        public void CommissionByLevels(IEnumerable<AccountPerformanceDataDto> accountsPicked, int level, int bonusTypeId, int periodId)
        {
            var tmpTable = AccountsPickedToTable(accountsPicked);
            int rowAffected = 0;
           
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCommission))
            {
                object[] parameters = { new SqlParameter("@Cuentasseleccionadas", SqlDbType.Structured) { Value = tmpTable, TypeName = CommissionSelectedTableType },
                                        new SqlParameter("@BonusTypeID", SqlDbType.Int) { Value = bonusTypeId },
                                        new SqlParameter("@PeriodID", SqlDbType.Int) { Value = periodId },
                                        new SqlParameter("@Nivel", SqlDbType.Int) { Value = level } };

                var result = context.Database.SqlQuery<int>(GenerateQueryString(CommissionLevelSP, parameters), parameters);
                rowAffected = result.First();
            }          
        }

        /// <summary>
        /// Calculate commission by group sales
        /// </summary>
        /// <param name="accountsPicked">Accounts picked</param>
        /// <param name="bonusTypeId">Bonus Type Id</param>
        /// <param name="periodId">Period Id</param>
        public void CommissionByGroupSales(IEnumerable<AccountPerformanceDataDto> accountsPicked, int bonusTypeId, int periodId)
        {
            var tmpTable = AccountsPickedToTable(accountsPicked);
            int rowAffected = 0;
            
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCommission))
            {
                object[] parameters ={ new SqlParameter("@Cuentasseleccionadas", SqlDbType.Structured) { Value = tmpTable, TypeName = CommissionSelectedTableType },
                                    new SqlParameter("@BonusTypeID", SqlDbType.Int) { Value = bonusTypeId },
                                    new SqlParameter("@PeriodID", SqlDbType.Int) { Value = periodId } };

                var result = context.Database.SqlQuery<int>(GenerateQueryString(CommissionGroupSP, parameters), parameters);
                rowAffected = result.First();
            }          
        }

        /// <summary>
        /// Calculate commission by Generation
        /// </summary>
        /// <param name="accountsPicked">Accounts picked</param>
        /// <param name="generation">Generation to calculate</param>
        /// <param name="bonusTypeId">Bonus Type Id</param>
        /// <param name="periodId">Period Id</param>
        public void CommissionByGeneration(IEnumerable<AccountPerformanceDataDto> accountsPicked, int generation, int bonusTypeId, int periodId)
        {
            var tmpTable = AccountsPickedToTable(accountsPicked);
            int rowAffected = 0;
           
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCommission))
            {
                object[] parameters = { new SqlParameter("@Cuentasseleccionadas", SqlDbType.Structured) { Value = tmpTable, TypeName = CommissionSelectedTableType },
                                        new SqlParameter("@BonusTypeID", SqlDbType.Int) { Value = bonusTypeId },
                                        new SqlParameter("@PeriodID", SqlDbType.Int) { Value = periodId },
                                        new SqlParameter("@Generacion", SqlDbType.Int) { Value = generation } };

                var result = context.Database.SqlQuery<int>(GenerateQueryString(CommissionGenerationSP, parameters), parameters);
                rowAffected = result.First();
            }          
        }

        /// <summary>
        /// Save Total
        /// </summary>
        /// <param name="periodId">Period Id</param>
        public void SaveTotal(int periodId)
        {
            int rowAffected = 0;
         
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCommission))
            {
                object[] parameters = { new SqlParameter("@PeriodID", SqlDbType.Int) { Value = periodId } };
                var result = context.Database.SqlQuery<int>(GenerateQueryString(CommissionSaveTotalSP, parameters), parameters);
                rowAffected = result.First();
            }            
        }

        /// <summary>
        /// Map IEnumerable accounts picked to Data Table
        /// </summary>
        /// <param name="accountsPicked">Accounts Picked</param>
        /// <returns>Data Table Account Selected</returns>
        private DataTable AccountsPickedToTable(IEnumerable<AccountPerformanceDataDto> accountsPicked)
        {
            DataTable tmpTable = new DataTable();
            tmpTable.Columns.Add("AccountId", typeof(int));
            tmpTable.Columns.Add("BonusTypeId", typeof(int));
            tmpTable.Columns.Add("Percentage", typeof(decimal));
            tmpTable.Columns.Add("CurrencyTypeID", typeof(int));

            foreach (var item in accountsPicked)
            {
                tmpTable.Rows.Add(item.AccountID, item.BonusTypeID, item.BonusPercent, item.CurrencyTypeID);
            }

            return tmpTable;
        }

        /// <summary>
        /// Add @ as pref of parameters
        /// </summary>
        /// <param name="query">Query or store procedure</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>string format Query @parameter ...</returns>
        private string GenerateQueryString(string query, params object[] parameters)
        {           
            if (!query.Contains("@") && parameters != null)
            {
                var parameterNames = from p in parameters select ((System.Data.SqlClient.SqlParameter)p).ParameterName;
                query = string.Format("{0} {1}", query, string.Join(", ", parameterNames));
            }

            return query;
        }

        //USP: store procedures used in this process
      
        /// <summary>
        /// Store procedure that calculate commission by Personal Volumen
        /// </summary>
        private const string CommissionPersonalVolumenSP = "uspCommissionCalculateByPersonal";
        
        /// <summary>
        /// Store procedure that calculate commission by Level
        /// </summary>
        private const string CommissionLevelSP = "uspCommissionCalculateByLevel";

        /// <summary>
        /// Store procedure that calculate commission by Group
        /// </summary>
        private const string CommissionGroupSP = "uspCommissionCalculateByGroup";

        /// <summary>
        /// Store procedure that calculate commission by Generation
        /// </summary>
        private const string CommissionGenerationSP = "uspCommissionCalculateByGeneration";

        /// <summary>
        /// Store procedure that save all commission calculated
        /// </summary>
        private const string CommissionSaveTotalSP = "uspCommissionCalculateSaveTotal";

        /// <summary>
        /// Table-valued parameter
        /// https://msdn.microsoft.com/en-us/library/bb510489.aspx
        /// </summary>
        private const string CommissionSelectedTableType = "dbo.SelectedAccountsType";
    }
}