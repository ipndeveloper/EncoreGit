using System;
using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Commissions.Service.Models
{
    [ContainerRegister(typeof(IAccountTitle), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest), Serializable]
    public class AccountTitle : IAccountTitle
    {
        public int AccountId { get; set; }

        public DateTime DateModified { get; set; }

        public int PeriodId { get; set; }

        public int TitleId { get; set; }

        public int TitleKindId { get; set; }

    }
}
