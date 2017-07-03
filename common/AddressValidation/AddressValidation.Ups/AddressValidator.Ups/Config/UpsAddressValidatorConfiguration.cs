using System.Configuration;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Encore.Core.IoC;

namespace AddressValidator.Ups.Config
{
	[ContainerRegister(typeof(UpsAddressValidatorConfiguration), RegistrationBehaviors.Default)]
    public class UpsAddressValidatorConfiguration : ConfigurationSection
    {
        public static readonly string SectionName = "upsAddressValidator";

        const string PropertyName_Username = "userName";
        const string PropertyName_Password = "password";
        const string PropertyName_EndpointUrl = "endpointUrl";
        const string PropertyName_AccessLicenseNumber = "accessLicenseNumber";

        [ConfigurationProperty(PropertyName_Username, IsRequired = true)]
        public string UserName
        {
            get { return (string)base[PropertyName_Username]; }
            set { base[PropertyName_Username] = value; }
        }
        [ConfigurationProperty(PropertyName_Password, IsRequired = true)]
        public string Password
        {
            get { return (string)base[PropertyName_Password]; }
            set { base[PropertyName_Password] = value; }
        }
        [ConfigurationProperty(PropertyName_EndpointUrl, IsRequired = true)]
        public string EndpointUrl
        {
            get { return (string)base[PropertyName_EndpointUrl]; }
            set { base[PropertyName_EndpointUrl] = value; }
        }

        [ConfigurationProperty(PropertyName_AccessLicenseNumber, IsRequired = true)]
        public string AccessLicenseNumber
        {
            get { return (string)base[PropertyName_AccessLicenseNumber]; }
            set { base[PropertyName_AccessLicenseNumber] = value; }
        }

        public static UpsAddressValidatorConfiguration Current
        {
            get
            {
                var current = ConfigurationManager.GetSection(SectionName) as UpsAddressValidatorConfiguration;
                return current ?? new UpsAddressValidatorConfiguration();
            }
        }
    }
}
