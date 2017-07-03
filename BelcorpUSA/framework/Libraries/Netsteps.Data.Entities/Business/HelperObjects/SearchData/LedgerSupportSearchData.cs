using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    public class LedgerSupportSearchData
    {
        public int AccountID { get; set; }
        public string EntryDescription { get; set; }
        public int EntryReasonID { get; set; }
        public int EntryOriginID { get; set; }
        public int EntryTypeID { get; set; }
        public int UserID { get; set; }
        public decimal EntryAmount { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public int BonusTypeID { get; set; }
        public int BonusValueID { get; set; }
        public int CurrencyTypeID { get; set; }
        public int OrderID { get; set; }
        public int? OrderPaymentID { get; set; }
        public string ETLNaturalKey { get; set; }
        public string ETLHash { get; set; }
        public string ETLPhase { get; set; }
        public DateTime ETLDate { get; set; }
        public int SupportTicketID { get; set; } 
    }
}
