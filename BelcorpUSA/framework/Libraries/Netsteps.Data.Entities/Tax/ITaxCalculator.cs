using System;
using System.Diagnostics.Contracts;
using NetSteps.Taxes.Common.Models;

namespace NetSteps.Taxes.Common
{
	/// <summary>
	/// Provides tax info for addresses and orders.
	/// </summary>
	[ContractClass(typeof(Contracts.TaxCalculatorContracts))]
	public interface ITaxCalculator
	{
		ITaxArea LookupTaxArea(ITaxAddress taxAddress);
		ITaxCalculationResult CalculateTax(ITaxOrder taxOrder);
		ITaxCalculationResult FinalizeTax(ITaxOrder taxOrder);
	}

	namespace Contracts
	{
		[ContractClassFor(typeof(ITaxCalculator))]
		internal abstract class TaxCalculatorContracts : ITaxCalculator
		{
			ITaxArea ITaxCalculator.LookupTaxArea(ITaxAddress taxAddress)
			{
				Contract.Requires<ArgumentNullException>(taxAddress != null);
				throw new NotImplementedException();
			}

			ITaxCalculationResult ITaxCalculator.CalculateTax(ITaxOrder taxOrder)
			{
				Contract.Requires<ArgumentNullException>(taxOrder != null);
				throw new NotImplementedException();
			}

			ITaxCalculationResult ITaxCalculator.FinalizeTax(ITaxOrder taxOrder)
			{
				Contract.Requires<ArgumentNullException>(taxOrder != null);
				throw new NotImplementedException();
			}
		}
	}
}
