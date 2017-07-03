using WatiN.Core;
using ListItems = WatiN.Core.ListItem;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.PWS.Shop
{
    /// <summary>
    /// Class related to Controls and methods of PWS Shipping page.
    /// </summary>
    public class PWS_Shop_Shipping_Page : PWS_Base_Page
    {
        private Link _cancel;
        private Address_Control _address;
        private Link _next;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _cancel = _content.GetElement<Link>(new Param("btnHideCreateProfile"));
            _cancel.CustomWaitForExist();
            _address = _content.GetElement<Div>(new Param("NewProfile", AttributeName.ID.ClassName, RegexOptions.None).Or(new Param("editAddressSection"))).As<Address_Control>();
            _next = _content.GetElement<Link>(new Param("btnNext"));
        }

        public override bool IsPageRendered()
        {
            return _cancel.Exists;
        }

        public Address_Control Address
        {
            get { return _address; }
        }

        /// <summary>
        /// Navigates to the next step
        /// </summary>
        /// <typeparam name="TPage"></typeparam>
        /// <param name="timeout"></param>
        /// <param name="delay"></param>
        /// <param name="pageRequired"></param>
        /// <returns></returns>
        public TPage ClickNext<TPage>(int? timeout =null, int? delay =null, bool pageRequired = true)
            where TPage : PWS_Base_Page, new()
        {
            timeout = _next.CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }

        [System.Obsolete("Use 'ClickShipToProfile<>()'", true)]
        public PWS_Shop_Billing_Page ClickShipToProfile(int? timeout = null, bool pageRequired = true)
        {
            //timeout = _content.GetElement<Link>(new Param("btnSelectedShipping")).CustomClick(timeout);
            return Util.GetPage<PWS_Shop_Billing_Page>(timeout, pageRequired);
        }

        public TPage ClickShipToProfile<TPage>(int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            timeout = _content.GetElement<Link>(new Param("btnSelectedShipping")).CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }
    }
}