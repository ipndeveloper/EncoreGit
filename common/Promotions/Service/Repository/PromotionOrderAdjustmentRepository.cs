using System;
using System.Linq;
using NetSteps.Data.Common;
using NetSteps.Data.Common.Entities;
using NetSteps.Encore.Core;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Common;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Common.Repository;
using NetSteps.Promotions.Service.EntityModels;
using System.Collections.Generic;

namespace NetSteps.Promotions.Service.Repository
{
	[ContainerRegister(typeof(IPromotionOrderAdjustmentRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class PromotionOrderAdjustmentRepository : BaseRepository<PromotionOrderAdjustment>, IPromotionOrderAdjustmentRepository
	{

		protected override string[] Includes
		{
			get { return new string[0]; }
		}

		public void AttachAdjustment(IOrderAdjustment adjustment)
		{
			PromotionOrderAdjustment foundAdjustment = Fetch().SingleOrDefault(x => x.OrderAdjustmentID == adjustment.OrderAdjustmentID);
			adjustment.Extension = foundAdjustment;
		}

		public IPromotionOrderAdjustment SaveAdjustmentExtension(IOrderAdjustment adjustment, IPromotionUnitOfWork unitOfWork)
		{
			SetDataContext(unitOfWork);

			IPromotionOrderAdjustment adjustmentExtension = (IPromotionOrderAdjustment)adjustment.Extension;
			PromotionOrderAdjustment currentAdjustment = Fetch().SingleOrDefault(x => x.OrderAdjustmentID == adjustmentExtension.OrderAdjustmentID);
			if (currentAdjustment != null)
			{
				UpdateEntity(currentAdjustment, adjustmentExtension);
				return Convert(currentAdjustment);
			}
			else
			{
				Add(Convert(adjustmentExtension));
				return adjustmentExtension;
			}
		}

		#region Private Methods

		private PromotionOrderAdjustment Convert(IPromotionOrderAdjustment promotionOrderAdjustment)
		{
			ICopier<IPromotionOrderAdjustment, PromotionOrderAdjustment> copier = Create.New<ICopier<IPromotionOrderAdjustment, PromotionOrderAdjustment>>();
			PromotionOrderAdjustment promotionOrderAdjustmentEntity = new PromotionOrderAdjustment();
			copier.CopyTo(promotionOrderAdjustmentEntity, promotionOrderAdjustment, CopyKind.Loose, Container.Current);

			return promotionOrderAdjustmentEntity;
		}

		private IPromotionOrderAdjustment Convert(PromotionOrderAdjustment promotionOrderAdjustment)
		{
			ICopier<PromotionOrderAdjustment, IPromotionOrderAdjustment> copier = Create.New<ICopier<PromotionOrderAdjustment, IPromotionOrderAdjustment>>();
			IPromotionOrderAdjustment promotionOrderAdjustmentDTO = Create.New<IPromotionOrderAdjustment>();
			copier.CopyTo(promotionOrderAdjustmentDTO, promotionOrderAdjustment, CopyKind.Loose, Container.Current);

			return promotionOrderAdjustmentDTO;
		}

		private void UpdateEntity(PromotionOrderAdjustment target, IPromotionOrderAdjustment source)
		{
			ICopier<IPromotionOrderAdjustment, PromotionOrderAdjustment> copier = Create.New<ICopier<IPromotionOrderAdjustment, PromotionOrderAdjustment>>();
			copier.CopyTo(target, source, CopyKind.Strict, Container.Current);
		}

		#endregion


		public IPromotionOrderAdjustment FindPromotionOrderAdjustment(int OrderAdjustmentID, IPromotionUnitOfWork unitOfWork)
		{
			SetDataContext(unitOfWork);

			return Convert(Fetch().SingleOrDefault(x => x.OrderAdjustmentID == OrderAdjustmentID));
		}


		public void DeletePromotionOrderAdjustment(int orderAdjustmentID, IPromotionUnitOfWork unitOfWork)
		{
			SetDataContext(unitOfWork);
            if (orderAdjustmentID > 0)
            {
                var extension = Fetch().SingleOrDefault(x => x.OrderAdjustmentID == orderAdjustmentID);
                if (extension != null)
                {
                    Delete(extension);
                }
                //Delete((x) => { return x.OrderAdjustmentID == orderAdjustmentID; });
            }
        }


		public int FindUseCountForPromotionID(IPromotionUnitOfWork unitOfWork, IEnumerable<int> previousOrderAdjustmentIDs, int promotionId)
		{
			SetDataContext(unitOfWork);

			var found = Fetch().Where(x => (x.PromotionID == promotionId) && previousOrderAdjustmentIDs.Contains(x.OrderAdjustmentID));
			return found.Count();
		}
	}
}
