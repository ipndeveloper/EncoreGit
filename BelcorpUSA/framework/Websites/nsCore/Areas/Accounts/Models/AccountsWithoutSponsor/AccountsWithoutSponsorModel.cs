using System.Collections.Generic;
using NetSteps.Commissions.Common.Models;
using NetSteps.Data.Entities.Business;

namespace nsCore.Areas.Accounts.Models
{
    public class AccountsWithoutSponsorModel
    {

        #region Constructors

        public AccountsWithoutSponsorModel()
        {
            //For default model binding.
        }

        #endregion

        #region Properties

        public bool SponsorIDValidation { get; set; }
        public Dictionary<int, string> AviablePeriods { get; set; }

        #endregion

        #region Methods

        #endregion

    }
}