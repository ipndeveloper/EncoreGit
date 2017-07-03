using WatiN.Core;

namespace NetSteps.Testing.Integration.PWS.Account
{
    public class PWS_Account_ProductCredit_Page : PWS_Base_Page
    {
        public override bool IsPageRendered()
        {
            return _content.GetElement<H1>(new Param("Product Credit Ledger", AttributeName.ID.InnerText)).Exists;
        }
    }
}
