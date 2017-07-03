using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Extensions;
using System;
using System.Linq;
using NetSteps.Web.Mvc.Controls.Models.Enrollment;
using NetSteps.Enrollment.Common.Models.Context;
using NetSteps.Encore.Core.IoC;
using NetSteps.Enrollment.Common.Provider;
using System.Collections.Generic;

namespace nsDistributor.Models.Shared
{
    public class MiniShopOrderItemModel
    {
        public virtual string Guid { get; set; }
        public virtual int ProductID { get; set; }
        public virtual string SKU { get; set; }
        public virtual string ProductName { get; set; }
        public virtual string AdjustedUnitPrice { get; set; }
        public virtual string OriginalUnitPrice { get; set; }
        public virtual bool SamePrice { get; set; }
        public virtual int Quantity { get; set; }
        public virtual string OriginalCommissionableTotal { get; set; }
        public virtual string AdjustedCommissionableTotal { get; set; }
        public virtual bool SameCommissionableTotal { get; set; }
        public virtual string AdjustedTotal { get; set; }
        public virtual string OriginalTotal { get; set; }
        public virtual bool SameTotal { get; set; }
        public virtual bool IsStaticKit { get; set; }
        public virtual bool IsDynamicKit { get; set; }
        public virtual bool IsDynamicKitFull { get; set; }
        public virtual bool IsHostReward { get; set; }
        public virtual string BundlePackItemsUrl { get; set; }
        public virtual IKitItemsModel KitItemsModel { get; set; }
        public virtual ICollection<string> Messages { get; set; }
        public virtual string retailPricePerItem { get; set; } //recently added

        // Calculated Properties
        public virtual bool IsRemovable { get; set; }
        public virtual bool IsKit { get; set; }
        public virtual bool IsQuantityEditable { get; set; }

        public virtual string CustomItemText { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this OrderItem has custom text.
        /// </summary>
        public virtual bool HasCustomText { get; set; }

        public virtual decimal TotalQV { get; set; } //CGI(CMR)-29/10/2014
        public virtual string TotalQV_Currency { get; set; } //CGI(CMR)-29/10/2014

        //public virtual string SKU { get; set; }  //Developed by Wesley Campos S. - CSTI
        //public virtual int ProductID { get; set; }
        //public virtual string Name { get; set; }
        //public virtual decimal Price { get; set; }
        //public virtual decimal PriceReal { get; set; }
        //public virtual int Quantity { get; set; }
        //public virtual decimal PriceQV { get; set; }
        //public virtual decimal SubTotal { get; set; }
        //public virtual decimal PriceCV { get; set; }
        //public virtual decimal PriceTotal { get; set; }
        //public virtual bool IsRemovable { get; set; }
        //public virtual string EditUrl { get; set; }
        //public virtual int ProductTypeID { get; set; }
        //public virtual bool EsEnrollment { get; set; }
        //public virtual bool TieneDescuento { get; set; }

        //public virtual string ModificationReason { get; set; } 
        //public virtual decimal RetailUnitPrice { get; set; } /*R2804 - CGI(JICM) - Adición de objetos para Columna RetailPrice -INI*/
        //public virtual decimal OriginalTotal { get; set; }
        //public virtual decimal AdjustedTotal { get; set; }
        //public virtual decimal OriginalUnitPrice { get; set; }
        //public virtual decimal AdjustedUnitPrice { get; set; }
        //public virtual decimal OriginalCommissionable { get; set; }
        //public virtual decimal AdjustedCommissionable { get; set; }
        //public virtual decimal OriginalCommissionableTotal { get; set; }
        //public virtual decimal AdjustedCommissionableTotal { get; set; }
        //public virtual decimal OriginalQV { get; set; }
        //public virtual decimal AdjustedQV { get; set; }
        //public virtual decimal OriginalQVTotal { get; set; }
        //public virtual decimal AdjustedQVTotal { get; set; }

        //#region agregados
        //// Calculated Properties
        //public virtual bool IsKit { get; set; }
        //bool IsQuantityEditable { get; set; }

        //decimal TotalQV { get; set; } 
        //string TotalQV_Currency { get; set; }

        //#endregion

        public virtual MiniShopOrderItemModel LoadResources(
            OrderItem orderItem,
            string editUrl = null, bool? showEditLink = false)
        {
            SKU = orderItem.SKU;  //Developed by Wesley Campos S. - CSTI
            ProductID = orderItem.ProductID.Value;
            ProductName = orderItem.ProductName;
            int ProductBaseID = Product.GetVariants(Convert.ToInt32(orderItem.ProductID)).FirstOrDefault().ProductBaseID;
            //ProductTypeID = ProductBase.LoadFull(ProductBaseID).ProductTypeID;
            //EsEnrollment = (ProductTypeID == (int)Constants.ProductType.EnrollmentItem);

            //var preAdjustmentUnitPrice = orderItem.GetPreAdjustmentPrice(orderItem.ProductPriceTypeID.Value);
            //var finalUnitPrice = orderItem.GetAdjustedPrice();

            //ModificationReason = orderItem.OrderAdjustmentOrderLineModifications.Any() ? orderItem.OrderAdjustmentOrderLineModifications.First().OrderAdjustment.Description : string.Empty;

            //OriginalUnitPrice = preAdjustmentUnitPrice;
            //AdjustedUnitPrice = finalUnitPrice;

            //Quantity = orderItem.Quantity;

            //OriginalCommissionable = orderItem.GetPreAdjustmentPrice(orderItem.OrderCustomer.CommissionablePriceTypeID);
            //AdjustedCommissionable = orderItem.GetAdjustedPrice(orderItem.OrderCustomer.CommissionablePriceTypeID);

            //OriginalCommissionableTotal = orderItem.GetPreAdjustmentPrice(orderItem.OrderCustomer.CommissionablePriceTypeID) * Quantity;
            //AdjustedCommissionableTotal = orderItem.GetAdjustedPrice(orderItem.OrderCustomer.CommissionablePriceTypeID) * Quantity;

            //AdjustedTotal = finalUnitPrice * Quantity;
            //OriginalTotal = preAdjustmentUnitPrice * Quantity;

            //OriginalQV = orderItem.GetPreAdjustmentPrice((int)Constants.ProductPriceType.QV);
            //AdjustedQV = orderItem.GetAdjustedPrice((int)Constants.ProductPriceType.QV);

            //OriginalQVTotal = orderItem.GetPreAdjustmentPrice((int)Constants.ProductPriceType.QV) * Quantity;
            //AdjustedQVTotal = orderItem.GetAdjustedPrice((int)Constants.ProductPriceType.QV) * Quantity;

            //RetailUnitPrice = orderItem.GetPreAdjustmentPrice((int)Constants.ProductPriceType.Retail);

            //TieneDescuento = (OriginalQVTotal != AdjustedQVTotal);

            //foreach (var item in orderItem.OrderItemPrices)
            //{
            //    var pType = ProductPriceType.Load(item.ProductPriceTypeID);
            //    if (pType.Name.Equals("QV")) PriceQV = item.UnitPrice * Quantity;
            //    if (pType.Name.Equals("CV")) PriceCV = item.UnitPrice * Quantity;
            //    if (pType.Name.Equals("Host Base")) Price = item.UnitPrice;
            //}

            //SubTotal = Price - Convert.ToDecimal(orderItem.Discount);
            //PriceTotal = PriceReal * Quantity;
            //IsRemovable = orderItem.ParentOrderItem != null ? false : orderItem.IsEditable; //don't display remove link for child items in a kit.
            //EditUrl = editUrl;

            return this;
        }

    }
}


