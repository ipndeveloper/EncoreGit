using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DistributorBackOffice.Areas.Orders.Models.Party
{
    public class AddIndividualGuestModel
    {
		public AddIndividualGuestModel(string firstName, string lastName, string fullName, string email, string id)
		{
			this.Id = id;
			this.FirstName = firstName;
			this.LastName = lastName;
			this.FullName = fullName;
			this.Email = email;
		}

		public AddIndividualGuestModel(string firstName, string lastName, string fullName, string email)
			: this(firstName, lastName, fullName, email, Guid.NewGuid().ToString("N"))
		{
		}

		public AddIndividualGuestModel()
			: this(string.Empty, string.Empty, "Guest Form", string.Empty)
		{
		}

		public string Id { get; private set; }
	
		public string FirstName { get; private set; }

		public string LastName { get; private set; }

		public string FullName { get; private set; }
	
		public string Email { get; private set; }
	}
}
