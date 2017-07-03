using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Common.Interfaces;

namespace NetSteps.Data.Entities.Business
{
    public class AccountSponsorSearchParameters
    {        
        public int AccountID { get; set; }
        public int PeriodID { get; set; }
        public int AccountStatusID { get; set; }
        public int TitleID { get; set; }

        public int NewPeriodID { get; set; }
        public int NewSponsorID { get; set; }
        public int NewSponsorTitleID { get; set; }
        public AccountSponsorLogSearchParameters LogParameters { get; set; }

        //To get Titles Translations
        public int LanguageID { get; set; }

        #region [AccountSponsor Insert Properties]

        public int SponsorID { get; set; }
        public int AccountSponsorTypeID { get; set; }
        public int Position { get; set; }
        public int ModifiedByUserID { get; set; }  

        #endregion
    }
}
