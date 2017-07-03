using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using System.Text;
using NetSteps.Foundation.Entity;
using E = NetSteps.Communication.Services.Entities;

namespace NetSteps.Communication.Services
{
    public interface ICommunicationContext : IDbContext
    {
        IDbSet<E.AccountAlert> AccountAlerts { get; }
        IDbSet<E.AccountAlertDisplayKind> AccountAlertDisplayKinds { get; }
        IDbSet<E.PromotionAccountAlert> PromotionAccountAlerts { get; }
        IDbSet<E.MessageAccountAlert> MessageAccountAlerts { get; }
    }
}
