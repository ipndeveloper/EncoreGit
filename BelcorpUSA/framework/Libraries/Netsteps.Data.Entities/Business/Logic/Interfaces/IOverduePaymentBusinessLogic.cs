using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
    #region Comment

    /// <summary>
    /// @01: BR-CB-002 Contract to implement in BizLogic
    /// </summary>

    #endregion

    public interface IOverduePaymentBusinessLogic
    {

        OverduePaymentReport ExecuteProcess(int TypeProcess, string fileSequentialCode, ref string errorMessage);

        bool AlternativeProcess(string fileSequentialCode);

        bool LoadOverdueErrors(List<int> AccountNumbers);

    }
}
