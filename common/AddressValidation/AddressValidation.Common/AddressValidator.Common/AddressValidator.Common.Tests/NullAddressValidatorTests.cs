using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AddressValidator.Common;
using AddressValidation.Common.Tests.Helpers;
using NetSteps.Common.Data;
using NetSteps.Data.Common.Entities;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;

namespace AddressValidation.Common.Tests
{
	[TestClass]
	public class NullAddressValidatorTests
	{		
		[TestMethod]
		public void NullAddressValidator_Returns_Positive_And_Exact_Address()
		{
			NullAddressValidator validator = new NullAddressValidator();

			IAddress a = new Address()
			{
				Address1 = "Address1",
				Address2 = "Address2",
				Address3 = "Address3",
				City = "City",
				State = "State",
				Country = "Country",
				PostalCode = "PostalCode"
			};

			IValidationAddress va = a.ToValidationAddress();

			IAddressValidationResult result = validator.ValidateAddress(va);

			Assert.AreEqual(result.Status, AddressInfoResultState.Success);
			Assert.AreEqual(result.ValidAddresses.First(), va);
		}

		[TestMethod]
		public void NullAddressValidator_Is_Default_IAddressValidator_For_IoC()
		{
			WireupCoordinator.SelfConfigure();
			IAddressValidator validator = Create.New<IAddressValidator>();
			Assert.IsInstanceOfType(validator, typeof(NullAddressValidator));
		}
	}
}
