using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using NetSteps.Communication.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Foundation.Common;

namespace NetSteps.Communication.Services
{
    public class AccountAlertService : IAccountAlertService
    {
        protected readonly IAccountAlertProviderCollection _providers;
        protected readonly Func<ICommunicationContext> _contextFactory;

        public AccountAlertService(
            IAccountAlertProviderCollection providers,
            Func<ICommunicationContext> contextFactory)
        {
            Contract.Requires<ArgumentNullException>(providers != null);
            Contract.Requires<ArgumentNullException>(contextFactory != null);

            _providers = providers;
            _contextFactory = contextFactory;
        }

        public IAccountAlertProviderCollection Providers
        {
            get { return _providers; }
        }

        public IAccountAlertStub GetStub(int accountAlertId)
        {
            using (var context = _contextFactory())
            {
                var accountAlertStub = context.AccountAlerts
                    .Where(x => x.AccountAlertId == accountAlertId)
                    .Select(x => new
                    {
                        x.AccountAlertId,
                        x.ProviderKey
                    })
                    .ToList()
                    .Select(x =>
                    {
                        var tmp = Create.New<IAccountAlertStub>();
                        tmp.AccountAlertId = x.AccountAlertId;
                        tmp.ProviderKey = x.ProviderKey;
                        return tmp;
                    })
                    .FirstOrDefault();

                if (accountAlertStub == null)
                {
                    throw new Exception();
                }

                return accountAlertStub;
            }
        }

        public IPaginatedList<IAccountAlertStub> Search(IAccountAlertSearchParameters searchParameters)
        {
            using (var context = _contextFactory())
            {
                var query = context.AccountAlerts
                    .AsQueryable();

                // Do not include dismissed or expired alerts.
                query = query
                    .Where(x => x.DismissedDateUtc == null
                        && (x.ExpirationDateUtc == null || x.ExpirationDateUtc > DateTime.UtcNow)
                    );

                if (searchParameters.AccountId != null)
                {
                    query = query
                        .Where(x => x.AccountId == searchParameters.AccountId);
                }

                if (searchParameters.ProviderKeys != null
                    && searchParameters.ProviderKeys.Any())
                {
                    query = query
                        .Where(x => searchParameters.ProviderKeys.Contains(x.ProviderKey));
                }

                if (searchParameters.AccountAlertDisplayKindIds != null
                    && searchParameters.AccountAlertDisplayKindIds.Any())
                {
                    query = query
                        .Where(x => searchParameters.AccountAlertDisplayKindIds.Contains(x.AccountAlertDisplayKindId));
                }

                var totalCount = query.Count();

                var orderBy = searchParameters.DynamicOrderByString();
                if (string.IsNullOrWhiteSpace(orderBy))
                {
                    orderBy = "CreatedDateUtc";
                }

                return DynamicQueryable.OrderBy(query, orderBy)
                    .ApplyPagination(searchParameters)
                    .Select(x => new
                    {
                        x.AccountAlertId,
                        x.ProviderKey
                    })
                    .ToList()
                    .Select(x =>
                    {
                        var accountAlertStub = Create.New<IAccountAlertStub>();
                        accountAlertStub.AccountAlertId = x.AccountAlertId;
                        accountAlertStub.ProviderKey = x.ProviderKey;
                        return accountAlertStub;
                    })
                    .ToPaginatedList(
                        searchParameters,
                        totalCount
                    );
            }
        }

        public IList<IAccountAlert> GetAll()
        {
            using (var context = _contextFactory())
            {
                var providerKeys = _providers.Keys;

                return context.AccountAlerts
                    .Where(x => providerKeys.Contains(x.ProviderKey))
                    .Select(x => new
                    {
                        x.AccountAlertId,
                        x.ProviderKey
                    })
                    .ToList()
                    .GroupBy(x => x.ProviderKey, x => x.AccountAlertId)
                    .SelectMany(x => _providers[x.Key].GetBatch(x))
                    .ToList();
            }
        }
    }
}
