using WatiN.Core;
using System.Collections.Generic;
using System;
using System.Threading;
using TestMasterHelpProvider;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Orders
{
    public class GMP_Orders_Entry_Page : GMP_Orders_Base_Page
    {
        #region Controls.

        private Div _divStandardOrder;
        private Table tableProducts;
        private TableBody tableBodyCartItems;
        private Div divShipToAddress;
        private Div divPaymentAddress;
        private TextField txtPaymentAmount;
        private Link lnkApplyAmount;
        private Table tablePayments;
        private Link lnkSubmitOrder;
        private ProductAdd_Control _productSelector;
        private OrderTotal_Control _orderTotals;
        private TableBody _products;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            this._divStandardOrder = Document.GetElement<Div>(new Param("SectionHeader", AttributeName.ID.ClassName));
            this.tableProducts = Document.GetElement<Table>(new Param("products"));
            this.tableBodyCartItems = Document.GetElement<TableBody>(new Param("CartItems"));
            this.divShipToAddress = Document.GetElement<Div>(new Param("shippingAddressDisplay", AttributeName.ID.ClassName));
            this.divPaymentAddress = Document.GetElement<Div>(new Param("paymentMethodDisplay", AttributeName.ID.ClassName));
            this.txtPaymentAmount = Document.GetElement<TextField>(new Param("txtPaymentAmount"));
            this.lnkApplyAmount = Document.GetElement<Link>(new Param("btnApplyPayment"));
            this.tablePayments = Document.GetElement<Table>(new Param("payments"));
            this.lnkSubmitOrder = Document.GetElement<Link>(new Param("btnSubmitOrder"));
            _productSelector = _content.GetElement<Para>(new Param("QuickAdd", AttributeName.ID.ClassName, RegexOptions.None)).As<ProductAdd_Control>();
            _orderTotals = _content.GetElement<TableRow>(new Param("totalBar")).As<OrderTotal_Control>();
            _products = _content.GetElement<TableBody>(new Param("CartItems"));
        }

        #endregion

        #region Methods

        public override bool IsPageRendered()
        {
            return this._divStandardOrder.CustomGetText().Contains("Standard Order");
        }

        public ProductAdd_Control ProductSelector
        {
            get { return _productSelector; }
        }

        public OrderTotal_Control OrderTotals
        {
            get { return _orderTotals; }
        }

        public OrderItem_Control GetCartItem(int index)
        {
            return _products.GetElement<TableRow>(new Param(index)).As<OrderItem_Control>();
        }

        public List<string> GetPaymentDetails(int row = 0)
        {
            return this.tablePayments.GetTableRowsData(1, row);
        }

        public List<string> GetAddedProductDetails(int row = 0)
        {
            return this.tableProducts.GetTableRowsData(row);
        }

        public GMP_Orders_Details_Page ClickSubmitOrder(int? timeout = null, bool pageRequired = true)
        {
            timeout = this.lnkSubmitOrder.CustomClick(timeout);
            _content.CustomWaitForSpinners();
            return Util.GetPage<GMP_Orders_Details_Page>(timeout, pageRequired);
        }

        public List<string> GetRowValueOfCartItems(int row = 0)
        {
            return this.tableProducts.GetTableRowsData(0, row);
        }

        public string[] GetShippingAddressDetails()
        {
            //this.divShipToAddress.CustomWaitUntilExist();

            return this.divShipToAddress.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

        public string[] GetBillingInfoDetails()
        {
            //this.divPaymentAddress.CustomWaitUntilExist();

            return this.divPaymentAddress.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

        public string GetPaymentAmount()
        {
            return this.txtPaymentAmount.CustomGetText();
        }

        public void ClickApplyAmount(int? timeout = null)
        {
            this.lnkApplyAmount.CustomClick(timeout);
            Thread.Sleep(2000);
        }
        #endregion

        #region validation Methods

        public bool ValidateShippingAddress(string addressOne, string addressTwo, string postalCode)
        {
            bool result = true;

            string[] shippingDetails = this.GetShippingAddressDetails();
            if (!Utils.AssertIsTrue(shippingDetails[1].Contains(addressOne), string.Format("Shipping address one '{0}' shown properly.", addressOne))) result = false;
            if (!Utils.AssertIsTrue(shippingDetails[2].Equals(addressTwo), string.Format("Shipping address two '{0}' shown properly.", addressTwo))) result = false;
            if (!Utils.AssertIsTrue(shippingDetails[3].Contains(postalCode), string.Format("Shipping address postal code '{0}' shown properly.", postalCode))) result = false;

            return result;
        }

        public bool ValidateBillingAddress(string nameOnCard, DateTime ccYear, string ccNo, string addressOne, string addressTwo, string postalCode)
        {
            bool result = true;
            
            string[] billingAdrss = this.GetBillingInfoDetails();
            if (!Utils.AssertIsTrue(billingAdrss[1].Contains(nameOnCard), string.Format("Name on card '{0}' under 'Billing to' section shown properly.", nameOnCard))) result = false;
            if (!Utils.AssertIsTrue(billingAdrss[2].Contains(ccYear.Year.ToString()), string.Format("Credit card year '{0}' under 'Billing to' section name shown properly.", ccYear.Year.ToString()))) result = false;
            if (!Utils.AssertIsTrue(billingAdrss[3].Contains(ccNo.Substring(ccNo.Length - 5, 4)), string.Format("Credit card number '{0}' under 'Billing to' section name shown properly.", ccNo.Substring(ccNo.Length - 5, 4)))) result = false;
            if (!Utils.AssertIsTrue(billingAdrss[4].Contains(addressOne), string.Format("Main address one '{0}' under 'Billing to' section name shown properly.", addressOne))) result = false;
            if (!Utils.AssertIsTrue(billingAdrss[5].Contains(addressTwo), string.Format("Main address two '{0}' under 'Billing to' section name shown properly.", addressTwo))) result = false;
            if (!Utils.AssertIsTrue(billingAdrss[6].Contains(postalCode), string.Format("Main address postal code '{0}' under 'Billing to' section name shown properly.", postalCode))) result = false;

            return result;
        }

        public bool ValidateAddedProductDetails(int rowNo, string prodSKU, string prodName, string prodPrice, string subTotal)
        {
            bool result = true;

            // Get added product details.
            List<string> intialOrders = this.GetAddedProductDetails(rowNo);
            if (!Utils.AssertIsTrue(intialOrders[0].Contains(prodSKU), string.Format("Initial order product id '{0}' is proper.", prodSKU))) result = false;
            if (!Utils.AssertIsTrue(intialOrders[1].Contains(prodName), string.Format("Initial order product name '{0}' is proper.", prodName))) result = false;
            if (!Utils.AssertIsTrue(intialOrders[2].Contains(prodPrice), string.Format("Initial order price per item '{0}' is proper.", prodPrice))) result = false;
            if (!Utils.AssertIsTrue(intialOrders[4].Contains(subTotal), string.Format("Initial order total price '{0}' is proper.", subTotal))) result = false;

            return result;
        }

        #endregion
    }
}
