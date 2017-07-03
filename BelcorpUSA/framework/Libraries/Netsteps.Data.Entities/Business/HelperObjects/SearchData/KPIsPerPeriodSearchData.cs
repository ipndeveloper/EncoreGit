using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class KPIsPerPeriodSearchData
    {
        [TermName("CommissionReportPeriodID")]                                                              
        public string PeriodID { get; set; }                                  
                                                                         
        [TermName("CommissionReportAccountID")]                          
        public string AccountID { get; set; }                                  
                                                                         
        [TermName("CommissionReportAccountName")]                        
        public string AccountName { get; set; }                                 
                                                                         
        [TermName("CommissionReportSponsorID")]                          
        public string SponsorID { get; set; }                            
                                                                         
        [TermName("CommissionReportSponsorName")]                        
        public string SponsorName { get; set; }                        
                                                                         
        [TermName("CommissionReportPaidAsCurrentMonth")]                 
        public string PaidAsCurrentMonth { get; set; }                                  
                                                                         
        [TermName("CommissionReportCareerTitle")]                        
        public string CareerTitle { get; set; }                             
                                                                         
        [TermName("CommissionReportPQV")]                                
        public string PQV { get; set; }                             
                                                                         
        [TermName("CommissionReportPCV")]                                
        public string PCV { get; set; }

        [TermName("CommissionReportDQV")]
        public string DQV { get; set; }

        [TermName("CommissionReportCQL")]
        public string CQL { get; set; }

        [TermName("CommissionReportTitle1Legs")]
        public string Title1Legs { get; set; }

        [TermName("CommissionReportTitle2Legs")]
        public string Title2Legs { get; set; }

        [TermName("CommissionReportTitle3Legs")]
        public string Title3Legs { get; set; }

        [TermName("CommissionReportTitle4Legs")]
        public string Title4Legs { get; set; }

        [TermName("CommissionReportTitle5Legs")]
        public string Title5Legs { get; set; }

        [TermName("CommissionReportTitle6Legs")]
        public string Title6Legs { get; set; }

        [TermName("CommissionReportTitle7Legs")]
        public string Title7Legs { get; set; }

        [TermName("CommissionReportTitle8Legs")]
        public string Title8Legs { get; set; }

        [TermName("CommissionReportTitle9Legs")]
        public string Title9Legs { get; set; }

        [TermName("CommissionReportTitle10Legs")]
        public string Title10Legs { get; set; }

        [TermName("CommissionReportTitle11Legs")]
        public string Title11Legs { get; set; }

        [TermName("CommissionReportTitle12Legs")]
        public string Title12Legs { get; set; }

        [TermName("CommissionReportTitle13Legs")]
        public string Title13Legs { get; set; }

        [TermName("CommissionReportTitle14Legs")]
        public string Title14Legs { get; set; }

    }
}
