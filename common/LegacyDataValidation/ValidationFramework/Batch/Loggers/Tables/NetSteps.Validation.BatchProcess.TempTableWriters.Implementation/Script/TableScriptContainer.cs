using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Validation.BatchProcess.TempTableWriters.Implementation.Script
{
    public class TableScriptContainer
    {
        public TableScriptContainer(string createScript, string commitScript)
        {
            CreateScript = createScript;
            CommitScript = commitScript;
        }
        public string CreateScript { get; private set; }
        public string CommitScript { get; private set; }
    }
}
