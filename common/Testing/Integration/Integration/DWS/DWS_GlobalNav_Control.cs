using WatiN.Core;
using System.Threading;
using NetSteps.Testing.Integration.DWS;
using NetSteps.Testing.Integration.DWS.Contacts;
using NetSteps.Testing.Integration.DWS.Performance;
using NetSteps.Testing.Integration.DWS.News;
using NetSteps.Testing.Integration.DWS.Training;
using NetSteps.Testing.Integration.DWS.Orders;
using NetSteps.Testing.Integration.DWS.MyAccount;
using NetSteps.Testing.Integration.DWS.Communication;
using NetSteps.Testing.Integration.DWS.Support;

namespace NetSteps.Testing.Integration.DWS
{
    /// <summary>
    /// Main navigation controls used by all pages.
    /// </summary>
    public class DWS_GlobalNav_Control : Control<UnorderedList>
    {
        private Link _performance;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _performance = Element.GetElement<Link>(new Param("Nav-Performance", AttributeName.ID.ClassName));
        }

        public DWS_Home_Page ClickHome(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("Nav-Dashboard", AttributeName.ID.ClassName)).CustomClick(timeout);
            return Util.GetPage<DWS_Home_Page>(timeout, pageRequired);           
        }

        public DWS_Contacts_Page ClickContacts(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("Nav-Contacts", AttributeName.ID.ClassName)).CustomClick(timeout);
            return Util.GetPage<DWS_Contacts_Page>(timeout, pageRequired);
        }

        public DWS_Performance_Dashboard_Page ClickPerformance(int? timeout = null, bool pageRequired = true)
        {
            timeout = _performance.CustomClick(timeout);
            return Util.GetPage<DWS_Performance_Dashboard_Page>(timeout, pageRequired);            
        }

        public TPage ClickPerformance<TPage>(int? timeout = null, bool pageRequired = true) where TPage : DWS_Performance_Base_Page, new()
        {
            timeout = _performance.CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }

        public DWS_News_Page ClickNews(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("Nav-News", AttributeName.ID.ClassName)).CustomClick(timeout);
            return Util.GetPage<DWS_News_Page>(timeout, pageRequired);             
        }

        public DWS_Documents_Page ClickDocuments(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("Nav-Documents", AttributeName.ID.ClassName)).CustomClick(timeout);
            return Util.GetPage<DWS_Documents_Page>(timeout, pageRequired);            
        }

        public DWS_Orders_Page ClickOrders(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("Nav-Orders", AttributeName.ID.ClassName)).CustomClick(timeout);
            Thread.Sleep(2000);
            return Util.GetPage<DWS_Orders_Page>(timeout, pageRequired);                 
        }

        public DWS_MyAccount_Page ClickMyAccount(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("Nav-Account", AttributeName.ID.ClassName)).CustomClick(timeout);
            Thread.Sleep(2000);
            return Util.GetPage<DWS_MyAccount_Page>(timeout, pageRequired);            
        }

        public DWS_Communication_Mailbox_Page ClickCommunication(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("Nav-Communication", AttributeName.ID.ClassName)).CustomClick(timeout);
            return Util.GetPage<DWS_Communication_Mailbox_Page>(timeout, pageRequired);              
        }

        public DWS_Support_Browse_Page ClickSupport(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("Nav-Support", AttributeName.ID.ClassName)).CustomClick(timeout);
            return Util.GetPage<DWS_Support_Browse_Page>(timeout, pageRequired);            
        }

        public TPage ClickTab<TPage>(string tab, int? timeout = null, bool pageRequired = true) where TPage : DWS_Base_Page, new()
        {
            timeout = Element.GetElement<Link>(new Param(tab, AttributeName.ID.ClassName)).CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }
    }
}
