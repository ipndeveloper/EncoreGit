﻿namespace NetSteps.Data.Entities.Business.Logic
 {
     using System;
     using System.Collections.Generic;
     using System.Text;
     using NetSteps.Data.Entities.Repositories.Interfaces;
     using NetSteps.Data.Entities.Dto;
     using NetSteps.Data.Entities.Repositories;

     /// <summary>
     /// Descripcion de la clase, de lo que hace no como lo hace
     /// </summary>
     public partial class ActivityBusinessLogic
     {
         /// <summary>
         /// Inicializa una nueva instancia de la clase ActivityBusinessLogic usando patron Singleton
         /// </summary>
         public static ActivityBusinessLogic Instance
         {
             get
             {
                 if (instance == null)
                 {
                     instance = new ActivityBusinessLogic();
                     //IOC
                     repository = new NetSteps.Data.Entities.Repositories.ActivityRepository();
                 }

                 return instance;
             }
         }

         #region Process Methods
         public string DeleteActivitiesByAccountID(int AccountID)
         {
             try
             {
                 ActivityRepository repository = new ActivityRepository();
                 string resultado = repository.DeleteActivitiesByAccountID(AccountID);
                 return resultado;
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         /// <summary>
         /// Gets activity by filters
         /// </summary>
         /// <param name="accountID">Account Id</param>
         /// <param name="periodID">Period Id</param>
         /// <returns>Activity Dto</returns>
         public Activity GetByFilters(int accountID, int periodID)
         {
             return DtoToBusiness(repository.GetByFilters(accountID, periodID));
         }

         /// <summary>
         /// Delete Activity
         /// </summary>
         /// <param name="activityID">Activity Id</param>
         public void Delete(long activityID)
         {
             repository.Delete(activityID);
         }

         /// <summary>
         /// Update Activity
         /// </summary>
         /// <param name="dto"></param>
         public void Update(Activity bo)
         {
             repository.Update(BusinessToDto(bo));
         }

         /// <summary>
         /// Gets Activities count in Period
         /// </summary>
         /// <param name="accountID">Account ID</param>
         /// <param name="periodID">Period Id</param>
         /// <returns>Activities count</returns>
         public int ActivitiesInPeriodLessCurrent(int accountID, int periodID, long currentActivityID)
         {
             return repository.ActivitiesInPeriodLessCurrent(accountID, periodID, currentActivityID);
         }
         #endregion

         #region Constructor - Singleton - Inyection

         /// <summary>
         /// Previene una clase por defecto de ActivityBusinessLogic.
         /// </summary>
         private ActivityBusinessLogic()
         { }

         /// <summary>
         /// instancia estatica de la clase ActivityBusinessLogic
         /// </summary>
         private static ActivityBusinessLogic instance;

         /// <summary>
         /// Interface para 
         /// </summary>
         private static IActivityRepository repository;

         #endregion

         #region Defaults

         /// <summary>
         /// Map from table to dto
         /// </summary>
         /// <param name="activity"></param>
         /// <returns></returns>
         ActivityDto BusinessToDto(Activity activity)
         {
             if (activity == null)
                 return null;

             return new ActivityDto()
             {
                 ActivityID = activity.ActivityID,
                 ActivityStatusID = activity.ActivityStatusID,
                 PeriodID = activity.PeriodID,
                 IsQualified = activity.IsQualified,
                 AccountID = activity.AccountID,
                 AccountConsistencyStatusID = activity.AccountConsistencyStatusID,  //INI-FIN - GR_Encore-07
                 HasContinuity = activity.HasContinuity                             //INI-FIN - GR_Encore-07
             };
         }

         /// <summary>
         /// Map from dto To table
         /// </summary>
         /// <param name="dto"></param>
         /// <returns></returns>
         Activity DtoToBusiness(ActivityDto dto)
         {
             if (dto == null)
                 return null;

             return new Activity()
             {
                 ActivityID = dto.ActivityID,
                 ActivityStatusID = dto.ActivityStatusID,
                 PeriodID = dto.PeriodID,
                 IsQualified = dto.IsQualified,
                 AccountID = dto.AccountID,
                 AccountConsistencyStatusID = dto.AccountConsistencyStatusID,  //INI-FIN - GR_Encore-07
                 HasContinuity = dto.HasContinuity                             //INI-FIN - GR_Encore-07
             };
         }
         #endregion
     }
 }