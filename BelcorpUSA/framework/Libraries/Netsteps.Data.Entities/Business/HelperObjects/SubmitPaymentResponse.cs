using System;
using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.PaymentGateways
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Customer response class to contain the SuccessCode of a payment authorization.
	/// Created: 04-15-2010
	/// </summary>
	[Serializable]
	public class SubmitPaymentResponse : BasicResponse
	{
		// Unsuccessful payments counter
		public int FailureCount { get; set; }
	}
}
