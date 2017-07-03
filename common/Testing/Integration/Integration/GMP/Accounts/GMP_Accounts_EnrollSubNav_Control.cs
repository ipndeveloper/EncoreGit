using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    public class GMP_Accounts_EnrollSubNav_Control : Control<Div>
    {
        private Link _enrollRetailCustomer;
        private Link _enrollPreferredCustomer;
        private Link _enrollDistributor;
        private Link _enrollBusinessEntity;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _enrollRetailCustomer = Element.GetElement<Link>(new Param("/Enrollment\\?acctTypeId=3", AttributeName.ID.Href, RegexOptions.None));
            _enrollPreferredCustomer = Element.GetElement<Link>(new Param("/Enrollment\\?acctTypeId=2", AttributeName.ID.Href, RegexOptions.None));
            _enrollDistributor = Element.GetElement<Link>(new Param("/Enrollment\\?acctTypeId=1", AttributeName.ID.Href, RegexOptions.None));
            _enrollBusinessEntity = Element.GetElement<Link>(new Param("/Enrollment\\?isEntity=true&acctTypeId=1", AttributeName.ID.Href, RegexOptions.None));
        }

        public GMP_Accounts_Enroll_Page ClickRetailCustomer(int? timeout = null, bool pageRequired = true)
        {
            Element.CustomRunScript(Util.strSlideDown);
            timeout = _enrollRetailCustomer.CustomClick(timeout);
            var page = Util.GetPage<GMP_Accounts_Enroll_Page>(timeout, pageRequired);
            return page.ConfigurePage(CustomerType.ID.Retail);
        }

        public GMP_Accounts_Enroll_Page ClickPreferredCustomer(int? timeout = null, bool pageRequired = true)
        {
            Element.CustomRunScript(Util.strSlideDown);
            timeout = _enrollPreferredCustomer.CustomClick(timeout);
            var page = Util.GetPage<GMP_Accounts_Enroll_Page>(timeout, pageRequired);
            return page.ConfigurePage(CustomerType.ID.Preferred);
        }

        public GMP_Accounts_Enroll_Page ClickDistributor(int? timeout = null, bool pageRequired = true)
        {
            Element.CustomRunScript(Util.strSlideDown);
            timeout = _enrollDistributor.CustomClick(timeout);
            var page = Util.GetPage<GMP_Accounts_Enroll_Page>(timeout, pageRequired);
            return page.ConfigurePage(CustomerType.ID.Distributor);
        }

        public GMP_Accounts_Enroll_Page ClickBusinessEntity(int? timeout = null, bool pageRequired = true)
        {
            Element.CustomRunScript(Util.strSlideDown);
            timeout = _enrollBusinessEntity.CustomClick(timeout);
            var page = Util.GetPage<GMP_Accounts_Enroll_Page>(timeout, pageRequired);
            return page.ConfigurePage(CustomerType.ID.Business);
        }
    }
}
