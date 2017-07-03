﻿namespace NetSteps.Data.Entities.Business.Logic
 {
     using System;
     using System.Collections.Generic;
     using System.Text;
     using NetSteps.Data.Entities.Repositories.Interfaces;
     using NetSteps.Data.Entities.Repositories;

     /// <summary>
     /// Descripcion de la clase, de lo que hace no como lo hace
     /// </summary>
     public partial class PeriodBusinessLogic
     {
         /// <summary>
         /// Inicializa una nueva instancia de la clase PeriodBusinessLogic usando patron Singleton
         /// </summary>
         public static PeriodBusinessLogic Instance
         {
             get
             {
                 if (instance == null)
                 {
                     instance = new PeriodBusinessLogic();
                     //IOC
                     //repository = new NetSteps.Data.Entities.Repositories.PeriodsRepository();
                 }

                 return instance;
             }
         }

         #region Process Methods

         #endregion

         #region Constructor - Singleton - Inyection

         /// <summary>
         /// Previene una clase por defecto de PeriodBusinessLogic.
         /// </summary>
         private PeriodBusinessLogic()
         { }

         /// <summary>
         /// instancia estatica de la clase PeriodBusinessLogic
         /// </summary>
         private static PeriodBusinessLogic instance;

         #endregion

         #region Defaults

         /// <summary>
         /// Gets current period
         /// </summary>
         /// <returns>Period ID</returns>
         public int GetOpenPeriodID()
         {
             return PeriodsRepository.GetOpenPeriodID();
         }

         /// <summary>
         /// Gets previous period
         /// </summary>
         /// <param name="periodId">Period From</param>
         /// <param name="levelDown">Level down</param>
         /// <returns>Period ID</returns>
         public int GetPreviousPeriod(int periodId, int levelDown)
         {
             return PeriodsRepository.GetPreviousPeriod(periodId, levelDown);
         }
         #endregion
     }
 }