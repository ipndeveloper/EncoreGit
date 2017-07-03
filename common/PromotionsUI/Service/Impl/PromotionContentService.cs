using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NetSteps.Promotions.UI.Common.Interfaces;
using NetSteps.Promotions.UI.Service.Context;

namespace NetSteps.Promotions.UI.Service.Impl
{
    public class PromotionContentService : IPromotionContentService
    {
        protected readonly Func<IPromotionsUIContext> _contextFactory;
        protected readonly IPromotionContentRepository _repository;

        public PromotionContentService(
            Func<IPromotionsUIContext> contextFactory,
            IPromotionContentRepository repository)
        {
            Contract.Requires<ArgumentNullException>(contextFactory != null);
            Contract.Requires<ArgumentNullException>(repository != null);

            _contextFactory = contextFactory;
            _repository = repository;
        }

        public IPromotionContent FirstOrDefault(int promotionID, int languageID)
        {
            using (var context = _contextFactory())
            {
                return _repository.FirstOrDefault(context, x =>
                    x.PromotionID == promotionID
                    && x.LanguageID == languageID
                );
            }
        }

        public void Add(IPromotionContent model)
        {
            using (var context = _contextFactory())
            {
                var entity = _repository.Add(context, model);
                context.SaveChanges();
                _repository.UpdateModel(model, entity);
            }
        }
    }
}
