namespace AddressValidator.Common
{
    public interface IAddressValidator
    {
        /// <summary>
        /// Used by the framework to initialize the validator. Multiple calls must be benign.
        /// </summary>
        void Init();

        IAddressValidationResult ValidateAddress(IValidationAddress address);
    }
}