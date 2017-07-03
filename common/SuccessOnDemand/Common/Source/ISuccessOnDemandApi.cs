using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.SOD.Common;

namespace NetSteps.SOD.Common
{
    /// <summary>
    /// Interface wrapper for the Success on Demand API.
    /// </summary>
    public interface ISuccessOnDemandApi
    {
        /// <summary>
        /// Creates an account in SOD.
        /// </summary>
        /// <param name="acctInfo">the account's info</param>
        /// <returns></returns>
        IResponse CreateAccount(IAccountInfo acctInfo);
        IResponse UpdateAccount(IAccountInfo acctInfo);
        bool Login(ILoginInfo loginInfo);
    }
}
