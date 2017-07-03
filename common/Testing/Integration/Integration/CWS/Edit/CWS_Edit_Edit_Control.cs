using WatiN.Core;

namespace NetSteps.Testing.Integration.CWS.Edit
{
    public class CWS_Edit_Edit_Control : Control<Div>
    {
        private Link _edit;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _edit = Element.GetElement<Link>(new Param("Edit ui-draggable", AttributeName.ID.ClassName));
        }

        public bool IsControlRendered
        {
            get { return _edit.Exists; }
        }
    }
}
