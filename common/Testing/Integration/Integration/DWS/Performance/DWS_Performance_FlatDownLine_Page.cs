using WatiN.Core;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using WatiN.Core.Extras;

namespace NetSteps.Testing.Integration.DWS.Performance
{
    public class DWS_Performance_FlatDownLine_Page : DWS_Performance_Base_Page
    {
        private Link lnkFilterSearch;
        private TextField txtFlatSearch;
        private Table _table;


        protected override void InitializeContents()
        {
            base.InitializeContents();

            this.lnkFilterSearch = Document.GetElement<Link>(new Param("Button ModSearch filterButton", AttributeName.ID.ClassName));
            this.txtFlatSearch = Document.GetElement<TextField>(new Param("searchValueInputFilter"));
            _table = Document.GetElement<Table>(new Param("paginatedGrid"));

        }

        /// <summary>
        /// Is DownLine page rendered.
        /// </summary>
        /// <returns>True if rendered, else false.</returns>
         public override bool IsPageRendered()
        {
            return (Util.Browser.Url.Contains("/Performance/FlatDownline") && lnkFilterSearch.Exists);
        }

        #region Methods

        public TControl GetDownline<TControl>(int? index = null) where TControl : DWS_Performance_Downline_Control, new()
        {
            Document.CustomWaitForSpinners();
            TableRowCollection rows = _table.TableBody(Find.Any).OwnTableRows;
            if (!index.HasValue)
                index = Util.GetRandom(0, rows.Count - 1);
            return rows[(int)index].As<TControl>();
        }

        public DWS_Performance_Card_Control GetCard()
        {
            return Document.GetElement<Div>(new Param("TreeNodeDetails UI-whiteBg brdrYYNN brdr1 infoCardWindow", AttributeName.ID.ClassName)).As<DWS_Performance_Card_Control>();
        }

        /// <summary>
        /// Search for flat down line for specific customer.
        /// </summary>
        /// <param name="customerName">Customer name.</param>
        public void SearchForFlatDownLine(string customerName, int? timeout = null)
        {
            this.txtFlatSearch.CustomSetTextHelper(customerName);
            this.lnkFilterSearch.CustomClick(timeout);
            //Util.WaitForBrowserStrong();
        }

        /// <summary>
        /// Get filtered search results.
        /// </summary>
        /// <returns>Available search results.</returns>
        public List<string> GetFilteredSearchResults()
        {
            var searchResults = new List<string>();

            //this.tableDWSCommonGrid.CustomWaitUntilExist();

            TableRowCollection tableRows = this.tableDWSCommonGrid.OwnTableRows;

            if (tableRows.Count == 2)
            {
                if (tableRows[1].CustomGetText().Contains("no records"))
                {
                    searchResults.Add(tableRows[1].CustomGetText());
                }
                else
                {
                    searchResults.Add(tableRows[1].TableCells[5].CustomGetText());
                }
            }
            else if (tableRows.Count > 2)
            {
                for (int index = 0; index < tableRows.Count - 1; index++)
                {
                    searchResults.Add(tableRows[index + 1].TableCells[5].CustomGetText());
                }
            }

            return searchResults;
        }


        #endregion

        #region Validation Methods

        #endregion
    }
}