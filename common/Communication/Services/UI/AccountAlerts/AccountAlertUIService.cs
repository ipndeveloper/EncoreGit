using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Models;
using NetSteps.Communication.Common;
using NetSteps.Communication.UI.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Foundation.Common;

namespace NetSteps.Communication.UI.Services
{
    public class AccountAlertUIService : IAccountAlertUIService
    {
        protected readonly IAccountAlertUIProviderCollection _providers;
        protected readonly IAccountAlertService _accountAlertService;

        public AccountAlertUIService(
            IAccountAlertUIProviderCollection providers,
            IAccountAlertService accountAlertService)
        {
            Contract.Requires<ArgumentNullException>(providers != null);
            Contract.Requires<ArgumentNullException>(accountAlertService != null);

            _providers = providers;
            _accountAlertService = accountAlertService;
        }

        public IAccountAlertUIProviderCollection Providers
        {
            get { return _providers; }
        }

        public IPaginatedList<IAccountAlertMessageModel> GetMessages(
            IAccountAlertUISearchParameters searchParameters,
            ILocalizationInfo localizationInfo)
        {
            var innerSearchParameters = CreateInnerSearchParameters(
                searchParameters,
                (int)CommunicationConstants.AccountAlertDisplayKind.Message
            );

            return Search(
                innerSearchParameters,
                (provider, accountAlertIds) => provider.GetMessages(accountAlertIds, localizationInfo)
            );
        }

        public IPaginatedList<IAccountAlertModalModel> GetModals(
            IAccountAlertUISearchParameters searchParameters,
            ILocalizationInfo localizationInfo)
        {
            var innerSearchParameters = CreateInnerSearchParameters(
                searchParameters,
                (int)CommunicationConstants.AccountAlertDisplayKind.Modal
            );

            return Search(
                innerSearchParameters,
                (provider, accountAlertIds) => provider.GetModals(accountAlertIds, localizationInfo)
            );
        }

        public void Dismiss(int accountAlertId, int accountId)
        {
            var accountAlertStub = _accountAlertService.GetStub(accountAlertId);

            if (!_providers.ContainsKey(accountAlertStub.ProviderKey))
            {
                throw new Exception("AccountAlertUI provider key not found: " + accountAlertStub.ProviderKey.ToString());
            }

            _providers[accountAlertStub.ProviderKey].Dismiss(accountAlertId, accountId);
        }

        protected IAccountAlertSearchParameters CreateInnerSearchParameters(
            IAccountAlertUISearchParameters searchParameters,
            int accountAlertDisplayKindId)
        {
            Contract.Requires<ArgumentNullException>(searchParameters != null);

            var innerSearchParameters = Create.New<IAccountAlertSearchParameters>();
            innerSearchParameters.AccountId = searchParameters.AccountId;
            innerSearchParameters.ProviderKeys = _providers.Keys;
            innerSearchParameters.AccountAlertDisplayKindIds = new[] { accountAlertDisplayKindId };
            innerSearchParameters.PageIndex = searchParameters.PageIndex;
            innerSearchParameters.PageSize = searchParameters.PageSize;
            innerSearchParameters.OrderBy = searchParameters.OrderBy;
            innerSearchParameters.OrderByDescending = searchParameters.OrderByDescending;

            return innerSearchParameters;
        }

        protected IPaginatedList<T> Search<T>(
            IAccountAlertSearchParameters innerSearchParameters,
            Func<IAccountAlertUIProvider, IEnumerable<int>, IEnumerable<T>> modelSelector)
            where T : IAccountAlertKey
        {
            Contract.Requires<ArgumentNullException>(innerSearchParameters != null);
            Contract.Requires<ArgumentNullException>(modelSelector != null);

            var accountAlertStubs = _accountAlertService
                .Search(innerSearchParameters);

            var models = accountAlertStubs
                .GroupBy(x => x.ProviderKey, x => x.AccountAlertId)
                .SelectMany(x => modelSelector(_providers[x.Key], x));

            // Join to preserve ordering.
            return (
                from accountAlertStub in accountAlertStubs
                join model in models on accountAlertStub.AccountAlertId equals model.AccountAlertId
                select model
            ).ToPaginatedList(
                accountAlertStubs.PageIndex,
                accountAlertStubs.PageSize,
                accountAlertStubs.TotalCount
            );
        }
    }
}
