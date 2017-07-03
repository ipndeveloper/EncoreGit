using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Models;
using NetSteps.Foundation.Common;

namespace NetSteps.Communication.UI.Common
{
    [ContractClass(typeof(Contracts.AccountAlertUIServiceContracts))]
    public interface IAccountAlertUIService
    {
        IAccountAlertUIProviderCollection Providers { get; }
        IPaginatedList<IAccountAlertMessageModel> GetMessages(IAccountAlertUISearchParameters searchParameters, ILocalizationInfo localizationInfo);
        IPaginatedList<IAccountAlertModalModel> GetModals(IAccountAlertUISearchParameters searchParameters, ILocalizationInfo localizationInfo);
        void Dismiss(int accountAlertId, int accountId);
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(IAccountAlertUIService))]
        internal abstract class AccountAlertUIServiceContracts : IAccountAlertUIService
        {
            IAccountAlertUIProviderCollection IAccountAlertUIService.Providers
            {
                get { throw new NotImplementedException(); }
            }

            IPaginatedList<IAccountAlertMessageModel> IAccountAlertUIService.GetMessages(IAccountAlertUISearchParameters searchParameters, ILocalizationInfo localizationInfo)
            {
                Contract.Requires<ArgumentNullException>(searchParameters != null);
                Contract.Requires<ArgumentNullException>(localizationInfo != null);
                throw new NotImplementedException();
            }

            IPaginatedList<IAccountAlertModalModel> IAccountAlertUIService.GetModals(IAccountAlertUISearchParameters searchParameters, ILocalizationInfo localizationInfo)
            {
                Contract.Requires<ArgumentNullException>(searchParameters != null);
                Contract.Requires<ArgumentNullException>(localizationInfo != null);
                throw new NotImplementedException();
            }

            void IAccountAlertUIService.Dismiss(int accountAlertId, int accountId)
            {
                Contract.Requires<ArgumentOutOfRangeException>(accountAlertId > 0);
                Contract.Requires<ArgumentOutOfRangeException>(accountId > 0);
                throw new NotImplementedException();
            }
        }
    }
}
