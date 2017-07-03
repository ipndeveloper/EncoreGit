using WatiN.Core;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.PWS.Enroll
{
    public class PWS_Enroll_Culture_Control : Control<Div>
    {
        private SelectList _country, _language;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _country = Element.GetElement<SelectList>(new Param("CountryID"));
            _language = Element.GetElement<SelectList>(new Param("LanguageID"));
        }

        public PWS_Enroll_Culture_Control SelectCountry(Country.ID country)
        {
            _country.CustomSelectDropdownItem(country.ToPattern());
            return this;
        }

        public PWS_Enroll_Culture_Control SelectLanguage(Language.ID language)
        {
            _language.CustomSelectDropdownItem(language.ToPattern());
            return this;
        }

        public TPage ClickUpgrade<TPage>(int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            timeout = Element.GetElement<Link>(new Param("upgrade", AttributeName.ID.Href, RegexOptions.IgnoreCase)).CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }

        public TPage ClickEnrollNow<TPage>(int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            timeout = Element.GetElement<Link>(new Param("Button btnEnrollDistributor", AttributeName.ID.ClassName)).CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }

        public List<Country.ID> GetMarkets()
        {
            List<Country.ID> markets = new List<Country.ID>();
            for (int index = 0; index < _country.Options.Count; index++)
            {
                markets.Add(Country.Parse(_country.Options[index].CustomGetText()));
            }
            return markets;
        }
    }
}
