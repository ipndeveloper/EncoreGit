using WatiN.Core;
using WatiN.Core.Extras;
using System.Text.RegularExpressions;
using NetSteps.Testing.Integration.CWS.Edit;

namespace NetSteps.Testing.Integration.CWS
{
    public abstract class CWS_Base_Page : NS_Page
    {
        private CWS_Edit_EditMode_Control _editModeControl;
        private ControlCollection<CWS_Edit_Edit_Control> _editContentControls;
        protected Div _content;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _editModeControl = Document.GetElement<UnorderedList>(new Param("NS-inlineNav NS-FR editModeControls", AttributeName.ID.ClassName)).As<CWS_Edit_EditMode_Control>();
            _editContentControls = Document.Divs.Filter(Find.ByClass("EditableContent")).As<CWS_Edit_Edit_Control>();
            _content = Document.GetElement<Div>(new Param("ContentWrap"));
        }

        public ControlCollection<CWS_Edit_Edit_Control> EditContent
        {
            get { return _editContentControls; }
        }

        public CWS_Edit_EditMode_Control EditMode
        {
            get { return _editModeControl; }
        }
    }
}
