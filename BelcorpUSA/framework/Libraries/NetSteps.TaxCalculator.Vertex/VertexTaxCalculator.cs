using System;
using NetSteps.Common.Exceptions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Taxes.Common;
using NetSteps.Taxes.Common.Models;

namespace NetSteps.TaxCalculator.Vertex
{
	public class VertexTaxCalculator : ITaxCalculator
	{
		public ITaxArea LookupTaxArea(ITaxAddress taxAddress)
		{
			try
			{
				return LookupTaxAreaServiceProxy.Lookup(taxAddress);
			}
			catch (Exception ex)
			{
				var nsEx = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.TaxesNotFoundForAddressException);
				throw new TaxesNotFoundForAddressException("The tax area lookup failed. This may mean that you have an invalid shipping address.", nsEx);
			}
		}

		public ITaxCalculationResult CalculateTax(ITaxOrder taxOrder)
		{
			return CalculateTaxServiceProxy.Quote(taxOrder);
		}

		public ITaxCalculationResult FinalizeTax(ITaxOrder taxOrder)
		{
			return CalculateTaxServiceProxy.Invoice(taxOrder);
		}
	}
}
