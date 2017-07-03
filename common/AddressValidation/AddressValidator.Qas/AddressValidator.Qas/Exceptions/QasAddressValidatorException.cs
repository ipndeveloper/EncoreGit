namespace AddressValidator.Qas.Exceptions
{
    using System;

    public class QasAddressValidatorException : Exception
    {
        public QasAddressValidatorException()
            : base("Invalid credentials for QAS Validator") { }

        public QasAddressValidatorException(string message)
            : base(message) { }
    }
}