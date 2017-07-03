using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using NetSteps.Data.Common;
using NetSteps.Encore.Core;
using NetSteps.Encore.Core.IoC;
using NetSteps.Extensibility.Core;
using NetSteps.Promotions.Common;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Common.ModelConcrete;
using NetSteps.Promotions.Common.Repository;
using NetSteps.Promotions.Service.EntityModels;
using NetSteps.Core.Cache;

namespace NetSteps.Promotions.Service.Repository
{
	public class PromotionRepository : BaseRepository<Promotion>, IPromotionRepository
	{
        private readonly IDataObjectExtensionProviderRegistry _extensionRegistry;
        private readonly IPromotionValidator _promotionValidator;
        private readonly IPromotionKindManager _promotionKindManager;
        private readonly IPromotionRewardKindManager _promotionRewardKindManager;
        private readonly Func<IPromotionRewardEffect> _promotionRewardEffectConstructor;
        
        public PromotionRepository(IDataObjectExtensionProviderRegistry extensionRegistry, IPromotionValidator promotionValidator, IPromotionKindManager promotionKindManager, IPromotionRewardKindManager promotionRewardKindManager, Func<IPromotionRewardEffect> promotionRewardEffectConstructor)
        {
            _extensionRegistry = extensionRegistry;
            _promotionValidator = promotionValidator;
            _promotionKindManager = promotionKindManager;
            _promotionRewardKindManager = promotionRewardKindManager;
            _promotionRewardEffectConstructor = promotionRewardEffectConstructor;
        }

		#region Public Methods

		private string[] _includes = new string[] { "PromotionQualifications", "PromotionRewards.PromotionRewardEffects" };

		protected override string[] Includes
		{
			get { return _includes; }
		}

		public IPromotion InsertPromotion(IPromotion Promotion, IUnitOfWork unitOfWork)
		{
			SetDataContext(unitOfWork);

			Promotion newEntity = Convert(Promotion);
			this.Add(newEntity);
			unitOfWork.SaveChanges();
			newEntity.RecursiveExecuteForChildren<IPromotionQualification>(x =>
			{
				IDataObjectExtensionProvider provider = _extensionRegistry.RetrieveExtensionProvider(x.ExtensionProviderKey);
				provider.SaveDataObjectExtension(x);
			});
			newEntity.RecursiveExecuteForChildren<IPromotionRewardEffect>(x =>
			{
				IDataObjectExtensionProvider provider = _extensionRegistry.RetrieveExtensionProvider(x.ExtensionProviderKey);
				provider.SaveDataObjectExtension(x);
			});
			promotionIDCache.FlushAll();

			return Convert(newEntity);
		}

		public IPromotion UpdateExistingPromotion(IPromotion promotion, IUnitOfWork unitOfWork)
		{
			SetDataContext(unitOfWork);

			Promotion promotionEntity = Fetch().Single(x => x.PromotionID == promotion.PromotionID);
			UpdateEntity(promotionEntity, promotion);
			promotionIDCache.FlushAll();

			return promotion;
		}

		public IPromotion RetrievePromotion(int PromotionID, IUnitOfWork unitOfWork)
		{
			SetDataContext(unitOfWork);
			return Validate(Convert(Fetch().Where(x => x.PromotionID == PromotionID).SingleOrDefault()), unitOfWork);
		}

		private IPromotion Validate(IPromotion promotion, IUnitOfWork unitOfWork)
		{
			if (promotion.PromotionStatusTypeID == (int)PromotionStatus.Error)
				return promotion;
			var status = _promotionValidator.CheckValidity(promotion);
			if (!status.IsValid)
			{
				promotion.PromotionStatusTypeID = (int)PromotionStatus.Error;
				UpdateExistingPromotion(promotion, unitOfWork);
				unitOfWork.SaveChanges();
				return null;
			}
			return promotion;
		}

		#endregion

		#region Private Methods

