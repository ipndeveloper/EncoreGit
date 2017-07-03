using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Repositories.Interfaces;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class MonthlyClosureDetailLogLogic
    {
        #region Private

        private static MonthlyClosureDetailLogLogic instance;

        private static IMonthlyClosureDetailLogRepository repositoryMonthlyClosureDetailLog;

        #endregion 

        #region "Singleton"

        private MonthlyClosureDetailLogLogic() { }

        public static MonthlyClosureDetailLogLogic Instance
        {
            get
            {
                if (instance==null)
	            {
                    instance = new MonthlyClosureDetailLogLogic();
                    repositoryMonthlyClosureDetailLog = new MonthlyClosureDetailLogRepository();
	            }
                return instance;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Saves the sub process
        /// </summary>
        /// <param name="oMonthlyClosureDetail">As input of MonthlyClosureDetailLogParameters class</param>
        /// <returns>returns the last id generated</returns>
        public int SaveSubProcess(MonthlyClosureDetailLogParameters oMonthlyClosureDetail, int LanguageID)
        {
            return repositoryMonthlyClosureDetailLog.SaveSubProcess(oMonthlyClosureDetail, LanguageID);
        }

        /// <summary>
        /// Updates the sub process
        /// </summary>
        /// <param name="oMonthlyClosureDetail">As input of MonthlyClosureDetailLogParameters class</param>
        /// <returns>returns the amount of affected rows</returns>
        public int UpdateSubProcess(MonthlyClosureDetailLogParameters oMonthlyClosureDetail, int LanguageID)
        {
            return repositoryMonthlyClosureDetailLog.UpdateSubProcess(oMonthlyClosureDetail, LanguageID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oMonthlyClosureDetail"></param>
        /// <returns></returns>
        public int UpdateStatusProcessToCanceled(MonthlyClosureDetailLogParameters oMonthlyClosureDetail)
        {
            return repositoryMonthlyClosureDetailLog.UpdateStatusProcessToCanceled(oMonthlyClosureDetail);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LanguageID"></param>
        /// <param name="CodeSubprocess"></param>
        /// <returns></returns>
        public string GetFailedSubprocessName(int LanguageID, string CodeSubprocess)
        {
            return repositoryMonthlyClosureDetailLog.GetFailedSubprocessName(LanguageID, CodeSubprocess);
        }
        #endregion
    }
}
