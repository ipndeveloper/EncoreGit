using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Repositories.Interfaces;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class MonthlyClosureLogLogic
    {
        #region Private

        private static MonthlyClosureLogLogic instance;

        private static IMonthlyClosureLogRepository repositoryMonthlyClosure;

        #endregion

        #region Singleton

        private MonthlyClosureLogLogic() { }

        public static MonthlyClosureLogLogic Instance
        { 
            get
            {
                if (instance==null)
	            {
		            instance = new MonthlyClosureLogLogic();
                    repositoryMonthlyClosure = new MonthlyClosureLogRepository();
	            }
                return instance;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Saves the main process
        /// </summary>
        /// <param name="oMonthlyClosure">As input of MonthlyClosureLogParameters class</param>
        /// <returns>returns the last id generated</returns>
        public int SaveMainProcess(MonthlyClosureLogParameters oMonthlyClosureLog, int LanguageID)
        {
            return repositoryMonthlyClosure.SaveMainProcess(oMonthlyClosureLog, LanguageID);
        }

        /// <summary>
        /// Updates the main process
        /// </summary>
        /// <param name="oMonthlyClosure"></param>
        /// <returns>returns the amount of affected rows</returns>
        public int UpdateMainProcess(MonthlyClosureLogParameters oMonthlyClosureLog, int LanguageID)
        {
            return repositoryMonthlyClosure.UpdateMainProcess(oMonthlyClosureLog, LanguageID);
        }

        public int ExecMonthlyClosing(MonthlyClosureLogParameters oMonthlyClosureLog)
        {
            return repositoryMonthlyClosure.ExecMonthlyClosing(oMonthlyClosureLog);
        }

        public int InitializePrepareNextCampaign(MonthlyClosureLogParameters oMonthlyClosureLog)
        {
            return repositoryMonthlyClosure.InitializePrepareNextCampaign(oMonthlyClosureLog);
        }
        #endregion
    }
}
