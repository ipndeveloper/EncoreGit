using System.Globalization;

using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Extensions
{
	/// <summary>
	/// Author: John Egbert
	/// Description: String Extensions
	/// Created: 01-13-2011
	/// </summary>
	public static class StringExtensions
	{
		public static string FormatPhone(this string phone, CultureInfo cultureInfo)
		{
			return Account.DisplayPhone(phone, cultureInfo.Name);
		}

		public static string FormatPhone(this string phone, string countryCultureInfoCode)
		{
			return Account.DisplayPhone(phone, countryCultureInfoCode);
		}

		public static string ReplaceCmsTokens(this string content)
		{
			if (string.IsNullOrWhiteSpace(content) || !Container.Current.Registry.IsTypeRegistered(typeof(NetSteps.Common.Interfaces.ITokenValueProviderFactory)))
			{
				return content;
			}

			var tokenProviderFactory = Create.New<NetSteps.Common.Interfaces.ITokenValueProviderFactory>();
			var tokenProvider = tokenProviderFactory.GetTokenProvider(NetSteps.Common.Constants.TokenValueProviderType.Cms);
			var tokenReplacer = new NetSteps.Common.TokenReplacement.TokenReplacer(tokenProvider, NetSteps.Common.Constants.BEGIN_TOKEN_DELIMITER, NetSteps.Common.Constants.END_TOKEN_DELIMITER);

			return tokenReplacer.ReplaceTokens(content);
		}
	}
}
