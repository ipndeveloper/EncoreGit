using System.Diagnostics.Contracts;

namespace NetSteps.Events.EmailEventTemplate.Common
{
	[ContractClass(typeof(IEmailEventTemplateRepositoryContracts))]
	public interface IEmailEventTemplateRepository
	{
		int GetEmailTemplateIdByEventTypeID(int eventTypeID);
	}

	[ContractClassFor(typeof(IEmailEventTemplateRepository))]
	internal abstract class IEmailEventTemplateRepositoryContracts : IEmailEventTemplateRepository
	{
		public int GetEmailTemplateIdByEventTypeID(int eventTypeID)
		{
			Contract.Requires(eventTypeID > 0);
			return default(int);
		}
	}
}
