using NetSteps.Common.Attributes;
using System;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class SalesSourceSearchData
    {
        [TermName("Month")]
        public string Month { get; set; }

        [TermName("NoKit")]
        public string NoKit { get; set; }

        [TermName("Kit")]
        public String Kit { get; set; }
    }
}
