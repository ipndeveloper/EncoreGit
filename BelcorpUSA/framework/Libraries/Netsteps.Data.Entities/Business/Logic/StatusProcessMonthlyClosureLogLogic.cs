using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Repositories.Interfaces;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class StatusProcessMonthlyClosureLogLogic
    {
        #region Private

        private static StatusProcessMonthlyClosureLogLogic instance;

        private static IStatusProcessMonthlyClosureLogRepository repositoryStatusProcessMonthlyClosureLog;

        #endregion

        #region Singleton

        private StatusProcessMonthlyClosureLogLogic() { }

        public static StatusProcessMonthlyClosureLogLogic Instance
        {
            get 
            {
                if (instance==null)
                {
                    instance = new StatusProcessMonthlyClosureLogLogic();
                    repositoryStatusProcessMonthlyClosureLog = new StatusProcessMonthlyClosureLogRepository();
                }
                return instance;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// List the Statuses
        /// </summary>
        /// <param name="LanguageID">Current User's languageID </param>
        /// <returns>Returns a generic list of StatusProcessMonthlyClosureSearchData class</returns>
        public List<StatusProcessMonthlyClosureLogSearchData> ListStatuses(int LanguageID)
        {
            var oList = repositoryStatusProcessMonthlyClosureLog.ListStatuses(LanguageID);
            return oList;
        }

        #endregion
    }
}
