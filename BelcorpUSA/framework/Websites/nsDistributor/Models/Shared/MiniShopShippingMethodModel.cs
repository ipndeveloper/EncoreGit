using System.Collections.Generic;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Extensions;

namespace nsDistributor.Models.Shared
{
    public class MiniShopShippingMethodModel
    {
        #region Models
        public virtual List<MiniShopShippingMethod> ShippingMethodList { get; set; }
        //public virtual MiniShopShippingMethod SelectedShippingMethod { get; set; }
        public virtual int CurrencyID { get; set; }
        
        #endregion

        #region Infrastructure

        public MiniShopShippingMethodModel(IEnumerable<ShippingMethodWithRate> shippingMethods, int currencyID)
        {
            LoadResources(shippingMethods, currencyID);
            //LoadValues(selectedShippingMethodID);
        }

        private void LoadShippingMethods(IEnumerable<ShippingMethodWithRate> shippingMethods)
        {
            this.ShippingMethodList = new List<MiniShopShippingMethod>();

            foreach (ShippingMethodWithRate method in shippingMethods)
            {
                this.ShippingMethodList.Add(new MiniShopShippingMethod
                {
                    ShippingMethodID = method.ShippingMethodID,
                    Name = method.Name,
                    ShippingAmount = method.ShippingAmount.ToString(this.CurrencyID)
                });
            }
        }

        //public virtual MiniShopShippingMethodModel LoadValues(int? selectedShippingMethodID)
        //{
        //    this.SelectedShippingMethod = ShippingMethodList.FirstOrDefault(x => x.ShippingMethodID == selectedShippingMethodID);
        //    return this;
        //}

        public virtual MiniShopShippingMethodModel LoadResources(IEnumerable<ShippingMethodWithRate> shippingMethods, int currencyID)
        {
            CurrencyID = currencyID;
            LoadShippingMethods(shippingMethods);
            return this;
        }
        #endregion
    }
}
