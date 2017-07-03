using System;
using System.Configuration;
using NetSteps.Configuration;

namespace BelcorpUSA.Edi.Service.Configuration
{
	public class EdiDropLocationConfigurationElement : DeclaredMemberConfigurationElement
	{
		public static readonly ConfigurationProperty LocationProperty = new ConfigurationProperty("location", typeof(string), String.Empty);
		public string Location
		{
			get
			{
				string retval = (string)this[LocationProperty];
				if (!String.IsNullOrWhiteSpace(retval))
				{
					bool isUri = Uri.IsWellFormedUriString(retval, UriKind.Absolute);
					if (isUri && !retval.EndsWith("/"))
					{
						retval += "/";
					}
					else if (!isUri && !retval.EndsWith("\\"))
					{
						retval += "\\";
					}
				}
				return retval;
			}
			set { this[LocationProperty] = value; }
		}

		public static readonly ConfigurationProperty PartnerNameProperty = new ConfigurationProperty("partnerName", typeof(string), String.Empty);
		public string PartnerName
		{
			get { return (string)this[PartnerNameProperty]; }
			set { this[PartnerNameProperty] = value; }
		}

		public static readonly ConfigurationProperty CredentialsProperty = new ConfigurationProperty("credentials", typeof(CredentialsConfigurationElement), null);
		public CredentialsConfigurationElement Credentials
		{
			get { return (CredentialsConfigurationElement)this[CredentialsProperty]; }
			set { this[CredentialsProperty] = value; }
		}

		public class CredentialsConfigurationElement : DeclaredMemberConfigurationElement
		{
			public static readonly ConfigurationProperty UserNameProperty = new ConfigurationProperty("userName", typeof(string), null);
			public string UserName
			{
				get { return (string)this[UserNameProperty]; }
				set { this[UserNameProperty] = value; }
			}

			public static readonly ConfigurationProperty PasswordProperty = new ConfigurationProperty("password", typeof(string), null);
			public string Password
			{
				get { return (string)this[PasswordProperty]; }
				set { this[PasswordProperty] = value; }
			}
		}
	}
}
