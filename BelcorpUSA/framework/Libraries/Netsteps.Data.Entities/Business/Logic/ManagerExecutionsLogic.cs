using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Repositories.Interfaces;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class ManagerExecutionsLogic
    {
        #region Private

        private static ManagerExecutionsLogic instace;

        private static IManagerExecutionsRepository repositoryManagerExecutions;

        #endregion

        #region Singleton

        private ManagerExecutionsLogic() { }

        public static ManagerExecutionsLogic Instance
        {
            get 
            {
                if (instace==null)
                {
                    instace = new ManagerExecutionsLogic();
                    repositoryManagerExecutions = new ManagerExecutionsRepository();
                }
                return instace;
            }
        }

        #endregion

        /// <summary>
        /// Returns the subprocesses
        /// </summary>
        /// <param name="LanguageID">current language</param>
        /// <returns>A generic list of SubProcessStatusSearchData class</returns>
        public List<SubProcessStatusSearchData> ListSubProcessStatus(int LanguageID, int status, int page, int pageSize, string column, string order)
        {
            return repositoryManagerExecutions.ListSubProcessStatus(LanguageID, status, page, pageSize, column, order);
        }

        /// <summary>
        /// Get an especific failed subprocess 
        /// </summary>
        /// <param name="MonthlyClosureDetailLogID">subprocess's id</param>
        /// <returns>retorns a FailedSubProcessSearchData object</returns>
        public FailedSubProcessSearchData GetFailedSubProcess(int LanguageID, int MonthlyClosureDetailLogID)
        {
            return repositoryManagerExecutions.GetFailedSubProcess(LanguageID, MonthlyClosureDetailLogID);                      
        }

         /// <summary>
        /// Get an especific failed subprocess 
        /// </summary>
        /// <param name="MonthlyClosureDetailLogID">subprocess's id</param>
        /// <returns>retorns a FailedSubProcessPersonalIndicatorSearchData object</returns>
        public FailedSubProcessPersonalIndicatorSearchData GetFailedSubProcess_PI(int LanguageID, int MonthlyClosureDetailLogID)
        {
            return repositoryManagerExecutions.GetFailedSubProcess_PI(LanguageID, MonthlyClosureDetailLogID);                      
        }
       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LanguageID"></param>
        /// <param name="MonthlyClosureLogID"></param>
        /// <returns></returns>
        public bool Reprocess(string Type, int LanguageID, int id)
        {
            try
            {
                if (Type == "MC")
                {
                    var datos = repositoryManagerExecutions.GetPlanAndPeriod(id);
                    var resutl = MonthlyClosingLogic.Instance.ExecuteCampaignMonthlyClosing(LanguageID, datos.PlanID.ToString(), datos.PeriodID.ToString());
                    //var resutl = MonthlyClosingLogic.Instance.ExecuteCampaignMonthlyClosing_Test(LanguageID, datos.PlanID.ToString(), datos.PeriodID.ToString());
                    if (resutl)
                        return true;
                    else
                        return false;
                }
                else
                {
                    var datos = repositoryManagerExecutions.GetOderAndOrderStatus(id);
                    var order = new Order();
                    order.UpdatePersonalIndicator(datos.OrderId, short.Parse(datos.OrderStatusID.ToString()));
                    
                    return true;
                }
               
            }
            catch (Exception) {return false;}
        }

        public List<MainProcessesDetailSearchData> ListMainProcessesDetail(int LanguageID, int status, int page, int pageSize, string column, string order)
        {
            return repositoryManagerExecutions.ListMainProcessesDetail(LanguageID, status, page, pageSize, column, order);
        }
    }
}
