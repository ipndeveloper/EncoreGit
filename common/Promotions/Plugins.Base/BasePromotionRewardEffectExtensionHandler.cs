using System.Diagnostics.Contracts;
using NetSteps.Data.Common;
using NetSteps.Data.Common.Context;
using NetSteps.Encore.Core.IoC;
using NetSteps.Extensibility.Core;
using NetSteps.OrderAdjustments.Common.Model;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Common.ModelConcrete;
using NetSteps.Promotions.Common.Repository;
using NetSteps.Promotions.Plugins.Base;

namespace NetSteps.Promotions.Plugins.Rewards.Base
{
	public abstract class BasePromotionRewardEffectExtensionHandler<T, R, TUnitOfWork> : IDataObjectExtensionProvider, IPromotionRewardEffectExtensionHandler
		where T : IPromotionRewardEffectExtension
		where R : IPromotionRewardEffectExtensionRepository<T>
		where TUnitOfWork : IUnitOfWork
	{
		public IDataObjectExtension SaveDataObjectExtension(IExtensibleDataObject entity)
		{
			Contract.Assert(entity is IPromotionRewardEffect);
			Contract.Assert(entity.Extension is T);

			IPromotionRewardEffect effect = entity as IPromotionRewardEffect;
			T extension = (T)(object)effect.Extension;

			extension.PromotionRewardEffectID = effect.PromotionRewardEffectID;
			using (IContainer scopeContainer = Create.NewContainer())
			{
				using (TUnitOfWork uow = Container.Current.New<TUnitOfWork>())
				{
					R repository = Create.New<R>();
					extension = repository.SaveExtension(extension, uow);
					effect.Extension = extension;
					uow.SaveChanges();
					return extension;
				}
			}
		}

		public IDataObjectExtension GetDataObjectExtension(IExtensibleDataObject entity)
		{
			Contract.Assert(entity is IPromotionRewardEffect);

			using (IContainer scopeContainer = Create.NewContainer())
			{
				using (TUnitOfWork uow = Container.Current.New<TUnitOfWork>())
				{
					R repository = Create.New<R>();
					entity.Extension = repository.RetrieveExtension(entity as IPromotionRewardEffectExtension, uow);
				}
				return entity.Extension;
			}
		}

		public virtual void UpdateDataObjectExtension(IExtensibleDataObject entity)
		{
			Contract.Assert(entity is IPromotionRewardEffect);
			Contract.Assert(entity.Extension is T);

			((T)(object)entity.Extension).PromotionRewardEffectID = ((IPromotionRewardEffectExtension)(object)entity).PromotionRewardEffectID;
		}

		public abstract string GetProviderKey();

		public void DeleteDataObjectExtension(IExtensibleDataObject entity)
		{
			Contract.Assert(entity is IPromotionRewardEffect);

			IPromotionRewardEffect reward = entity as IPromotionRewardEffect;
			using (IContainer scopeContainer = Create.NewContainer())
			{
				using (TUnitOfWork uow = Container.Current.New<TUnitOfWork>())
				{
					R repository = Create.New<R>();
					repository.DeleteExtension(reward.PromotionRewardID, uow);
				}
			}
		}

		public virtual bool RequiresCommitNotification { get { return false; } }
		public virtual bool RequiresRemoveNotification { get { return false; } }

		public virtual void Commit(IPromotionRewardEffect qualification, IPromotionRewardEffect orderContext)
		{ }

		public virtual void Remove(IPromotionRewardEffect qualification, IPromotionRewardEffect orderContext)
		{ }


		public abstract bool AreEqual(IPromotionRewardEffectExtension effect1, IPromotionRewardEffectExtension effect2);

		public void CheckValidity(string promotionRewardEffectKey, IPromotionRewardEffect rewardEffect, IPromotionState state)
		{
			Contract.Requires(rewardEffect != null);
			Contract.Requires(rewardEffect.Extension != null);
			Contract.Requires(typeof(T).IsAssignableFrom(rewardEffect.Extension.GetType()));

			CheckValidity(promotionRewardEffectKey, (T)rewardEffect.Extension, state);
		}

		public abstract void CheckValidity(string promotionRewardEffectKey, T rewardEffect, IPromotionState state);
	}
}
