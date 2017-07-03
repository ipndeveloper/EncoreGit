using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using NetSteps.Foundation.Entity.Mocks;
using E = NetSteps.Promotions.UI.Service.Entities;

namespace NetSteps.Promotions.UI.Service.Context.Mocks
{
    public class MockPromotionsUIContext : MockDbContext, IPromotionsUIContext
    {
        public IDbSet<E.PromotionContent> PromotionContent { get; private set; }

        public MockPromotionsUIContext()
        {
            PromotionContent = new MockDbSet<E.PromotionContent>(x => x.PromotionContentID);
        }
    }
}
