﻿namespace NetSteps.Data.Entities.Business.Logic
 {
     using System;
     using System.Collections.Generic;
     using System.Text;
     using NetSteps.Data.Entities.Repositories.Interfaces;

     /// <summary>
     /// Descripcion de la clase, de lo que hace no como lo hace
     /// </summary>
     public partial class ProductPriceTypeExtensionBusinessLogic
     {
         /// <summary>
         /// Inicializa una nueva instancia de la clase ProductPriceTypeExtensionBusinessLogic usando patron Singleton
         /// </summary>
         public static ProductPriceTypeExtensionBusinessLogic Instance
         {
             get
             {
                 if (instance == null)
                 {
                     instance = new ProductPriceTypeExtensionBusinessLogic();
                     //IOC
                     repository = new NetSteps.Data.Entities.Repositories.ProductPriceTypeRepository();
                 }

                 return instance;
             }
         }

         #region Process Methods
         public bool GetMandatory(int id) 
         {
             return repository.GetMandatory(id);
         }
         #endregion

         #region Constructor - Singleton - Inyection

         /// <summary>
         /// Previene una clase por defecto de ProductPriceTypeExtensionBusinessLogic.
         /// </summary>
         private ProductPriceTypeExtensionBusinessLogic()
         { }

         /// <summary>
         /// instancia estatica de la clase ProductPriceTypeExtensionBusinessLogic
         /// </summary>
         private static ProductPriceTypeExtensionBusinessLogic instance;

         /// <summary>
         /// Interface para 
         /// </summary>
         private static NetSteps.Data.Entities.Repositories.IProductPriceTypeRepository repository;

         #endregion

         #region Defaults
         #endregion
     }
 }