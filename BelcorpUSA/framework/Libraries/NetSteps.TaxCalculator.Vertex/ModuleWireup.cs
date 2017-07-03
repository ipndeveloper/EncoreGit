using System;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Tax;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.TaxCalculator.Vertex.CalculateTaxService60;
using NetSteps.Taxes.Common;
using NetSteps.Taxes.Common.Models;

// Indicates this assembly is dependent upon its own wireup command:
[assembly: Wireup(typeof(NetSteps.TaxCalculator.Vertex.ModuleWireup))]

namespace NetSteps.TaxCalculator.Vertex
{
	/// <summary>
	/// Wireup command called at bootstrap time by the wireup coordinator.
	/// </summary>
	[WireupDependency(typeof(NetSteps.Data.Entities.ModuleWireup))]
	public class ModuleWireup : WireupCommand
	{
		/// <summary>
		/// Wires this module.
		/// </summary>
		/// <param name="coordinator">the coordinator</param>
		/// <seealso cref="IWireupCoordinator"/>
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
			var root = Container.Root;

		    root.ForType<IEnumConverter<JurisdictionLevelCodeType, JurisdictionLevel>>()
		        .Register(typeof(JurisdictionConverter))
		        .ResolveAsSingleton()
		        .End();

			root.ForType<ITaxCalculator>()
				.Register<VertexTaxCalculator>()
				.ResolveAnInstancePerRequest()
				.End()
				.ForType<Func<ITaxCalculator>>()
				.Register<Func<ITaxCalculator>>((c, p) => c.New<ITaxCalculator>)
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<ITaxService>()
				.Register<TaxService>(
					Param.Resolve<Func<ITaxCalculator>>(),
					Param.Resolve<Func<InventoryBaseRepository>>()
				)
				.ResolveAnInstancePerRequest()
				.End();
		}
	}
}
