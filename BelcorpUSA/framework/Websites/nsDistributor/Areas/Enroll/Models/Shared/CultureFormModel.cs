using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Extensions;

namespace nsDistributor.Areas.Enroll.Models.Shared
{
    public class CultureFormModel
    {
        #region Values
        [NSRequired]
        [NSDisplayName("Country")]
        public virtual int? CountryID { get; set; }

        [NSRequired]
        [NSDisplayName("Language")]
        public virtual int? LanguageID { get; set; }
        #endregion

        #region Resources
        public virtual IEnumerable<SelectListItem> Countries { get; set; }
        public virtual IEnumerable<SelectListItem> Languages { get; set; }
        #endregion

        #region Infrastructure
        public CultureFormModel LoadValues(
            int countryID,
            int languageID)
        {
            this.CountryID = countryID;
            this.LanguageID = languageID;

            return this;
        }

        public CultureFormModel LoadResources(
            IEnumerable<Country> countries,
            IEnumerable<Language> languages)
        {
            this.Countries = countries
                .Select(x => new SelectListItem
                {
                    Text = x.GetTerm(),
                    Value = x.CountryID.ToString()
                })
                .OrderBy(x => x.Text);

            this.Languages = languages
                .Select(x => new SelectListItem
                {
                    Text = x.GetTerm(),
                    Value = x.LanguageID.ToString()
                })
                .OrderBy(x => x.Text);

            return this;
        }
        #endregion
    }
}