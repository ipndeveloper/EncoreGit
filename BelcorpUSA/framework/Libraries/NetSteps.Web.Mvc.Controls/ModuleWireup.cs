// -----------------------------------------------------------------------
// <copyright file="Wireup.cs" company="NetSteps">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

[assembly: Wireup( typeof( NetSteps.Web.Mvc.Controls.ModuleWireup ) )]

namespace NetSteps.Web.Mvc.Controls
{
	[WireupDependency( typeof( NetSteps.Data.Entities.ModuleWireup ) )]
	public class ModuleWireup : WireupCommand
	{
		/// <summary>
		/// Wires this module.
		/// </summary>
		/// <param name="coordinator">the coordinator</param>
		/// <seealso cref="IWireupCoordinator"/>
		protected override void PerformWireup( IWireupCoordinator coordinator )
		{
		}
	}
}
