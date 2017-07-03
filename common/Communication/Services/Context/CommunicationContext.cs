using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.IoC;
using NetSteps.Foundation.Common;
using NetSteps.Foundation.Entity;
using E = NetSteps.Communication.Services.Entities;

namespace NetSteps.Communication.Services
{
    [ContainerRegister(typeof(ICommunicationContext), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class CommunicationContext : DbContext, ICommunicationContext
    {
        public CommunicationContext() : base("name=" + ConnectionStringNames.Core) { }

        IDbSet<TEntity> IDbContext.Set<TEntity>()
        {
            return base.Set<TEntity>();
        }

        public IDbSet<E.AccountAlert> AccountAlerts { get; set; }
        public IDbSet<E.AccountAlertDisplayKind> AccountAlertDisplayKinds { get; set; }
        public IDbSet<E.PromotionAccountAlert> PromotionAccountAlerts { get; set; }
        public IDbSet<E.MessageAccountAlert> MessageAccountAlerts { get; set; }
    }
}
