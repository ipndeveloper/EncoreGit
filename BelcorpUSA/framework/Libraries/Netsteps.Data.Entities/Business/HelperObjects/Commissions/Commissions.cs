using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Commissions
{
    public class Commissions
    {
        /// <summary>
        /// The currently selected plan
        /// </summary>
        private CommissionsPlan _currentPlan;
        public CommissionsPlan CurrentPlan
        {
            get
            {
                if (_currentPlan == null && Plans.Count > 0)
                    _currentPlan = Plans[0];

                return _currentPlan;
            }
            set
            {
                _currentPlan = value;
            }
        }

        /// <summary>
        /// The currently selected period
        /// </summary>
        private CommissionsPeriod _currentPeriod;
        public CommissionsPeriod CurrentPeriod
        {
            get
            {
                if (_currentPeriod == null && Periods.Count > 0)
                    _currentPeriod = Periods[0];

                return _currentPeriod;
            }
            set
            {
                _currentPeriod = value;
            }
        }

        /// <summary>
        /// The current list of processes based on the selected plan and period
        /// </summary>
        private List<CommissionsProcess> _currentActiveProcesses;
        public List<CommissionsProcess> CurrentActiveProcesses
        {
            get
            {
                if (Plans.Count > 0 && Periods.Count > 0)
                {
                    // use the private variable so we don't hit the database more than we need to
                    _currentActiveProcesses = Adapter.GetProcessHistory(CurrentPeriod.PeriodID, false, false);
                    // set the RunInProgress flag to tell the UI to disable or enable the buttons to run commissions
                    if (_currentActiveProcesses.Where(x => x.Status == CommissionsProcessStatus.InProgress).ToList().Count > 0)
                        RunInProgress = true;
                    else
                        RunInProgress = false;

                }
                else
                {
                    _currentActiveProcesses = new List<CommissionsProcess>();
                }

                return _currentActiveProcesses;
            }
            set
            {
                _currentActiveProcesses = value;
            }
        }

        /// <summary>
        /// The current list of manual processes based on the selected plan and period
        /// </summary>
        private List<CommissionsProcess> _currentManualProcesses;
        public List<CommissionsProcess> CurrentManualProcesses
        {
            get
            {
                if (CurrentPeriod != null)
                    _currentManualProcesses = Adapter.GetManualProcesses(CurrentPeriod.PeriodID);
                else
                    _currentManualProcesses = new List<CommissionsProcess>();
                return _currentManualProcesses;
            }
            set
            {
                _currentManualProcesses = value;
            }
        }

        /// <summary>
        /// A list of possible plans
        /// </summary>
        private List<CommissionsPlan> _plans;
        public List<CommissionsPlan> Plans
        {
            get
            {
                if (_plans == null)
                    _plans = Adapter.GetPlans();

                return _plans;
            }
            set
            {
                _plans = value;
            }
        }

        /// <summary>
        /// A list of periods based on the currently selected plan
        /// </summary>
        private List<CommissionsPeriod> _periods;
        public List<CommissionsPeriod> Periods
        {
            get
            {
                if (Plans.Count > 0 && (_periods == null || _periods.Count < 1))
                    _periods = Adapter.GetPeriods(CurrentPlan.PlanID);

                return _periods;
            }
            set
            {
                _periods = value;
            }
        }

        ///// <summary>
        ///// The commissions ui adapter, used to run the commissions stored procs
        ///// </summary>
        //TODO: Commissions Refactor - find a new way to access stuffs.
        //private ICommissionsUiRepository _repository;
        //public ICommissionsUiRepository Adapter { get { return _repository; } }

        /// <summary>
        /// The most recent date that commissions was run
        /// </summary>
        public DateTime? LastRun
        {
            get
            {
                // use the private variable when we can so we don't hit the database very often
                if (_currentActiveProcesses == null)
                    _currentActiveProcesses = CurrentActiveProcesses;

                if (_currentActiveProcesses.Count > 0)
                    return _currentActiveProcesses.Max(x => x.LastCompleted);
                else
                    return DateTime.Today;
            }
        }

        /// <summary>
        /// A bit in the database that affects how earnings are displayed to the client's reps
        /// </summary>
        public bool DisplayEarnings
        {
            get
            {
                if (CurrentPeriod != null)
                    return Adapter.ToggleDisplayEarnings(CurrentPeriod.PeriodID, null);
                else
                    return true;
            }
            set
            {
                if (CurrentPeriod != null)
                    Adapter.ToggleDisplayEarnings(CurrentPeriod.PeriodID, value);
            }
        }

        /// <summary>
        /// A flag to tell the ui that a commissions run is still in progress
        /// </summary>

        private bool _runInProgress;
        public bool RunInProgress
        {
            get
            {
                // it could take up to 1 minute to start a commissions run
                if (DateTime.Now <= RunStarted.AddMinutes(1))
                    _runInProgress = true;

                return _runInProgress;
            }
            set
            {
                _runInProgress = value;
            }
        }

        /// <summary>
        /// The time that a commissions run started.  The commissions run will not start right away, 
        /// it runs on a scheduled task which will fire once per minute.  
        /// We need to disable the interface for at least one minute after a commissions run 
        /// is started to ensure that multiple commissions runs will not occur at the same time.
        /// </summary>
        public DateTime RunStarted { get; set; }

        /// <summary>
        /// The message to display telling the ui the status of the current commissions run
        /// </summary>
        public string PercentCompleteMsg
        {
            get
            {
                string result = "";
                if (RunInProgress)
                {
                    // Get a list of processes which have not yet completed
                    List<CommissionsProcess> processes = CurrentActiveProcesses.Where(x => x.Status == CommissionsProcessStatus.Complete || x.Status == CommissionsProcessStatus.Skipped).ToList();
                    decimal percentComplete = 0;
                    if (processes.Count > 0)
                    {
                        // calculate the percent of the processes which have completed
                        result = "In Progress: ";
                        percentComplete = ((decimal)processes.Count / (decimal)CurrentActiveProcesses.Count);
                        result += percentComplete.ToString("P");
                    }
                }
                else
                {
                    result = "Completed: " + (LastRun.HasValue ? LastRun.Value.ToLongDateString() : "Never");
                }
                return result;
            }
        }

        /// <summary>
        /// The constructor for the commissions object
        /// </summary>
        /// 
        public Commissions() : this(null)
        {
        }

        //public Commissions(ICommissionsUiRepository repository)
        //{
        //    _repository = repository ?? Create.New<ICommissionsUiRepository>();
        //    RunInProgress = false;
        //}

        //public static usp_get_performancelandingwidgets_Result GetPerformanceWidgetsData(int accountId, int periodId)
        //{
        //    var performanceResults = new CommissionsUiRepository().GetPerformanceWidgetData(accountId, periodId);
        //    return performanceResults;
        //}

        //public static List<uspGetKPIsForAccount_Result> GetPerformanceOverviewWidgetData(int accountId, int periodId)
        //{
        //    var performanceResults = new CommissionsUiRepository().GetPerformanceOverviewWidgetData(accountId, periodId);
        //    return performanceResults;
        //}
    }

    /// <summary>
    /// The commissions period object
    /// </summary>
    public class CommissionsPeriod
    {
        public Int32 PeriodID { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Closed { get; set; }
    }

    /// <summary>
    /// A list of statuses for a commissions process
    /// </summary>
    public enum CommissionsProcessStatus
    {
        Pending = 1,
        Skipped = 2,
        InProgress = 3,
        Complete = 4,
        Error = 5
    }
}
