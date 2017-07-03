using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Validation.Common.Model
{
    public interface IRecordSource
    {
        string SchemaName { get; }
        string TableName { get; }
    }
}
