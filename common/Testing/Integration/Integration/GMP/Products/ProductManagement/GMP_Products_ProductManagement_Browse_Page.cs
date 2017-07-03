﻿using WatiN.Core;
using System.Threading;
using System.Collections.Generic;
using NetSteps.Testing.Integration.GMP.Products.ProductManagement.Product;

using System; // only for obsolete attributes

namespace NetSteps.Testing.Integration.GMP.Products.ProductManagement
{

    public class GMP_Products_ProductManagement_Browse_Page : GMP_Products_ProductManagement_Base_Page
    {
        #region Controls.

        private SelectList _status, _type;
        private Link _apply;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _status = Document.SelectList("activeSelectFilter");
            _type = Document.SelectList("typeSelectFilter");
            _apply = Document.GetElement<Link>(new Param("Button ModSearch filterButton", AttributeName.ID.ClassName));
        }

        #endregion

        #region Methods

         public override bool IsPageRendered()
        {
            return Document.GetElement<TextField>(new Param("queryInputFilter")).Exists;
        }

        public GMP_Products_ProductManagement_Browse_Page SelectStatus(BrowseProductsStatus productStatus, int? timeout = null)
        {
            _status.CustomSelectDropdownItem((int)productStatus, timeout);
            return this;
        }

        public GMP_Products_ProductManagement_Browse_Page SelectType(int index, int? timeout = null)
        {
            _type.CustomSelectDropdownItem(index, timeout);
            return this;
        }

        public GMP_Products_ProductManagement_Browse_Page ClickApplyFilter(int? timeout = null, int? delay = 2, bool pageRequired = true)
        {
            timeout = _apply.CustomClick(timeout);
            Thread.Sleep(2000); // Wait for table to display
            return Util.GetPage<GMP_Products_ProductManagement_Browse_Page>(timeout, pageRequired);
        }

        public ControlCollection<GMP_Products_ProductManagement_Product_Control> GetProducts(int? timeout = null, int? delay = 2)
        {

            return GetTable().As<GMP_Products_ProductManagement_Product_Control>();
        }

        public GMP_Products_ProductManagement_Product_Control GetProduct(int? index = null, int? timeout = null, int? delay = 2)
        {
            TableRowCollection rows = GetTable();
            if (!index.HasValue)
            {
                index = Util.GetRandom(0, rows.Count - 1);
            }
            return rows[(int)index].As<GMP_Products_ProductManagement_Product_Control>();
        }

        private TableRowCollection GetTable(int? timeout = null, int? delay = 1)
        {
            Table tbl = Document.GetElement<Table>(new Param("paginatedGrid"));
            tbl.CustomWaitForSpinner(timeout, delay);
            return tbl.TableBody(Find.Any).TableRows;
        }
        #endregion Methods.
    }
}