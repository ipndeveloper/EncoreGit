using NetSteps.Validation.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Validation.Conversion.Core.Model
{
    public class RecordSource : IRecordSource
    {
        public RecordSource(string schemaName, string tableName)
        {
            SchemaName = schemaName;
            TableName = tableName;
        }

        public string SchemaName { get; private set; }

        public string TableName { get; private set; }
    }
}
