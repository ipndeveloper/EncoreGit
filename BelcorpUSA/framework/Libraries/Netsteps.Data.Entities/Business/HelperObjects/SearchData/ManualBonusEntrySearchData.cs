using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class ManualBonusEntrySearchData
    {
        public int RowNumber { get; set; }
        public bool Period { get; set; }
        public bool BonusType { get; set; }
        public bool Account { get; set; }
        public bool BonusAmount { get; set; }
        public bool Duplicated { get; set; }

        public int PeriodID { get; set; }
        public int BonusTypeID { get; set; }
        public int AccountID { get; set; }
        public decimal BonusAmountVal { get; set; }
    }
}
