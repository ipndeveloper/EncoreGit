using System.Collections.Generic;
using NetSteps.Commissions.Common.Models;
using NetSteps.Data.Entities.Business;

namespace nsCore.Areas.Commissions.Models
{
    public class ManualBonusEntryModel
    {

        #region Constructors

        public ManualBonusEntryModel()
        {
            //For default model binding.
        }

        #endregion

        #region Properties

        public int OpenPeriod { get; set; }

        public List<ManualBonusEntrySearchData> Errors { get; set; }

        public bool IsValid { get; set; }
        public string Message { get; set; }

        #endregion

        #region Methods

        #endregion

    }
}