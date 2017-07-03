using System.Collections.Generic;
using System.Linq;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Web.Mvc.Controls.Models.Shared.ShippingMethods
{
    public class ShippingMethodModel
    {
        public List<ShippingMethod> ShippingMethodList { get; set; }
        public ShippingMethod SelectedShippingMethod { get; set; }
        public int CurrencyID { get; set; }

        public ShippingMethodModel()
        {
            ShippingMethodList = new List<ShippingMethod>();
        }

        public ShippingMethodModel LoadResources(IEnumerable<ShippingMethodWithRate> shippingMethods, int currencyID)
        {
            CurrencyID = currencyID;

            foreach (ShippingMethodWithRate method in shippingMethods)
            {
                ShippingMethodList.Add(new ShippingMethod 
                    { 
                        ShippingMethodID = method.ShippingMethodID,
                        Name = method.Name, 
                        ShippingAmount = method.ShippingAmount.ToString(currencyID)
                    });
            }

            return this;
        }

        public void LoadValues(int selectedShippingMethodID)
        {
            SelectedShippingMethod = ShippingMethodList.FirstOrDefault(x => x.ShippingMethodID == selectedShippingMethodID);
        }
    }
}