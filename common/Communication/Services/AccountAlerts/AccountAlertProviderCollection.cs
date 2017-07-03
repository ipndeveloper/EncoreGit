using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NetSteps.Communication.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Foundation.Common;

namespace NetSteps.Communication.Services
{
    public class AccountAlertProviderCollection
        : LazyDictionary<Guid, IAccountAlertProvider>, IAccountAlertProviderCollection
    {
        public AccountAlertProviderCollection()
        {

        }

        public AccountAlertProviderCollection(IAccountAlertConfiguration configuration)
            : base(configuration.GetDefaultProviders())
        {
            Contract.Requires<ArgumentNullException>(configuration != null);
        }

        public AccountAlertProviderCollection(IEnumerable<KeyValuePair<Guid, Lazy<IAccountAlertProvider>>> defaultProviders)
            : base(defaultProviders)
        {
            Contract.Requires<ArgumentNullException>(defaultProviders != null);
        }
    }
}
