using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Web.Mvc;
using NetSteps.Common.Models;

namespace NetSteps.Communication.UI.Common
{
    [ContractClass(typeof(Contracts.AccountAlertUIProviderContracts))]
    public interface IAccountAlertUIProvider
    {
        IEnumerable<IAccountAlertMessageModel> GetMessages(IEnumerable<int> accountAlertIds, ILocalizationInfo localizationInfo);
        IEnumerable<IAccountAlertModalModel> GetModals(IEnumerable<int> accountAlertIds, ILocalizationInfo localizationInfo);
        void Dismiss(int accountAlertId, int accountId);
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(IAccountAlertUIProvider))]
        internal abstract class AccountAlertUIProviderContracts : IAccountAlertUIProvider
        {
            IEnumerable<IAccountAlertMessageModel> IAccountAlertUIProvider.GetMessages(IEnumerable<int> accountAlertIds, ILocalizationInfo localizationInfo)
            {
                Contract.Requires<ArgumentNullException>(accountAlertIds != null);
                Contract.Requires<ArgumentNullException>(localizationInfo != null);
                throw new NotImplementedException();
            }

            IEnumerable<IAccountAlertModalModel> IAccountAlertUIProvider.GetModals(IEnumerable<int> accountAlertIds, ILocalizationInfo localizationInfo)
            {
                Contract.Requires<ArgumentNullException>(accountAlertIds != null);
                Contract.Requires<ArgumentNullException>(localizationInfo != null);
                throw new NotImplementedException();
            }

            void IAccountAlertUIProvider.Dismiss(int accountAlertId, int accountId)
            {
                Contract.Requires<ArgumentOutOfRangeException>(accountAlertId > 0);
                Contract.Requires<ArgumentOutOfRangeException>(accountId > 0);
                throw new NotImplementedException();
            }
        }
    }
}
