using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NetSteps.Encore.Core.IoC;
using NetSteps.Foundation.Entity;
using NetSteps.Promotions.UI.Common.Interfaces;
using NetSteps.Promotions.UI.Service.Context;
using E = NetSteps.Promotions.UI.Service.Entities;

namespace NetSteps.Promotions.UI.Service.Impl
{
    public interface IPromotionContentRepository : IEntityModelRepository<E.PromotionContent, IPromotionContent, IPromotionsUIContext>
    {
    }

    [ContainerRegister(typeof(IPromotionContentRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class PromotionContentRepository : EntityModelRepository<E.PromotionContent, IPromotionContent, IPromotionsUIContext>, IPromotionContentRepository
    {
        public override Expression<Func<E.PromotionContent, bool>> GetPredicateForModel(IPromotionContent m)
        {
            return e => e.PromotionContentID == m.PromotionContentID;
        }

        public override void UpdateEntity(E.PromotionContent e, IPromotionContent m)
        {
            e.PromotionID = m.PromotionID;
            e.LanguageID = m.LanguageID;
            e.Title = m.Title;
            e.ActionText = m.ActionText;
            e.Description = m.Description;
            e.ImagePath = m.ImagePath;
            e.AlertTitle = m.AlertTitle;
        }

        public override void UpdateModel(IPromotionContent m, E.PromotionContent e)
        {
            m.PromotionContentID = e.PromotionContentID;
            m.PromotionID = e.PromotionID;
            m.LanguageID = e.LanguageID;
            m.Title = e.Title;
            m.ActionText = e.ActionText;
            m.Description = e.Description;
            m.ImagePath = e.ImagePath;
            m.AlertTitle = e.AlertTitle;
        }
    }
}
