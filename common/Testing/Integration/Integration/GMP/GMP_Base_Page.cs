using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP
{
    public abstract class GMP_Base_Page : NS_Page
    {
        private GMP_GlobalNav_Control _globalNav;
        private GMP_GlobalUtilities_Control _globalUtilities;
        protected Div _content;
        protected Div _secHeader;
        protected UnorderedList _secNav;
        protected UnorderedList _subNav;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _globalNav = Control.CreateControl<GMP_GlobalNav_Control>(Util.Browser.GetElement<UnorderedList>(new Param("GlobalNav")));
            _globalUtilities = Control.CreateControl<GMP_GlobalUtilities_Control>(Document.GetElement<Div>(new Param("GlobalUtilities")));
            _content = Document.GetElement<Div>(new Param("ContentWrap"));
            _secHeader = Document.GetElement<Div>(new Param("SectionHeader", AttributeName.ID.ClassName));
            _secNav = Document.GetElement<Div>(new Param("SectionNav", AttributeName.ID.ClassName)).UnorderedList(Find.Any);
            _subNav = Document.UnorderedList("SubNav");

        }

        public GMP_GlobalNav_Control GlobalNav
        {
            get { return _globalNav; }
        }

        public GMP_GlobalUtilities_Control GlobalUtilities
        {
            get { return _globalUtilities; }
        }

        public string GetProperty(Param parameter, bool selectList)
        {
            string value;
            if (selectList)
                value = Document.GetElement<SelectList>(parameter).CustomGetSelectedItem();
            else
                value = Document.GetElement<TextField>(parameter).CustomGetText();
            return value;
        }

        public int? SelectProperty(Param parameter, int? selection = null, int? timeout = null, int min = 1)
        {
            return Document.GetElement<SelectList>(parameter).CustomSelectDropdownItem(selection, timeout, min);
        }

        public void SetProperty(Param parameter, string selection)
        {
            Document.GetElement<TextField>(parameter).CustomSetTextQuicklyHelper(selection);
        }

        public bool PropertyExists(Param parameter)
        {
            bool exist = true;
            if (!Document.GetElement<SelectList>(parameter).Exists)
                if (!Document.GetElement<TextField>(parameter).Exists)
                    exist = false;

            return exist;
        }

        public bool PropertyVisible(Param parameter)
        {
            bool visible = true;
            if (!Document.GetElement<SelectList>(parameter).IsVisible())
                if (!Document.GetElement<TextField>(parameter).IsVisible())
                    visible = false;

            return visible;
        }

        public string Title
        {
            get { return _secHeader.ElementWithTag("h2", Find.Any).CustomGetText(); }
        }
    }
}
