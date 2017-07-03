using WatiN.Core;
using WatiN.Core.Extras;
using NetSteps.Testing.Integration.DWS.Edit;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.DWS
{
    public abstract class DWS_Base_Page : NS_Page
    {
        protected Div _content;
        private DWS_GlobalNav_Control _nav;
        private DWS_UserInfo_Control _userInfo;
        protected UnorderedList _sectionNavList;
        protected Div divPageTitle;
        private DWS_Edit_EditMode_Control _editModeControl;
        private ControlCollection<DWS_Edit_Edit_Control> _editContentControls;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _content = Document.GetElement<Div>(new Param("ContentContainer"));
            _nav = Control.CreateControl<DWS_GlobalNav_Control>(Util.Browser.GetElement<UnorderedList>(new Param("mainNav", AttributeName.ID.ClassName, RegexOptions.None)));
            _userInfo = Document.GetElement<Div>(new Param("UserInfo")).As<DWS_UserInfo_Control>();
            this.divPageTitle = Document.GetElement<Div>(new Param("PageTitle", AttributeName.ID.ClassName));
            _sectionNavList = Document.GetElement<UnorderedList>(new Param("flatList", AttributeName.ID.ClassName));
            _editModeControl = Document.GetElement<UnorderedList>(new Param("NS-inlineNav NS-FR editModeControls", AttributeName.ID.ClassName)).As<DWS_Edit_EditMode_Control>();
            _editContentControls = Document.Divs.Filter(Find.ByClass("EditableContent")).As<DWS_Edit_Edit_Control>();
        }

        public DWS_GlobalNav_Control GlobalNavigation
        {
            get { return _nav; }
        }

        public DWS_UserInfo_Control UserInfo
        {
            get { return _userInfo; }
        }

        public ControlCollection<DWS_Edit_Edit_Control> EditContent
        {
            get { return _editContentControls; }
        }

        public DWS_Edit_EditMode_Control EditMode
        {
            get { return _editModeControl; }
        }

        /// <summary>
        /// Get page header title.
        /// </summary>
        public string HeaderPageTitle
        {
            get
            {
                if (!divPageTitle.CustomGetText().Equals(""))
                    return divPageTitle.CustomGetText();
                else
                    return divPageTitle.InnerHtml; // this returns the text if it's hidden in a child element
            }
        }
    }
}
