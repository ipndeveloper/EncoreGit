
using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Commissions.Service.Models
{
    [ContainerRegister(typeof(IEFTAccount), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest), System.Serializable]
	public class EFTAccount : IEFTAccount
	{
		public string AccountNumber { get; set; }

		public BankAccountKind AccountType { get; set; }

		public string BankAddress1 { get; set; }

		public string BankAddress2 { get; set; }

		public string BankAddress3 { get; set; }

		public string BankCity { get; set; }

		public string BankCountry { get; set; }

		public string BankCounty { get; set; }

		public string BankName { get; set; }

		public string BankPhone { get; set; }

		public string BankState { get; set; }

		public string BankZip { get; set; }

		public int DisbursementProfileId { get; set; }

		public string Name { get; set; }

		public int PercentToDeposit { get; set; }

		public string RoutingNumber { get; set; }

	    public bool IsEnabled { get; set; }
    }
}
