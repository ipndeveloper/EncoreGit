using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Orders
{
    public class GMP_Orders_NewOrder_Page : GMP_Orders_Base_Page
    {
        private SearchSuggestionBox_Control _search;
        private Link _lnkStartANewOrder;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            this._search = Document.GetElement<TextField>(new Param("txtCustomerSuggest")).As<SearchSuggestionBox_Control>();
            this._lnkStartANewOrder = Document.GetElement<Link>(new Param("Button BigBlue", AttributeName.ID.ClassName));
        }

         public override bool IsPageRendered()
        {
            return _search.Exists;
        }

         public GMP_Orders_Entry_Page SearchAndSelectCustomer(string customerIdentifier, string exactCustomerIdentifier = null, bool exactMatch = false, int? timeout = null, bool pageRequired = true)
        {
            _search.Select(customerIdentifier, exactCustomerIdentifier, exactMatch, timeout);
            timeout = this._lnkStartANewOrder.CustomClick(timeout);
            return Util.GetPage<GMP_Orders_Entry_Page>(timeout, pageRequired);
        }
    }
}
