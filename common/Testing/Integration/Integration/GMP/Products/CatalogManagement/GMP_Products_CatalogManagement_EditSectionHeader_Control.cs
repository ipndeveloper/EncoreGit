using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Products.CatalogManagement
{
    public class GMP_Products_CatalogManagement_EditSectionHeader_Control : GMP_Products_CatalogManagement_BrowseSectionHeader_Control
    {
        private SelectList _language;

        protected override void  InitializeContents()
        {
 	         base.InitializeContents();
             _language = Element.SelectList("sLanguage");
        }

        public void SelectLanguage(int index)
        {
            _language.CustomSelectDropdownItem(index);
        }

        public void SelectLanguage(string language)
        {
            _language.CustomSelectDropdownItem(language);
        }
    }
}
