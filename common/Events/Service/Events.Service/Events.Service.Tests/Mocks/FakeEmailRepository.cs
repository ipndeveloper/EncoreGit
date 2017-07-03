using System;
using System.Collections.Generic;
using NetSteps.Common.Interfaces;
using NetSteps.Encore.Core.IoC;
using NetSteps.Events.Common.Repositories;

namespace NetSteps.Events.Service.Tests.Mocks
{
	public class FakeEmailRepository : IEmailRepository
	{
		public static void Initialize()
		{
			var root = Container.Root;
			root.ForType<IEmailRepository>()
				.Register<FakeEmailRepository>()
				.ResolveAsSingleton()
				.End();
		}

		public void SendEmail(int accountID, int eventTypeID, ITokenValueProvider tokenValueProvider, IEnumerable<string> extraRecipients)
		{
		}

		public void LogException(Exception ex)
		{
		}

		public string FormatDecimalAccordingToCulture(decimal toFormat, int currencyID)
		{
			throw new NotImplementedException();
		}
	}
}
