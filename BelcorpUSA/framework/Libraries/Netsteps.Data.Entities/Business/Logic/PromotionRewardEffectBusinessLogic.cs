﻿namespace NetSteps.Data.Entities.Business.Logic
 {
     using System;
     using System.Collections.Generic;
     using System.Text;
     using NetSteps.Data.Entities.Repositories.Interfaces;
     using System.Linq;

     /// <summary>
     /// Descripcion de la clase, de lo que hace no como lo hace
     /// </summary>
     public partial class PromotionRewardEffectBusinessLogic
     {
         /// <summary>
         /// Inicializa una nueva instancia de la clase PromotionRewardEffectBusinessLogic usando patron Singleton
         /// </summary>
         public static PromotionRewardEffectBusinessLogic Instance
         {
             get
             {
                 if (instance == null)
                 {
                     instance = new PromotionRewardEffectBusinessLogic();
                     //IOC
                     repository = new NetSteps.Data.Entities.Repositories.PromotionRewardEffectRepository();
                 }

                 return instance;
             }
         }

         #region Process Methods
         public IEnumerable<PromotionRewardEffect> GetAssociated(int promotionRewardId)
         {
             return from r in repository.GetAssociated(promotionRewardId)
                    select DtoToBO(r);
         }
         #endregion

         #region Constructor - Singleton - Inyection

         /// <summary>
         /// Previene una clase por defecto de PromotionRewardEffectBusinessLogic.
         /// </summary>
         private PromotionRewardEffectBusinessLogic()
         { }

         
         /// <summary>
         /// instancia estatica de la clase PromotionRewardEffectBusinessLogic
         /// </summary>
         private static PromotionRewardEffectBusinessLogic instance;

         /// <summary>
         /// Interface para 
         /// </summary>
         private static IPromotionRewardEffectRepository repository;

         #endregion

         #region Defaults
         PromotionRewardEffect DtoToBO(NetSteps.Data.Entities.Dto.PromotionRewardEffectDto dto)
         {
             if (dto == null)
                 return null;

             return new PromotionRewardEffect()
             {
                 PromotionRewardEffectID = dto.PromotionRewardEffectID,
                 ExtensionProviderKey = dto.ExtensionProviderKey,
                 PromotionRewardID = dto.PromotionRewardID,
                 RewardPropertyKey = dto.RewardPropertyKey,
                 DecimalValue = dto.DecimalValue
             };
 
         }

         #endregion
     }
 }