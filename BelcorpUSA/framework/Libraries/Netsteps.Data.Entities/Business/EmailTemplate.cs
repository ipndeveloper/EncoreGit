using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NetSteps.Common.Base;
using NetSteps.Core.Cache;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
	public partial class EmailTemplate
	{
		// TODO: Communications Engine - this is now on EmailTemplateTranslation
		public string Body { get; set; }
		public string Subject
		{
			get
			{
				if (EmailTemplateTranslations.Any())
				{
					var langMatch = EmailTemplateTranslations.FirstOrDefault(ett => ett.LanguageID == ApplicationContext.Instance.CurrentLanguageID);
					return langMatch == null ? EmailTemplateTranslations.First().Subject : langMatch.Subject;
				}
				return string.Empty;
			}
		}
		public string AttachmentPath { get; set; }
		public string FromAddress { get; set; }

		public string ReplaceTokens()
		{
			return string.Empty;
		}

		public string ReplaceTokens(List<EmailTemplateToken> tokens)
		{
			var replacements = new Dictionary<string, string>();
			foreach(EmailTemplateToken token in tokens)
			{
				replacements[token.Token] = token.Value;
			}

			return ReplaceTokens(replacements);
		}

		public string ReplaceTokens(Dictionary<string, string> replacements)
		{
			var result = Body ?? "";
			foreach(KeyValuePair<string, string> replacement in replacements)
			{
				result = result.Replace("{{" + replacement.Key + "}}", replacement.Value);
			}

			return result;
		}

		public List<string> FindInvalidTokens(List<string> tokens)
		{
			var result = new List<string>();
			const string regExpression = @"{{(.*?)}}";

			MatchCollection matches = Regex.Matches(Body, regExpression, RegexOptions.IgnoreCase);

			if (matches.Count > 0)
			{
				result.AddRange(from Match m in matches
								where !tokens.Contains(m.Groups[1].ToString()) //.ToString().Replace("{{", "").Replace("}}", "")
								select m.ToString());
			}

			return result;
		}

		public string GeneratePreviewHtml(Account account, Account sponsorAccount)
		{
			return GeneratePreviewHtml(GetPreviewTokens(account, sponsorAccount));
		}

		public List<EmailTemplateToken> GetPreviewTokens(Account account, Account sponsorAccount)
		{
			return EmailTemplateToken.GetPreviewTokens(this, account, sponsorAccount);
		}

		public string GeneratePreviewHtml(List<EmailTemplateToken> tokens)
		{
			return BusinessLogic.GeneratePreviewHtml(this, tokens);
		}

		public static string RemoveLinksForPreview(string htmlBody)
		{
			return BusinessLogic.RemoveLinksForPreview(htmlBody);
		}

		public static PaginatedList<EmailTemplate> Search(EmailTemplateSearchParameters searchParameters)
		{
			try
			{
				return Repository.Search(searchParameters);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static Dictionary<short, List<EmailTemplate>> GetEvitesEmailTemplates()
		{
			try
			{
				return Repository.GetEvitesEmailTemplates();
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static List<EmailTemplate> GetEmailTemplatesByEmailTemplateTypeID(short emailTemplateTypeID)
		{
			try
			{
				return Repository.GetEmailTemplatesByEmailTemplateTypeID(emailTemplateTypeID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		class FirstTemplateByTemplateTypeIDResolver : DemuxCacheItemResolver<short, EmailTemplate>
		{
			protected override bool DemultiplexedTryResolve(short key, out EmailTemplate value)
			{
				var template = EmailTemplate.Search(new NetSteps.Data.Entities.Business.EmailTemplateSearchParameters()
				{
					Active = true,
					PageIndex = 0,
					PageSize = 1,
					EmailTemplateTypeIDs = new List<short>() { key }
				}).FirstOrDefault();

				if (template != null)
				{
					value = template;
					return true;
				}
				value = default(EmailTemplate);
				return false;
			}
		}

		static ICache<short, EmailTemplate> __firstTemplateByTemplateTypeID =
            new ActiveMruLocalMemoryCache<short, EmailTemplate>("FirstTemplateByTemplateTypeID", new FirstTemplateByTemplateTypeIDResolver());

		public static EmailTemplate GetFirstTemplateByTemplateTypeID(short templateTypeID)
		{
			EmailTemplate value;
			__firstTemplateByTemplateTypeID.TryGet(templateTypeID, out value);
			return value;
		}
	}
}