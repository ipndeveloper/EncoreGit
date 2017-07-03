using System.Linq;
using NetSteps.Data.Common;
using NetSteps.Encore.Core;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Plugins.Base;

namespace NetSteps.Promotions.Plugins.Qualifications.Base
{
    public abstract class BasePromotionQualificationRepository<T, C> : BaseRepository<C>, IPromotionQualificationRepository<T>
        where T : IPromotionQualificationExtension
        where C : class, IPromotionQualificationSimpleExtension, new()
    {
        public T SaveExtension(T qualificationExtension, IUnitOfWork unitOfWork)
        {
            SetDataContext(unitOfWork);

            C found = Fetch().SingleOrDefault(x => x.PromotionQualificationID == qualificationExtension.PromotionQualificationID);
            if (found != null)
            {
                UpdateEntity(found, qualificationExtension);
            }
            else
            {
                found = Convert(qualificationExtension);
                Add(found);
            }
            return Convert(found);
        }

        public T RetrieveExtension(IPromotionQualification qualification, IUnitOfWork unitOfWork)
        {
            SetDataContext(unitOfWork);

            C found = Fetch().SingleOrDefault(x => x.PromotionQualificationID == qualification.PromotionQualificationID);
            if (found != null)
                return Convert(found);
            else
                return default(T);
        }

        public void DeleteExtension(int PromotionQualificationID, IUnitOfWork unitOfWork)
        {
            Delete((x) => { return x.PromotionQualificationID == PromotionQualificationID; });
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
            dto.PromotionQualificationID = entity.PromotionQualificationID;
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
            copied.PromotionQualificationID = dto.PromotionQualificationID;
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
            entity.PromotionQualificationID = dto.PromotionQualificationID;
        }
    }
}
