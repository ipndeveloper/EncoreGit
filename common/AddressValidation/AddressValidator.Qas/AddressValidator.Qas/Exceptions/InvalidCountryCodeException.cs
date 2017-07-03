namespace AddressValidator.Qas.Exceptions
{
    public class InvalidCountryCodeException : QasAddressValidatorException
    {
        public InvalidCountryCodeException(string code)
            : base(string.Format("Invalid country code \"{0}\", QAS validator expects an ISO 3166-1 alpha-3 code, the conversion for the given ISO 3166-1 alpha-2 failed. There is likely no conversion specified for the given country and the QAS module must be modified to add it.", code)) { }
    }
}