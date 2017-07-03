using WatiN.Core;
using System;
using System.Text.RegularExpressions;

using NetSteps.Testing.Integration.GMP.Products.CatalogManagement;
using NetSteps.Testing.Integration.GMP.Products.ProductManagement;
using NetSteps.Testing.Integration.GMP.Products.WarehouseManagement;
using NetSteps.Testing.Integration.GMP.Products.PromotionsManagement;

namespace NetSteps.Testing.Integration.GMP.Products
{
    public class GMP_Products_SubNav_Control : Control<UnorderedList>
    {
        public GMP_Products_CatalogManagement_SubNav_Control CatalogManagement
        {
            get
            {
                return Element.GetElement<Div>(new Param("DropDown", AttributeName.ID.ClassName).And(new Param(0))).As<GMP_Products_CatalogManagement_SubNav_Control>();
            }
        }

        public GMP_Products_ProductManagement_SubNav_Control ProductManagement
        {
            get
            {
                return Element.GetElement<Div>(new Param("DropDown", AttributeName.ID.ClassName).And(new Param(1))).As<GMP_Products_ProductManagement_SubNav_Control>();
            }
        }

        public GMP_Products_WarehouseManagement_SubNav_Control WarehouseManagement(int index = 3)
        {
            GMP_Products_WarehouseManagement_SubNav_Control warehouseManagement = Element.GetElement<Div>(new Param("DropDown", AttributeName.ID.ClassName).And(new Param(index))).As<GMP_Products_WarehouseManagement_SubNav_Control>();
            warehouseManagement.Index = index;
            return warehouseManagement;
        }

        public GMP_Products_PromotionsManagement_SubNav_Control Promotions(int index = 2)
        {
            GMP_Products_PromotionsManagement_SubNav_Control promotions = Element.GetElement<Div>(new Param("DropDown", AttributeName.ID.ClassName).And(new Param(index))).As<GMP_Products_PromotionsManagement_SubNav_Control>();
            promotions.Index = index;
            return promotions;
        }

        /// <summary>
        /// Some promotions do not have the drop down.  The method below is to be able to click it.
        /// </summary>
        public GMP_Products_PromotionsManagement_BrowsePromotions_Page ClickPromotions(int? timeout = null, bool pageRequired = true)
        {
            var promo = Element.GetElement<Link>(new Param("/Products/Promotions", AttributeName.ID.Href, RegexOptions.None));
            timeout = promo.CustomClick(timeout);
            return Util.GetPage<GMP_Products_PromotionsManagement_BrowsePromotions_Page>(timeout, pageRequired);
        }

    }
}
