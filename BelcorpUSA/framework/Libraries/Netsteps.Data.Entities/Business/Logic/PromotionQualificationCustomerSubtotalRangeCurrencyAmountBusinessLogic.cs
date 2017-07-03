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
     public partial class PromotionQualificationCustomerSubtotalRangeCurrencyAmountBusinessLogic
     {
         /// <summary>
         /// Inicializa una nueva instancia de la clase PromotionQualificationCustomerSubtotalRangeCurrencyAmountBusinessLogic usando patron Singleton
         /// </summary>
         public static PromotionQualificationCustomerSubtotalRangeCurrencyAmountBusinessLogic Instance
         {
             get
             {
                 if (instance == null)
                 {
                     instance = new PromotionQualificationCustomerSubtotalRangeCurrencyAmountBusinessLogic();
                     //IOC
                     repository = new NetSteps.Data.Entities.Repositories.PromotionQualificationCustomerSubtotalRangeCurrencyAmountRepository();
                 }

                 return instance;
             }
         }

         #region Process Methods
         public IEnumerable<PromotionQualificationCustomerSubtotalRangeCurrencyAmount> GetByPromotionTypeConfiguration(int promotionTypeConfigurationID)
         {
             return from data in repository.GetByPromotionTypeConfiguration(promotionTypeConfigurationID)
                    select new PromotionQualificationCustomerSubtotalRangeCurrencyAmount()
               {
                   PromotionQualificationCustomerSubtotalRangeCurrencyAmountID = data.PromotionQualificationCustomerSubtotalRangeCurrencyAmountID,
                   PromotionQualificationID = data.PromotionQualificationID,
                   PromotionID = data.PromotionID,
                   CurrencyID = data.CurrencyID,
                   MinimumAmount = data.MinimumAmount,
                   MaximumAmount = data.MaximumAmount,
                   Discount = data.Discount
               };
         }
         #endregion

         #region Constructor - Singleton - Inyection

         /// <summary>
         /// Previene una clase por defecto de PromotionQualificationCustomerSubtotalRangeCurrencyAmountBusinessLogic.
         /// </summary>
         private PromotionQualificationCustomerSubtotalRangeCurrencyAmountBusinessLogic()
         { }

         /// <summary>
         /// instancia estatica de la clase PromotionQualificationCustomerSubtotalRangeCurrencyAmountBusinessLogic
         /// </summary>
         private static PromotionQualificationCustomerSubtotalRangeCurrencyAmountBusinessLogic instance;

         /// <summary>
         /// Interface para 
         /// </summary>
         private static IPromotionQualificationCustomerSubtotalRangeCurrencyAmountRepository repository;

         #endregion

         #region Defaults
         #endregion
     }
 }