using WatiN.Core;
using System.Collections.Generic;
using TestMasterHelpProvider;
using System;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    /// <summary>
    /// Class related to Autoship order page.
    /// </summary>
    public class GMP_Accounts_EnrollInitialOrder_Page : GMP_Accounts_Base_Page
    {
        private SearchSuggestionBox_Control _search;
        private TextField _quantity;
        private Link _addToOrder;
        private Table tableAutoShipOrder;
        private Span spanSubTotalUnderTotal;
        private Span spanTaxTotal;
        private Span spanShippingTax;
        private Span spanGrandTotal;
        private Link lnkNext, _skip;
        private Div _quickAdd;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _search = Document.GetElement<TextField>(new Param("txtQuickAddSearch")).As<SearchSuggestionBox_Control>();
            _quantity = Document.GetElement<TextField>(new Param("txtQuickAddQuantity"));
            _quantity.CustomWaitForVisibility();
            _addToOrder = Document.GetElement<Link>(new Param("btnQuickAdd"));
            this.tableAutoShipOrder = Document.GetElement<Table>(new Param("DataGrid", AttributeName.ID.ClassName));
            this.spanSubTotalUnderTotal = Document.GetElement<Span>(new Param("subtotal", AttributeName.ID.ClassName).And(new Param(1)));
            this.spanTaxTotal = Document.GetElement<Span>(new Param("taxTotal", AttributeName.ID.ClassName));
            this.spanShippingTax = Document.GetElement<Span>(new Param("shippingTotal", AttributeName.ID.ClassName));
            this.spanGrandTotal = Document.GetElement<Span>(new Param("grandTotal", AttributeName.ID.ClassName));
            this.lnkNext = Document.GetElement<Link>(new Param("btnNext"));
            _skip = Document.GetElement<Link>(new Param("btnSkip"));
            _quickAdd = Document.GetElement<Div>(new Param("QuickAdd", AttributeName.ID.ClassName));
        }

         public override bool IsPageRendered()
        {
            return spanGrandTotal.Exists;
        }

        /// <summary>
        /// Click next.
        /// </summary>
         public TPage ClickNext<TPage>(int? timeout = null, bool pageRequired = true) where TPage : GMP_Accounts_Base_Page, new()
        {
            timeout = this.lnkNext.CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }

         public TPage ClickSkip<TPage>(int? timeout = null, bool pageRequired = true) where TPage : GMP_Accounts_Base_Page, new()
        {
            timeout = _skip.CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }

         public GMP_Accounts_EnrollInitialOrder_Page AddOrderItem(OrderItem orderItem, int? timeout = null, int? delay = 2)
        {
            _search.Select(orderItem.Product.SKU);
            _quantity.CustomSetTextQuicklyHelper(orderItem.Quantity.ToString());
            timeout = _addToOrder.CustomClick(timeout);
            _quickAdd.CustomWaitForSpinner(timeout, delay);
            return this;
        }

        public bool ValidateOrderItem(OrderItem orderItem, int rowNumber)
        {
            bool result = true;

            List<string> orderDetails = this.tableAutoShipOrder.GetTableRowsData(0, rowNumber);

            decimal totalPrice = (decimal)orderItem.Product.Price * orderItem.Quantity;

            if (!Utils.AssertIsTrue(orderDetails[0].Contains(orderItem.Product.SKU), string.Format("Product SKU '{0}' under autoship order table is shown properly.", orderItem.Product.SKU))) result = false;
            if (!Utils.AssertIsTrue(orderDetails[1].Contains(orderItem.Product.Name), string.Format("Product name '{0}' under autoship order table is shown properly.", orderItem.Product.Name))) result = false;
            if (!Utils.AssertIsTrue(orderDetails[2].Contains(orderItem.Product.Price.ToString()), string.Format("Product price per item '{0}' under autoship order table is shown properly.", orderItem.Product.Price))) result = false;
            if (!Utils.AssertIsTrue(orderDetails[3].Contains(orderItem.Quantity.ToString()), string.Format("Product quantity '{0}' under autoship order table is shown properly.", orderItem.Quantity))) result = false;
            if (!Utils.AssertIsTrue(orderDetails[4].Contains(totalPrice.ToString()), string.Format("Product price '{0}' under autoship order table is shown properly.", totalPrice))) result = false;

            return result;
        }

        /// <summary>
        /// Validate Order total price.
        /// </summary>
        /// <param name="prodTotal">Product total.</param>
        /// <returns>True if all validations are passed.</returns>
        [Obsolete("", true)]
        public bool ValidateOrderTotalprice(string prodTotal)
        {
            bool result = true;

            if (!Utils.AssertIsTrue(this.spanSubTotalUnderTotal.CustomGetText().Contains(prodTotal),
                string.Format("Subtotal '{0}' is shown properly.", prodTotal))) result = false;

            double subTotal = Convert.ToDouble(this.spanSubTotalUnderTotal.CustomGetText().Remove(0, 1));
            double tax = Convert.ToDouble(this.spanTaxTotal.CustomGetText().Remove(0, 1));
            double shipping = Convert.ToDouble(this.spanShippingTax.CustomGetText().Remove(0, 1));

            if (!Utils.AssertIsTrue(this.spanGrandTotal.CustomGetText().Contains((subTotal + tax + shipping).ToString()),
                string.Format("Order total '{0}' is shown properly.", (subTotal + tax + shipping).ToString()))) result = false;

            return result;
        }
    }
}