using System.ComponentModel.DataAnnotations;

namespace nsDistributor.Models
{
	public class ContactMeModel
	{
		[Required]
		public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }

		[Required]
		public string Email { get; set; }

		public string Phone { get; set; }

        public string State { get; set; }

		public string Comments { get; set; }

	    public void ApplyTo(NetSteps.Data.Entities.Account account)
	    {
	        account.FirstName = FirstName;
	        account.LastName = LastName;
	        account.EmailAddress = Email;
	        account.HomePhone = Phone;
	    }
	}
}