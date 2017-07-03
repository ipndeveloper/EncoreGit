namespace NetSteps.Data.Entities.Business.Logic
{
    using NetSteps.Data.Entities.Repositories.Interfaces;
    using NetSteps.Data.Entities.Repositories;
    using System.Collections.Generic;
    using System.Linq;
    using System;
    using NetSteps.Data.Entities.Dto;
    using System.Transactions;
    using NetSteps.Data.Common.Entities;
    using NetSteps.Data.Entities.Generated;

    public class PromoPromotionLogic
    {
        #region Private

        private static PromoPromotionLogic instance;

        private static IPromoPromotionRepository repository;

        /// <summary>
        /// Transforms dto object in business object
        /// </summary>
        /// <param name="dto">Promotion Dto</param>
        /// <returns>Promotion BO</returns>
        private PromoPromotion DtoToBO(NetSteps.Data.Entities.Dto.PromoPromotionDto dto)
        {
            if (dto == null)
                return null;

            return new PromoPromotion()
            {
                PromotionTypeConfigurationPerPromotionID = dto.PromotionTypeConfigurationPerPromotionID,
                PromotionID = dto.PromotionID,
                StartDate = dto.StartDate ?? DateTime.Now,
                EndDate = dto.EndDate ?? DateTime.Now,
                Description = dto.Description,
                PromotionStatusTypeID = dto.PromotionStatusTypeID,
                SuccessorPromotionID = dto.SuccessorPromotionID,
                PromotionKind = dto.PromotionKind,
                Status = dto.Status,
                Cumulative = dto.Cumulative,
                ConditionProductPriceTypeId = dto.ConditionProductPriceTypeId,
                RewardProductPriceTypeId = dto.RewardProductPriceTypeId
            };
        }

        /// <summary>
        /// Transforms bo jecto to dto object
        /// </summary>
        /// <param name="bo">Promotion BO</param>
        /// <returns>Promotion Dto</returns>
        private NetSteps.Data.Entities.Dto.PromoPromotionDto BoToDto(PromoPromotion bo)
        {
            if (bo == null)
                return null;

            return new NetSteps.Data.Entities.Dto.PromoPromotionDto()
            {
                PromotionTypeConfigurationPerPromotionID = bo.PromotionTypeConfigurationPerPromotionID,
                PromotionID = bo.PromotionID,
                StartDate = bo.StartDate,
                EndDate = bo.EndDate,
                Description = bo.Description,
                PromotionStatusTypeID = bo.PromotionStatusTypeID,
                SuccessorPromotionID = bo.SuccessorPromotionID,
                PromotionKind = bo.PromotionKind,
                Status = bo.Status
            };
        }

        #endregion

        #region Singleton

        private PromoPromotionLogic() { }

        public static PromoPromotionLogic Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PromoPromotionLogic();
                    repository = new PromoPromotionRepository();
                }

                return instance;
            }
        }

        #endregion

        #region "Methods"

        #region HasACombinationOfItems

        public void InsOrCondition(int promotionID)
        {
            repository.InsOrCondition(promotionID);
        }

        public bool ExistsOrCondition(int promotionID)
        {
            return repository.ExistsOrCondition(promotionID);
        }

        #endregion

        #region HasADefinedQVTotal

        public void InsAndConditionQVTotal(int promotionID, decimal QvMin, decimal QvMax)
        {
            repository.InsAndConditionQVTotal(promotionID, QvMin, QvMax);
        }

        public Dictionary<bool, Dictionary<decimal, decimal>> ExistsAndConditionQVTotal(int promotionID)
        {
            return repository.ExistsAndConditionQVTotal(promotionID);
        }

        #endregion

        public PromoPromotion GetById(int promotionId)
        {
            return DtoToBO(repository.GetByID(promotionId));
        }

        public List<PromoPromotion> ListPromotions(string Descripcion)
        {
            var data = repository.ListPromotions(Descripcion);
            return (from r in data select DtoToBO(r)).ToList();
        }

        public List<PromoPromotion> ListPromotionsByPromotionTypeConfigurationPerPromotions()
        {
            var data = repository.ListPromotionsByPromotionTypeConfigurationPerPromotions();
            return (from r in data select DtoToBO(r)).ToList();
        }

        public void InsertPromotionRewardEffectApplyOrderItemPropertyValues(int promotionID, int productPriceTypeID)
        {
            repository.InsertPromotionRewardEffectApplyOrderItemPropertyValues(promotionID, productPriceTypeID);
        }

        public void UpdatePromotionRewardEffectApplyOrderItemPropertyValues(int promotionID, int productPriceTypeID)
        {
            repository.UpdatePromotionRewardEffectApplyOrderItemPropertyValues(promotionID, productPriceTypeID);
        }

        /// <summary>
        /// Update cumulative
        /// </summary>
        /// <param name="promotionQualificationId"></param>
        /// <param name="cumulative"></param>
        public void UpdatePromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmounts(int promotionQualificationId, bool cumulative)
        {
            repository.UpdatePromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmounts(promotionQualificationId, cumulative);
        }

        #region promotion type 1-2

        /// <summary>
        /// Promotion tipo 1
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public Order ApplyPromotionType1(Order order, out int promotionId, out bool configurationCumulative)
        {
            promotionId = 0;
            configurationCumulative = false;

            decimal totalAmountInOrder = order.OrderCustomers.SelectMany(m => m.OrderItems.Where(t => t.ProductPriceTypeID == (int)ConstantsGenerated.ProductPriceType.Retail).Select(k => k.ItemPrice * k.Quantity)).Sum();
            var promotion = PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountBusinessLogic.Instance.GetByAmount(totalAmountInOrder);
            if (promotion != null)
            {
                var percentagePromotion = PromotionRewardEffectApplyOrderItemPropertyValueBusinessLogic.Instance.GetByPromotion(promotion.PromotionID);

                if (percentagePromotion != null)
                {

                    foreach (var orderCustomer in order.OrderCustomers)
                    {
                        foreach (var orderItem in orderCustomer.OrderItems)
                        {
                            foreach (var adjustment in orderItem.OrderAdjustmentOrderLineModifications)
                            {
                                if (adjustment.OrderAdjustment.Extension is NetSteps.Promotions.Common.ModelConcrete.PromotionOrderAdjustment)
                                {
                                    //TODO:modificar solo la promocion a aplicar?
                                    //Siempre se esta aplicando solo nuestra promocion, a menos que no entre en la condicional
                                    //este valor no es necesario, dado que se vuelve a calcular con el promotionID, seleccionado.
                                    //si se va a usar, quitar la sobrecarga en OrderItem.cs->GetAdjustedPrice(int priceTypeID)
                                    //->total -= mod.CalculatedValue(total);
                                    adjustment.ModificationDecimalValue = percentagePromotion.DecimalValue;

                                    ((NetSteps.Promotions.Common.ModelConcrete.PromotionOrderAdjustment)adjustment.OrderAdjustment.Extension).PromotionID = promotion.PromotionID;

                                }
                            }
                        }
                    }
                }

                promotionId = promotion.PromotionID;
                configurationCumulative = promotion.Cumulative ?? false;
            }

            return order;
        }

        /// <summary>
        /// Promotion tipo 2
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public Order ApplyPromotionType2(Order order, out int promotionId, out bool configurationCumulative)
        {
            promotionId = 0;
            configurationCumulative = false;
            decimal totalAmountInOrder = order.OrderCustomers.SelectMany(m => m.OrderItems.Where(t => t.ProductPriceTypeID == (int)ConstantsGenerated.ProductPriceType.Retail).Select(k => k.ItemPrice * k.Quantity)).Sum();
            int currentPeriodId = PeriodBusinessLogic.Instance.GetOpenPeriodID();
            IEnumerable<Order> ordersInPeriod = OrderBusinessLogic.Instance.GetOrdersInPeriod(currentPeriodId);
            if (ordersInPeriod.Count() >= 1)
            {
                var promotionConfigurationControl = PromotionConfigurationControlBusinessLogic.Instance.GetByAccount(order.OrderCustomers[0].AccountID, currentPeriodId);
                decimal amountBefore = promotionConfigurationControl == null ? 0m : promotionConfigurationControl.Amount;
                var promotion = PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountBusinessLogic.Instance.GetByAmount(totalAmountInOrder + amountBefore);
                if (promotion != null)
                {
                    var percentagePromotion = PromotionRewardEffectApplyOrderItemPropertyValueBusinessLogic.Instance.GetByPromotion(promotion.PromotionID);
                    if (percentagePromotion != null)
                    {
                        foreach (var orderCustomer in order.OrderCustomers)
                        {
                            foreach (var orderItem in orderCustomer.OrderItems)
                            {
                                foreach (var adjustment in orderItem.OrderAdjustmentOrderLineModifications)
                                {
                                    if (adjustment.OrderAdjustment.Extension is NetSteps.Promotions.Common.ModelConcrete.PromotionOrderAdjustment)
                                    {
                                        //TODO:modificar solo la promocion a aplicar?
                                        //Siempre se esta aplicando solo nuestra promocion, a menos que no entre en la condicional
                                        //este valor no es necesario, dado que se vuelve a calcular con el promotionID, seleccionado.
                                        //si se va a usar, quitar la sobrecarga en OrderItem.cs->GetAdjustedPrice(int priceTypeID)
                                        //->total -= mod.CalculatedValue(total);
                                        adjustment.ModificationDecimalValue = percentagePromotion.DecimalValue;

                                        ((NetSteps.Promotions.Common.ModelConcrete.PromotionOrderAdjustment)adjustment.OrderAdjustment.Extension).PromotionID = promotion.PromotionID;

                                    }
                                }
                            }
                        }
                    }

                    promotionId = promotion.PromotionID;
                    configurationCumulative = promotion.Cumulative ?? false;
                }
            }

            return order;
        }

        /// <summary>
        /// Actualiza la configuracion de la promocion, aplicar si es acumulativo
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="amount"></param>
        /// <param name="promotionId"></param>
        public void ApplyConfigurationCumulative(int accountId, decimal amount, int promotionId)
        {
            int currentPeriodId = PeriodBusinessLogic.Instance.GetOpenPeriodID();
            PromotionConfigurationControlBusinessLogic.Instance.UpdateAmount(accountId, currentPeriodId, amount, promotionId);
        }
        #endregion

        #region discount type 3
        public void ApplyPromotionType3()
        {
            int promotionTypeConfigurationId = PromoPromotionTypeConfigurationsLogic.Instance.GetActiveID();
            List<PromotionQualificationCustomerSubtotalRangeCurrencyAmount> promotions = PromotionQualificationCustomerSubtotalRangeCurrencyAmountBusinessLogic.Instance.GetByPromotionTypeConfiguration(promotionTypeConfigurationId).ToList();
            int currentPeriodId = PeriodBusinessLogic.Instance.GetOpenPeriodID();
            IEnumerable<Order> orders = OrderBusinessLogic.Instance.GetOrdersInPeriod(currentPeriodId);

            foreach (var item in orders)
            {
                if (item.TmpOrderCustomer == 0)
                {
                    var discount = promotions
                        .Where(m => m.MinimumAmount >= item.TmpRetailTotal && m.MaximumAmount < item.TmpRetailTotal)
                        .Select(d => d.Discount)
                        .FirstOrDefault();
                    item.TmpOrderDiscount = discount;
                }
                else
                {
                    item.TmpOrderDiscount = 0;
                }
            }

            decimal ordersTotal = orders.Select(m => m.TmpRetailTotal).Sum();
            //variable Discount
            PromotionQualificationCustomerSubtotalRangeCurrencyAmount discountPromotion = promotions
                        .Where(m => m.MinimumAmount >= ordersTotal && m.MaximumAmount < ordersTotal)
                        .FirstOrDefault();

            bool ba = PromoPromotionTypeConfigurationsPerOrderLogic.Instance.GetBA(promotionTypeConfigurationId);
            if (ba)
            {
                InsertBonusValues(orders, discountPromotion.Discount, currentPeriodId);
            }
            else
            {
                InsertBonusValues(orders.Where(m => m.TmpOrderCustomer == 0), discountPromotion.Discount, currentPeriodId);
            }
        }

        private void InsertBonusValues(IEnumerable<Order> orders, decimal discount, int periodId)
        {
            foreach (var item in orders)
            {
                if (item.TmpOrderDiscount < discount)
                {
                    decimal compensation = item.TmpRetailTotal * discount - item.TmpRetailTotal * item.TmpOrderDiscount;
                    BonusValueBusinessLogic.Instance.Insert(new BonusValue()
                    {
                        BonusValueID = 0,
                        BonusTypeID = 14,
                        AccountID = item.TmpAccountID,
                        BonusAmount = compensation,
                        CurrencyTypeID = 36,
                        CorpBonusAmount = compensation,
                        CorpCurrencyTypeID = 36,
                        PeriodID = periodId,
                        CountryID = 73,
                        DateModified = DateTime.Now
                    });
                }
            }
        }
        #endregion

        #region Discount type 4
        public Order ApplyPromotionType4(Order order, out int promotionId, out bool configurationCumulative)
        {
            promotionId = 0;
            configurationCumulative = false;
            int promotionTypeConfigurationId = PromoPromotionTypeConfigurationsLogic.Instance.GetActiveID();
            if (promotionTypeConfigurationId == 4) //TODO: Cambiar en Order service tipo1 a default
            {
                int currentPeriodId = PeriodBusinessLogic.Instance.GetOpenPeriodID();
                var configuration = PromotionConfigurationControlBusinessLogic.Instance.GetByAccount(order.OrderCustomers[0].AccountID, currentPeriodId);
                if (configuration != null)
                {
                    bool isAssociated = PromoPromotionTypeConfigurationPerPromotionLogic.Instance.IsAssociated(configuration.PromotionID, promotionTypeConfigurationId);
                    if (isAssociated)
                    {
                        var promotionPercentageOld = PromotionRewardEffectApplyOrderItemPropertyValueBusinessLogic.Instance.GetByPromotion(configuration.PromotionID);
                        decimal discountOld = promotionPercentageOld != null ? promotionPercentageOld.DecimalValue : 0m;

                        decimal totalAmountInCurrentOrder = order.OrderCustomers.SelectMany(m => m.OrderItems.Where(t => t.ProductPriceTypeID == (int)ConstantsGenerated.ProductPriceType.Retail).Select(k => k.ItemPrice * k.Quantity)).Sum();
                        var promotionAvailableNew = PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountBusinessLogic.Instance.GetByAmount(totalAmountInCurrentOrder);
                        var promotionPercentageNew = PromotionRewardEffectApplyOrderItemPropertyValueBusinessLogic.Instance.GetByPromotion(promotionAvailableNew.PromotionID);
                        decimal discountNew = promotionPercentageNew != null ? promotionPercentageNew.DecimalValue : 0m;

                        decimal discountToApply = 0m;
                        if (discountOld > discountNew)
                        {
                            discountToApply = discountOld;
                            promotionId = configuration.PromotionID;
                        }
                        else
                        {
                            discountToApply = discountNew;
                            promotionId = promotionAvailableNew.PromotionID;
                        }

                        foreach (var orderCustomer in order.OrderCustomers)
                        {
                            foreach (var orderItem in orderCustomer.OrderItems)
                            {
                                foreach (var adjustment in orderItem.OrderAdjustmentOrderLineModifications)
                                {
                                    if (adjustment.OrderAdjustment.Extension is NetSteps.Promotions.Common.ModelConcrete.PromotionOrderAdjustment)
                                    {
                                        //TODO:modificar solo la promocion a aplicar?
                                        //Siempre se esta aplicando solo nuestra promocion, a menos que no entre en la condicional
                                        //este valor no es necesario, dado que se vuelve a calcular con el promotionID, seleccionado.
                                        //si se va a usar, quitar la sobrecarga en OrderItem.cs->GetAdjustedPrice(int priceTypeID)
                                        //->total -= mod.CalculatedValue(total);
                                        adjustment.ModificationDecimalValue = discountToApply;

                                        ((NetSteps.Promotions.Common.ModelConcrete.PromotionOrderAdjustment)adjustment.OrderAdjustment.Extension).PromotionID = promotionId;

                                    }
                                }
                            }
                        }

                        configurationCumulative = false;
                    }
                }
            }
            else
            {
                ///: TODO: Original - restablecer precios
                return ApplyPromotionType1(order, out promotionId, out configurationCumulative);
            }

            return order;
        }
        #endregion

        #endregion
    }
}
