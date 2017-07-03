using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nsCore.Areas.Admin.Models
{
	public class UsersSaveModel
	{
		public int? userId { get; set; }
		public string firstName { get; set; }
		public string lastName { get; set; }
		public string username { get; set; }
		public string password { get; set; }
		public string passwordQuestion { get; set; }
		public string passwordAnswer { get; set; }
		public string confirmPassword { get; set; }
		public bool hasAccessToAllSites { get; set; }
		public bool userChangingPassword { get; set; }
		public string email { get; set; }
		public short statusId { get; set; }
		public int defaultLanguageId { get; set; }
		public List<int> roles { get; set; }
		public List<int> sites { get; set; }
		public bool createShoppingAccount { get; set; }
	}
}
