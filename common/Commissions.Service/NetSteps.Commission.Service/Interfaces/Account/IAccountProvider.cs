using NetSteps.Commissions.Common.Models;
using NetSteps.Core.Cache;

namespace NetSteps.Commissions.Service.Interfaces.Account
{
    public interface IAccountProvider : ICache<int, IAccount>
    {
        IAccount AddAccount(IAccount account);
    }
}
