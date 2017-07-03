﻿namespace NetSteps.Data.Entities.Business.Logic
 {
     using System;
     using System.Collections.Generic;
     using System.Text;
     using NetSteps.Data.Entities.Repositories.Interfaces;

     /// <summary>
     /// Descripcion de la clase, de lo que hace no como lo hace
     /// </summary>
     public partial class PromotionRewardBusinessLogic
     {
         /// <summary>
         /// Inicializa una nueva instancia de la clase PromotionRewardBusinessLogic usando patron Singleton
         /// </summary>
         public static PromotionRewardBusinessLogic Instance
         {
             get
             {
                 if (instance == null)
                 {
                     instance = new PromotionRewardBusinessLogic();
                     //IOC
                     repository = new NetSteps.Data.Entities.Repositories.PromotionRewardRepository();
                 }

                 return instance;
             }
         }

         #region Process Methods
         public PromotionReward GetByPromotionID(int promotionId)
         {
             return DtoToBo(repository.GetByPromotionID(promotionId));
         }
         #endregion

         #region Constructor - Singleton - Inyection

         /// <summary>
         /// Previene una clase por defecto de PromotionRewardBusinessLogic.
         /// </summary>
         private PromotionRewardBusinessLogic()
         { }

         /// <summary>
         /// instancia estatica de la clase PromotionRewardBusinessLogic
         /// </summary>
         private static PromotionRewardBusinessLogic instance;

         /// <summary>
         /// Interface para 
         /// </summary>
         private static IPromotionRewardRepository repository;

         #endregion

         #region Defaults

         PromotionReward DtoToBo(NetSteps.Data.Entities.Dto.PromotionRewardDto dto)
         {
             if (dto == null)
                 return null;

             return new PromotionReward() 
             {
                 PromotionID = dto.PromotionID,
                 PromotionRewardID = dto.PromotionRewardID,
                 PromotionPropertyKey = dto.PromotionPropertyKey,
                 PromotionRewardKind = dto.PromotionRewardKind
             };
         }

         #endregion
     }
 }