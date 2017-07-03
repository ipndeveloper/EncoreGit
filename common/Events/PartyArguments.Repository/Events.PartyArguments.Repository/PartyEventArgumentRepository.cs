using System;
using System.Linq.Expressions;
using NetSteps.Events.PartyArguments.Repository.Context;
using NetSteps.Events.PartyArguments.Repository.Entities;
using NetSteps.Encore.Core.IoC;
using NetSteps.Events.PartyArguments.Common;

namespace NetSteps.Events.PartyArguments.Repository
{

	[ContainerRegister(typeof(IPartyEventArgumentRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class PartyEventArgumentRepository : Foundation.Entity.EntityModelRepository<PartyEventArgumentEntity, IPartyEventArgument, IPartyEventArgumentContext>, IPartyEventArgumentRepository
	{
		#region IPartyEventArgumentRepository Members

		public int GetPartyIDByEventID(int eventID)
		{
			using (var context = Create.New<IPartyEventArgumentContext>())
			{
				IPartyEventArgument result = FirstOrDefault(context, m => m.EventID == eventID);
				return result != null ? result.PartyID : 0;
			}
		}

		public IPartyEventArgument Save(IPartyEventArgument partyArg)
		{
			using (var context = Create.New<IPartyEventArgumentContext>())
			{
				bool argumentExists = Any(context, a => a.ArgumentID == partyArg.ArgumentID);
				PartyEventArgumentEntity savedEntity = argumentExists ? Update(context, partyArg) : Add(context, partyArg);

				context.SaveChanges();
				UpdateModel(partyArg, savedEntity);

				return partyArg;
			}
		}

		#endregion

		public override Expression<Func<PartyEventArgumentEntity, bool>> GetPredicateForModel(IPartyEventArgument model)
		{
			return m => m.ArgumentID == model.ArgumentID;
		}

		public override void UpdateEntity(PartyEventArgumentEntity entity, IPartyEventArgument model)
		{
			entity.EventID = model.EventID;
			entity.PartyID = model.PartyID;
		}

		public override void UpdateModel(IPartyEventArgument model, PartyEventArgumentEntity entity)
		{
			model.ArgumentID = entity.ArgumentID;
			model.PartyID = entity.PartyID;
			model.EventID = entity.EventID;
		}
	}
}
