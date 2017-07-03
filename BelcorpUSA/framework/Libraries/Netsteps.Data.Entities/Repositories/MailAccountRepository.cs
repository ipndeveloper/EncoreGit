using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Interfaces;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Security;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class MailAccountRepository : BaseRepository<MailAccount, int, NetStepsEntities>, IMailAccountRepository, IDefaultImplementation
	{
		#region Members
		protected override Func<NetStepsEntities, IQueryable<MailAccount>> loadAllFullQuery
		{
			get
			{
				return CompiledQuery.Compile<NetStepsEntities, IQueryable<MailAccount>>(
				   (context) => from a in context.MailAccounts
											   .Include("Account")
								select a);
			}
		}
		#endregion

		#region Methods
		public MailAccount Authenticate(string email, string password)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					var mailAccount = context.MailAccounts.FirstOrDefault(u => u.EmailAddress == email);
					if (SimpleHash.VerifyHash(password, SimpleHash.Algorithm.SHA512, mailAccount.PasswordHash))
						return mailAccount;
					else
						throw new NetStepsBusinessException("Invalid credentials.");
				}
			});
		}

		public MailAccount LoadByAccountID(int accountID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					var mailAccount = context.MailAccounts.FirstOrDefault(u => u.AccountID == accountID);
					return mailAccount;
				}
			});
		}

		public List<MailAccountSearchData> LoadSlimBatchByAccountIDs(IEnumerable<int> accountIDs)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					var mailAccounts = context.MailAccounts.Where(u => accountIDs.Contains(u.AccountID)).Select(a => new MailAccountSearchData()
					{
						MailAccountID = a.MailAccountID,
						AccountID = a.AccountID,
						EmailAddress = a.EmailAddress,
					});
					
					string query;
					ObjectQuery oq = mailAccounts as ObjectQuery;
					if(oq != null){
						query = oq.ToTraceString();
					}
					return mailAccounts.ToList();
				}
			});
		}

        public bool IsAvailable(string emailAddress, int accountID)
        {
            emailAddress = emailAddress.ToLower();

            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return !context.MailAccounts.Any(x =>
                        x.EmailAddress.ToLower() == emailAddress.ToLower()
                        && x.AccountID != accountID
                    );
                }
            });
        }

        public bool IsAvailable(string emailAddress)
        {
            emailAddress = emailAddress.ToLower();

            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return !context.Accounts.Any(x =>
                        x.EmailAddress.ToLower() == emailAddress.ToLower()
                    );
                }
            });
        }

        public bool IsOtherAvailable(string emailAddress, int accountID)
        {
            emailAddress = emailAddress.ToLower();

            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return !context.Accounts.Any(x =>
                        x.EmailAddress.ToLower() == emailAddress.ToLower()
                        && x.AccountID != accountID
                    );
                }
            });
        }
		#endregion
	}
}