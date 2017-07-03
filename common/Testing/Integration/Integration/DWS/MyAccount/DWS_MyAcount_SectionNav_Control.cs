using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.DWS.MyAccount
{
    
    public class DWS_MyAcount_SectionNav_Control : Control<UnorderedList>
    {
        public DWS_MyAccount_EditProfile_Page ClickEditProfile(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Account/Edit", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<DWS_MyAccount_EditProfile_Page>(timeout, pageRequired);
        }

        public DWS_MyAccount_EditSecuritySettings_Page ClickEditSecuritySetttings(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Account/Edit/SecuritySettings", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<DWS_MyAccount_EditSecuritySettings_Page>(timeout, pageRequired);
        }

        public DWS_MyAccount_ProductCredit_Page ClickProductCredit(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Account/Ledger/ProductCredit", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<DWS_MyAccount_ProductCredit_Page>(timeout, pageRequired);
        }

        public TPage ClickDisbursementProfiles<TPage>(int? timeout = null, bool pageRequired = true) where TPage : DWS_Base_Page, new()
        {
            timeout = Element.GetElement<Link>(new Param("/Account/DisbursementProfiles", AttributeName.ID.Href, RegexOptions.None).Or(new Param("/Account/Edit/PaymentCard", AttributeName.ID.Href, RegexOptions.None))).CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);             
        }

        public TPage ClickPaymentCard<TPage>(int? timeout = null) where TPage : DWS_MyAccount_Base_Page, new()
        {
            timeout = Element.GetElement<Link>(new Param("/Account/Edit/PaymentCard", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<TPage>(timeout);
        }
    }
}
