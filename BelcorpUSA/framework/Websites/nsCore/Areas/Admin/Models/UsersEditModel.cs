using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities;

namespace nsCore.Areas.Admin.Models
{
	public class UsersEditModel
	{
		public UsersEditModel(int? userId)
		{
			this.CorporateUser = (userId.HasValue && userId.Value > 0) ?
				this.CorporateUser = CorporateUser.LoadFull(userId.Value) : this.CorporateUser = new CorporateUser() { User = new User() };

			this.Account = (this.CorporateUser != null) ? Account.GetByUserId(this.CorporateUser.UserID) : null;
		}

		public CorporateUser CorporateUser { get; private set; }

		public Account Account { get; private set; }
	}
}
