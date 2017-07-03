using System;
using NetSteps.Encore.Core.IoC;

namespace AddressValidator.Common
{
	/// <summary>
	/// Default address validator implementation; does nothing.
	/// </summary>
	[ContainerRegister(typeof(IAddressValidator), RegistrationBehaviors.Default | RegistrationBehaviors.IfNoneOther, ScopeBehavior = ScopeBehavior.Singleton)]
	public sealed class NullAddressValidator : AbstractAddressValidator, IAddressValidator
	{				
		protected override IAddressValidationResult PerformValidateAddress(IValidationAddress address)
		{
			IAddressValidationResult result = Create.New<IAddressValidationResult>();
			result.Status = AddressValidation.Common.AddressInfoResultState.Success;
			result.ValidAddresses = new IValidationAddress[] { address };
			result.Message = String.Empty;
			return result;
		}
	}
}
