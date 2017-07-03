using NetSteps.Common.Attributes;
using System;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class EnrollmentsSearchData
    {
        [TermName("Month")]
        public string Month { get; set; }

        [TermName("State")]
        public string State { get; set; }

        [TermName("NewEnrollments")]
        public String NewEnrollments { get; set; }
    }
}
