using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NetSteps.Common.Interfaces;

namespace NetSteps.Common.TokenReplacement
{
	public class TokenReplacer
	{
		private class TokenDefault
		{
			public string Token { get; private set; }
			public string Default { get; private set; }
			public string FullRepresentation { get; private set; }

			public TokenDefault(string prefix, string suffix, string delim, string token, string def = null)
			{
				Token = token;
				Default = def;
				if (def != null)
				{
					FullRepresentation = String.Concat(prefix, token, delim, def, suffix);
				}
				else
				{
					FullRepresentation = String.Concat(prefix, token, suffix);
				}
			}
		}

		public static readonly string TOKEN_DEFAULT_PREFIX = Constants.BEGIN_TOKEN_DELIMITER;
		public static readonly string TOKEN_DEFAULT_SUFFIX = Constants.END_TOKEN_DELIMITER;
		public static readonly string TOKEN_DEFAULT_DEFAULTDELIMITER = "||";
		private static readonly string tokenRegEx = @"(.*?)";

		private ITokenValueProvider _tokenValueProvider;

		public string TokenPrefix { get; private set; }
		public string TokenSuffix { get; private set; }
		public string TokenDefaultDelimiter { get; private set; }
		public Regex TokenRegex { get; private set; }

		public TokenReplacer(ITokenValueProvider tokenValueProvider, string tokenPrefix = null, string tokenSuffix = null, string defaultDelimiter = null)
		{
			if (tokenValueProvider == null)
				throw new ArgumentNullException("tokenValueProvider");

			_tokenValueProvider = tokenValueProvider;
			TokenPrefix = tokenPrefix ?? TOKEN_DEFAULT_PREFIX;
			TokenSuffix = tokenSuffix ?? TOKEN_DEFAULT_SUFFIX;
			TokenDefaultDelimiter = defaultDelimiter ?? TOKEN_DEFAULT_DEFAULTDELIMITER;

			TokenRegex = new Regex(String.Concat(TokenPrefix, tokenRegEx, TokenSuffix), RegexOptions.Compiled | RegexOptions.IgnoreCase);
		}

		private IEnumerable<TokenDefault> ProcessMatches(MatchCollection matches)
		{
			if (matches != null && matches.Count > 0)
			{
				List<TokenDefault> result = new List<TokenDefault>();
				foreach (Match m in matches)
				{
					if (m.Groups.Count > 1)
					{
						var match = m.Groups[1].ToString();
						if (match.Contains(TokenDefaultDelimiter))
						{
							var parts = match.Split(new string[] { TokenDefaultDelimiter }, 2, StringSplitOptions.None);
							if (parts.Length == 2)
							{
								result.Add(new TokenDefault(TokenPrefix, TokenSuffix, TokenDefaultDelimiter, parts[0], parts[1]));
								continue;
							}
						}
						result.Add(new TokenDefault(TokenPrefix, TokenSuffix, TokenDefaultDelimiter, match));
					}
				}
				return result;
			}
			return Enumerable.Empty<TokenDefault>();
		}

		public string ReplaceTokens(string tokenizedString)
		{
			if (!String.IsNullOrWhiteSpace(tokenizedString))
			{
				string output = tokenizedString;
				var stringTokens = ProcessMatches(TokenRegex.Matches(tokenizedString));
				var providerTokens = _tokenValueProvider.GetKnownTokens();
				foreach (var item in stringTokens.Where(st => providerTokens.Contains(st.Token)))
				{
					var pval = _tokenValueProvider.GetTokenValue(item.Token);
					var val = (pval == null || pval == item.Token) ? item.Default : pval;
					output = output.Replace(item.FullRepresentation, val);
				}
				return output;
			}
			return tokenizedString;
		}

		public IEnumerable<string> FindUnknownTokens(string tokenizedString)
		{
			var stringTokens = ProcessMatches(TokenRegex.Matches(tokenizedString));
			var providerTokens = _tokenValueProvider.GetKnownTokens();
			return stringTokens.Where(st => !providerTokens.Contains(st.Token)).Select(st => st.Token).ToArray();
		}

		public static string GetDelimitedTokenizedString(string token, string defaultValue = null)
		{
			return (new TokenDefault(TOKEN_DEFAULT_PREFIX, TOKEN_DEFAULT_SUFFIX, TOKEN_DEFAULT_DEFAULTDELIMITER, token, defaultValue)).FullRepresentation;
		}
	}
}