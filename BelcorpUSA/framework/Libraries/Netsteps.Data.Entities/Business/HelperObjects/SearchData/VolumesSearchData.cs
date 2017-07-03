using NetSteps.Common.Attributes;
using System;

namespace NetSteps.Data.Entities.Business
{
    public class VolumesSearchData
    {
        [TermName("AccountNumber")]
        public string AccountNumber { get; set; }

        [TermName("Name")]
        public string Name { get; set; }

        [TermName("QV")]
        public decimal QV { get; set; }

        [TermName("CV")]
        public decimal CV { get; set; }

    }
}
