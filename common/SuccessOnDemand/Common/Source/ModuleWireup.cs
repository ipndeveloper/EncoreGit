using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Representation;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

// Indicates this assembly is dependent upon its own wireup command:
[assembly: Wireup(typeof(NetSteps.SOD.Common.ModuleWireup))]

namespace NetSteps.SOD.Common
{
	/// <summary>
	/// Wireup command called at bootstrap time by the wireup coordinator.
	/// </summary>
    [WireupDependency(typeof(NetSteps.Encore.Core.ModuleWireup))]
    public class ModuleWireup : WireupCommand
	{
		/// <summary>
		/// Wires this module.
		/// </summary>
		/// <param name="coordinator">the coordinator</param>
		/// <seealso cref="NetSteps.Encore.Core.IWireupCoordinator"/>
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
            var root = Container.Root;

            var generatedResponseType = root.New<IResponse>();
            root.ForType<IJsonRepresentation<IResponse>>()
                .Register(typeof(DelegatedJsonRepresentationLoose<,>).MakeGenericType(typeof(IResponse), generatedResponseType.GetType()))
                .End();
		}
	}
}