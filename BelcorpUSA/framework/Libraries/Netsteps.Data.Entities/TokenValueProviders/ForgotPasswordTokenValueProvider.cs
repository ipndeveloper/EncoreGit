using System.Collections.Generic;
using NetSteps.Common.Interfaces;

namespace NetSteps.Data.Entities.TokenValueProviders
{
	public class ForgotPasswordTokenValueProvider : ITokenValueProvider
	{
		private const string FORGOTPASSWORD_URL = "ForgotPasswordUrl";
		private const string FORGOTPASSWORD_ACCOUNT_NAME = "ForgotPasswordAccountName";

		private readonly string _url;
		private readonly string _name;


		public ForgotPasswordTokenValueProvider(string url, string name)
		{
			_url = url;
			_name = name;
		}

		public IEnumerable<string> GetKnownTokens()
		{
			return new List<string>()
			{
				FORGOTPASSWORD_URL,
				FORGOTPASSWORD_ACCOUNT_NAME
			};
		}

		public string GetTokenValue(string token)
		{
			switch (token)
			{
				case FORGOTPASSWORD_URL:
					return _url;
				case FORGOTPASSWORD_ACCOUNT_NAME:
					return _name;
				default:
					return null;
			}
		}
	}
}
