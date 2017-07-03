using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.Common;
using NetSteps.Validation.Common.Model;

namespace NetSteps.Validation.Common
{

    public interface IRecordRepository
    {
        IEnumerable<IRecord> RetrieveRecords(IEnumerable<object> recordIDs );
        IEnumerable<IRecord> RetrieveRecords(IRecordQuery query);
        IEnumerable<object> RetrieveRecordKeys(IRecordQuery query);
    }
}
