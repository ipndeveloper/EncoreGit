using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public class BonusValue
    {
        /// <summary>
        /// Obtiene o establece BonusValueID
        /// </summary>
        public int BonusValueID { get; set; }

        public int BonusTypeID { get; set; }

        public int AccountID { get; set; }

        public decimal? BonusAmount { get; set; }

        public int CurrencyTypeID { get; set; }

        public decimal? CorpBonusAmount { get; set; }

        public int? CorpCurrencyTypeID { get; set; }

        public int PeriodID { get; set; }

        public int? CountryID { get; set; }

        public DateTime DateModified { get; set; }
    }
}
