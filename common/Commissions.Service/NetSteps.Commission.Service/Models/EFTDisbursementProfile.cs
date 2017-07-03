using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Commissions.Service.Models
{
    [ContainerRegister(typeof(IEFTDisbursementProfile), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest), Serializable]
	public class EFTDisbursementProfile : IEFTDisbursementProfile
	{
		public string AccountNumber { get; set; }

		public int BankAccountTypeId { get; set; }

		public string BankName { get; set; }

		public string BankPhone { get; set; }

		public string RoutingNumber { get; set; }

		public int AccountId { get; set; }

        public DisbursementMethodKind DisbursementMethod { get { return DisbursementMethodKind.EFT; } }

		public int DisbursementProfileId { get; set; }

		public bool EnrollmentFormReceived { get; set; }

		public bool IsEnabled { get; set; }

		public string NameOnAccount { get; set; }

		public decimal Percentage { get; set; }

		public int UserId { get; set; }

        public int AddressId { get; set; }

        public int CurrencyId { get; set; }
    }
}
