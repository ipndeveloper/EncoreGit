﻿namespace NetSteps.Data.Entities.Business.Logic
 {
     using System;
     using System.Collections.Generic;
     using System.Text;
     using NetSteps.Data.Entities.Repositories.Interfaces;

     /// <summary>
     /// Descripcion de la clase, de lo que hace no como lo hace
     /// </summary>
     public partial class PromotionConfigurationControlBusinessLogic
     {
         /// <summary>
         /// Inicializa una nueva instancia de la clase PromotionConfigurationControlBusinessLogic usando patron Singleton
         /// </summary>
         public static PromotionConfigurationControlBusinessLogic Instance
         {
             get
             {
                 if (instance == null)
                 {
                     instance = new PromotionConfigurationControlBusinessLogic();
                     //IOC
                     repository = new NetSteps.Data.Entities.Repositories.PromotionConfigurationControlRepository();
                 }

                 return instance;
             }
         }

         #region Process Methods

         /// <summary>
         /// Se llamara al aprobarse una orden/ si no existe se crea
         /// summa el ammount al que ya se tiene
         /// </summary>
         /// <param name="accountId"></param>
         /// <param name="periodId"></param>
         /// <param name="amount"></param>
         public void UpdateAmount(int accountId, int periodId, decimal amount, int promotionID)
         {
             repository.UpdateAmount(accountId, periodId, amount, promotionID);
         }

         /// <summary>
         /// Update promotion
         /// </summary>
         /// <param name="promotionTypeConfigurationId"></param>
         /// <param name="newPromotionID"></param>
         public void UpdatePromotion(int promotionTypeConfigurationId, int newPromotionID)
         {
             repository.UpdatePromotion(promotionTypeConfigurationId, newPromotionID);
         }

         public void Insert(PromoPromotionConfigurationControl bo)
         {
             repository.Insert(BoToDto(bo));
         }

         public PromoPromotionConfigurationControl GetById(int promotionID)
         {
             return DtoToBo(repository.GetByPromotionID(promotionID));
         }

         public PromoPromotionConfigurationControl GetByAccount(int accountId)
         {
             return DtoToBo(repository.GetByAccount(accountId));
         }

         public PromoPromotionConfigurationControl GetByAccount(int accountId, int periodId)
         {
             return DtoToBo(repository.GetByAccount(accountId, periodId));
         }
         #endregion

         #region Constructor - Singleton - Inyection

         /// <summary>
         /// Previene una clase por defecto de PromotionConfigurationControlBusinessLogic.
         /// </summary>
         private PromotionConfigurationControlBusinessLogic()
         { }

         /// <summary>
         /// instancia estatica de la clase PromotionConfigurationControlBusinessLogic
         /// </summary>
         private static PromotionConfigurationControlBusinessLogic instance;

         /// <summary>
         /// Interface para 
         /// </summary>
         private static IPromotionConfigurationControlRepository repository;

         #endregion

         #region Defaults

         PromoPromotionConfigurationControl DtoToBo(NetSteps.Data.Entities.Dto.PromoPromotionConfigurationControlDto dto)
         {
             if (dto == null)
                 return null;
             return new PromoPromotionConfigurationControl()
             {
                 PromotionTypeConfigurationID = dto.PromotionTypeConfigurationID,
                 PromotionID = dto.PromotionID,
                 AccountID = dto.AccountID,
                 PeriodID = dto.PeriodID,
                 Amount = dto.Amount,
                 PromotionConfigurationControlID = dto.PromotionConfigurationControlID //autogenerado
             };
         }

         NetSteps.Data.Entities.Dto.PromoPromotionConfigurationControlDto BoToDto(PromoPromotionConfigurationControl bo)
         {
             if (bo == null)
                 return null;
             return new Dto.PromoPromotionConfigurationControlDto()
             {
                 PromotionTypeConfigurationID = bo.PromotionTypeConfigurationID,
                 PromotionID = bo.PromotionID,
                 AccountID = bo.AccountID,
                 PeriodID = bo.PeriodID,
                 Amount = bo.Amount,
                 PromotionConfigurationControlID = bo.PromotionConfigurationControlID //autogenerado
             };
         }
         #endregion

     }
 }