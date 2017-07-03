using WatiN.Core;
using System;
using NetSteps.Testing.Integration.GMP.Accounts;

namespace NetSteps.Testing.Integration.GMP.Orders
{
    public class GMP_Orders_Order_Control : Control<TableRow>
    {
        private string _number, orderUrl, _firstName, _lastName, _clientUrl, _sponsor, _sponsorUrl;
        private string _created, _completed, _subtotal, _total;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            Link order = Element.GetElement<TableCell>(new Param(1)).GetElement<Link>();
            _number = order.CustomGetText();
            orderUrl = order.Url;
            Link name = Element.GetElement<TableCell>(new Param(2)).GetElement<Link>();
            _firstName = name.CustomGetText();
            _clientUrl = name.Url;
            _lastName = Element.GetElement<TableCell>(new Param(3)).CustomGetText();
            _completed = Element.GetElement<TableCell>(new Param(6)).CustomGetText();
            _subtotal = Element.GetElement<TableCell>(new Param(7)).CustomGetText();
            _total = Element.GetElement<TableCell>(new Param(8)).CustomGetText();
            Link sponsor = Element.GetElement<TableCell>(new Param(9)).GetElement<Link>();
            _sponsor = sponsor.CustomGetText();
            _sponsorUrl = sponsor.Url;
        }

        public string Number
        {
            get { return _number; }
        }

        public string FirstName
        {
            get { return _firstName; }
        }

        public string LastName
        {
            get { return _lastName; }
        }

        public string Sponsor
        {
            get { return _sponsor; }
        }

        public DateTime CreateDate
        {
            get { return DateTime.Parse(_created); }
            set { _created = value.ToShortDateString(); }
        }

        public DateTime CompleteDate
        {
            get { return DateTime.Parse(_completed); }
        }

        public string Total
        {
            get { return _total; }
        }

        public string Subtotal
        {
            get { return _subtotal; }
        }

        public GMP_Orders_Details_Page SelectOrder(int? timeout = null)
        {
            return Util.Browser.Navigate<GMP_Orders_Details_Page>(orderUrl, timeout);
        }

        public GMP_Accounts_Overview_Page SelectClient(int? timeout = null)
        {
            return Util.Browser.Navigate<GMP_Accounts_Overview_Page>(_clientUrl, timeout);

        }

        public GMP_Accounts_Overview_Page SelectSponsor(int? timeout = null)
        {
            return Util.Browser.Navigate<GMP_Accounts_Overview_Page>(_sponsorUrl, timeout);
        }
    }
}
