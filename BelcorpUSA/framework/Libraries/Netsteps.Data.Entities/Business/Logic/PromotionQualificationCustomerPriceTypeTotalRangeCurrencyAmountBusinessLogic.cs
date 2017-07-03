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
     public partial class PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountBusinessLogic
     {
         /// <summary>
         /// Inicializa una nueva instancia de la clase PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountBusinessLogic usando patron Singleton
         /// </summary>
         public static PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountBusinessLogic Instance
         {
             get
             {
                 if (instance == null)
                 {
                     instance = new PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountBusinessLogic();
                     //IOC
                     repository = new NetSteps.Data.Entities.Repositories.PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountRepository();
                 }

                 return instance;
             }
         }

         #region Process Methods
         public PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmount GetByAmount(decimal amount)
         {
             return DtoToBo(repository.GetByAmount(amount));
         }

         public PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmount GetById(int id)
         {
             return DtoToBo(repository.GetById(id));
         }

         /// <summary>
         /// Obtiene por la promocion
         /// </summary>
         /// <param name="id">Id de la promocion</param>
         /// <returns></returns>
         public PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmount GetByPromotionID(int id)
         {
             return DtoToBo(repository.GetByPromotionID(id));
         }
         #endregion

         #region Constructor - Singleton - Inyection

         /// <summary>
         /// Previene una clase por defecto de PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountBusinessLogic.
         /// </summary>
         private PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountBusinessLogic()
         { }

         /// <summary>
         /// instancia estatica de la clase PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountBusinessLogic
         /// </summary>
         private static PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountBusinessLogic instance;

         /// <summary>
         /// Interface para 
         /// </summary>
         private static IPromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountRepository repository;

         #endregion

         private PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmount DtoToBo(PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountDto dto)
         {
             if (dto == null)
                 return null;

             return new PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmount()
             {
                 PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountID = dto.PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountID,
                 PromotionQualificationID = dto.PromotionQualificationID,
                 CurrencyID = dto.CurrencyID,
                 MinimumAmount = dto.MinimumAmount,
                 MaximumAmount = dto.MaximumAmount,
                 Cumulative = dto.Cumulative,
                  PromotionID = dto.PromotionID
             };
         }

         private PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountDto BoToDto( PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmount bo)
         {
             if (bo == null)
                 return null;

             return new PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountDto() 
             {
                 PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountID = bo.PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountID,
                 PromotionQualificationID = bo.PromotionQualificationID,
                 CurrencyID = bo.CurrencyID,
                 MinimumAmount = bo.MinimumAmount,
                 MaximumAmount = bo.MaximumAmount,
                 Cumulative = bo.Cumulative
             }; 
         }
     }
 }