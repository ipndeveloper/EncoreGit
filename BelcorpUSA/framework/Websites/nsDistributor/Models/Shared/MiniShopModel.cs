using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Dynamic;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Extensions;
using System.Web.Mvc;
using System;
using nsDistributor.Areas.Enroll.Models.Products;
using nsDistributor.Areas.Enroll.Models.Shared;
using NetSteps.Common.Globalization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace nsDistributor.Models.Shared
{
    public class MiniShopModel
    {
        public virtual string GetProductsUrl { get; set; }
        public virtual string AddOrderItemUrl { get; set; }
        public virtual string RemoveOrderItemUrl { get; set; }
        public virtual string UpdateOrderItemsUrl { get; set; }
        public virtual string AddOrderItemLabel { get; set; }

        public virtual string Subtotal { get; set; }
        public virtual string Volume { get; set; }
        public virtual string MinimumVolume { get; set; }
        public virtual bool HasMetMinimumVolume { get; set; }
        public virtual IEnumerable<MiniShopCategoryModel> Categories { get; set; }
        public virtual IEnumerable<MiniShopProductModel> Products { get; set; }
        //public virtual IEnumerable<MiniShopOrderItemModel> OrderItems { get; set; }
        public virtual IList<IOrderItemModel> OrderItems { get; set; }
        public virtual int? SelectedShippingMethodID { get; set; }
        public virtual MiniShopShippingMethodModel ShippingMethods { get; set; }

        public virtual TotalsOrderItemModel TotalsOrderItem { get; set; }
        public virtual decimal TotalQV { get; set; }
        public virtual decimal TotalCV { get; set; }
        public virtual decimal TotalSubTotal { get; set; }
        public virtual decimal TotalPrice { get; set; }
        public virtual Order Order { get; set; }

        [NSDisplayName("MyPersonalWebsite", "My Personal Website")]
        [NSRegularExpression(@"^[a-zA-Z0-9]+(-[a-zA-Z0-9]+)*$")]
        public virtual string Subdomain { get; set; }

        public virtual int NumberOfItemsInCart
        {
            get
            {
                if (OrderItems != null)
                {
                    return OrderItems.Sum(o => o.Quantity);
                }
                return 0;
            }
        }

        public virtual bool ShowVolume
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.MinimumVolume);
            }
        }

        #region Agregados

        public virtual string SubtotalAdjusted { get; set; }
        public virtual string TotalQV_Sum { get; set; }
        public virtual string OriginalCommissionableTotal_Sum { get; set; }
        public virtual string AdjustedCommissionableTotal_Sum { get; set; }
        public virtual string CountItems { get; set; }
        public virtual string OriginalTotal_Sum { get; set; }
        public virtual string AdjustedTotal_Sum { get; set; }
        public virtual string OriginalTotal_Texto { get; set; }
        public virtual string AdjustedTotal_Texto { get; set; }

        public virtual object ApplicablePromotions { get; set; }
        public virtual IEnumerable<object> FreeGiftModels { get; set; }

        //public virtual IOrderEntryModel IOrderModel { get; set; }
        public virtual IIndexModel IndexModel { get; set; }
        public virtual object DataJsonModel { get; set; }
        public virtual object OptionsJsonModel { get; set; }
        

        #endregion


        public virtual MiniShopModel LoadResources(
            dynamic modelData)
        {
            GetProductsUrl = modelData.GetProductsUrl;
            AddOrderItemUrl = modelData.AddOrderItemUrl;
            RemoveOrderItemUrl = modelData.RemoveOrderItemUrl;
            UpdateOrderItemsUrl = modelData.UpdateOrderItemsUrl;
            AddOrderItemLabel = modelData.AddOrderItemLabel;
            Subtotal = modelData.Subtotal;
            Volume = modelData.Volume;
            MinimumVolume = modelData.MinimumVolume;
            HasMetMinimumVolume = modelData.HasMetMinimumVolume;
            Categories = modelData.Categories;
            Products = modelData.Products;
            OrderItems = modelData.OrderItems;
            TotalsOrderItem = modelData.TotalsOrderItem;
            ShippingMethods = modelData.ShippingMethods;
            SelectedShippingMethodID = modelData.SelectedShippingMethodID;


            SubtotalAdjusted = modelData.SubtotalAdjusted;
            TotalQV_Sum = modelData.TotalQV_Sum;
            OriginalCommissionableTotal_Sum = modelData.OriginalCommissionableTotal_Sum;
            AdjustedCommissionableTotal_Sum = modelData.AdjustedCommissionableTotal_Sum;
            CountItems = modelData.CountItems;
            OriginalTotal_Sum = modelData.OriginalTotal_Sum;
            AdjustedTotal_Sum = modelData.AdjustedTotal_Sum;
            OriginalTotal_Texto = modelData.OriginalTotal_Texto;
            AdjustedTotal_Texto = modelData.AdjustedTotal_Texto;


            ApplicablePromotions = modelData.ApplicablePromotions;
            FreeGiftModels = modelData.FreeGiftModels;
            
            IndexModel = modelData.IndexModel;
            DataJsonModel = modelData.DataJsonModel;
            OptionsJsonModel = modelData.OptionsJsonModel; 
            return this;
        }

        /// <summary>
        /// Builds a <see cref="DynamicDictionary"/> of data to either be used to load a viewmodel or to return as JSON.
        /// All parameters are optional.
        /// </summary>
        public static dynamic GetModelData(
            Order order = null,
            decimal? minimumVolume = null,
            IEnumerable<MiniShopCategoryModel> categories = null,
            IEnumerable<MiniShopProductModel> products = null,
            //IEnumerable<MiniShopOrderItemModel> orderItems = null,
            IList<IOrderItemModel> orderItems = null,
            MiniShopShippingMethodModel shippingMethods = null,

            TotalsOrderItemModel totalsOrderItem = null, 
            #region agregados
                object applicablePromotions = null,
                IEnumerable<object> freeGiftModels = null,
            #endregion

            int? selectedShippingMethodID = null) 
        {
            dynamic modelData = new DynamicDictionary();

            if (order != null)
            {
                modelData.Subtotal = order.Subtotal.ToString(order.CurrencyID);
                modelData.Volume = order.CommissionableTotal.ToString(order.CurrencyID);

                //modelData.ApplicablePromotions = applicablePromotions;
                //modelData.FreeGiftModels = freeGiftModels;

                if (minimumVolume != null)
                {
                    modelData.MinimumVolume = minimumVolume.Value.ToString(order.CurrencyID);
                }
                modelData.HasMetMinimumVolume = (order.CommissionableTotal ?? 0) >= (minimumVolume ?? 0);

                
                #region Agregados
                if (orderItems != null)
                {
                    //SubtotalAdjusted = order.OrderCustomers[0].AdjustedSubTotal.ToString(order.CurrencyID);
                    /*CS.29JUN2016.Inicio.Quitar formato Decimal y símbolo moneda*/
                    int totalQV = Convert.ToInt32(orderItems.Sum(x => (x.TotalQV != null ? x.TotalQV : 0)));
                    modelData.TotalQV_Sum = totalQV.ToString();
                    /*CS.29JUN2016.Fin.Quitar formato Decimal y símbolo moneda*/
                    modelData.OriginalCommissionableTotal_Sum = orderItems.Sum(x => (x.OriginalCommissionableTotal.Length > 0 ? x.OriginalCommissionableTotal.Replace("$", "").FormatGlobalizationDecimal() : 0)).ToString(order.CurrencyID);
                    modelData.AdjustedCommissionableTotal_Sum = orderItems.Sum(x => (x.AdjustedCommissionableTotal.Length > 0 ? x.AdjustedCommissionableTotal.Replace("$", "").FormatGlobalizationDecimal() : 0)).ToString(order.CurrencyID);
                    modelData.OriginalTotal_Sum = orderItems.Sum(x => (x.OriginalTotal.Length > 0 ? x.OriginalTotal.Replace("$", "").FormatGlobalizationDecimal() : 0)).ToString(order.CurrencyID);
                    modelData.OriginalTotal_Texto = modelData.OriginalTotal_Sum + "(" + Translation.GetTerm("Subtotal", "Sub total") + ")";
                    modelData.AdjustedTotal_Sum = orderItems.Sum(x => (x.AdjustedTotal.Length > 0 ? x.AdjustedTotal.Replace("$", "").FormatGlobalizationDecimal() : 0)).ToString(order.CurrencyID);
                    modelData.AdjustedTotal_Texto = modelData.AdjustedTotal_Sum + "(" + Translation.GetTerm("Subtotaldco", "Sub total dco") + ")";
                    modelData.CountItems = orderItems.Sum(x => x.Quantity).ToString();
                }
                if (applicablePromotions != null)
                {
                    modelData.ApplicablePromotions = applicablePromotions;
                }

                if (freeGiftModels != null)
                {
                    modelData.FreeGiftModels = freeGiftModels;
                }

        #endregion

            }

            if (categories != null)
            {
                modelData.Categories = categories;
            }

            if (products != null)
            {
                modelData.Products = products;
            }

            if (orderItems != null)
            {
                modelData.OrderItems = orderItems;
            }

            if (orderItems != null)
            {
                totalsOrderItem = new TotalsOrderItemModel();
                //totalsOrderItem.TotalQV = Convert.ToDecimal(order.SubtotalRetail)
                //totalsOrderItem.TotalCV = Convert.ToDecimal(order.CommissionableTotal);
                //totalsOrderItem.TotalSubTotal = Convert.ToDecimal(order.Subtotal);
                //totalsOrderItem.TotalPrice =  Convert.ToDecimal(order.GrandTotal);
                modelData.TotalsOrderItem = totalsOrderItem;

                modelData.TotalQV = Convert.ToDecimal(order.SubtotalRetail);
                modelData.TotalCV = Convert.ToDecimal(order.CommissionableTotal);
                modelData.TotalSubTotal = Convert.ToDecimal(order.Subtotal);
                modelData.TotalPrice = Convert.ToDecimal(order.GrandTotal);
            }

            if (shippingMethods != null)
                modelData.ShippingMethods = shippingMethods;

            if (selectedShippingMethodID != null)
                modelData.SelectedShippingMethodID = selectedShippingMethodID;

            modelData.NumberOfItemsInCart = orderItems != null ? orderItems.Sum(o => o.Quantity) : 0;

            return modelData;
        }




    }
}