using WatiN.Core;

namespace NetSteps.Testing.Integration.DWS.Training
{
    /// <summary>
    /// Class related to controls and ops of DWS Training page.
    /// </summary>
    public class DWS_Documents_Page : DWS_Base_Page
    {
        SelectList _fileTypes;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _fileTypes = Document.SelectList("fileTypesSelectFilter");
        }

        public override bool IsPageRendered()
        {
            return _fileTypes.Exists;
        }
    }
}