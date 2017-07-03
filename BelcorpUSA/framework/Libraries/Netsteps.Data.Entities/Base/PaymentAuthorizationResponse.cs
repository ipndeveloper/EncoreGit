using System;
using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Base
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Helper class to return an payment authorization response.
	/// Created: 01-19-2011
	/// </summary>
	[Serializable]
	public class PaymentAuthorizationResponse : BasicResponse
	{
		public NetSteps.Data.Entities.Constants.GatewayAuthorizationStatus GatewayAuthorizationStatus { get; set; }
	}
}
