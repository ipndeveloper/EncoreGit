using System.Collections.Generic;
using NetSteps.Commissions.Common.Models;
using NetSteps.Data.Entities.Business;

namespace nsCore.Areas.Accounts.Models
{
    public class EditSponsorModel
    {

        #region Constructors

        public EditSponsorModel()
        {
            //For default model binding.
        }

        #endregion

        #region Properties

        public int OpenPeriodID { get; set; }
        public AccountSponsorSearchData Account { get; set; }
        public AccountSponsorSearchData AccountSponsor { get;  set; }
        public Dictionary<string, string> AviablePeriods { get; set; }

        #endregion

        #region Methods

        #endregion

    }
}