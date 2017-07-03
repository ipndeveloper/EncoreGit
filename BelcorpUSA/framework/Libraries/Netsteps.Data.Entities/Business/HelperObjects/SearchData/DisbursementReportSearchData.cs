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
    /// reason          : Description of each query column Disbursement Report
    /// modified    : 
    /// reason          :
    /// </summary>
    [Serializable]
    public class DisbursementReportSearchData
    {

        [TermName("PeriodID", "Period ID")]
        [Display(Name = "Period ID")]
        public int PeriodID { get; set; }

        [TermName("AccountID", "Account ID")]
        [Display(Name = "Account ID")]
        public int AccountID { get; set; }

        [TermName("Name", "Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [TermName("Amount", "Amount")]
        [Display(Name = "Amount")]
        public string Amount { get; set; }

        [TermName("DisbursementProfileID", "Disbursement Profile ID")]
        [Display(Name = "Disbursement Profile ID")]
        public int DisbursementProfileID { get; set; }

        [TermName("DisbursementTypeID", "Disbursement Type ID")]
        [Display(Name = "Disbursement Type ID")]
        public string DisbursementTypeID { get; set; }

        [TermName("DisbursementID", "Disbursement ID")]
        [Display(Name = "Disbursement ID")]
        public string DisbursementID { get; set; }

        [TermName("DisbursementStatusID", "Disbursement Status ID")]
        [Display(Name = "Disbursement Status ID")]
        public int DisbursementStatusID { get; set; }

        [TermName("DisbursementStatus", "Disbursement Status")]
        [Display(Name = "Disbursement Status")]
        public String DisbursementStatus { get; set; }

        [TermName("DisbursementDateUTC", "Disbursement Date UTC")]
        [Display(Name = "Disbursement Status ID")]
        public Nullable<DateTime> DisbursementDateUTC { get; set; }

        [TermName("DateModifiedUTC", "Date Modified UTC")]
        [Display(Name = "Date Modified UTC")]
        public Nullable<DateTime> DateModifiedUTC { get; set; }

        [TermName("EntryNotes", "Entry Notes")]
        [Display(Name = "Entry Notes", AutoGenerateField = false)]
        public String EntryNotes { get; set; }

        [TermName("EntryReason", "Entry Reason")]
        [Display(Name = "Entry Reason", AutoGenerateField = false)]
        public String EntryReason { get; set; }

        [TermName("EntryOrigin", "Entry Origin")]
        [Display(Name = "Entry Origin", AutoGenerateField = false)]
        public String EntryOrigin { get; set; }

        [TermName("EntryType", "Entry Type")]
        [Display(Name = "Entry Type", AutoGenerateField = false)]
        public String EntryType { get; set; }

        [TermName("EntryDescription", "Entry Description")]
        [Display(Name = "Entry Description", AutoGenerateField = false)]
        public String EntryDescription { get; set; }
    }

    public class DisbursementReportStatus
    {
        public int StatusID { get; set; }        
        public String Name { get; set; }
    }
}
