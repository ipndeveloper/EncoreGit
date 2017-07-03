using WatiN.Core;

namespace NetSteps.Testing.Integration.DWS.Edit
{
    public class DWS_Edit_Edit_Control : Control<Div>
    {
        private Link _edit;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _edit = Element.GetElement<Link>(new Param("Edit", AttributeName.ID.ClassName));
        }

        public bool IsControlRendered
        {
            get { return _edit.Exists; }
        }

        public DWS_Edit_CorporateEdit_Page ClickEdit(int? timeout = null, bool pageRequired = true)
        {
            timeout = _edit.CustomClick(timeout);
            return Util.GetPage<DWS_Edit_CorporateEdit_Page>(timeout, pageRequired);
        }
    }
}
