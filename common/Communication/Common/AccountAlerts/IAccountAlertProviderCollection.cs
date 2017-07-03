using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Foundation.Common;

namespace NetSteps.Communication.Common
{
    public interface IAccountAlertProviderCollection : ILazyDictionary<Guid, IAccountAlertProvider>
    {
    }
}
