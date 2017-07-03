using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    /// <summary>
    /// author           : mescobar
    /// company         : CSTI - Peru
    /// create        : 12/18/2015
    /// reasons          : Descripcion de cada columna de la consulta Managment LedgerSearch 
    /// modified    : 
    /// reason          :
    /// </summary>
    [Serializable]
    public class ManagmentLedgerSearchData
    {
        [TermName("AccountID")]
        [Display(Name = "Account ID")]
        public int AccountID { get; set; }

        [TermName("ConsultantName", "Consultant Name")]
        [Display(Name = "Consultant Name")]
        public String ConsultantName { get; set; }

        [TermName("PeriodID", "Period ID")]
        [Display(Name = "Period ID")]
        public String PeriodID { get; set; }

        [TermName("BonusType")]
        [Display(Name = "Bonus Type")]
        public String BonusType { get; set; }

        [TermName("EntryAmount", "Entry Amount")]
        [Display(Name = "Entry Amount")]
        public String EntryAmount { get; set; }

        [TermName("EndingBalance", "Ending Balance")]
        [Display(Name = "Ending Balance")]
        public String EndingBalance { get; set; }

        [TermName("EntryDate", "Entry Date")]
        [Display(Name = "Entry Date")]
        public Nullable<DateTime> EntryDate { get; set; }

        [TermName("EffectiveDate", "Effective Date")]
        [Display(Name = "Effective Date")]
        public Nullable<DateTime> EffectiveDate { get; set; }

        [TermName("UserID", "UserID")]
        [Display(Name = "User ID")]
        public String UserID { get; set; }

        [TermName("EntryNotes", "Entry Notes")]
        [Display(Name = "Entry Notes")]
        public String EntryNotes { get; set; }

        [TermName("EntryReason")]
        [Display(Name = "Entry Reason")]
        public String EntryReason { get; set; }

        [TermName("EntryOrigin", "Entry Origin")]
        [Display(Name = "Entry Origin")]
        public String EntryOrigin { get; set; }

        [TermName("EntryType", "Entry Type")]
        [Display(Name = "Entry Type")]
        public String EntryType { get; set; }

        [TermName("EntryDescription", "Entry Description")]
        [Display(Name = "Entry Description")]
        public String EntryDescription { get; set; }

        [TermName("DisbursementID", "Disbursement ID")]
        [Display(Name = "Disbursement ID")]
        public String DisbursementID { get; set; }

        [TermName("DisbursementStatus", "Disbursement Status")]
        [Display(Name = "Disbursement Status")]
        public String DisbursementStatusTermName { get; set; }
    }

    public class ManagmenteLedgerPeriod
    {
        public int PeriodID { get; set; }
    }

    public class ManagmenteLedgerBonusType
    {
        public Int16 BonusTypeID { get; set; }
        public string Name { get; set; }
    }

    public class ManagmenteLedgerEntryReason
    {
        public Int16  EntryReasonID { get; set; }
        public string Name { get; set; }
    }

    public class ManagmenteLedgerEntryOrigin
    {
        public Int16  EntryOriginID { get; set; }
        public string Name { get; set; }
    }

    public class ManagmenteLedgerEntryType
    {
        public Int16 EntryTypeID { get; set; }
        public string Name { get; set; }
    }
}
