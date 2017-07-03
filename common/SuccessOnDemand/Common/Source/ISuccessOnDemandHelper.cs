using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.SOD.Common
{
    public interface ISuccessOnDemandHelper
    {
        IAccountInfo CopyAccountInfo(int accountID);
        IResponse CreateOrUpdate(IAccountInfo acctInfo);
        void SaveDistID(string accountID, string distID);
    }
}
