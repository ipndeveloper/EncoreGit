using System;
using System.Collections.Generic;
using NetSteps.Data.Entities.Base;
using NetSteps.Data.Entities.Business.Logic.Interfaces;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities
{
	//[Serializable]
	public partial class MailDomain : EntityBusinessBase<MailDomain, Int32, IMailDomainRepository, IMailDomainBusinessLogic>
	{
		#region Members
		#endregion

		#region Constructors
		#endregion

		#region Properties
		#endregion

		#region IAssociable Members
		#endregion

		#region Methods

		public static IEnumerable<string> LoadInternalDomains()
		{
			try
			{
				return Repository.LoadInternalDomains();
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static MailDomain LoadDefaultForInternal()
		{
			try
			{
				return Repository.LoadDefaultForInternal();
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		#endregion
	}
}
