using WatiN.Core;

namespace NetSteps.Testing.Integration.DWS.Communication
{
    /// <summary>
    /// Class related to DWS Communication page.
    /// </summary>
    public class DWS_Communication_Mailbox_Page : DWS_Communications_Base_Page
    {
        private Link _delete;
        private string _type;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _delete = Document.GetElement<Link>(new Param("deleteSelected"));
            _type = Util.Browser.Url.Split('=')[1];
        }

        /// <summary>
        /// Is Communication page rendered.
        /// </summary>
        /// <returns>True if rendered, else false.</returns>
         public override bool IsPageRendered()
        {
            return _delete.Exists;
        }

        public string Type
        {
            get { return _type; }
        }

    }
}