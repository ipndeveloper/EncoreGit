using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.IoC;
using NetSteps.Foundation.Entity.Mocks;
using E = NetSteps.Communication.Services.Entities;

namespace NetSteps.Communication.Services.Context.Mocks
{
    public class MockCommunicationContext : MockDbContext, ICommunicationContext
    {
        public MockCommunicationContext() : this(new MockCommunicationDatabase()) { }

        public MockCommunicationContext(MockCommunicationDatabase database)
        {
            AccountAlerts = new MockDbSet<E.AccountAlert>(database.AccountAlerts, x => x.AccountAlertId);
            AccountAlertDisplayKinds = new MockDbSet<E.AccountAlertDisplayKind>(database.AccountAlertDisplayKinds, x => x.AccountAlertDisplayKindId);
            MessageAccountAlerts = new MockDbSet<E.MessageAccountAlert>(database.MessageAccountAlerts, x => x.AccountAlertId);
            PromotionAccountAlerts = new MockDbSet<E.PromotionAccountAlert>(database.PromotionAccountAlerts, x => x.AccountAlertId);
        }

        public IDbSet<E.AccountAlert> AccountAlerts { get; set; }
        public IDbSet<E.AccountAlertDisplayKind> AccountAlertDisplayKinds { get; set; }
        public IDbSet<E.MessageAccountAlert> MessageAccountAlerts { get; set; }
        public IDbSet<E.PromotionAccountAlert> PromotionAccountAlerts { get; set; }
    }
}
