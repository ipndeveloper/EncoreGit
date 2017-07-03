using System;

namespace AddressValidator.Ups.Exceptions
{
    public class UpsAddressValidatorCredentialException : Exception
    {
        public UpsAddressValidatorCredentialException()
            : base("Invalid credentials for UPS Validator")
        {
        }

        public UpsAddressValidatorCredentialException(string message)
            : base(message)
        {
        }
    }
}
