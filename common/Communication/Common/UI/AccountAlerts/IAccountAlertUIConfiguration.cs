using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.UI.Common
{
    public interface IAccountAlertUIConfiguration
    {
        IEnumerable<KeyValuePair<Guid, Lazy<IAccountAlertUIProvider>>> GetDefaultProviders();
    }
}
