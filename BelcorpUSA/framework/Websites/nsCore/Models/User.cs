using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nsCore.Models
{
	public class User
	{
		public string Username { get; set; }
		public string Password { get; set; }
		public string FullName { get { return "John Doe"; } }
	}
}