using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Commissions.Service.Models
{
    [ContainerRegister(typeof(IPropayDisbursementProfile), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest), System.Serializable]
	public class PropayDisbursementProfile : IPropayDisbursementProfile
	{

		public int PropayAccountNumber { get; set; }

		public int AccountId { get; set; }

        public DisbursementMethodKind DisbursementMethod { get { return DisbursementMethodKind.ProPay; } }

		public int DisbursementProfileId { get; set; }

		public bool EnrollmentFormReceived { get; set; }

		public bool IsEnabled { get; set; }

		public string NameOnAccount { get; set; }

		public decimal Percentage { get; set; }

		public int UserId { get; set; }

        public int CurrencyId { get; set; }
    }
}
