using System;

namespace NetSteps.Data.Entities.EntityModels
{
    public class EarningsTotal
    {
        public Int32 Index { get; set; }
        public Int32 BonusTypeID { get; set; }
        public String Name { get; set; }
        public String BonusCode { get; set; }
        public Decimal CV { get; set; }
        public Decimal Percentage { get; set; }
        public Decimal Porcentaje { get; set; }
        public Decimal BonusValue { get; set; }
        public Decimal BonusValueAnt { get; set; } 

    }
}
