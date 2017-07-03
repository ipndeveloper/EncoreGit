using NetSteps.Common.Attributes;
using System;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class ReportAccountKPIDetailSearchData
    {
        [TermName("AccountKPIDetailID")]
        public Int32 AccountKPIDetailID { get; set; }

        [TermName("DownlineID")]
        public Int32 DownlineID { get; set; }

        [TermName("DownlineName")]
        public String DownlineName { get; set; }

        [TermName("Level")]
        public Int32 Level { get; set; }

        [TermName("Generation")]
        public Int32 Generation { get; set; }

        [TermName("CareerTitle")]
        public String CareerTitle { get; set; }

        [TermName("DownlinePaidAsTitle")]
        public String DownlinePaidAsTitle { get; set; }

        [TermName("PQV")]
        public Decimal PQV { get; set; }

        [TermName("DQV")]
        public Decimal DQV { get; set; } 

    }
}
