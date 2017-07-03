using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Products.PromotionsManagement
{
    public class GMP_Products_PromotionsManagement_SectionNav_Control : Control<UnorderedList>
    {
        public GMP_Products_PromotionsManagement_BrowsePromotions_Page ClickBrowsePromotions(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Products/Promotions", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_PromotionsManagement_BrowsePromotions_Page>(timeout, pageRequired);
        }

        public GMP_Products_PromotionsManagement_CreatePromotions_Page ClickCreateProductPromotions(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Products/ProductPromotions/Edit", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_PromotionsManagement_CreatePromotions_Page>(timeout, pageRequired);
        }

        public GMP_Products_PromotionsManagements_CreateCartRewards ClickCreateCartRewards(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Products/CartRewardsPromotions/Edit", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_PromotionsManagements_CreateCartRewards>(timeout, pageRequired);
        }
    }
}
