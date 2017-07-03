using NetSteps.Auth.Common.Model;
using System;

namespace NetSteps.Auth.UI.Common.DataObjects
{
	public class Credentials : ICredentials
	{
		private string _password;
		public string Password
		{
			get
			{
				return _password;
			}
			set
			{
				_password = value;
			}
		}

		private int _siteID;
		public int SiteID
		{
			get
			{
				return _siteID;
			}
			set
			{
				_siteID = value;
			}
		}

        private string _CFP;
        public string CFP
        {
            get { return _CFP; }
            set { _CFP = value; }
        }

        private DateTime _birthDay;
        public DateTime BirthDay
        {
            get { return _birthDay; }
            set { _birthDay = value; }
        }

		private string _userUniqueIdentifier;
		public string UserUniqueIdentifier
		{
			get
			{
				return _userUniqueIdentifier;
			}
			set
			{
				_userUniqueIdentifier = value;
			}
		}
	}
}
