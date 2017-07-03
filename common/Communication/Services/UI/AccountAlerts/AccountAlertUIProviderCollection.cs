using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NetSteps.Communication.Services;
using NetSteps.Communication.UI.Common;
using NetSteps.Foundation.Common;

namespace NetSteps.Communication.UI.Services
{
    public class AccountAlertUIProviderCollection
        : LazyDictionary<Guid, IAccountAlertUIProvider>, IAccountAlertUIProviderCollection
    {
        public AccountAlertUIProviderCollection()
        {

        }

        public AccountAlertUIProviderCollection(IAccountAlertUIConfiguration configuration)
            : base(configuration.GetDefaultProviders())
        {
            Contract.Requires<ArgumentNullException>(configuration != null);
        }

        public AccountAlertUIProviderCollection(IEnumerable<KeyValuePair<Guid, Lazy<IAccountAlertUIProvider>>> defaultProviders)
            : base(defaultProviders)
        {
            Contract.Requires<ArgumentNullException>(defaultProviders != null);
        }
    }
}
