using NetSteps.Common.Attributes;
using System;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class ProductCreditBalanceSearchData
    {
        [TermName("BA_ID")]
        public string BA_ID { get; set; }

        [TermName("Name")]
        public string Name { get; set; }       

        [TermName("EffectiveDate")]
        public string EffectiveDate { get; set; }

        [TermName("EntryDescription")]
        public string EntryDescription { get; set; }

        [TermName("EntryReasonName")]
        public string EntryReasonName { get; set; }

        [TermName("EntryOriginName")]
        public string EntryOriginName { get; set; }

        [TermName("EntryTypeName")]
        public string EntryTypeName { get; set; }

        //[TermName("Movimiento")]
        //public string Movimiento  { get; set; }
        [TermName("Credit_Balance")]
        public string Credit_Balance { get; set; }

        [TermName("EndingBalance")]
        public string EndingBalance { get; set; }

        [TermName("Ticket")]
        public string Ticket { get; set; }

        [TermName("Order")]
        public string Order { get; set; }

        [TermName("Soporte")]
        public string Soporte { get; set; }

    }
}
