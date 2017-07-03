using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Business.Logic.Interfaces;

namespace NetSteps.Data.Entities.Business.Logic
{
    #region Comment

    /// <summary>
    /// @01: BR-CB-002 BizLogic
    /// </summary>
    
    #endregion

    public class OverduePaymentBusinessLogic : IOverduePaymentBusinessLogic
    {

        #region Instance

        private static IOverduePaymentBusinessLogic _instance;

        public static IOverduePaymentBusinessLogic Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new OverduePaymentBusinessLogic();

                return _instance;
            }
        }


        #endregion


        public OverduePaymentReport ExecuteProcess(int TypeProcess, string fileSequentialCode, ref string errorMessage)
        {
            OverduePaymentReport report = new OverduePaymentReport();

            int fileCode;
            bool canCast = Int32.TryParse(fileSequentialCode, out fileCode);

            if (canCast && fileSequentialCode.Length <= 5)
            {
                if (TypeProcess == 1)
                    report = new OverduePaymentsRepository().RegularProcess(fileCode, ref errorMessage);
                else
                    report = new OverduePaymentsRepository().AlternativeProcess(fileCode, ref errorMessage);
            }
            else
                errorMessage = "Invalid File Code";

            return report;
        }

        public bool AlternativeProcess(string fileSequentialCode)
        {
            throw new NotImplementedException();
        }

        public bool LoadOverdueErrors(List<int> AccountNumbers)
        {
            if (AccountNumbers != null && AccountNumbers.Count() > 0)
            {
                return new OverduePaymentsRepository().LoadOverdueErrors(AccountNumbers);
            }

            return false;
        }

    }
}
