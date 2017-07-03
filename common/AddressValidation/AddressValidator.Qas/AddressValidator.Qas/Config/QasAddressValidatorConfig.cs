namespace AddressValidator.Qas.Config
{
	using System.Configuration;
	using NetSteps.Encore.Core.IoC;

	[ContainerRegister(typeof(QasAddressValidatorConfig), RegistrationBehaviors.Default)]
	public class QasAddressValidatorConfig : ConfigurationSection
	{
		public static readonly string SectionName = "qasAddressValidator";

		const string PropertyNameUsername = "userName";
		const string PropertyNamePassword = "password";
		const string PropertyNameEndpointUrl = "endpointUrl";

		[ConfigurationProperty(PropertyNameUsername, IsRequired = true)]
		public virtual string UserName
		{
			get { return (string)base[PropertyNameUsername]; }
			set { base[PropertyNameUsername] = value; }
		}
		[ConfigurationProperty(PropertyNamePassword, IsRequired = true)]
		public virtual string Password
		{
			get { return (string)base[PropertyNamePassword]; }
			set { base[PropertyNamePassword] = value; }
		}
		[ConfigurationProperty(PropertyNameEndpointUrl, IsRequired = true)]
		public virtual string EndpointUrl
		{
			get { return (string)base[PropertyNameEndpointUrl]; }
			set { base[PropertyNameEndpointUrl] = value; }
		}

		public static QasAddressValidatorConfig Current
		{
			get
			{
				var current = ConfigurationManager.GetSection(SectionName) as QasAddressValidatorConfig;
				return current ?? new QasAddressValidatorConfig();
			}
		}
	}
}