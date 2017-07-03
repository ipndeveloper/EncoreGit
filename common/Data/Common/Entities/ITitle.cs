using System;
using System.Linq;

namespace NetSteps.Data.Common.Entities
{
    public interface ITitle
    {
        int TitleID { get; set; }
        int SortOrder { get; set; }
        string TermName { get; set; }
        string TitleCode { get; set; }
        bool Active { get; set; }
    }
}
