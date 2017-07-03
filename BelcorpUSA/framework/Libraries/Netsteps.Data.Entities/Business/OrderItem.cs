using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Data.Entities.Interfaces;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Tax;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Common.ModelConcrete;
using NetSteps.Promotions.Common;
using NetSteps.Data.Entities.Context;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities
{
    #region Modifications
    // @01  BR-AT-008 GYS EFP: Envío de Productos Por Reclamo
    #endregion

    public partial class OrderItem : ITaxInfo, ITempGuid, IOrderItem, IDateLastModified
    {
        #region Members

        private bool _isRemoveable = true;
        private OrderItemTaxes _taxes;

        #endregion

        #region Properties

        private bool _hasChanges = false;
        public bool HasChanges
        {
            get { return _hasChanges; }
            set { _hasChanges = value; }
        }


        // For use in UI's (not persisted to the DB) - JHE
        public bool IsEditable
        {
            get
            {
                return _isRemoveable;
            }
            set
            {
                _isRemoveable = value;
            }
        }

        public bool IsHostReward
        {
            get
            {
                var orderItemType = this.OrderItemType ?? SmallCollectionCache.Instance.OrderItemTypes.GetById(OrderItemTypeID);
                if (orderItemType != null)
                    return orderItemType.IsHostessReward;
                else
                    return false;
            }
        }

        public OrderItemTaxes Taxes
        {
            get
            {
                if (_taxes == null)
                {
                    _taxes = new OrderItemTaxes();
                    this.CopyTaxValuesFromOrderItem();
                    //Calculations.TaxService.CopyTaxValuesFromOrderItem(this);
                }

                return _taxes;
            }
            set
            {
                _taxes = value;
            }
        }

        public bool HasChildOrderItems
        {
            get
            {
                return (this.ChildOrderItems != null && this.ChildOrderItems.Count > 0);
            }
        }

        public decimal ReturnedPrice { get; set; }

        public bool WasBackordered
        {
            get
            {
                var prop = OrderItemProperties.FirstOrDefault(p => p.OrderItemPropertyTypeID == Constants.OrderItemPropertyType.WasBackordered.ToInt());
                bool ret = false;
                if (prop != null)
                {
                    bool.TryParse(prop.PropertyValue, out ret);
                }
                return ret;
            }
            set
            {
                var prop = OrderItemProperties.FirstOrDefault(p => p.OrderItemPropertyTypeID == Constants.OrderItemPropertyType.WasBackordered.ToInt());
                if (prop == null)
                {
                    prop = new OrderItemProperty { OrderItemPropertyTypeID = Constants.OrderItemPropertyType.WasBackordered.ToInt(), Active = true };
                    OrderItemProperties.Add(prop);
                }
                prop.PropertyValue = value.ToString();
            }
        }
        #endregion

        #region ITempGuid Members

        private Guid? _guid = null;
        public Guid Guid
        {
            get
            {
                if (_guid == null)
                    _guid = Guid.NewGuid();
                return _guid.Value;
            }
            internal set
            {
                _guid = value;
            }
        }

        #endregion

        void IOrderItem.MarkAsDeleted()
        {
            this.MarkAsDeleted();
        }

        IList<IOrderAdjustmentOrderLineModification> IOrderItem.OrderLineModifications
        {
            get
            {
                return this.OrderAdjustmentOrderLineModifications.Cast<IOrderAdjustmentOrderLineModification>().ToList();
            }
        }

        IOrderItem IOrderItem.ParentOrderItem
        {
            get { return ParentOrderItem; }
        }

        /// <summary>
        public virtual bool CanBeCombinedWith(OrderItem item)
        {
            Contract.Requires<ArgumentNullException>(item != null);

            if (item.ProductID != ProductID || item.HostessRewardRuleID != HostessRewardRuleID || IsDynamicKit || item.IsDynamicKit || item.ProductPriceTypeID != ProductPriceTypeID)
                return false;

            bool canCombine = true;
            if (this.OrderItemProperties.Any() && item.OrderItemProperties.Any())
            {
                var customizationType = Create.New<InventoryBaseRepository>().GetProduct(ProductID.Value).CustomizationType();
                canCombine = (from toip in this.OrderItemProperties
                              from ioip in item.OrderItemProperties
                              where toip.Name == customizationType && ioip.Name == customizationType
                              select String.Equals(toip.Value, ioip.Value, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            }
            else if (this.OrderItemProperties.Any() || item.OrderItemProperties.Any())
            {
                canCombine = false;
            }

            return canCombine;
        }

        /// Determines whether this instance [can combine with virtual kit] the specified item.
        /// For this to return true all kit items must match both ProductID and Quantity.
        /// This should be moved into a service call eventually to remove business logic from our data objects.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if this instance [can combine with virtual kit] the specified item; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool CanCombineWithVirtualKit(OrderItem item)
        {
            Contract.Requires<ArgumentNullException>(item != null);

            Func<OrderItem, OrderItem, bool> comparer = (item1, item2) =>
            {
                return (item1.ProductID == item2.ProductID) &&  // productid check
                    (item1.Quantity == item2.Quantity); // quantity check
            };

            if (ChildOrderItems.Except(item.ChildOrderItems, comparer).Any())
                return false;

            if (item.ChildOrderItems.Except(ChildOrderItems, comparer).Any())
                return false;

            return true;
        }

        /// <summary>
        /// Gets a value indicating whether this OrderItem is a dynamic kit.
        /// This should be moved into a service call eventually to remove business logic from our data objects.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is dynamic kit; otherwise, <c>false</c>.
        /// </value>
        private bool IsDynamicKit
        {
            get
            {
                if (Product == null)
                {
                    var inventoryRepository = Create.New<InventoryBaseRepository>();
                    var product = inventoryRepository.GetProduct(ProductID.Value);

                    return product.IsDynamicKit();
                }
                else
                {
                    return Product.IsDynamicKit();
                }
            }
        }

        public void AddOrderLineModification(IOrderAdjustmentOrderLineModification lineModification)
        {
            OrderAdjustmentOrderLineModifications.Add((OrderAdjustmentOrderLineModification)lineModification);
        }

        private bool _retailHasOverride;
        public bool RetailHasOverride
        {
            get
            {
                return _retailHasOverride;
            }
            set
            {
                _retailHasOverride = value;
            }
        }

        /// <summary>
        /// Calls GetAdjustedPrice(priceType) passing the default product price type for the customer account and order type.
        /// </summary>
        public decimal GetAdjustedPrice()
        {
            return GetAdjustedPrice(OrderCustomer.ProductPriceTypeID);
        }

        public decimal GetAdjustedPrice(int priceTypeID)
        {
            var overridePrice = GetOverriddenPrice(priceTypeID);
            if (overridePrice.HasValue)
            {
                return overridePrice.Value.GetRoundedNumber();
            }

            decimal total = GetPreAdjustmentPrice(priceTypeID);

            if (OrderAdjustmentOrderLineModifications != null && OrderAdjustmentOrderLineModifications.Count > 0)
            {
                InventoryBaseRepository invRep = Create.New<InventoryBaseRepository>();
                Product targetProduct = invRep.GetProduct(this.ProductID.GetValueOrDefault());
                bool isPromotionRestricted = targetProduct != null
                    && targetProduct.ProductBase != null
                    && targetProduct.ProductBase.ProductType != null
                    && targetProduct.ProductBase.ProductType.ProductPropertyTypes != null
                    && targetProduct.ProductBase.ProductType.ProductPropertyTypes.Any(ptype => ptype.Name.Equals("Restricted for promotion"));

                if (!isPromotionRestricted)
                {
                    foreach (var mod in OrderAdjustmentOrderLineModifications)
                    {
                        PromotionOrderAdjustment orderAdjustment;
                        bool isCartReward = false;
                        if (mod.OrderAdjustment.Extension == null)
                        {
                            Dictionary<int, string> promocion = new Dictionary<int, string>();
                            promocion = OrderExtensions.GetPromotionIDByOrderAdjustmentID(mod.OrderAdjustmentID);
                            if (promocion.Count > 0)
                            {
                                int PromotionID = Convert.ToInt16(promocion.Keys.ElementAt(0));
                                var promo = Create.New<IPromotionService>().GetPromotion(PromotionID);
                                   if (promo != null)
                                    isCartReward = promo.PromotionKind.Equals("Default Cart Rewards") && promo.PromotionRewards.First().Key.Equals("Reduced Subtotal");
                            }
                        }
                        else
                        if (mod.OrderAdjustment.Extension is PromotionOrderAdjustment)
                        {
                            orderAdjustment = (PromotionOrderAdjustment)mod.OrderAdjustment.Extension;
                            var promo = Create.New<IPromotionService>().GetPromotion(orderAdjustment.PromotionID);
                            isCartReward = promo.PromotionKind.Equals("Default Cart Rewards") && promo.PromotionRewards.First().Key.Equals("Reduced Subtotal");
                        }

                        var priceTypeService = Create.New<IPriceTypeService>();
                        var priceType = priceTypeService.GetPriceType(priceTypeID);

                        //INI-FIN - Encore_16 - Se añade mod.PropertyName == "Retail" para incluir lógica de USA - CDAS
                        if (mod.PropertyName == priceType.Name && (mod.PropertyName == "Wholesale" || mod.PropertyName == "Retail" || !isCartReward))
                        {
                            int countryId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EnvironmentCountry"]);
                            //INI-FIN - Encore_16 - Se añade condición para ejecutar solo si es BRA, sino ejecutará de la forma que lo hace USA - CDAS
                            if (isCartReward & mod.PropertyName == "Wholesale" && countryId == (int)Constants.Country.Brazil)
                            {
                                decimal descuentoRegular = 0;
                                decimal descuentoEspecial = 0;

                                switch (mod.OrderAdjustment.Description)
                                {
                                    case "Faixa 35% Desconto":
                                        descuentoRegular = (Decimal)0.0714;
                                        descuentoEspecial = (Decimal)0.1875;
                                        break;
                                    case "Faixa 40% Desconto":
                                        descuentoRegular = (Decimal)0.143;
                                        descuentoEspecial = (Decimal)0.25;
                                        break;
                                    case "Faixa 45% Desconto":
                                        descuentoRegular = (Decimal)0.2143;
                                        descuentoEspecial = (Decimal)0.3125;
                                        break;
                                }
                                if (targetProduct.ProductBase.ProductType.Name == "Regular" | targetProduct.ProductBase.ProductType.ProductTypeID == 105)
                                    total -= (total * descuentoRegular);
                                else
                                    if (targetProduct.ProductBase.ProductType.Name == "Desconto Especial" | targetProduct.ProductBase.ProductType.ProductTypeID == 102)
                                        total -= (total * descuentoEspecial);
                            }
                            else
                                total -= mod.CalculatedValue(total);
                        }
                    }
                }
            }

            return (Math.Max(total, 0)).GetRoundedNumber();
        }


        //public decimal GetAdjustedPrice(int priceTypeID)
        //{
        //    var overridePrice = GetOverriddenPrice(priceTypeID);
        //    if (overridePrice.HasValue)
        //    {
        //        return overridePrice.Value.GetRoundedNumber();
        //    }

        //    decimal total = GetPreAdjustmentPrice(priceTypeID);

        //    if (OrderAdjustmentOrderLineModifications != null && OrderAdjustmentOrderLineModifications.Count > 0)
        //    {
        //        InventoryBaseRepository invRep = Create.New<InventoryBaseRepository>();
        //        Product targetProduct = invRep.GetProduct(this.ProductID.GetValueOrDefault());
        //        bool isPromotionRestricted = targetProduct != null
        //            && targetProduct.ProductBase != null
        //            && targetProduct.ProductBase.ProductType != null
        //            && targetProduct.ProductBase.ProductType.ProductPropertyTypes != null
        //            && targetProduct.ProductBase.ProductType.ProductPropertyTypes.Any(ptype => ptype.Name.Equals("Restricted for promotion"));
                 
        //        if (!isPromotionRestricted)
        //        {
        //            foreach (var mod in OrderAdjustmentOrderLineModifications)
        //            {
        //                PromotionOrderAdjustment orderAdjustment;
        //                bool isCartReward = false;
        //                int promotionRewardProductPriceTypeId = 0;
        //                decimal promotionDiscount = 0m;
        //                if (mod.OrderAdjustment.Extension is PromotionOrderAdjustment)
        //                {
        //                    orderAdjustment = (PromotionOrderAdjustment)mod.OrderAdjustment.Extension;
        //                    var promo = Create.New<IPromotionService>().GetPromotion(orderAdjustment.PromotionID);

        //                    //var promotionModified = NetSteps.Data.Entities.Business.Logic.PromotionRewardEffectApplyOrderItemPropertyValueBusinessLogic.Instance.GetByPromotion(orderAdjustment.PromotionID);
        //                    //promotionRewardProductPriceTypeId = promotionModified != null ? promotionModified.ProductPriceTypeID : 0;
        //                    //promotionDiscount = promotionModified != null ? promotionModified.DecimalValue : 0;

        //                    isCartReward = promo.PromotionKind.Equals("Default Cart Rewards") && promo.PromotionRewards.First().Key.Equals("Reduced Subtotal");
        //                }

        //                var priceTypeService = Create.New<IPriceTypeService>();
        //                var priceType = priceTypeService.GetPriceType(priceTypeID);

        //                //Si el tipo de precio es el del reward configurado, calcular descuento relacionado a esa promocion
        //                //if (promotionRewardProductPriceTypeId > 0
        //                //    && mod.PropertyName == priceType.Name)
        //                if (mod.PropertyName == priceType.Name && (mod.PropertyName == "Retail" || !isCartReward))
        //                    //&& (promotionRewardProductPriceTypeId == priceTypeID))
        //                {
        //                    total -= mod.CalculatedValue(total);
        //                }
        //                //else if (mod.PropertyName == priceType.Name && (mod.PropertyName == "Retail" || !isCartReward))//Configuracion por defecto, el % de descuento fue modificado en OrderService->PromotionLogic-Solo si encontro una promocion que cumpla las condiciones
        //                //{
        //                //    //else if (
        //                //    //         mod.PropertyName == priceType.Name &&
        //                //    //         (mod.PropertyName == ProductPriceType.Load(OrderCustomer.ProductPriceTypeID).Name || !isCartReward)
        //                //    //        )//only test
        //                //    //{
        //                //    total -= mod.CalculatedValue(total);
        //                //}
        //            }
        //        }
        //    }

        //    return (Math.Max(total, 0)).GetRoundedNumber();
        //}

        /// <summary>
        /// Calls GetPreAdjustmentPrice(priceType) passing the default product price type for the customer account and order type.
        /// </summary>
        public decimal GetPreAdjustmentPrice()
        {
            return GetPreAdjustmentPrice(OrderCustomer.ProductPriceTypeID);
        }

        public decimal GetPreAdjustmentPrice(int priceTypeID)
        {
            var overridePrice = GetOverriddenPrice(priceTypeID);
            if (overridePrice.HasValue)
                return overridePrice.Value.GetRoundedNumber();

            var priceTypeService = Create.New<IPriceTypeService>();
            var defaultPriceTypeID = GetDefaultPriceType(Constants.PriceRelationshipType.Products).PriceTypeID;
            var originalDefaultCurrencyPrice = GetOriginalPrice(defaultPriceTypeID);

            if (priceTypeID == defaultPriceTypeID)
            {
                // priceTypeID is the default currency price type
                var total = originalDefaultCurrencyPrice;
                if (DiscountPercent.HasValue && DiscountPercent > 0)
                    total *= (1 - DiscountPercent.Value);
                else if (Discount.HasValue && Quantity > 0)
                    total -= Discount.Value / Quantity;

                decimal truncatedTotal = ((int)(total * 100)) / 100.0m;
                return truncatedTotal;
            }

            var originalPrice = GetOriginalPrice(priceTypeID);
            if (priceTypeService.GetCurrencyPriceTypes().Any(x => x.PriceTypeID == priceTypeID))
            {
                // priceTypeID is a currency price type
                // we have to calculate the modification percentage
                var modifiedDefaultCurrencyPriceTypePrice = GetPreAdjustmentPrice(defaultPriceTypeID);
                var multiplier = originalDefaultCurrencyPrice > 0 ? modifiedDefaultCurrencyPriceTypePrice / originalDefaultCurrencyPrice : 0;

                return (originalPrice * multiplier).GetRoundedNumber();
            }
            else
            {
                //currently no clients have commissionable values for hostess rewards, so we are returning 0 here
                if (this.IsHostReward)
                    return 0;
                // priceTypeID is a commissions price type - in all cases we're dividing the original modifications to come up with a value we can adjust,
                // as we have legacy stuff and we don't want to mess with those.
                // we have to calculate the modification percentage

                var modifiedCurrencyPriceTypePrice = GetPreAdjustmentPrice(defaultPriceTypeID);
                decimal multiplier = originalDefaultCurrencyPrice > 0 ? modifiedCurrencyPriceTypePrice / originalDefaultCurrencyPrice : 0;
                var commissionAmount = originalPrice * multiplier;

                return Math.Max(commissionAmount, 0).GetRoundedNumber();
            }
        }

        #region Pricing Helpers

        /// <summary>
        /// Returns the original price for the default price type.
        /// </summary>
        /// <returns></returns>
        public decimal GetOriginalPrice()
        {
            return GetOriginalPrice(GetDefaultPriceType(ConstantsGenerated.PriceRelationshipType.Products).PriceTypeID);
        }

        /// <summary>
        /// Returns the original price for the given price type.
        /// </summary>
        /// <param name="priceTypeID"></param>
        /// <returns></returns>
        public decimal GetOriginalPrice(int priceTypeID)
        {
            if (priceTypeID == 0)
            {
                priceTypeID = GetDefaultPriceType(ConstantsGenerated.PriceRelationshipType.Products).PriceTypeID;
            }

            decimal? price = null;
            var orderItemPrice = OrderItemPrices.FirstOrDefault(p => p.ProductPriceTypeID == priceTypeID);
            if (orderItemPrice != null)
                price = orderItemPrice.OriginalUnitPrice;

            if (!price.HasValue)
            {
                Product targetProduct = Product;
                if (targetProduct == null)
                {
                    var inventoryRepository = Create.New<InventoryBaseRepository>();
                    targetProduct = inventoryRepository.GetProduct(ProductID.Value);
                }

                price = targetProduct.GetPriceByPriceType(priceTypeID, OrderCustomer.Order.CurrencyID);
            }

            return price ?? 0;
        }

        public decimal? GetOverriddenPrice(int priceTypeId)
        {
            var priceTypeService = Create.New<IPriceTypeService>();

            if (priceTypeService.GetCurrencyPriceTypes().Any(x => x.PriceTypeID == priceTypeId))
            {
                // if ItemPriceActual is null, there is NO OVERRIDE.
                if (!ItemPriceActual.HasValue)
                    return null;

                // priceType is a currency type
                if (priceTypeId == ProductPriceTypeID.Value)
                {
                    return ItemPriceActual;
                }
                else
                {
                    // the price type is NOT the price type of the order item (or the default price type if there is no product price type)
                    // so the overridden price is a fraction of the original price.
                    Product targetProduct = Product;
                    if (targetProduct == null)
                    {
                        var inventoryRepository = Create.New<InventoryBaseRepository>();
                        targetProduct = inventoryRepository.GetProduct(ProductID.Value);
                    }
                    var originalPrimaryPriceTypePrice = targetProduct.GetPriceByPriceType(ProductPriceTypeID.Value, OrderCustomer.Order.CurrencyID);
                    var multiplier = originalPrimaryPriceTypePrice > 0 ? ItemPriceActual.Value / originalPrimaryPriceTypePrice : 0;

                    return multiplier * targetProduct.GetPriceByPriceType(priceTypeId, OrderCustomer.Order.CurrencyID).GetRoundedNumber();
                }
            }

            if (priceTypeService.GetVolumePriceTypes().Any(x => x.PriceTypeID == priceTypeId))
            {
                // priceType is a commission type

                if (!CommissionableTotalOverride.HasValue)
                {
                    return null;
                }
                var defaultVolumeType = GetDefaultPriceType(Constants.PriceRelationshipType.Commissions);
                if (defaultVolumeType.PriceTypeID == priceTypeId)
                {
                    if (Quantity == 0)
                        return (CommissionableTotalOverride.Value).GetRoundedNumber();
                    else
                        return (CommissionableTotalOverride.Value / Quantity).GetRoundedNumber();
                }
                else
                {
                    Product targetProduct = Product;
                    if (targetProduct == null)
                    {
                        var inventoryRepository = Create.New<InventoryBaseRepository>();
                        targetProduct = inventoryRepository.GetProduct(ProductID.Value);
                    }

                    var defaultVolumeTypeOriginalPrice = targetProduct.GetPriceByPriceType(
                        defaultVolumeType.PriceTypeID, OrderCustomer.Order.CurrencyID);

                    var percentage = Quantity > 0 && defaultVolumeTypeOriginalPrice != 0
                        ? CommissionableTotalOverride.Value / Quantity / defaultVolumeTypeOriginalPrice : 0m;

                    return (percentage * targetProduct.GetPriceByPriceType(priceTypeId, OrderCustomer.Order.CurrencyID)).GetRoundedNumber();
                }
            }

            // for any other cases
            switch ((Constants.ProductPriceType)priceTypeId)
            {
                case Constants.ProductPriceType.ShippingFee:
                    return ShippingTotalOverride;
                case Constants.ProductPriceType.HandlingFee:
                    return HandlingTotal;
            }
            throw new Exception(String.Format("Unknown price type {0}", priceTypeId));
        }

        private bool PriceTypeIsDefault(ConstantsGenerated.PriceRelationshipType priceRelationshipType, int priceTypeID)
        {
            return GetDefaultPriceType(priceRelationshipType).PriceTypeID == priceTypeID;
        }

        public IPriceType GetDefaultPriceType(ConstantsGenerated.PriceRelationshipType priceRelationshipType)
        {
            var priceTypeService = Create.New<IPriceTypeService>();
            var found = priceTypeService.GetPriceType(OrderCustomer.AccountTypeID, (int)priceRelationshipType, ApplicationContext.Instance.StoreFrontID, OrderCustomer.Order.OrderTypeID);

            return found;
        }

        public void SetItemPrice(int priceTypeID)
        {
            var orderItemPrice = OrderItemPrices.FirstOrDefault(q => q.ProductPriceTypeID == priceTypeID);
            if (orderItemPrice == null)
            {
                priceTypeID = (int)Constants.ProductPriceType.Retail;
                orderItemPrice = OrderItemPrices.FirstOrDefault(q => q.ProductPriceTypeID == priceTypeID);
            }

            ProductPriceTypeID = priceTypeID;
            ItemPrice = orderItemPrice == null ? 0 : orderItemPrice.OriginalUnitPrice ?? 0;
            if (ItemPriceActual.HasValue && ItemPriceActual.Value == ItemPrice)
            {
                ItemPriceActual = null;
            }
        }

        #endregion

        public void AddOrUpdateOrderItemProperty(string name, string value)
        {
            Contract.Requires<ArgumentNullException>(name != null);
            Contract.Requires<ArgumentException>(name.Length > 0);
            Contract.Requires<ArgumentNullException>(value != null);

            // Match the given name to a property type
            var propertyTypeId = SmallCollectionCache.Instance.OrderItemPropertyTypes
                .Where(x => x.Name.EqualsIgnoreCase(name))
                .Select(x => x.OrderItemPropertyTypeID)
                .FirstOrDefault();

            if (propertyTypeId == 0)
            {
                throw new NetStepsDataException("Unable to locate an OrderItemPropertyType for Name: " + name);
            }

            // Check if this is a "fixed value" property type
            var propertyValueRepository = Create.New<IOrderItemPropertyValueRepository>();
            var propertyValues = propertyValueRepository
                .WhereSelect(
                    x => x.OrderItemPropertyTypeID == propertyTypeId,
                    x => new
                    {
                        x.OrderItemPropertyValueID,
                        x.Value
                    }
                );
            int? propertyValueId = null;
            if (propertyValues.Any())
            {
                var propertyValue = propertyValues
                    .FirstOrDefault(x => x.Value.EqualsIgnoreCase(value));
                if (propertyValue == null)
                {
                    throw new NetStepsDataException(String.Format("Properties with Name \"{0}\" appear to have a list of valid values, but no OrderItemPropertyValue for Value \"{1}\" exists.", name, value));
                }
                propertyValueId = propertyValue.OrderItemPropertyValueID;
            }

            // Get existing property or create a new one
            var property = OrderItemProperties
                .FirstOrDefault(x => x.OrderItemPropertyTypeID == propertyTypeId);
            if (property == null)
            {
                property = new OrderItemProperty
                {
                    OrderItemPropertyTypeID = propertyTypeId,
                    Active = true
                };
                OrderItemProperties.Add(property);
            }

            // Set the value
            if (propertyValueId.HasValue)
            {
                property.OrderItemPropertyValueID = propertyValueId.Value;
                property.PropertyValue = null;
            }
            else
            {
                property.OrderItemPropertyValueID = null;
                property.PropertyValue = value;
            }
        }

        IList<IOrderItemProperty> IOrderItem.OrderItemProperties
        {
            get
            {
                if (this.OrderItemProperties != null)
                {
                    return this.OrderItemProperties.AsEnumerable<IOrderItemProperty>().ToList();
                }

                return null;
            }
        }

        int IOrderItem.ProductPriceTypeID
        {
            get
            {
                return ProductPriceTypeID.Value;
            }
            set
            {
                ProductPriceTypeID = value;
            }
        }

        #region Modifications @01

        public int GetSupportTicketID(string orderNumber)
        {
            return new OrderItemRepository().GetSupportTicketID(orderNumber);
        }
        public List<ClaimedOrderItem> LoadItemsToClaim(string orderNumber)
        {
            return new OrderItemRepository().LoadItemsToClaim(orderNumber);
        }
        public bool ClaimOrderItems(Dictionary<int, int> listToClaim, string orderNumber, string ticketSupport)
        {
            return new OrderItemRepository().ClaimOrderItems(listToClaim, orderNumber, ticketSupport);
        }

        #endregion

    }
}
