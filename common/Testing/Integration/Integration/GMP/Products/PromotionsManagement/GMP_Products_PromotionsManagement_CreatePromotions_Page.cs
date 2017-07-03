using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Products.PromotionsManagement
{
    public class GMP_Products_PromotionsManagement_CreatePromotions_Page : GMP_Products_PromotionsManagement_Base_Page
    {
         public override bool IsPageRendered()
        {
            return Title.Contains("Create a Product Promotion");
        }
    }
}
