using System;
using System.Diagnostics.Contracts;
using System.Linq;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Services;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Common;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Common.ModelConcrete;
using NetSteps.Promotions.Plugins.Base;
using NetSteps.Promotions.Plugins.Common;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Promotions.Plugins.Common.Qualifications.Concrete;

namespace NetSteps.Promotions.Plugins.Qualifications
{
    public class UseCountQualificationHandler : BasePromotionQualificationHandler<IUseCountQualificationExtension, IUseCountQualificationRepository, IEncorePromotionsPluginsUnitOfWork>, IUseCountQualificationHandler
    {
        private IPromotionProvider _promotionProvider;
        private IOrderService _orderService;
        private IUseCountQualificationRepository _useCountQualificationRepository;
        private Func<IEncorePromotionsPluginsUnitOfWork> _unitOfWorkConstructor;

        public UseCountQualificationHandler(IOrderService orderService, IPromotionProvider promotionProvider, IUseCountQualificationRepository useCountQualificationRepository, Func<IEncorePromotionsPluginsUnitOfWork> unitOfWorkConstructor)
        {
            _promotionProvider = promotionProvider;
            _orderService = orderService;
            _useCountQualificationRepository = useCountQualificationRepository;
            _unitOfWorkConstructor = unitOfWorkConstructor;
        }

        public override string GetProviderKey()
        {
            return QualificationExtensionProviderKeys.UseCountProviderKey;
        }

        public override PromotionQualificationResult Matches(IPromotion promotion, IPromotionQualificationExtension promotionQualification, Data.Common.Context.IOrderContext orderContext)
        {
            Contract.Assert(promotionQualification != null);
            Contract.Assert(orderContext != null);
            Contract.Assert(promotionQualification != null);
            Contract.Assert(promotionQualification is IUseCountQualificationExtension);

            var extension = promotionQualification as IUseCountQualificationExtension;
            
            using (IEncorePromotionsPluginsUnitOfWork unitOfWork = _unitOfWorkConstructor())
            {
                var customerIds = orderContext.Order.OrderCustomers.Select(x => x.AccountID).ToList();
                if (extension.FirstOrdersOnly)
                {
                    customerIds =
                        customerIds.Where(x => _orderService.GetAccountOrderCount(x) < extension.MaximumUseCount).ToList();
                }
                else
                {
                    customerIds = customerIds.Where(x => extension.MaximumUseCount > _useCountQualificationRepository.GetUseCount(extension, x, unitOfWork)).ToList();
                }
                if (customerIds.Count() > 0)
                    return PromotionQualificationResult.MatchForSelectCustomers(customerIds);
                
            }
            return PromotionQualificationResult.NoMatch;
        }

        public override bool RequiresCommitNotification
        {
            get { return true; }
        }

        public override void Commit(IPromotion promotion, IPromotionQualificationExtension qualification, IOrderContext orderContext)
        {
            Contract.Assert(qualification != null);
            Contract.Assert(qualification != null);
            Contract.Assert(qualification is IUseCountQualificationExtension);
            Contract.Assert(orderContext != null);
            Contract.Assert(orderContext.Order != null);
            Contract.Assert(orderContext.Order.OrderCustomers != null);
            Contract.Assert(orderContext.Order.OrderAdjustments.Count > 0);

            using (IEncorePromotionsPluginsUnitOfWork unitOfWork = _unitOfWorkConstructor())
            {
                _useCountQualificationRepository.RecordUse(qualification, orderContext, unitOfWork);
                unitOfWork.SaveChanges();
            }

        }

        public override bool AreEqual(IPromotionQualificationExtension promotionQualification1, IPromotionQualificationExtension promotionQualification2)
        {
            Contract.Assert(promotionQualification1 != null);
            Contract.Assert(promotionQualification2 != null);
            Contract.Assert(promotionQualification1 is IUseCountQualificationExtension);
            Contract.Assert(promotionQualification2 is IUseCountQualificationExtension);

            var extension1 = promotionQualification1 as IUseCountQualificationExtension;
            var extension2 = promotionQualification2 as IUseCountQualificationExtension;

            return extension1.MaximumUseCount == extension2.MaximumUseCount;
        }

        public override bool ValidFor<TProperty>(IPromotionQualificationExtension qualification, string propertyName, TProperty value)
        {
            Contract.Assert(qualification != null);
            Contract.Assert(!string.IsNullOrEmpty(propertyName));
            Contract.Assert(value != null);
            Contract.Assert(typeof(IUseCountQualificationExtension).IsAssignableFrom(qualification.GetType()));

            switch (propertyName)
            {
                case UseCountQualification.propAccountID:
                    var extension = qualification as IUseCountQualificationExtension;
                    using (IEncorePromotionsPluginsUnitOfWork unitOfWork = _unitOfWorkConstructor())
                    {
                        var count = _useCountQualificationRepository.GetUseCount(extension, Convert.ToInt32(value), unitOfWork);
                        return extension.MaximumUseCount > count;
                    }
            }
            return true;
        }

		public override void CheckValidity(string qualificationKey, IUseCountQualificationExtension qualification, IPromotionState state)
		{
			if (qualification.MaximumUseCount <= 0)
			{
				state.AddConstructionError
					(
						String.Format("Promotion Qualification {0}", qualificationKey),
						"Use count cannot be less than 1."
					);
			}
		}
	}
}
