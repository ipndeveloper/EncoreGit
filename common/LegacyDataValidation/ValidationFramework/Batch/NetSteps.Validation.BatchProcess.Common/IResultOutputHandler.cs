using NetSteps.Validation.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Validation.BatchProcess.Common
{
    public interface IResultOutputHandler
    {
        void Handle(IRecord record);
        void Close();
    }
}
