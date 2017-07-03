using System.Collections.Generic;
using WatiN.Core;
using WatiN.Core.Extras;

namespace NetSteps.Testing.Integration.DWS.Edit
{
    /// <summary>
    /// Controls and Ops related to DWS Edit choices page.
    /// </summary>
    public class DWS_Edit_Choices_Page : DWS_Base_Page
    {
        #region Edit Choice controls

        private Div divContentWYSIWYG;

        protected override void InitializeContents()
        {
            base.InitializeContents();

            // Choices.
            this.divContentWYSIWYG = Document.GetElement<Div>(new Param("content ContentEditor WYSIWYG", AttributeName.ID.ClassName));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Is Choices page rendered.
        /// </summary>
         public override bool IsPageRendered()
        {
            return this.divContentWYSIWYG.Exists;
        }

        #endregion
    }
}
