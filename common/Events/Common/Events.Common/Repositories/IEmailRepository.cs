using System;
using System.Collections.Generic;
using NetSteps.Common.Interfaces;

namespace NetSteps.Events.Common.Repositories
{
	public interface IEmailRepository
	{
		void SendEmail(int accountID, int eventTypeID, ITokenValueProvider tokenValueProvider, IEnumerable<string> extraRecipients);
		void LogException(Exception ex);
		string FormatDecimalAccordingToCulture(decimal toFormat, int currencyID);
	}
}
