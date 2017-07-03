﻿namespace NetSteps.Data.Entities.Business.Logic
 {
     using System;
     using System.Collections.Generic;
     using System.Text;
     using NetSteps.Data.Entities.Repositories.Interfaces;
     using NetSteps.Data.Entities.Dto;

     /// <summary>
     /// Descripcion de la clase, de lo que hace no como lo hace
     /// </summary>
     public partial class PromotionRewardEffectApplyOrderItemPropertyValueBusinessLogic
     {
         /// <summary>
         /// Inicializa una nueva instancia de la clase PromotionRewardEffectApplyOrderItemPropertyValueBusinessLogic usando patron Singleton
         /// </summary>
         public static PromotionRewardEffectApplyOrderItemPropertyValueBusinessLogic Instance
         {
             get
             {
                 if (instance == null)
                 {
                     instance = new PromotionRewardEffectApplyOrderItemPropertyValueBusinessLogic();
                     //IOC
                     repository = new NetSteps.Data.Entities.Repositories.PromotionRewardEffectApplyOrderItemPropertyValueRepository();
                 }

                 return instance;
             }
         }

         #region Process Methods

         public PromotionRewardEffectApplyOrderItemPropertyValue GetByPromotion(int promotionID)
         {
             return DtoToBo(repository.GetByPromotion(promotionID));
         }

         #endregion

         #region Constructor - Singleton - Inyection

         /// <summary>
         /// Previene una clase por defecto de PromotionRewardEffectApplyOrderItemPropertyValueBusinessLogic.
         /// </summary>
         private PromotionRewardEffectApplyOrderItemPropertyValueBusinessLogic()
         { }

         /// <summary>
         /// instancia estatica de la clase PromotionRewardEffectApplyOrderItemPropertyValueBusinessLogic
         /// </summary>
         private static PromotionRewardEffectApplyOrderItemPropertyValueBusinessLogic instance;

         /// <summary>
         /// Interface para 
         /// </summary>
         private static IPromotionRewardEffectApplyOrderItemPropertyValueRepository repository;

         #endregion

         #region Defaults

         PromotionRewardEffectApplyOrderItemPropertyValue DtoToBo(PromotionRewardEffectApplyOrderItemPropertyValueDto dto)
         {
             if (dto == null)
                 return null;

             return new PromotionRewardEffectApplyOrderItemPropertyValue()
             {
                 PromotionRewardEffectID = dto.PromotionRewardEffectID,
                 ProductPriceTypeID = dto.ProductPriceTypeID,
                 DecimalValue = dto.DecimalValue,
                 PromotionRewardID = dto.PromotionRewardID
             };
         }
         #endregion
     }
 }