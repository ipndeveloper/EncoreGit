using System.Collections.Generic;
using NetSteps.Common.Interfaces;

namespace NetSteps.Events.Service.Tests.Mocks
{
	public class FakeTokenValueProvider : ITokenValueProvider
	{
		public IEnumerable<string> GetKnownTokens()
		{
			return new[]
			{
				"Token1",
				"Token2",
				"Token3"
			};
		}

		public string GetTokenValue(string token)
		{
			return token + "value";
		}
	}
}
