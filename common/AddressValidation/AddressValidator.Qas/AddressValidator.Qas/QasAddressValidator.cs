using System.Collections.Generic;

namespace AddressValidator.Qas
{
	using System;
	using System.Diagnostics.Contracts;
	using AddressValidation.Common;
	using Common;
	using Config;
	using Exceptions;
	using Helpers;
	using NetSteps.Encore.Core.IoC;

	[ContainerRegister(typeof(IAddressValidator), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class QasAddressValidator : AbstractAddressValidator
	{
		private readonly IQasHelper _qasHelper;

		public QasAddressValidator()
			: this(null) { }

		public QasAddressValidator(QasAddressValidatorConfig configuration)
		{
			var localConfig = configuration ?? QasAddressValidatorConfig.Current;

			if (string.IsNullOrEmpty(localConfig.UserName) || string.IsNullOrEmpty(localConfig.Password))
				throw new QasAddressValidatorException();

			_qasHelper = new QasHelper(localConfig);
		}

		protected override IAddressValidationResult PerformValidateAddress(IValidationAddress address)
		{
			Contract.Requires<ArgumentNullException>(address != null);

			var addressValidationResult = Create.New<IAddressValidationResult>();

			try
			{
				// Verify Address
				var result = _qasHelper.QasVerifyAddress(address);

				// Check response status & convert to local IValidationAddress
				addressValidationResult = _qasHelper.ProcessResponse(addressValidationResult, result);

			}
			catch (Exception ex)
			{
				addressValidationResult.Status = AddressInfoResultState.Error;
				addressValidationResult.Message = ex.Message;
				addressValidationResult.ValidAddresses = new List<IValidationAddress>(0);
			}

			return addressValidationResult;
		}
	}
}
