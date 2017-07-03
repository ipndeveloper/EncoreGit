using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Products.PromotionsManagement
{
    public class GMP_Products_PromotionsManagement_NewPromotions_Page : GMP_Products_PromotionsManagement_Base_Page
    {
        public override bool IsPageRendered()
        {
            return Document.GetElement<SelectList>(new Param("sCurrency")).Exists;
        }
    }
}
