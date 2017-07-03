using System;
using System.Linq.Expressions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Events.AccountArguments.Common;
using NetSteps.Events.AccountArguments.Repository.Context;
using NetSteps.Events.AccountArguments.Repository.Entities;

namespace NetSteps.Events.AccountArguments.Repository
{
	[ContainerRegister(typeof(IAccountEventArgumentRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
	public class AccountEventArgumentRepository : NetSteps.Foundation.Entity.EntityModelRepository<AccountEventArgumentEntity, IAccountEventArgument, IAccountEventArgumentContext>, IAccountEventArgumentRepository
	{
		public int GetAccountIdByEventId(int eventId)
		{
			using (var context = Create.New<IAccountEventArgumentContext>())
			{
				IAccountEventArgument result = FirstOrDefault(context, m => m.EventID == eventId);
				return result != null ? result.AccountID : 0;
			}
		}



		public IAccountEventArgument Save(IAccountEventArgument accountArg)
		{
			using (var context = Create.New<IAccountEventArgumentContext>())
			{
				bool argumentExists = Any(context, a => a.ArgumentID == accountArg.ArgumentID);
				AccountEventArgumentEntity savedEntity = argumentExists ? Update(context, accountArg) : Add(context, accountArg);

				context.SaveChanges();
				UpdateModel(accountArg, savedEntity);

				return accountArg;
			}
		}

		public override Expression<Func<AccountEventArgumentEntity, bool>> GetPredicateForModel(IAccountEventArgument model)
		{
			return m => m.ArgumentID == model.ArgumentID;
		}

		public override void UpdateEntity(AccountEventArgumentEntity entity, IAccountEventArgument model)
		{
			entity.AccountID = model.AccountID;
			entity.EventID = model.EventID;
		}

		public override void UpdateModel(IAccountEventArgument model, AccountEventArgumentEntity entity)
		{
			model.ArgumentID = entity.ArgumentID;
			model.AccountID = entity.AccountID;
			model.EventID = entity.EventID;
		}
	}
}
