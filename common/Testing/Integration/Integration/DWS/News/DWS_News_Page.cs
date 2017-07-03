using WatiN.Core;

namespace NetSteps.Testing.Integration.DWS.News
{
    /// <summary>
    /// Class related to controls and ops of DWS News page.
    /// </summary>
    public class DWS_News_Page : DWS_Base_Page
    {
        #region Controls.

        protected override void InitializeContents()
        {
            base.InitializeContents();        
        }

        #endregion

        #region Methods.

        /// <summary>
        /// Is News page rendered.
        /// </summary>
        /// <returns>True if rendered, else false.</returns>
         public override bool IsPageRendered()
        {
            return this.HeaderPageTitle.Contains("News");
        }

        #endregion
    }
}