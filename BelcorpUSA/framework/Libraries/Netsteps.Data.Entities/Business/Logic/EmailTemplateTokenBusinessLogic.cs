using System.Collections.Generic;
using System.Linq;

namespace NetSteps.Data.Entities.Business.Logic
{
    public partial class EmailTemplateTokenBusinessLogic
    {
        protected const string DISTRIBUTOR_CONTENT = "DistributorContent";
        protected const string ACCOUNTNUMBER = "AccountNumber";
        protected const string FIRSTNAME = "FirstName";
        protected const string LASTNAME = "LastName";
        protected const string EMAIL = "Email";
        protected const string PHONE = "Phone";
        protected const string TITLE = "Title";
        protected const string SPONSOR_ACCOUNTNUMBER = "SponsorAccountNumber";
        protected const string SPONSOR_FIRSTNAME = "SponsorFirstName";
        protected const string SPONSOR_LASTNAME = "SponsorLastName";
        protected const string SPONSOR_EMAIL = "SponsorEmail";
        protected const string SPONSOR_PHONE = "SponsorPhone";

        protected List<string> tokenNames = new List<string>()
                                  {
                                    DISTRIBUTOR_CONTENT,
                                    ACCOUNTNUMBER,
                                    FIRSTNAME,
                                    LASTNAME,
                                    EMAIL,
                                    PHONE,
                                    TITLE,
                                    SPONSOR_ACCOUNTNUMBER,
                                    SPONSOR_FIRSTNAME,
                                    SPONSOR_LASTNAME,
                                    SPONSOR_EMAIL,
                                    SPONSOR_PHONE
                                  };

        public virtual List<string> GetTokenNames()
        {
            return tokenNames;
        }

        public virtual List<EmailTemplateToken> GetStandardTokens(Account account)
        {
            List<EmailTemplateToken> result = new List<EmailTemplateToken>();
            result.Add(new EmailTemplateToken() { Token = ACCOUNTNUMBER, Value = account.AccountNumber });
            result.Add(new EmailTemplateToken() { Token = EMAIL, Value = account.EmailAddress });
            result.Add(new EmailTemplateToken() { Token = FIRSTNAME, Value = account.FirstName });
            result.Add(new EmailTemplateToken() { Token = LASTNAME, Value = account.LastName });
            result.Add(new EmailTemplateToken() { Token = PHONE, Value = account.MainPhone });
            return result;
        }

        public virtual List<EmailTemplateToken> GetSponsorStandardTokens(Account account)
        {
            List<EmailTemplateToken> result = new List<EmailTemplateToken>();
            result.Add(new EmailTemplateToken() { Token = SPONSOR_ACCOUNTNUMBER, Value = account.AccountNumber });
            result.Add(new EmailTemplateToken() { Token = SPONSOR_EMAIL, Value = account.EmailAddress });
            result.Add(new EmailTemplateToken() { Token = SPONSOR_FIRSTNAME, Value = account.FirstName });
            result.Add(new EmailTemplateToken() { Token = SPONSOR_LASTNAME, Value = account.LastName });
            result.Add(new EmailTemplateToken() { Token = SPONSOR_PHONE, Value = account.MainPhone });
            return result;
        }

        public virtual List<EmailTemplateToken> CombineTokens(List<EmailTemplateToken> defaultTokens, List<EmailTemplateToken> overrideTokens)
        {
            List<EmailTemplateToken> result = new List<EmailTemplateToken>();
            result.AddRange(defaultTokens);
            foreach (EmailTemplateToken overrideToken in overrideTokens)
            {
                EmailTemplateToken tokenToReplace = result.Where(x => x.Token == overrideToken.Token).FirstOrDefault();
                if (tokenToReplace != null)
                {
                    tokenToReplace.Value = overrideToken.Value;
                }
                else
                {
                    result.Add(new EmailTemplateToken() { Token = overrideToken.Token, Value = overrideToken.Value });
                }
            }

            return result;
        }

