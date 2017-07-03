using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Common.Repository.Filter;
using System.Data.Objects;
using NetSteps.Promotions.Common.Model;

namespace NetSteps.Promotions.Service.Filter
{
    public class PromotionsAvailableForDate : IPromotionFilter
    {
        private DateTime _queryDate;

        public PromotionsAvailableForDate(DateTime QueryDate)
        {
            _queryDate = QueryDate;
        }

        public IQueryable<IPromotion> BuildQueryFrom(IQueryable<IPromotion> QueryBase)
        {
            return (from promotion in QueryBase
                    where promotion.StartDate == null || promotion.StartDate < _queryDate && promotion.EndDate == null || promotion.EndDate >= _queryDate
                    select promotion).AsQueryable();
        }
    }
}
