using WatiN.Core;

namespace NetSteps.Testing.Integration.DWS.Performance
{
    /// <summary>
    /// Class related to Tree view page.
    /// </summary>
    public class DWS_Performance_TreeView_Page : DWS_Performance_Base_Page
    {
        #region Controls.

        private TextField txtAccountSearch;
        private Div divAccountsTreeContainer;

        protected override void InitializeContents()
        {
            base.InitializeContents();

            this.txtAccountSearch = Document.GetElement<TextField>(new Param("searchText"));
            this.divAccountsTreeContainer = Document.GetElement<Div>(new Param("treeContainer"));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Is Tree view page rendered.
        /// </summary>
         public override bool IsPageRendered()
        {
            return this.txtAccountSearch.Exists;
        }

        /// <summary>
        ///  Is Accounts tree container shown.
        /// </summary>
        /// <rereturns>True if accounts tree is shown, else false.</rereturns>
        public bool IsAccountsTreeShown()
        {
            return this.divAccountsTreeContainer.Exists;
        }

        #endregion
    }
}