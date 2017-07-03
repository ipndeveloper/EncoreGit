using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.IoC;
using NetSteps.Foundation.Common;
using NetSteps.Foundation.Entity;
using E = NetSteps.Promotions.UI.Service.Entities;

namespace NetSteps.Promotions.UI.Service.Context
{
    [ContainerRegister(typeof(IPromotionsUIContext), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class PromotionsUIContext : DbContext, IPromotionsUIContext
    {
        public PromotionsUIContext() : base("name=" + ConnectionStringNames.Core) { }

        IDbSet<TEntity> IDbContext.Set<TEntity>()
        {
            return base.Set<TEntity>();
        }

        public IDbSet<E.PromotionContent> PromotionContent { get; set; }
    }
}
