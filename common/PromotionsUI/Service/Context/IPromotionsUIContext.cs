using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using NetSteps.Foundation.Entity;
using E = NetSteps.Promotions.UI.Service.Entities;

namespace NetSteps.Promotions.UI.Service.Context
{
    public interface IPromotionsUIContext : IDbContext
    {
        IDbSet<E.PromotionContent> PromotionContent { get; }
    }
}
