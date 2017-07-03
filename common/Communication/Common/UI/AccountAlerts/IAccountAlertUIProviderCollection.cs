using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Communication.Common;
using NetSteps.Foundation.Common;

namespace NetSteps.Communication.UI.Common
{
    public interface IAccountAlertUIProviderCollection : ILazyDictionary<Guid, IAccountAlertUIProvider>
    {
    }
}
