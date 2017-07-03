using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Repositories.Interfaces;
using NetSteps.Data.Entities.Repositories;
using System.Transactions;
using System.Threading;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Generated;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class MonthlyClosingLogic
    {
        #region Private

        private static MonthlyClosingLogic instance;

        private static IMonthlyClosingRepository repositoryMonthlyClosing;

        #endregion

        #region Singleton

        private MonthlyClosingLogic() { }

        public static MonthlyClosingLogic Instance
        {
            get
            {
                if (instance==null)
	            {
		            instance = new MonthlyClosingLogic();
                    repositoryMonthlyClosing = new MonthlyClosingRepository();

	            }
                return instance;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// List available plans
        /// </summary>
        /// <returns>Dictionary of available plans</returns>
        public Dictionary<string, string> ListAvailablePlans()
        {
            var dictionary = repositoryMonthlyClosing.ListAvailablePlans();
            return dictionary;
        }

        /// <summary>
        /// Get active period
        /// </summary>
        /// <returns></returns>
        public string GetActivePeriod() 
        {
            var result = repositoryMonthlyClosing.GetActivePeriod();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Plan"></param>
        /// <param name="Period"></param>
        /// <returns></returns>
        /*public bool ProcessCampaignMonthlyClosing(string Plan, string Period)
        {            
            using (var mainScope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    ///MLM-008
                    MonthlyClosureLogParameters oMonthlyClosureLog = new MonthlyClosureLogParameters { PlanID = Int32.Parse(Plan), PeriodID = Int32.Parse(Period)};
                    
                    var iResult = MonthlyClosureLogLogic.Instance.ExecMonthlyClosing(oMonthlyClosureLog);


                    var result = true;
                    if (result)
                    {
                        mainScope.Complete();
                        mainScope.Dispose();
                        return true;
                    }
                    else
                        return false;
                }
                catch (Exception)
                {
                    mainScope.Dispose();
                    return false;
                }
            }
        }*/

        public bool ProcessCampaignMonthlyClosing(string Plan, string Period)
        {
            //using (var mainScope = new TransactionScope(TransactionScopeOption.Required))
            //{
            try
            {
                var result = InitializePrepareNextCampaign(Convert.ToInt32(Plan), Convert.ToInt32(Period));

                //mainScope.Complete();
                //mainScope.Dispose();
                return true;
                ///MLM-008
                //var result = true;
                //if (result)
                //{
                //    mainScope.Complete();
                //    mainScope.Dispose();
                //    return true;
                //}
                //else
                //    return false;
            }
            catch (Exception)
            {
                //mainScope.Dispose();
                return false;
            }
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Plan"></param>
        /// <param name="Period"></param>
        /// <returns></returns>
        //Se Agrego en base a las fuentes de BelcorpUSA
        public bool ExecuteCampaignMonthlyClosing(string Plan, string Period)
        {
            //var transactionOptions = new TransactionOptions
            //{
            //    IsolationLevel = IsolationLevel.ReadCommitted
            //};
            //using (var mainScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            //{
            try
            {
                var result = ExecMonthlyClosing(Convert.ToInt32(Plan), Convert.ToInt32(Period));

                //mainScope.Complete();
                //mainScope.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                //mainScope.Dispose();
                return false;
            }
            //}

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Plan"></param>
        /// <param name="Period"></param>
        /// <returns></returns>
        public bool ExecuteCampaignMonthlyClosing( int LanguageID, string Plan, string Period)
        {
            
            var transactionOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            };
            using (var mainScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                var statusCode = ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.StatusProcessMonthlyClosureLog.CodeStatusProcessing);
                var mainProcessName = ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.MainProcesses.MainProcessMonthlyClosure);
                var currentMainProcessLogID = SaveMainProcess(LanguageID, int.Parse(Plan), int.Parse(Period), mainProcessName, DateTime.Now, ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.StatusProcessMonthlyClosureLog.CodeStatusProcessing));
                var currentSubProcessID = 0;
                var currentCodeSubProcess = "";
                var currentSubProcessName = "";
                var amountAffectedRows = 0;
                try
                {
                    currentSubProcessName = ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.SubProcess.CodeSubProcessPaymentTitle);
                    currentCodeSubProcess = "CodeSubProcessPaymentTitle";
                    currentSubProcessID = SaveSubProcessLog(LanguageID, currentMainProcessLogID, currentCodeSubProcess, currentSubProcessName, DateTime.Now, ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.StatusProcessMonthlyClosureLog.CodeStatusProcessing), "", "");
                    ///BR-MLM-004 
                    /// No desarrollado
                    amountAffectedRows = UpdateSubProcessLog(LanguageID, currentSubProcessID, DateTime.Now, ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.StatusProcessMonthlyClosureLog.CodeStatusFinished), "", "");
                    if (amountAffectedRows<=0){return false;}

                    currentSubProcessName = ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.SubProcess.CodeSubProcessCommissions);
                    currentCodeSubProcess = "CodeSubProcessCommissions";
                    currentSubProcessID = SaveSubProcessLog(LanguageID, currentMainProcessLogID, currentCodeSubProcess, currentSubProcessName, DateTime.Now, ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.StatusProcessMonthlyClosureLog.CodeStatusProcessing), "", "");

                    //[este proceso se debe llamar desde el SP "uspCommissionCalculateByPeriod" se ejecutará en el proceso diario batch] segun "BR-MLM-009 - Cierre mensual - Plan 2015"
                    //NetSteps.Data.Entities.Business.Logic.CommissionCalculateBusinessLogic.Instance.Calculate(int.Parse(Plan));

                    amountAffectedRows = UpdateSubProcessLog(LanguageID, currentSubProcessID, DateTime.Now, ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.StatusProcessMonthlyClosureLog.CodeStatusFinished), "", "");
                    if (amountAffectedRows <= 0) { return false; }

                    currentSubProcessName = ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.SubProcess.CodeSubProcessBonus);
                    currentCodeSubProcess = "CodeSubProcessBonus";
                    currentSubProcessID = SaveSubProcessLog(LanguageID, currentMainProcessLogID, currentCodeSubProcess, currentSubProcessName, DateTime.Now, ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.StatusProcessMonthlyClosureLog.CodeStatusProcessing), "", "");                    
                    
                    AdvanceBonus.InsAdvanceBonus(int.Parse(Period));

                    amountAffectedRows = UpdateSubProcessLog(LanguageID, currentSubProcessID, DateTime.Now, ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.StatusProcessMonthlyClosureLog.CodeStatusFinished), "", "");
                    if (amountAffectedRows <= 0) { return false; }

                    currentSubProcessName = ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.SubProcess.CodeSubProcessCareerTitle);
                    currentCodeSubProcess = "CodeSubProcessCareerTitle";
                    currentSubProcessID = SaveSubProcessLog(LanguageID, currentMainProcessLogID, currentCodeSubProcess, currentSubProcessName, DateTime.Now, ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.StatusProcessMonthlyClosureLog.CodeStatusProcessing), "", "");

                   
                    // se tiene que enviar el Plan 
                    NetSteps.Data.Entities.Business.TitleEstimate.Instance.CalculateCareerTitle(int.Parse(Plan));
                    
                    // se agrego el calculo de bono  Team Building y turbo Infinity y calculo de bono por titulo de pago
                    // segun BR-MLM-009 - Cierre mensual - Plan 2015

                    //[   BR-BO-023 - Team Building Bonus]
                    BonusLogic.Instance.CalcularBonusTeamBuilding(int.Parse(Period));

                    //[ BR-BO-024 - Turbo Infinity Bonus]
                    BonusLogic.Instance.CalcularBonuTurboInfinityByPeriod(int.Parse(Period));

                    //[ BR-PD-008 - BR-PD-008 Retail Profit Bonus]
                    BonusLogic.Instance.CalcularDiscountType(int.Parse(Period));


                    //[BR-MLM-004 - Cálculo de título de pago]
                    NetSteps.Data.Entities.Business.TitleEstimate.Instance.CalculatePaidAsTitle(int.Parse(Plan));
                        

                    amountAffectedRows = UpdateSubProcessLog(LanguageID, currentSubProcessID, DateTime.Now, ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.StatusProcessMonthlyClosureLog.CodeStatusFinished), "", "");
                    if (amountAffectedRows <= 0) { return false; }

                    currentSubProcessName = ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.SubProcess.CodeSubProcessNetworkCompression);
                    currentCodeSubProcess = "CodeSubProcessNetworkCompression";
                    currentSubProcessID = SaveSubProcessLog(LanguageID, currentMainProcessLogID, currentCodeSubProcess, currentSubProcessName, DateTime.Now, ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.StatusProcessMonthlyClosureLog.CodeStatusProcessing), "", "");
                    
                    NetworkPermanenceRulesBusinessLogic.Instance.ApplyNetworkPermanenceRules();

                    amountAffectedRows = UpdateSubProcessLog(LanguageID, currentSubProcessID, DateTime.Now, ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.StatusProcessMonthlyClosureLog.CodeStatusFinished), "", "");
                    if (amountAffectedRows <= 0) { return false; }

                    var result = UpdateMainProcess(LanguageID, currentMainProcessLogID, DateTime.Now, ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.StatusProcessMonthlyClosureLog.CodeStatusFinished));
                   
                    mainScope.Complete();
                    mainScope.Dispose();
                    return true;
                }
                catch (Exception ex)
                {
                    var result1 = UpdateStatusProcessToCanceled(currentMainProcessLogID, currentSubProcessID, ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.StatusProcessMonthlyClosureLog.CodeStatusCanceled));
                    var result2 = UpdateSubProcessLog(LanguageID, currentSubProcessID, DateTime.Now, ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.StatusProcessMonthlyClosureLog.CodeStatusFinishedError), "An error has ocurred in " +  MonthlyClosureDetailLogLogic.Instance.GetFailedSubprocessName(LanguageID, currentSubProcessName )+ " subprocess", ex.Message);
                    var result3 = UpdateMainProcess(LanguageID, currentMainProcessLogID, DateTime.Now, ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.StatusProcessMonthlyClosureLog.CodeStatusFinishedError));
                    mainScope.Dispose();
                    return false;
                }
            }
        }

        public bool ExecuteCampaignMonthlyClosing_Test(int LanguageID, string Plan, string Period)
        {
            var transactionOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            };
            using (var mainScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                var statusCode = ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.StatusProcessMonthlyClosureLog.CodeStatusProcessing);
                var mainProcessName = ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.MainProcesses.MainProcessMonthlyClosure);
                var currentMainProcessLogID = SaveMainProcess(LanguageID, int.Parse(Plan), int.Parse(Period), mainProcessName, DateTime.Now, ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.StatusProcessMonthlyClosureLog.CodeStatusProcessing));
                var currentSubProcessID = 0;
                var currentCodeSubProcess = "";
                var currentSubProcessName = "";
                var amountAffectedRows = 0;
                try
                {
                    currentSubProcessName = ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.SubProcess.CodeSubProcessPaymentTitle);
                    currentCodeSubProcess = "CodeSubProcessPaymentTitle";
                    currentSubProcessID = SaveSubProcessLog(LanguageID, currentMainProcessLogID, currentCodeSubProcess, currentSubProcessName, DateTime.Now, ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.StatusProcessMonthlyClosureLog.CodeStatusProcessing), "", "");
                    Thread.Sleep(1000);
                    amountAffectedRows = UpdateSubProcessLog(LanguageID, currentSubProcessID, DateTime.Now, ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.StatusProcessMonthlyClosureLog.CodeStatusFinished), "", "");
                    if (amountAffectedRows <= 0) { return false; }

                    currentSubProcessName = ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.SubProcess.CodeSubProcessCommissions);
                    currentCodeSubProcess = "CodeSubProcessCommissions";
                    currentSubProcessID = SaveSubProcessLog(LanguageID, currentMainProcessLogID, currentCodeSubProcess, currentSubProcessName, DateTime.Now, ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.StatusProcessMonthlyClosureLog.CodeStatusProcessing), "", "");
                    Thread.Sleep(1000);
                    amountAffectedRows = UpdateSubProcessLog(LanguageID, currentSubProcessID, DateTime.Now, ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.StatusProcessMonthlyClosureLog.CodeStatusFinished), "", "");
                    if (amountAffectedRows <= 0) { return false; }

                    currentSubProcessName = ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.SubProcess.CodeSubProcessBonus);
                    currentCodeSubProcess = "CodeSubProcessBonus";
                    currentSubProcessID = SaveSubProcessLog(LanguageID, currentMainProcessLogID, currentCodeSubProcess, currentSubProcessName, DateTime.Now, ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.StatusProcessMonthlyClosureLog.CodeStatusProcessing), "", "");
                    Thread.Sleep(30000);
                    amountAffectedRows = UpdateSubProcessLog(LanguageID, currentSubProcessID, DateTime.Now, ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.StatusProcessMonthlyClosureLog.CodeStatusFinished), "", "");
                    if (amountAffectedRows <= 0) { return false; }

                    currentSubProcessName = ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.SubProcess.CodeSubProcessCareerTitle);
                    currentCodeSubProcess = "CodeSubProcessCareerTitle";
                    currentSubProcessID = SaveSubProcessLog(LanguageID, currentMainProcessLogID, currentCodeSubProcess, currentSubProcessName, DateTime.Now, ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.StatusProcessMonthlyClosureLog.CodeStatusProcessing), "", "");
                    Thread.Sleep(1000);
                    amountAffectedRows = UpdateSubProcessLog(LanguageID, currentSubProcessID, DateTime.Now, ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.StatusProcessMonthlyClosureLog.CodeStatusFinished), "", "");
                    if (amountAffectedRows <= 0) { return false; }

                    currentSubProcessName = ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.SubProcess.CodeSubProcessNetworkCompression);
                    currentCodeSubProcess = "CodeSubProcessNetworkCompression";
                    currentSubProcessID = SaveSubProcessLog(LanguageID, currentMainProcessLogID, currentCodeSubProcess, currentSubProcessName, DateTime.Now, ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.StatusProcessMonthlyClosureLog.CodeStatusProcessing), "", "");
                    Thread.Sleep(1000);
                    amountAffectedRows = UpdateSubProcessLog(LanguageID, currentSubProcessID, DateTime.Now, ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.StatusProcessMonthlyClosureLog.CodeStatusFinished), "", "");
                    if (amountAffectedRows <= 0) { return false; }

                    var result = UpdateMainProcess(LanguageID, currentMainProcessLogID, DateTime.Now, ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.StatusProcessMonthlyClosureLog.CodeStatusFinished));
                    
                    mainScope.Complete();
                    mainScope.Dispose();
                    return true;
                }
                catch (Exception ex)
                {
                    var result1 = UpdateStatusProcessToCanceled(currentMainProcessLogID, currentSubProcessID, ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.StatusProcessMonthlyClosureLog.CodeStatusCanceled));
                    var result2 = UpdateSubProcessLog(LanguageID, currentSubProcessID, DateTime.Now, ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.StatusProcessMonthlyClosureLog.CodeStatusFinishedError), "An error has ocurred in " + currentSubProcessName + " subprocess", ex.Message);
                    var result3 = UpdateMainProcess(LanguageID, currentMainProcessLogID, DateTime.Now, ConstantsGenerated.StringEnum.GetStringValue(ConstantsGenerated.StatusProcessMonthlyClosureLog.CodeStatusFinishedError));
                    mainScope.Dispose();
                    return false;
                }
            }
        }

        //Se Agregar en base a las fuentes de USA para ejecucion de proceso de cierre
        private int InitializePrepareNextCampaign(int PlanId, int PeriodID)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    var oMainProcessLog = new MonthlyClosureLogParameters();
                    oMainProcessLog.PlanID = PlanId;
                    oMainProcessLog.PeriodID = PeriodID;

                    var result = MonthlyClosureLogLogic.Instance.InitializePrepareNextCampaign(oMainProcessLog);

                    scope.Complete();
                    scope.Dispose();
                    return result;
                }
                catch (Exception)
                {
                    scope.Dispose();
                    throw;
                }

            }
        }

        private int SaveMainProcess(int LanguageID, int PlanId, int PeriodID, string ProcessName, DateTime StarTime, string Result)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    var oMainProcessLog = new MonthlyClosureLogParameters();
                    oMainProcessLog.PlanID = PlanId;
                    oMainProcessLog.PeriodID = PeriodID;
                    oMainProcessLog.TermName = ProcessName;
                    oMainProcessLog.StarTime = StarTime;
                    oMainProcessLog.Result = Result;

                    var result = MonthlyClosureLogLogic.Instance.SaveMainProcess(oMainProcessLog, LanguageID);

                    scope.Complete();
                    scope.Dispose();
                    return result;
                }
                catch (Exception)
                {
                    scope.Dispose();
                    throw;
                }
                
            }             
        }

        private int UpdateMainProcess(int LanguageID, int currentMainProcessLogID, DateTime EndTime, string Result)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    var oMainProcessLog = new MonthlyClosureLogParameters();
                    oMainProcessLog.MonthlyClosureLogID = currentMainProcessLogID;
                    oMainProcessLog.EndTime = EndTime;
                    oMainProcessLog.Result = Result;

                    var result = MonthlyClosureLogLogic.Instance.UpdateMainProcess(oMainProcessLog, LanguageID);

                    scope.Complete();
                    scope.Dispose();
                    return result;
                }
                catch (Exception)
                {
                    scope.Dispose();
                    throw;
                }               
            } 
        }
        //Se Agrego en base a las fuentes de BelcorpUSA
        private int ExecMonthlyClosing(int PlanId, int PeriodID)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    var oMainProcessLog = new MonthlyClosureLogParameters();
                    oMainProcessLog.PlanID = PlanId;
                    oMainProcessLog.PeriodID = PeriodID;

                    var result = MonthlyClosureLogLogic.Instance.ExecMonthlyClosing(oMainProcessLog);

                    scope.Complete();
                    scope.Dispose();
                    return result;
                }
                catch (Exception)
                {
                    scope.Dispose();
                    throw;
                }

            }
        }

        private int SaveSubProcessLog(int LanguageID, int currentMainProcessLogID, string CodeSubProcess, string subProcessName, DateTime StarTime, string status, string Message, string Error)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    var oSubProcessLog = new MonthlyClosureDetailLogParameters();
                    oSubProcessLog.MonthlyClosureLogID = currentMainProcessLogID;
                    oSubProcessLog.TermName = subProcessName;
                    oSubProcessLog.CodeSubProcess = CodeSubProcess;
                    oSubProcessLog.StarTime = StarTime;
                    oSubProcessLog.CodeStatusName = status;
                    oSubProcessLog.MessageToShow = Message;
                    oSubProcessLog.RealError = Error;
                    var result = MonthlyClosureDetailLogLogic.Instance.SaveSubProcess(oSubProcessLog, LanguageID);

                    scope.Complete();
                    scope.Dispose();
                    return result;
                }
                catch (Exception)
                {
                    scope.Dispose();
                    throw;
                }                
            }
        }

        private int UpdateSubProcessLog(int LanguageID, int currentSubProcessLogID, DateTime EndTime, string status, string Message, string Error)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    var oSubProcessLog = new MonthlyClosureDetailLogParameters();
                    oSubProcessLog.MonthlyClosureDetailLogID = currentSubProcessLogID;
                    oSubProcessLog.EndTime = EndTime;
                    oSubProcessLog.CodeStatusName = status;
                    oSubProcessLog.MessageToShow = Message;
                    oSubProcessLog.RealError = Error;
                    var result = MonthlyClosureDetailLogLogic.Instance.UpdateSubProcess(oSubProcessLog, LanguageID);

                    scope.Complete();
                    scope.Dispose();
                    return result;
                }
                catch (Exception)
                {
                    scope.Dispose();
                    throw;
                }                
            }
        }

        private int UpdateStatusProcessToCanceled(int currentMainProcessLogID, int currentSubProcessLogID, string status)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    var oSubProcessLog = new MonthlyClosureDetailLogParameters();
                    oSubProcessLog.MonthlyClosureLogID = currentMainProcessLogID;
                    oSubProcessLog.MonthlyClosureDetailLogID = currentSubProcessLogID;
                    oSubProcessLog.CodeStatusName = status;
                    var result = MonthlyClosureDetailLogLogic.Instance.UpdateStatusProcessToCanceled(oSubProcessLog);

                    scope.Complete();
                    scope.Dispose();
                    return result;
                }
                catch (Exception)
                {
                    scope.Dispose();
                    throw;
                }
            }
        }

        #endregion
    }
}
