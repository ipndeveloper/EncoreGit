using WatiN.Core;
using System.Threading;

namespace NetSteps.Testing.Integration.DWS.Performance
{
    public class DWS_Performance_GraphicalDownLine_Page : DWS_Performance_Base_Page
    {
        #region Controls

        private TextField txtSearchGraphicalFile;
        private Link lnkSearchGraphicalFile;
        private Div divAccountsGraphTree;

        protected override void InitializeContents()
        {
            base.InitializeContents();

            this.txtSearchGraphicalFile = Document.GetElement<TextField>(new Param("txtSearch"));
            this.lnkSearchGraphicalFile = Document.GetElement<Link>(new Param("btnGo"));
            this.divAccountsGraphTree = Document.GetElement<Div>(new Param("infovis-canvaswidget"));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Is Graphical down line page rendered.
        /// </summary>
         public override bool IsPageRendered()
        {
            return this.txtSearchGraphicalFile.Exists;
        }

        /// <summary>
        /// Search for a graphical down line customer.
        /// </summary>
        /// <param name="customerId">Customer Id.</param>
        /// <param name="suggestionName">Customer name from suggestion results.</param>
         public void SearchGraphicalDownlineCustomer(string customerId, string suggestionName, int? timeout = null)
        {
            this.txtSearchGraphicalFile.CustomSetTextHelper(customerId);
            Util.CustomSelectEnrollerFromSuggestion(suggestionName);
            this.lnkSearchGraphicalFile.CustomClick(timeout);
            //Util.WaitForBrowserStrong();
        }

        /// <summary>
        /// Is graphical down line shown.
        /// </summary>
        /// <param name="customerName">Customer for which graphical down line to be shown.</param>
        /// <returns>True if shown, else false.</returns>
        public bool IsGraphicalDownLineShown(string customerName)
        {
            Link graphicalDownLine = Document.GetElement<Link>(new Param(customerName, AttributeName.ID.InnerText));

            return graphicalDownLine.Exists;
        }

        /// <summary>
        /// Is Accounts Graphical Tree shown.
        /// </summary>
        /// <returns>True if shown, else false.</returns>
        public bool IsAccountsGraphicalTreeShown(int delay = 1)
        {
            //Thread.Sleep(delay * 1000);
            Document.CustomWaitForSpinners();
            return this.divAccountsGraphTree.Exists;
        }

        #endregion

        #region Validation Methods


        #endregion
    }
}