        // This is a more performant version of the method above for internal uses- JHE
        protected virtual Dictionary<string, EmailTemplateToken> CombineTokenDictionaries(Dictionary<string, EmailTemplateToken> defaultTokens, Dictionary<string, EmailTemplateToken> overrideTokens)
        {
            Dictionary<string, EmailTemplateToken> result = new Dictionary<string, EmailTemplateToken>();
            foreach (var defaultToken in defaultTokens)
                result.Add(defaultToken.Key, defaultToken.Value);

            foreach (var overrideToken in overrideTokens.Values)
            {
                EmailTemplateToken tokenToReplace = null;
                if (result.ContainsKey(overrideToken.Token))
                    tokenToReplace = result[overrideToken.Token];
                if (tokenToReplace != null)
                {
                    tokenToReplace.Value = overrideToken.Value;
                }
                else
                {
                    result.Add(overrideToken.Token, new EmailTemplateToken() { Token = overrideToken.Token, Value = overrideToken.Value });
                }
            }

            return result;
        }


        public virtual List<EmailTemplateToken> GetFakeTokensForPreview()
        {
            return new List<EmailTemplateToken>()
				{
					new EmailTemplateToken() { Token = ACCOUNTNUMBER, Value = "-1" },
					new EmailTemplateToken() { Token = EMAIL, Value = "JaneDoe@testemail.com" },
					new EmailTemplateToken() { Token = FIRSTNAME, Value = "Jane" },
					new EmailTemplateToken() { Token = LASTNAME, Value = "Doe" },
					new EmailTemplateToken() { Token = PHONE, Value = "(555) 555-1111" },
					new EmailTemplateToken() { Token = TITLE, Value = "Title" },

					new EmailTemplateToken() { Token = SPONSOR_ACCOUNTNUMBER, Value = "-1" },
					new EmailTemplateToken() { Token = SPONSOR_EMAIL, Value = "JohnDoe@testemail.com" },
					new EmailTemplateToken() { Token = SPONSOR_FIRSTNAME, Value = "Bob" },
					new EmailTemplateToken() { Token = SPONSOR_LASTNAME, Value = "White" },
					new EmailTemplateToken() { Token = SPONSOR_PHONE, Value = "(444) 444-1111" },
					new EmailTemplateToken() { Token = DISTRIBUTOR_CONTENT, Value = "This content is editable by the distributor" }
				};
        }

        public virtual List<EmailTemplateToken> GetPreviewTokens(EmailTemplate emailTemplate, Account account, Account sponsorAccount)
        {
            Dictionary<string, EmailTemplateToken> tokens = EmailTemplateToken.GetFakeTokensForPreview().ToDictionary(t => t.Token);
            tokens = CombineTokenDictionaries(tokens, EmailTemplateToken.GetDefaultTokens(emailTemplate.EmailTemplateID).ToDictionary(t => t.Token));
            if (account != null)
            {
                tokens = CombineTokenDictionaries(tokens, EmailTemplateToken.GetStandardTokens(account).ToDictionary(t => t.Token));
                tokens = CombineTokenDictionaries(tokens, EmailTemplateToken.GetAllCustomTokensForAccount(emailTemplate.EmailTemplateID, account.AccountID).ToDictionary(t => t.Token));
            }

            if (sponsorAccount != null)
            {
                tokens = CombineTokenDictionaries(tokens, EmailTemplateToken.GetSponsorStandardTokens(sponsorAccount).ToDictionary(t => t.Token));
                tokens = CombineTokenDictionaries(tokens, EmailTemplateToken.GetAllCustomTokensForAccount(emailTemplate.EmailTemplateID, sponsorAccount.AccountID).GroupBy(t => t.Token).ToDictionary(tg => tg.Key, tg => tg.OrderByDescending(t => t.EmailTemplateTokenID).FirstOrDefault()));
            }

            return tokens.Values.ToList();
        }

    }
}
