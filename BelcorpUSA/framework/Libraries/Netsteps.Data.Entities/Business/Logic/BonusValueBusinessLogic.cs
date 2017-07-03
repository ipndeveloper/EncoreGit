﻿namespace NetSteps.Data.Entities.Business.Logic
 {
     using System;
     using System.Collections.Generic;
     using System.Text;
     using NetSteps.Data.Entities.Repositories.Interfaces;

     /// <summary>
     /// Descripcion de la clase, de lo que hace no como lo hace
     /// </summary>
     public partial class BonusValueBusinessLogic
     {
         /// <summary>
         /// Inicializa una nueva instancia de la clase BonusValueBusinessLogic usando patron Singleton
         /// </summary>
         public static BonusValueBusinessLogic Instance
         {
             get
             {
                 if (instance == null)
                 {
                     instance = new BonusValueBusinessLogic();
                     //IOC
                     repository = new NetSteps.Data.Entities.Repositories.BonusValueRepository();
                 }

                 return instance;
             }
         }

         #region Process Methods
         public void Insert(BonusValue bonusValue)
         {
             repository.Insert(new Dto.BonusValueDto() 
             {
                 BonusValueID = bonusValue.BonusValueID,
                 AccountID = bonusValue.AccountID,
                 BonusAmount = bonusValue.BonusAmount,
                 BonusTypeID = bonusValue.BonusTypeID,
                 CorpBonusAmount = bonusValue.CorpBonusAmount,
                 CorpCurrencyTypeID = bonusValue.CorpCurrencyTypeID,
                 CountryID = bonusValue.CountryID,
                 CurrencyTypeID = bonusValue.CurrencyTypeID,
                 DateModified = bonusValue.DateModified,
                 PeriodID = bonusValue.PeriodID
             });
         }
         #endregion

         #region Constructor - Singleton - Inyection

         /// <summary>
         /// Previene una clase por defecto de BonusValueBusinessLogic.
         /// </summary>
         private BonusValueBusinessLogic()
         { }

         /// <summary>
         /// instancia estatica de la clase BonusValueBusinessLogic
         /// </summary>
         private static BonusValueBusinessLogic instance;

         /// <summary>
         /// Interface para 
         /// </summary>
         private static IBonusValueRepository repository;

         #endregion

         #region Defaults
         #endregion
     }
 }