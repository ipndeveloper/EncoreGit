using System.Collections.Generic;
using System.Linq;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Encore.Core.IoC;

namespace nsDistributor.Areas.Enroll.Models.Products
{
    public class ShippingMethodModel : SectionModel
    {
        #region Values
        public virtual int? SelectedShippingMethodID { get; set; }
        #endregion

        #region Resources
        public virtual IEnumerable<ShippingMethodWithRate> ShippingMethods { get; set; }
        public virtual int CurrencyID { get; set; }
        public virtual bool ShippingRequired { get; set; }
        #endregion

        #region Helpers
        public virtual ShippingMethodWithRate SelectedShippingMethod
        {
            get
            {
                return SelectedShippingMethodID == null || ShippingMethods == null
                    ? null
                    : ShippingMethods.FirstOrDefault(x => x.ShippingMethodID == SelectedShippingMethodID);
            }
        }
        #endregion

        #region Infrastructure
        public ShippingMethodModel()
        {
            ShippingMethods = Enumerable.Empty<ShippingMethodWithRate>();
        }

        public virtual ShippingMethodModel LoadValues(
            int? selectedShippingMethodID)
        {
            this.SelectedShippingMethodID = selectedShippingMethodID;
            
            return this;
        }

        public virtual ShippingMethodModel LoadResources(
            IEnumerable<ShippingMethodWithRate> shippingMethods,
            Order order)
        {
					var inventory = Create.New<InventoryBaseRepository>();

            this.ShippingMethods = shippingMethods;
            this.CurrencyID = order.CurrencyID;
            this.ShippingRequired = order.OrderCustomers.SelectMany(oc => oc.OrderItems).Any(oi => inventory.GetProduct(oi.ProductID.Value).ProductBase.IsShippable);
            
            return this;
        }
        #endregion
    }
}