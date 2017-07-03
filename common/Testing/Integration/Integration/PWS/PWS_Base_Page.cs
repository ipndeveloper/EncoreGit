using WatiN.Core;
using WatiN.Core.Extras;
//using NetSteps.Testing.Integration.PWS.Shop;
using System.Text.RegularExpressions;
using NetSteps.Testing.Integration.PWS.Edit;


namespace NetSteps.Testing.Integration.PWS
{
    public abstract class PWS_Base_Page : NS_Page
    {
        Div _header;
        private PWS_GlobalFooter_Control _footer;
        protected Div _secondaryNav;
        protected Div _content;
        protected Head _head;
        private PWS_Edit_EditMode_Control _editModeControl;
        private ControlCollection<PWS_Edit_Edit_Control> _editContentControls;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _header = Document.GetElement<Div>(new Param("TopPane", AttributeName.ID.Id, RegexOptions.None));
            _footer = Document.GetElement<Div>(new Param("Footer")).As<PWS_GlobalFooter_Control>();
            _secondaryNav = Document.GetElement<Div>(new Param("SecondaryNav"));
            _content = Document.GetElement<Div>(new Param("ContentWrap"));
            _head = Document.GetElement<Head>();
            _editModeControl = Document.GetElement<UnorderedList>(new Param("NS-inlineNav NS-FR editModeControls", AttributeName.ID.ClassName)).As<PWS_Edit_EditMode_Control>();
            _editContentControls = Document.Divs.Filter(Find.ByClass("EditableContent")).As<PWS_Edit_Edit_Control>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TControl">PWS_GlobalHeader_Control  or CWS_GlobalHeader_Control</typeparam>
        /// <returns></returns>
        public TControl GlobalHeader<TControl>() where TControl : Header_Control, new()
        {
            return _header.As<TControl>();
        }

        public PWS_GlobalHeader_Control GlobalHeader()
        {
            return _header.As<PWS_GlobalHeader_Control>();
        }

        public PWS_GlobalFooter_Control GlobalFooter
        {
            get { return _footer; }
        }

        public ControlCollection<PWS_Edit_Edit_Control> EditContent
        {
            get { return _editContentControls; }
        }

        public PWS_Edit_EditMode_Control EditMode
        {
            get { return _editModeControl; }
        }
    }
}
