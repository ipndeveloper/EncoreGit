using System.Linq;
using NetSteps.Encore.Core;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Common.Model;
using NetSteps.Data.Common;
using NetSteps.Promotions.Plugins.Base;
using System;

namespace NetSteps.Promotions.Plugins.Rewards.Base
{
    public abstract class BasePromotionRewardEffectExtensionRepository<T, C> : BaseRepository<C>, IPromotionRewardEffectExtensionRepository<T>
        where T : IPromotionRewardEffectExtension
        where C : class, IPromotionRewardEffectSimpleExtension, new()
    {
        public T SaveExtension(T rewardExtension, IUnitOfWork unitOfWork)
        {
            SetDataContext(unitOfWork);

            C found = Fetch().SingleOrDefault(x => x.PromotionRewardEffectID == rewardExtension.PromotionRewardEffectID);
            if (found != null)
            {
                UpdateEntity(found, rewardExtension);
            }
            else
            {
                found = Convert(rewardExtension);
                Add(found);
            }
            return Convert(found);
        }

        public T RetrieveExtension(IPromotionRewardEffectExtension reward, IUnitOfWork unitOfWork)
        {
            SetDataContext(unitOfWork);

            C found = Fetch().SingleOrDefault(x => x.PromotionRewardEffectID == reward.PromotionRewardEffectID);
            if (found != null)
                return Convert(found);
            else
                return default(T);
        }

        public void DeleteExtension(int PromotionRewardID, IUnitOfWork unitOfWork)
        {
            SetDataContext(unitOfWork);

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Converts the specified entity to a DTO.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        protected virtual internal T Convert(C entity)
        {
            ICopier<C, T> copier = Create.New<ICopier<C, T>>();
            T dto = copier.Copy(entity);
            dto.PromotionRewardEffectID = entity.PromotionRewardEffectID;
            return dto;
        }

        /// <summary>
        /// Converts the specified DTO to a specific data object.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns></returns>
        protected virtual internal C Convert(T dto)
        {
            ICopier<T, C> copier = Create.New<ICopier<T, C>>();
            C copied = copier.Copy(dto);
            copied.PromotionRewardEffectID = dto.PromotionRewardEffectID;
            return copied;
        }

        /// <summary>
        /// Updates an entity with information in the DTO.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="dto">The dto.</param>
        protected virtual internal void UpdateEntity(C entity, T dto)
        {
            ICopier<T, C> copier = Create.New<ICopier<T, C>>();
            copier.CopyTo(entity, dto, CopyKind.Loose, Container.Current);
            entity.PromotionRewardEffectID = dto.PromotionRewardEffectID;
        }

    }
}
