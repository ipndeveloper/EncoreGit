using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NetSteps.Foundation.Common;

namespace NetSteps.Communication.Common
{
    [ContractClass(typeof(Contracts.AccountAlertServiceContracts))]
    public interface IAccountAlertService
    {
        IAccountAlertProviderCollection Providers { get; }
        IAccountAlertStub GetStub(int accountAlertId);
        IPaginatedList<IAccountAlertStub> Search(IAccountAlertSearchParameters searchParameters);
        IList<IAccountAlert> GetAll();
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(IAccountAlertService))]
        internal abstract class AccountAlertServiceContracts : IAccountAlertService
        {
            IAccountAlertProviderCollection IAccountAlertService.Providers
            {
                get { throw new NotImplementedException(); }
            }

            IAccountAlertStub IAccountAlertService.GetStub(int accountAlertId)
            {
                Contract.Requires<ArgumentOutOfRangeException>(accountAlertId > 0);
                throw new NotImplementedException();
            }

            IPaginatedList<IAccountAlertStub> IAccountAlertService.Search(IAccountAlertSearchParameters searchParameters)
            {
                Contract.Requires<ArgumentNullException>(searchParameters != null);
                throw new NotImplementedException();
            }

            IList<IAccountAlert> IAccountAlertService.GetAll()
            {
                throw new NotImplementedException();
            }
        }
    }
}
