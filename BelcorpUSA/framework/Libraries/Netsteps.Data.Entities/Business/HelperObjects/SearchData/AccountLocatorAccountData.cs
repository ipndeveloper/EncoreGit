using NetSteps.Encore.Core.IoC;
namespace NetSteps.Data.Entities.Business
{
	public interface IAccountLocatorAccountData
	{
		int AccountID { get; }
		string FirstName { get; }
		string LastName { get; }

		string City { get; }
		string State { get; }
		int? CountryID { get; }

		double? Distance { get; }
        string EmailAddress { get; set; }
        string PhoneNumber { get; set; }
	}

	[ContainerRegister(typeof(IAccountLocatorAccountData), RegistrationBehaviors.Default)]
	public class AccountLocatorAccountData : IAccountLocatorAccountData
	{
		public int AccountID { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public string City { get; set; }
		public string State { get; set; }
		public int? CountryID { get; set; }

		public double? Distance { get; set; }


        public string EmailAddress
        {
            get;
            set;

        }

        public string PhoneNumber
        {
            get;
            set;
        }
    }
}
