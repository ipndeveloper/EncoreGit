using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Validation.Common
{
    public interface IRecordQuery
    {
        int MaximumBufferRecords { get; set; }
        IQueryable GetRecords();
        string GetWhereClauseString(string recordAlias);
    }
}
