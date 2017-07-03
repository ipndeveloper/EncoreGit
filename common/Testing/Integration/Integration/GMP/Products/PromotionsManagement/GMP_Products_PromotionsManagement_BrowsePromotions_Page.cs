using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Products.PromotionsManagement
{
    public class GMP_Products_PromotionsManagement_BrowsePromotions_Page : GMP_Products_PromotionsManagement_Base_Page
    {
         public override bool IsPageRendered()
        {
            return Title.Contains("Promotions");
        }

         public GMP_Products_PromotionsManagement_NewPromotions_Page ClickNewPromotion(int? timeout = null, bool pageRequired = true)
         {
             timeout = Document.GetElement<Link>(new Param("/Products/Promotions/Edit", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
             return Util.GetPage<GMP_Products_PromotionsManagement_NewPromotions_Page>(timeout, pageRequired);
         }
    }
}
