using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Common.Model;
using System.Data.Objects;

namespace NetSteps.Promotions.Common.Repository.Filter
{
    public interface IPromotionFilter<T> where T:IPromotion
    {
        IQueryable<T> BuildQueryFrom(IQueryable<T> QueryBase);
    }

    public interface IPromotionFilter : IPromotionFilter<IPromotion>
    {
        
    }
}
