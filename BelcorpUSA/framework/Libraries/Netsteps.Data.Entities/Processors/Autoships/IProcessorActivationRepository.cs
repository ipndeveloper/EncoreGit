using System;
using System.Diagnostics.Contracts;

namespace NetSteps.Data.Entities.Processors.Autoships
{
	[ContractClass(typeof(Contracts.ProcessorActivationRepositoryContracts))]
	public interface IProcessorActivationRepository
	{
		void ActivateSite(int siteID);
		void DeactivateSite(int siteID);

		int GetNumberOfIntervalsUnpaid(int intervalTypeID, DateTime nextRunDate, DateTime origin);
		IAutoshipScheduleDTO GetActiveSchedule(int autoshipScheduleID);

		ISiteDTO LoadSite(int accountID, int autoshipOrderID);

		IIntervalTypeDTO LoadInterval(int intervalTypeID, DateTime runDate);
	}

	namespace Contracts
	{
		[ContractClassFor(typeof(IProcessorActivationRepository))]
		abstract class ProcessorActivationRepositoryContracts : IProcessorActivationRepository
		{
			public void ActivateSite(int siteID)
			{
				throw new NotImplementedException();
			}

			public void DeactivateSite(int siteID)
			{
				throw new NotImplementedException();
			}

			public int GetNumberOfIntervalsUnpaid(int intervalTypeID, DateTime nextRunDate, DateTime origin)
			{
				Contract.Requires<ArgumentNullException>(nextRunDate != null);
				Contract.Requires<ArgumentNullException>(origin != null);
				Contract.Requires<ArgumentException>(intervalTypeID > 0);

				throw new NotImplementedException();
			}

			public IAutoshipScheduleDTO GetActiveSchedule(int autoshipScheduleID)
			{
				throw new NotImplementedException();
			}

			public ISiteDTO LoadSite(int accountID, int autoshipOrderID)
			{
				throw new NotImplementedException();
			}

			public IIntervalTypeDTO LoadInterval(int intervalTypeID, DateTime runDate)
			{
				throw new NotImplementedException();
			}
		}
	}
}