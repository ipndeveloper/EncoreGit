using System.Collections.Generic;
using NetSteps.Data.Entities;

namespace nsDistributor.Areas.Enroll.Models.Shared
{
    public class EnrollmentTypeModel
    {
        // Resources
        public short? AccountTypeID { get; set; }
        public bool Resume { get; set; }
        public string FormTitle { get; set; }
        public bool ShowLoginLink { get; set; }
        public string LoginText { get; set; }
        public bool ShowUpgradeLink { get; set; }
        public string UpgradeText { get; set; }
        public string UpgradeUrl { get; set; }

        // Models
        public virtual CultureFormModel CultureForm { get; set; }

        // Infrastructure
        public EnrollmentTypeModel()
        {
            CultureForm = new CultureFormModel();
        }

        public EnrollmentTypeModel LoadResources(
            short accountTypeID,
            bool resume,
            string formTitle,
            bool showLoginLink,
            string loginText,
            bool showUpgradeLink,
            string upgradeText,
            string upgradeUrl,
            int countryID,
            IEnumerable<Country> countries,
            int languageID,
            IEnumerable<Language> languages)
        {
            AccountTypeID = accountTypeID;
            Resume = resume;
            FormTitle = formTitle;
            ShowLoginLink = showLoginLink;
            LoginText = loginText;
            ShowUpgradeLink = showUpgradeLink;
            UpgradeText = upgradeText;
            UpgradeUrl = upgradeUrl;

            CultureForm
                .LoadValues(
                    countryID,
                    languageID)                
                .LoadResources(
                    countries,
                    languages);

            return this;
        }
    }
}