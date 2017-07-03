using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using E = NetSteps.Communication.Services.Entities;

namespace NetSteps.Communication.Services.Context.Mocks
{
    public class MockCommunicationDatabase
    {
        public HashSet<E.AccountAlert> AccountAlerts = new HashSet<E.AccountAlert>();
        public HashSet<E.AccountAlertDisplayKind> AccountAlertDisplayKinds = new HashSet<E.AccountAlertDisplayKind>();
        public HashSet<E.MessageAccountAlert> MessageAccountAlerts = new HashSet<E.MessageAccountAlert>();
        public HashSet<E.PromotionAccountAlert> PromotionAccountAlerts = new HashSet<E.PromotionAccountAlert>();
    }
}
