using System;
using System.Diagnostics.Contracts;
namespace AddressValidator.Common
{
    public abstract class AbstractAddressValidator : IAddressValidator
    {
        private bool _initailized;
        public void Init()
        {
            if (!_initailized)
            {
                PerformInit();
                _initailized = true;
            }
        }

        protected virtual void PerformInit()
        {
            // Do your initialization shiite.
        }


		public IAddressValidationResult ValidateAddress(IValidationAddress address)
		{
			Contract.Requires<ArgumentNullException>(address != null);
			Contract.Ensures(Contract.Result<IAddressValidationResult>() != null);

			return PerformValidateAddress(address);
		}

		protected abstract IAddressValidationResult PerformValidateAddress(IValidationAddress address);
	}
}
