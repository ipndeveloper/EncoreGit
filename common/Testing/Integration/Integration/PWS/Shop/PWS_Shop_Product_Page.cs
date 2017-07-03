using WatiN.Core;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;
using NetSteps.Testing.Integration.PWS.Enroll;

namespace NetSteps.Testing.Integration.PWS.Shop
{
    public class PWS_Shop_Product_Page : PWS_Shop_Base_Page
    {
        private Div _product;
        private Link _lnkAddToCart;
        private string _productName;
        private SelectList _selectOption;
        private RadioButtonCollection _checkVariants;
        private Div _productOptionRadio;
        private Div _productTitle;
        private List<RadioButton> selections = new List<RadioButton>();
        private bool _backOrder, _outOfStock, _createBundle;
        private TextField _quantity;
        private Div _spinnerDiv;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _product = _content.GetElement<Div>(new Param("ProductDetails ", AttributeName.ID.ClassName, RegexOptions.None));
            _product.CustomWaitForExist();
            _lnkAddToCart = _product.GetElement<Link>(new Param("FL Button btnAddToCart AddCart ", AttributeName.ID.ClassName));
            _selectOption = _product.GetElement<SelectList>(new Param("productPropertyValueId", AttributeName.ID.ClassName));
            _productOptionRadio = _product.GetElement<Div>(new Param("productVariants", AttributeName.ID.ClassName));
            _checkVariants = _product.RadioButtons;
            _productTitle = _product.GetElement<Div>(new Param("productTitle", AttributeName.ID.ClassName));
            _productTitle.CustomWaitForExist(3, true, false);
            if (_product.GetElement<Div>(new Param("productTitle", AttributeName.ID.ClassName)).Exists)
                _productName = _product.GetElement<Div>(new Param("productTitle", AttributeName.ID.ClassName)).CustomGetText();
            else
                _productName = _product.Span(Find.ByClass("title block mb10 uppercase", false)).CustomGetText(); // For ItWorks
            _backOrder = _product.Span(Find.ByClass("backOrder")).Exists;
            _outOfStock = _product.GetElement<Div>(new Param("UI-lightBg pad10 brdrAll AddToCart", AttributeName.ID.ClassName)).CustomGetText().Contains("Out of stock");
            _createBundle = _product.GetElement<Link>(new Param("buildBundle", AttributeName.ID.ClassName, RegexOptions.None)).Exists;
            _quantity = _product.GetElement<TextField>(new Param("quantity", AttributeName.ID.ClassName));
            _spinnerDiv = _product.GetElement<Div>(new Param("UI-lightBg pad10 brdrAll AddToCart", AttributeName.ID.ClassName));
        }

        public bool BackOrder
        {
            get { return _backOrder; }
        }

        public bool OutOfStock
        {
            get { return _outOfStock; }
        }

        public bool CreateBundle
        {
            get { return _createBundle; }
        }

        public string ProductName
        {
            get { return _productName; }
        }

        public int? MinOption
        {
            get
            {
                int? minOption;
                if (_selectOption.Exists)
                    minOption = 1;
                else if (_checkVariants.Count > 0)
                    minOption = 0;
                else
                    minOption = null;
                return minOption;
            }
        }

        public int OptionCount
        {            
            get
            {
                int optionCount;
                if (_selectOption.Exists)
                    optionCount = _selectOption.Options.Count;
                else
                    optionCount = _checkVariants.Count;
                return optionCount;
            }
        }

        public void SelectOption(int? index = null, int? timeout = null)
        {
            if (_selectOption.Exists)
            {
                timeout = _selectOption.CustomSelectDropdownItem(index, timeout);
            }
            else if (_checkVariants.Count > 0)
            {
                if (!index.HasValue)
                    index = Util.GetRandom(0, _checkVariants.Count - 1);
                timeout = _checkVariants[(int)index].CustomSelectRadioButton(true, timeout);
                Util.Browser.CustomWaitForComplete(timeout);
            }
            InitializeContents();
        }

        public void SetQuantity(int amount)
        {
            _quantity.Value = amount.ToString();
        }

        public bool ClickAddToCart(int? timeout = null, int? delay = 2)
        {
            bool added = false;
            if (_lnkAddToCart.IsVisible())
            {
                timeout = _lnkAddToCart.CustomClick(timeout);
                _spinnerDiv = _product.GetElement<Div>(new Param("UI-lightBg pad10 brdrAll AddToCart", AttributeName.ID.ClassName));
                timeout = _spinnerDiv.CustomWaitForSpinners(timeout, delay);
                string msg = GetPopUpMessage();
                added = (msg.Contains("added to your") || msg.Contains("in Ihrem Warenkorb") || msg.Contains("été ajouté à  votre panier"));
            }
            return added;
        }

        public PWS_Enroll_SponsorBrowse_Page ClickFindConsultant(int? timeout = null)
        {
            timeout = _lnkAddToCart.CustomClick(timeout);
            return Util.GetPage<PWS_Enroll_SponsorBrowse_Page>();
        }

         public override bool IsPageRendered()
        {
            return _product.Exists;
        }
    }
}
