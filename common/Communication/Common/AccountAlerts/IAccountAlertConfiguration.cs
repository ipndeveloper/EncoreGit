using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common
{
    public interface IAccountAlertConfiguration
    {
        IEnumerable<KeyValuePair<Guid, Lazy<IAccountAlertProvider>>> GetDefaultProviders();
    }
}
