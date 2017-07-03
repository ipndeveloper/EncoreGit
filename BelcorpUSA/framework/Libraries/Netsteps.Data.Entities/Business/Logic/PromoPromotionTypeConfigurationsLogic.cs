namespace NetSteps.Data.Entities.Business.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NetSteps.Data.Entities.Repositories.Interfaces;
    using NetSteps.Data.Entities.Repositories;
    using System.Transactions;
    using NetSteps.Data.Entities.Dto;
    public class PromoPromotionTypeConfigurationsLogic
    {
        #region Private

        private static PromoPromotionTypeConfigurationsLogic instance;

        private static IPromoPromotionTypeConfigurationsRepository repository;

        /// <summary>
        /// Transforms business object in dto object
        /// </summary>
        /// <param name="bo">Promotion Type Configurations business object</param>
        /// <returns>Promotion Type Configurations Dto</returns>
        private PromoPromotionTypeConfigurationsDto BoToDto(PromoPromotionTypeConfigurations bo)
        {
            if (bo == null)
                return null;

            return new NetSteps.Data.Entities.Dto.PromoPromotionTypeConfigurationsDto()
            {
                PromotionTypeConfigurationID = bo.PromotionTypeConfigurationID,
                PromotionTypeID = bo.PromotionTypeID,
                Active = bo.Active,
                IncludeBAorders = bo.IncludeBAorders
            };
        }

        /// <summary>
        /// Transforms dto object in business object
        /// </summary>
        /// <param name="dto">Promotion Type Configurations Dto</param>
        /// <returns>Promotion Type Configurations business object</returns>
        private PromoPromotionTypeConfigurations DtoToBo(NetSteps.Data.Entities.Dto.PromoPromotionTypeConfigurationsDto dto)
        {
            if (dto == null)
                return null;

            return new PromoPromotionTypeConfigurations()
            {
                PromotionTypeConfigurationID = dto.PromotionTypeConfigurationID,
                PromotionTypeID = dto.PromotionTypeID,
                Active = dto.Active,
                IncludeBAorders = dto.IncludeBAorders
            };
        }

        #endregion

        #region Singleton

        private PromoPromotionTypeConfigurationsLogic() { }

        public static PromoPromotionTypeConfigurationsLogic Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PromoPromotionTypeConfigurationsLogic();
                    repository = new PromoPromotionTypeConfigurationsRepository();
                }

                return instance;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Inactivates all records
        /// </summary>
        /// <returns></returns>
        public bool InactivateAll()
        {
            var result = repository.InactivateAll();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oPromotionConfigurationBo"></param>
        /// <param name="outLastGeneratedID"></param>
        /// <returns></returns>
        public bool Insert(PromoPromotionTypeConfigurations oPromotionConfigurationBo, out int outLastGeneratedID)
        {
            var result = repository.Insert(BoToDto(oPromotionConfigurationBo), out outLastGeneratedID);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<PromoPromotionTypeConfigurations> ListAll()
        {
            var data = repository.ListAll();
            return (from r in data select DtoToBo(r)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetLastConfiguration()
        {
            return repository.GetLastConfiguration();
        }

        public int GetPromocionTypeDescuentoAcumulativo()
        {
            return repository.GetPromocionTypeDescuentoAcumulativo();
        }

        /// <summary>
        /// Obtiene el PromotionType asociado para el promotionTypeConfiguration Activo
        /// </summary>
        /// <returns></returns>
        public int GetActiveID()
        {
            return repository.GetActive();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PromotionTypeID"></param>
        /// <param name="Promotions"></param>
        /// <param name="BAOrders"></param>
        /// <returns></returns>
        public bool SavePromotionConfiguration(string PromotionTypeID, List<PromoPromotion> NewPromotions, List<PromoPromotionTypeConfigurationPerPromotion> PromotionToDelete, bool BAOrders, bool newConfiguration)
        {
            var transactionOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            };
            using (var mainScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                try
                {
                    var result = false;
                    var promotionTypeConfigurationID = 0;

                    if (newConfiguration == true)
                    {
                        ///Inactivar las demas configuraciones
                        result = PromoPromotionTypeConfigurationsLogic.Instance.InactivateAll();
                        if (result == false) { mainScope.Dispose(); return false; }

                        ///Insertar una consiguracion activa
                        var oBOPromotionTypeConfiguration = new PromoPromotionTypeConfigurations()
                        {
                            PromotionTypeID = int.Parse(PromotionTypeID),
                            Active = true,
                            IncludeBAorders = BAOrders
                        };
                        result = PromoPromotionTypeConfigurationsLogic.Instance.Insert(oBOPromotionTypeConfiguration, out promotionTypeConfigurationID);
                        if (result == false) { mainScope.Dispose(); return false; }

                        ///insertar las promociones a la nueva configuracion
                        foreach (var item in NewPromotions)
                        {
                            var obj = new PromoPromotionTypeConfigurationPerPromotion()
                            {
                                PromotionTypeConfigurationPerPromotionID = 0,
                                PromotionTypeConfigurationID = promotionTypeConfigurationID,
                                PromotionID = item.PromotionID,
                            };
                            result = PromoPromotionTypeConfigurationPerPromotionLogic.Instance.Insert(obj);
                            if (result == false) { mainScope.Dispose(); return false; }
                        }

                        ///insertar configuracion por order
                        //////////var obj2 = new PromoPromotionTypeConfigurationsPerOrder()
                        //////////{
                        //////////    PromotionTypeConfigurationsPerOrderID = 0,
                        //////////    PromotionTypeConfigurationID = promotionTypeConfigurationID,
                        //////////    IncludeBAorders = BAOrders
                        //////////};
                        //////////result = PromoPromotionTypeConfigurationsPerOrderLogic.Instance.Insert(obj2);
                        //////////if (result == false) { mainScope.Dispose(); return false; }
                    }
                    else
                    {
                        promotionTypeConfigurationID = GetLastConfiguration();
                        foreach (var item in NewPromotions)
                        {
                            var obj = new PromoPromotionTypeConfigurationPerPromotion()
                            {
                                PromotionTypeConfigurationPerPromotionID = 0,
                                PromotionTypeConfigurationID = promotionTypeConfigurationID,
                                PromotionID = item.PromotionID
                            };
                            result = PromoPromotionTypeConfigurationPerPromotionLogic.Instance.Insert(obj);
                            if (result == false) { mainScope.Dispose(); return false; }
                        }

                        foreach (var itemPerPromotion in PromotionToDelete)
                        {
                            result = PromoPromotionTypeConfigurationPerPromotionLogic.Instance.Delete(itemPerPromotion);
                            if (result == false) { mainScope.Dispose(); return false; }
                        }
                    }

                    mainScope.Complete();
                    mainScope.Dispose();
                    return true;
                }
                catch (Exception)
                {
                    mainScope.Dispose();
                    return false;
                }
            }
        }

        #endregion
    }
}