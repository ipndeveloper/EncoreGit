using System;
using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Commissions.Service.Models
{
    [ContainerRegister(typeof(IDisbursementProfile), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest), Serializable]
	public class DisbursementProfile : IDisbursementProfile
	{
        public int AccountId { get; set; }

        public int CurrencyId { get; set; }

        public DisbursementMethodKind DisbursementMethod { get; set; }

        public int DisbursementProfileId { get; set; }

        public bool EnrollmentFormReceived { get; set; }

        public bool IsEnabled { get; set; }

        public string NameOnAccount { get; set; }

        public decimal Percentage { get; set; }
    }
}
