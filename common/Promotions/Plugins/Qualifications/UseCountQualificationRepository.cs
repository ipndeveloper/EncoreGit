using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.Qualifications.Base;
using NetSteps.Promotions.Plugins.EntityModel;
using NetSteps.Data.Common.Entities;
using NetSteps.Promotions.Plugins.Common;
using NetSteps.Promotions.Common.Model;
using NetSteps.Core.Cache;

namespace NetSteps.Promotions.Plugins.Qualifications
{
    public class UseCountQualificationRepository :  BasePromotionQualificationRepository<IUseCountQualificationExtension, PromotionQualificationUseCount>, IUseCountQualificationRepository
    {

        private static ICache<Tuple<int, int>, useCountContainerWrapper> cachedUseCounts = new ActiveMruLocalMemoryCache<Tuple<int, int>, useCountContainerWrapper>("promotionUseCounts", new DelegatedDemuxCacheItemResolver<Tuple<int, int>, useCountContainerWrapper>(ResolvePromotionUseCount));
        
        public void RecordUse(IPromotionQualificationExtension qualification, Data.Common.Context.IOrderContext orderContext, IEncorePromotionsPluginsUnitOfWork unitOfWork)
        {
            SetDataContext(unitOfWork);

            var found = Fetch().SingleOrDefault(x => x.PromotionQualificationID == qualification.PromotionQualificationID);
            if (found != null)
            {
                // there is a bug here - if the order has customers that did not qualify this will still "use" the promotion.  Sucks.  Will need to determine which users actually qualified.  Could be a problem.
                foreach (var customer in orderContext.Order.OrderCustomers)
                {
                    found.PromotionQualificationUseCountAccounts.Add(new PromotionQualificationUseCountAccount() { AccountID = customer.AccountID });
                }
                unitOfWork.SaveChanges();
                foreach (var customer in orderContext.Order.OrderCustomers)
                {
                    useCountContainerWrapper wrapper;
                    cachedUseCounts.TryRemove(new Tuple<int, int>(qualification.PromotionQualificationID, customer.AccountID), out wrapper);
                    // this should just remove the cache entry so that a user is unable to quickly place another order and get the same promotion (if uses are surpassed).
                }
            }
        }

        public int GetUseCount(IUseCountQualificationExtension promotionQualification, int accountID, IEncorePromotionsPluginsUnitOfWork unitOfWork)
        {
            useCountContainerWrapper counter = null;
            if (cachedUseCounts.TryGet(new Tuple<int, int>(promotionQualification.PromotionQualificationID, accountID), out counter))
            {
                return counter.UseCount;
            }
            else
            {
                return 0;
            }
        }

        private static bool ResolvePromotionUseCount(Tuple<int, int> promotionQualificationIDAndAccountID, out useCountContainerWrapper useCount)
        {
            var promotionQualificationID = promotionQualificationIDAndAccountID.Item1;
            var accountID = promotionQualificationIDAndAccountID.Item2;

            using (IEncorePromotionsPluginsUnitOfWork unitOfWork = Create.New<IEncorePromotionsPluginsUnitOfWork>())
            {
                var objectSet = unitOfWork.CreateObjectSet<PromotionQualificationUseCount>();
                var found = objectSet.SingleOrDefault(x => x.PromotionQualificationID == promotionQualificationID);
                if (found == null)
                {
                    useCount = new useCountContainerWrapper() { UseCount = 0 };
                    return false;
                }
                else
                {
                    useCount = new useCountContainerWrapper() { UseCount = found.PromotionQualificationUseCountAccounts.Count(x => x.AccountID == accountID) };
                    return true;
                }
            }
            
        }

        [Serializable]
        public class useCountContainerWrapper
        {
            public int UseCount { get; set; }
        }
    }
}
