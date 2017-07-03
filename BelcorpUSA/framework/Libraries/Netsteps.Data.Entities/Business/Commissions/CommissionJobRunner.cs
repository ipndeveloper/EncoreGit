using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Repositories;
using Core = NetSteps.Encore.Core;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Commissions
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CommissionJobRunner : BaseCommissionJobRunner
    {

        #region Members

        private ICommissionsUiRepository _repository;

        #endregion

        #region Constructor

        public CommissionJobRunner() : this(null)
        {
        }

		public CommissionJobRunner(ICommissionsUiRepository commissionsRepo)
        {
            _repository = commissionsRepo ?? Create.New<ICommissionsUiRepository>();
        }

        #endregion

        #region Public Methods

		public IEnumerable<CommissionRunType> GetCommissionRunTypes()
		{
			return _repository.GetCommissionRunTypes();
		}

		public CommissionRunType GetCommissionRunTypeByID(int commissionRunTypeID)
		{
			return _repository.GetCommissionRunTypeByID(commissionRunTypeID);
		}

		public IEnumerable<CommissionJob> GetCommissionJobs(int planID)
		{
			return _repository.GetCommissionJobs(planID);
		}

		public CommissionJobStatus GetCommissionJobStatus(string jobName)
		{
			return _repository.GetCommissionJobStatus(jobName);
		}

		public RunOutput GetCommissionJobOutput(string jobName)
		{
			return _repository.GetCommissionJobOutput(jobName);
		}

		public bool StartCommissionJob(string jobName)
		{
			return _repository.StartCommissionJob(jobName);
		}

        #endregion

        #region Protected Methods



        #endregion

    }
}