		public Promotion Convert(IPromotion Promotion)
		{
			Contract.Assert(Promotion != null);

			var copier = Create.New<ICopier<IPromotion, Promotion>>();
			var PromotionEntity = new Promotion();
			copier.CopyTo(PromotionEntity, Promotion, CopyKind.Loose, Container.Current);

			if (Promotion.PromotionQualifications != null && Promotion.PromotionQualifications.Count() > 0)
			{
				foreach (var qualification in Promotion.PromotionQualifications)
				{
					var qualificationEntity = new PromotionQualification
					{
						PromotionPropertyKey = qualification.Key,
						ExtensionProviderKey = qualification.Value.ExtensionProviderKey,
						Extension = qualification.Value
					};
					PromotionEntity.PromotionQualifications.Add(qualificationEntity);
				}
			}

			if (Promotion.PromotionRewards != null && Promotion.PromotionRewards.Count() > 0)
			{
				ICopier<IPromotionReward, PromotionReward> rewardCopier = Create.New<ICopier<IPromotionReward, PromotionReward>>();
				foreach (var reward in Promotion.PromotionRewards)
				{
					var effectCopier = Create.New<ICopier<IPromotionRewardEffect, NetSteps.Promotions.Service.EntityModels.PromotionRewardEffect>>();
					PromotionReward rewardEntity = rewardCopier.Copy(reward.Value, CopyKind.Loose);
					rewardEntity.PromotionPropertyKey = reward.Key;
					PromotionEntity.PromotionRewards.Add(rewardEntity);
					foreach (var effect in reward.Value.Effects)
					{
						NetSteps.Promotions.Service.EntityModels.PromotionRewardEffect effectEntity = effectCopier.Copy(effect.Value, CopyKind.Loose);
						effectCopier.CopyTo(effectEntity, effect.Value, CopyKind.Loose, Container.Current);
						effectEntity.RewardPropertyKey = effect.Key;
						effectEntity.ExtensionProviderKey = effect.Value.ExtensionProviderKey;
						effectEntity.Extension = effect.Value.Extension;
						rewardEntity.PromotionRewardEffects.Add(effectEntity);
					}
				}
			}

			return PromotionEntity;
		}

		public IPromotion Convert(Promotion Promotion)
		{
			Contract.Assert(Promotion != null);

			ICopier<Promotion, IPromotion> copier = Create.New<ICopier<Promotion, IPromotion>>();
			IPromotion PromotionDTO = _promotionKindManager.CreatePromotion(Promotion.PromotionKind ?? BasicPromotion.PromotionKindName);
			copier.CopyTo(PromotionDTO, Promotion, CopyKind.Loose, Container.Current);
			if (Promotion.PromotionQualifications != null && Promotion.PromotionQualifications.Count() > 0)
			{
				Dictionary<string, IPromotionQualificationExtension> qualifications = new Dictionary<string, IPromotionQualificationExtension>();
				var qualificationCopier = Create.New<ICopier<IPromotionQualification, IPromotionQualification>>();
				foreach (PromotionQualification qualification in Promotion.PromotionQualifications)
				{
					IDataObjectExtensionProvider pluginProvider = _extensionRegistry.RetrieveExtensionProvider(qualification.ExtensionProviderKey);
					qualifications.Add(qualification.PromotionPropertyKey, pluginProvider.GetDataObjectExtension(qualification) as IPromotionQualificationExtension);
				}
				PromotionDTO.PromotionQualifications = qualifications;
			}

			if (Promotion.PromotionRewards != null && Promotion.PromotionRewards.Count() > 0)
			{
				Dictionary<string, IPromotionReward> rewards = new Dictionary<string, IPromotionReward>();
				ICopier<IPromotionRewardEffect, IPromotionRewardEffect> rewardCopier = Create.New<ICopier<IPromotionRewardEffect, IPromotionRewardEffect>>();
				foreach (PromotionReward reward in Promotion.PromotionRewards)
				{
					var rewardDTO = _promotionRewardKindManager.CreatePromotionReward(reward.PromotionRewardKind);
					foreach (var effect in reward.PromotionRewardEffects)
					{
						IDataObjectExtensionProvider pluginProvider = _extensionRegistry.RetrieveExtensionProvider(effect.ExtensionProviderKey);
						if (effect.Extension == null)
							effect.Extension = pluginProvider.GetDataObjectExtension(effect);
                        IPromotionRewardEffect effectDTO = _promotionRewardEffectConstructor();
						effectDTO.Extension = effect.Extension;
						effectDTO.ExtensionProviderKey = effect.ExtensionProviderKey;
						if (!rewardDTO.Effects.ContainsKey(effect.RewardPropertyKey))
							rewardDTO.Effects.Add(effect.RewardPropertyKey, effectDTO);
						else
							rewardDTO.Effects[effect.RewardPropertyKey] = effectDTO;
					}

					if (rewards.ContainsKey(reward.PromotionPropertyKey))
						rewards[reward.PromotionPropertyKey] = rewardDTO;
					else
						rewards.Add(reward.PromotionPropertyKey, rewardDTO);
				}
				PromotionDTO.PromotionRewards = rewards;
			}
			return PromotionDTO;
		}

		private void UpdateEntity(Promotion PromotionEntity, IPromotion Promotion)
		{
			Contract.Assert(PromotionEntity != null);
			Contract.Assert(Promotion != null);

			ICopier<IPromotion, Promotion> copier = Create.New<ICopier<IPromotion, Promotion>>();
			copier.CopyTo(PromotionEntity, Promotion, CopyKind.Loose, Container.Current);
		}

