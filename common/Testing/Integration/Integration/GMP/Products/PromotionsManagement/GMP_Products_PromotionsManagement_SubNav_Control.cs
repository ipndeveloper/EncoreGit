using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Products.PromotionsManagement
{
    public class GMP_Products_PromotionsManagement_SubNav_Control : Control<Div>
    {
        public int Index
        { get; set; }

        public GMP_Products_PromotionsManagement_BrowsePromotions_Page ClickBrowsePromotions(int? timeout = null, bool pageRequired = true)
        {
            Element.CustomRunScript(Util.strSlideDown, null, Index);
            timeout = Element.GetElement<Link>(new Param("/Products/Promotions", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_PromotionsManagement_BrowsePromotions_Page>(timeout, pageRequired);
        }

        public GMP_Products_PromotionsManagement_CreatePromotions_Page ClickCreateProductPromotions(int? timeout = null, bool pageRequired = true)
        {
            Element.CustomRunScript(Util.strSlideDown, null, Index);
            timeout = Element.GetElement<Link>(new Param("/Products/ProductPromotions/Edit", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_PromotionsManagement_CreatePromotions_Page>(timeout, pageRequired);
        }

        public GMP_Products_PromotionsManagements_CreateCartRewards ClickCreateCartRewards(int? timeout = null, bool pageRequired = true)
        {
            Element.CustomRunScript(Util.strSlideDown, null, Index);
            timeout = Element.GetElement<Link>(new Param("/Products/CartRewardsPromotions/Edit", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_PromotionsManagements_CreateCartRewards>(timeout, pageRequired);
        }
    }
}
