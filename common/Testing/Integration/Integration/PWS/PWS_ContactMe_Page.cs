using NetSteps.Testing.Integration;
using WatiN.Core;

namespace NetSteps.Testing.Integration.PWS
{
    /// <summary>
    /// Class related to Contact Me page.
    /// </summary>
    public class PWS_ContactMe_Page : PWS_Base_Page
    {
        #region Controls

        protected TextField txtFirstname;

        protected override void InitializeContents()
        {
            base.InitializeContents();

            this.txtFirstname = Document.GetElement<TextField>(new Param("FirstName"));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Is Contact Me page rendered.
        /// </summary>
         public override bool IsPageRendered()
        {
            return this.txtFirstname.Exists;
        }
        #endregion
    }
}