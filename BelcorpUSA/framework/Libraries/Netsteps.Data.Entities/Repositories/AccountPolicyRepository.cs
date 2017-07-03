using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using NetSteps.Accounts.Common.Models;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Encore.Core.IoC;
using System.Data.SqlClient;
using System.Data;

namespace NetSteps.Data.Entities.Repositories
{
	[ContainerRegister(typeof(Events.Common.Repositories.IAccountPolicyRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
	public partial class AccountPolicyRepository : NetSteps.Events.Common.Repositories.IAccountPolicyRepository
	{
		#region Members
		protected override Func<NetStepsEntities, IQueryable<AccountPolicy>> loadAllFullQuery
		{
			get
			{
				return CompiledQuery.Compile<NetStepsEntities, IQueryable<AccountPolicy>>(
				   (context) => from p in context.AccountPolicies
														.Include("Policy")
								select p);
			}
		}
		#endregion

		public List<AccountPolicy> LoadByAccountID(int accountID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return context.AccountPolicies.Include("Policy").Where(p => p.AccountID == accountID).ToList();
				}
			});
		}

		public IEnumerable<IAccountPolicy> GetAccountPoliciesByAccountID(int accountID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (var context = CreateContext())
				{
					IEnumerable<AccountPolicy> policies = context.AccountPolicies.Where(ap => ap.AccountID == accountID);
					return policies.Select(createPolicyDTOFromPolicyEntity).ToList();
				}
			});
		}

		private IAccountPolicy createPolicyDTOFromPolicyEntity(AccountPolicy accountPolicy)
		{
			var returnValue = Create.New<IAccountPolicy>();
			returnValue.AccountID = accountPolicy.AccountID;
			returnValue.AccountPolicyID = accountPolicy.AccountPolicyID;
			returnValue.DateAcceptedUTC = accountPolicy.DateAcceptedUTC ?? new DateTime();
			return returnValue;
		}

        public string DeleteAccountPoliciesByAccountID(int accountID)
        {
            string resultado = string.Empty;
            using (SqlConnection connection = new SqlConnection(DataAccess.GetConnectionString("Core")))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "dbo.uspDeleteAccountPoliciesByAccountID";
                    command.CommandType = CommandType.StoredProcedure;
                    SqlParameter DispatchID = command.Parameters.AddWithValue("@AccountID", accountID);
                    SqlParameter Error = command.Parameters.AddWithValue("@Error", "");
                    Error.Direction = ParameterDirection.Output;

                    SqlDataReader dr = command.ExecuteReader();
                    resultado = Error.Value.ToString();
                }
            }
            return resultado;
        }
	}
}
