using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Data.Entities.Tax;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.TaxCalculator.Vertex.Tests
{
	[TestClass]
	public class ModuleWireupTests
	{
		[TestMethod]
		public void ITaxService_IsTaxServiceAndVertexTaxCalculator()
		{
			var taxService = Create.New<ITaxService>();
			Assert.IsNotNull(taxService);
			Assert.IsInstanceOfType(taxService, typeof(TaxService));

			var taxCalculator = ((TaxService)taxService).TaxCalculator;
			Assert.IsNotNull(taxCalculator);
			Assert.IsInstanceOfType(taxCalculator, typeof(VertexTaxCalculator));
		}
	}
}
