using System.Collections.Generic;
using WatiN.Core;
using WatiN.Core.Extras;

namespace NetSteps.Testing.Integration.DWS.Edit
{
    /// <summary>
    /// Control and Ops related to DWS Edit Corporate only page.
    /// </summary>
    public class DWS_Edit_CorporateEdit_Page : DWS_Base_Page
    {
        #region Edit corporate only controls

        private Div divUploadAFile;

        protected override void InitializeContents()
        {
            base.InitializeContents();

            // Edit Corporate only.
            this.divUploadAFile = Document.GetElement<Div>(new Param("qq-upload-button emButton NS-qq-upload-button", AttributeName.ID.ClassName));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Is Corporate only page rendered.
        /// </summary>
         public override bool IsPageRendered()
        {
            return this.divUploadAFile.Exists;
        }

        #endregion
    }
}
