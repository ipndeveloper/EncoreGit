using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    #region Modifications
    // @01 BR-CB-002 GYS: Añadiendo metodos requeridos por el diseño tecnico
    #endregion

    public interface IOverduePaymentsRepository
    {

        #region Modifications @01
        OverduePaymentReport RegularProcess(int fileSequentialCode, ref string errorMessage);
        OverduePaymentReport AlternativeProcess(int fileSequentialCode, ref string errorMessage);

        bool LoadOverdueErrors(List<int> AccountNumbers);
        #endregion

    }
}