		#endregion

		private static ICache<Tuple<PromotionStatus, DateTime?, DateTime?, string>, IEnumerable<int>> promotionIDCache = new ActiveMruLocalMemoryCache<Tuple<PromotionStatus, DateTime?, DateTime?, string>, IEnumerable<int>>("promotionIDQueryResult", new DelegatedDemuxCacheItemResolver<Tuple<PromotionStatus, DateTime?, DateTime?, string>, IEnumerable<int>>(ResolvePromotionIDList));

		public IEnumerable<IPromotion> RetrievePromotions(IPromotionUnitOfWork unitOfWork, PromotionStatus statusTypes, Predicate<IPromotion> filter, IEnumerable<string> ofKinds)
		{
			SetDataContext(unitOfWork);

			// Default: Filter by dates and status.
			var query = Fetch()
				.Where(x =>
					(x.StartDate == null || x.StartDate <= DateTime.UtcNow)
					&& (x.EndDate == null || x.EndDate > DateTime.UtcNow)
					&& (x.PromotionStatusTypeID & (int)statusTypes) != 0
				);

			// ofKind
			if (ofKinds.Any())
			{
				query = query
						.Where(x => ofKinds.Any(kind => kind.Equals(x.PromotionKind, StringComparison.InvariantCultureIgnoreCase)));
			}

			var profiles = query
				.ToList();

			List<IPromotion> returnProfiles = new List<IPromotion>();
			foreach (var profile in profiles)
			{
				try
				{
					returnProfiles.Add(Convert(profile));
				}
				catch (Exception)
				{
					//TODO: NEED EXCEPTION HANDLING FOR THIS!!!
				}
			}
			// applying the filter post-pull... not efficient but effective for the moment.
			return returnProfiles.Where((x) => filter(x));
		}

		public int RetrievePromotionIDByPromotionQualificationID(IPromotionUnitOfWork unitOfWork, int promotionQualificationID)
		{
			SetDataContext(unitOfWork);

			var found = Fetch().SingleOrDefault(x => x.PromotionQualifications.Any(y => y.PromotionQualificationID == promotionQualificationID));
			if (found != null)
				return found.PromotionID;
			return 0;
		}


		public IEnumerable<int> RetrievePromotionIDs(IPromotionUnitOfWork unitOfWork, PromotionStatus statusTypes, IPromotionInterval searchInterval, IEnumerable<string> ofKinds)
		{
			SetDataContext(unitOfWork);
			IEnumerable<int> promotionIDs = new int[0];
			if (promotionIDCache.TryGet(new Tuple<PromotionStatus, DateTime?, DateTime?, string>(statusTypes, searchInterval.StartDate, searchInterval.EndDate, String.Join("|", ofKinds)), out promotionIDs))
			{
				return promotionIDs;
			}
			else
			{
				throw new Exception("Unable to locate promotion IDs.");
			}
		}

		private static bool ResolvePromotionIDList(Tuple<PromotionStatus, DateTime?, DateTime?, string> query, out IEnumerable<int> promotionIDList)
		{
			var statusTypes = query.Item1;
			var startDate = query.Item2;
			var endDate = query.Item3;
			var ofKinds = query.Item4.Trim();
			var kindsArray = new string[0];
			if (!ofKinds.Equals(String.Empty))
			{
				kindsArray = ofKinds.Split('|');
			}
            using (var unitOfWork = Create.New<IPromotionUnitOfWork>())
			{
				promotionIDList = unitOfWork.CreateObjectSet<Promotion>()
					.Where(promotion =>
					(
						((int)statusTypes & promotion.PromotionStatusTypeID) == promotion.PromotionStatusTypeID) &&
						(startDate == null || promotion.StartDate == null || promotion.StartDate <= startDate) &&
						(endDate == null || promotion.EndDate == null || promotion.EndDate >= endDate) &&
						(!kindsArray.Any() || kindsArray.Any(x => promotion.PromotionKind.Equals(x, StringComparison.CurrentCultureIgnoreCase)))
					)
					.Select(promo => promo.PromotionID)
					.ToList();
				return true;
			}
		}

		private static string GetCacheKey(PromotionStatus status, IPromotionInterval searchInterval, IEnumerable<string> ofKinds)
		{
			return String.Format("{0}|{1}|{2}|{3}", status.ToString(), searchInterval.StartDate.ToString(), searchInterval.EndDate.ToString(), String.Join(",", ofKinds));
		}
	}
}
