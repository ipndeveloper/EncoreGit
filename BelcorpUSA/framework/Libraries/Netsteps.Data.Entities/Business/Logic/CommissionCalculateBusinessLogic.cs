﻿namespace NetSteps.Data.Entities.Business.Logic
 {
     using System;
     using System.Collections.Generic;
     using System.Linq;
     using System.Text;
     using NetSteps.Data.Entities.Repositories.Interfaces;

     /// <summary>
     /// Commission Operations
     /// </summary>
     public partial class CommissionCalculateBusinessLogic
     {
         /// <summary>
         /// Initialize a new instance of the CommissionBusinessLogic using singleton pattern
         /// </summary>
         public static CommissionCalculateBusinessLogic Instance
         {
             get
             {
                 if (instance == null)
                 {
                     instance = new CommissionCalculateBusinessLogic();
                     //IOC
                     repository = new NetSteps.Data.Entities.Repositories.CommissionCalculateRepository();
                 }

                 return instance;
             }
         }

         /// <summary>
         /// Calculate Commision By Period
         /// CommissionBy Personal Volumen
         /// Commission By Levels (1)
         /// Commission By Levels (2)
         /// Commission By GroupSales
         /// Commission By Generation (1)
         /// Commission By Generation (2)
         /// Commission By Generation (3)
         /// Commission By Generation (4)
         /// Commission By Generation (5)
         /// SaveTotal
         /// </summary>
         /// <param name="periodId">Period Id</param>
         public virtual void Calculate(int periodId)
         {
             this.PeriodId = periodId;

             //Obtiene el plan por defecto
             var defaultPlan = PlanLogic.Instance.GetAll().Where(m => m.DefaultPlan == true).FirstOrDefault();
             int defaultPlanId = defaultPlan == null ? PlanId : defaultPlan.PlanId;

             //Obtiene el country Id para Brazil
             int countryId = CountrySPLogic.Instance.CountryIdByName(BrazilName);
             countryId = countryId == null ? CountryId : countryId;

             //Obtiene los titulos activos
             var titles = TitleBusinessLogic.Instance.GetAllActives();

             foreach (var title in titles)
             {
                 IEnumerable<AccountPerformanceData> selectedAccounts = AccountPerformanceDataBusinessLogic.Instance.GetPickedAccounts(title.TitleId, defaultPlanId, countryId);
                 if (selectedAccounts.Any())
                 {
                     CommissionByPersonalVolumen(selectedAccounts);
                     CommissionByLevels(selectedAccounts, 1);
                     CommissionByLevels(selectedAccounts, 2);
                     CommissionByGroupSales(selectedAccounts);
                     CommissionByGeneration(selectedAccounts, 1);
                     CommissionByGeneration(selectedAccounts, 2);
                     CommissionByGeneration(selectedAccounts, 3);
                     CommissionByGeneration(selectedAccounts, 4);
                     CommissionByGeneration(selectedAccounts, 5);

                     SaveTotal();
                 }
             }
         }

         #region Process Methods

         /// <summary>
         /// Calculate Commission By Personal Volumen
         /// </summary>
         /// <param name="accountsPicked">Accounts picked</param>
         private void CommissionByPersonalVolumen(IEnumerable<AccountPerformanceData> accountsPicked)
         {
             DateTime startTime = DateTime.Now;

             try
             {
                 int bonusTypeIdLevel1 = BonusTypeIdByCode(Level1);
                 repository.CommissionByPersonalVolumen(from r in accountsPicked select AccountPerformanceDataBusinessLogic.Instance.BOToDto(r), bonusTypeIdLevel1, this.PeriodId);

                 SaveLog(startTime, DateTime.Now, OK, CommissionByPersonalVolumenMessage);
             }
             catch (Exception ex)
             {
                 SaveLog(startTime, DateTime.Now, Failed, CommissionByPersonalVolumenMessage);
                 throw ex;
             }
         }

         /// <summary>
         /// Calculate Commission By Levels
         /// </summary>
         /// <param name="accountsPicked">Accounts picked</param>
         /// <param name="level">Level to Calculate</param>
         private void CommissionByLevels(IEnumerable<AccountPerformanceData> accountsPicked, int level)
         {
             DateTime startTime = DateTime.Now;

             try
             {
                 int bonusTypeId = BonusTypeIdByCode(Level2);
                 var data = from r in accountsPicked select AccountPerformanceDataBusinessLogic.Instance.BOToDto(r);
                 repository.CommissionByLevels(data, level, bonusTypeId, this.PeriodId);
                 SaveLog(startTime, DateTime.Now, OK, CommissionByLevelsMessage);
             }
             catch (Exception ex)
             {
                 SaveLog(startTime, DateTime.Now, Failed, CommissionByLevelsMessage);
                 throw ex;
             }
         }

         /// <summary>
         /// Calculate Commission By Group sales
         /// </summary>
         /// <param name="accountsPicked">Accounts picked</param>
         private void CommissionByGroupSales(IEnumerable<AccountPerformanceData> accountsPicked)
         {
             DateTime startTime = DateTime.Now;

             try
             {
                 int bonusTypeId = BonusTypeIdByCode(GroupBonus);
                 var data = from r in accountsPicked select AccountPerformanceDataBusinessLogic.Instance.BOToDto(r);
                 repository.CommissionByGroupSales(data, bonusTypeId, this.PeriodId);
                 SaveLog(startTime, DateTime.Now, OK, CommissionByGroupSalesMessage);
             }
             catch (Exception ex)
             {
                 SaveLog(startTime, DateTime.Now, Failed, CommissionByGroupSalesMessage);
                 throw ex;
             }
         }

         /// <summary>
         /// Calculate Commission by Generation
         /// </summary>
         /// <param name="accountsPicked">Accounts picked</param>
         /// <param name="generation">Generation to calculate</param>
         private void CommissionByGeneration(IEnumerable<AccountPerformanceData> accountsPicked, int generation)
         {
             DateTime startTime = DateTime.Now;

             try
             {
                 int bonusTypeId = BonusTypeIdByGeneration(generation);
                 var data = from r in accountsPicked select AccountPerformanceDataBusinessLogic.Instance.BOToDto(r);
                 repository.CommissionByGeneration(data, generation, bonusTypeId, this.PeriodId);
                 SaveLog(startTime, DateTime.Now, OK, CommissionByGenerationMessage);
             }
             catch (Exception ex)
             {
                 SaveLog(startTime, DateTime.Now, Failed, CommissionByGenerationMessage);
                 throw ex;
             }
         }

         /// <summary>
         /// Save Total
         /// </summary>
         private void SaveTotal()
         {
             DateTime startTime = DateTime.Now;
             try
             {
                 repository.SaveTotal(this.PeriodId);
                 SaveLog(startTime, DateTime.Now, OK, SaveTotalMessage);
             }
             catch (Exception ex)
             {
                 SaveLog(startTime, DateTime.Now, Failed, SaveTotalMessage);
                 throw ex;
             }
         }

         /// <summary>
         /// Save row on Log commission
         /// </summary>
         /// <param name="startTime">Start Time</param>
         /// <param name="endTime">End Time</param>
         /// <param name="result">Result</param>
         /// <param name="description">Descripcion of process</param>
         void SaveLog(DateTime startTime, DateTime endTime, string result, string description)
         {
             LogCommissionBusinessLogic.Instance.Insert(new LogCommission()
             {
                 StartTime = startTime,
                 EndTime = endTime,
                 Result = result,
                 Description = description
             });
         }

         private bool Validations()
         {
             //TODO: add here validations
             throw new NotImplementedException();
         }

         #endregion

         #region Properties

         /// <summary>
         /// Gets or sets Period Id
         /// </summary>
         private int PeriodId { get; set; }

         #endregion

         #region Constructor - Singleton - Inyection

         /// <summary>
         /// Prevents a default instance of the CommissionBusinessLogic class.
         /// </summary>
         private CommissionCalculateBusinessLogic()
         { }

         /// <summary>
         /// Private instance of the CommissionBusinessLogic class used by singleton
         /// </summary>
         private static CommissionCalculateBusinessLogic instance;

         /// <summary>
         /// Commission Calculare Interface
         /// </summary>
         private static ICommissionCalculateRepository repository;

         /// <summary>
         /// Gets Bonus Type Id by Code
         /// </summary>
         /// <param name="bonusCode">Bonus Type Code</param>
         /// <returns>Bonus Type Id</returns>
         private int BonusTypeIdByCode(string bonusCode)
         {
             int bonusTypeId = BonusTypeLogic.Instance.GetAll().Where(m => m.BonusCode == bonusCode).Select(m => m.BonusTypeId).First();

             if (bonusTypeId == null)
                 throw new Exception("Bonus type not found");

             return bonusTypeId;
         }

         /// <summary>
         /// Get Bonus Type by Generation
         /// </summary>
         /// <param name="generation">Commission Generation</param>
         /// <returns>Bonus Type Id</returns>
         private int BonusTypeIdByGeneration(int generation)
         {
             string generationCode = Generation1.Substring(0, 1) + generation;
             return BonusTypeIdByCode(generationCode);
         }
         #endregion

        #region CommissionTypes

        /// <summary>
        /// Group Bonus Code GROUP
        /// </summary>
        private const string GroupBonus = "GROUP";

        /// <summary>
        /// Level 1 Type L1
        /// </summary>
        private const string Level1 = "L1";

        /// <summary>
        /// Level 2 Type L2
        /// </summary>
        private const string Level2 = "L2";

        /// <summary>
        /// Generation 1 Type G1
        /// </summary>
        private const string Generation1 = "G1";

        /// <summary>
        /// Generation 2 Type G2
        /// </summary>
        private const string Generation2 = "G2";

        /// <summary>
        /// Generation 3 Type G3
        /// </summary>
        private const string Generation3 = "G3";

        /// <summary>
        /// Generation 4 Type G4
        /// </summary>
        private const string Generation4 = "G4";

        /// <summary>
        /// Generation 5 Type G5
        /// </summary>
        private const string Generation5 = "G5";
        #endregion
         
        #region ProcessResult
         
        /// <summary>
        /// Ok
        /// </summary>
        private const string OK = "Ok";

        /// <summary>
        /// Failed
        /// </summary>
        private const string Failed = "Failed";

        /// <summary>
        /// Commission By Personal Volumen
        /// </summary>
        private const string CommissionByPersonalVolumenMessage = "Commission By Personal Volumen";

        /// <summary>
        /// Commission By Levels
        /// </summary>
        private const string CommissionByLevelsMessage = "Commission By Levels";

        /// <summary>
        /// Commission By Group Sales
        /// </summary>
        private const string CommissionByGroupSalesMessage = "Commission By GroupSales";

        /// <summary>
        /// Commission By Generation
        /// </summary>
        private const string CommissionByGenerationMessage = "Commission By Generation";

        /// <summary>
        /// Save Total
        /// </summary>
        private const string SaveTotalMessage = "Save Total";
        #endregion

        #region Defaults

        private const string BrazilName = "Brazil";
        private const int PlanId = 1;
        private const int CountryId = 51;
        #endregion
     }
 }