using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Encore.Core.Wireup;
using System.Reflection;

// Let the wireup coordinator know where it can start our wireup process.
[assembly: Wireup(typeof(NetSteps.Encore.Core.Tests.Wireup.DependentWireupCommand))]

// NOTE: You can also indicate this at the module level (for multi-module assemblies).
//[module: Wireup(typeof(NetSteps.Encore.Core.Tests.Wireup.DependentWireupCommand))]

// NOTE: It is also possible to declare dependencies directly on the assembly or module.
//       Doing so enables the wireup coordinator to ensure that everything is wired up
//       in the proper order.
[assembly: WireupDependency(typeof(NetSteps.Encore.Core.ModuleWireup))]

namespace NetSteps.Encore.Core.Tests.Wireup
{
	/// <summary>
	/// This is a wireup command that is dependent on another.
	/// Its dependency exists in this same assembly, but most
	/// dependencies will be between assemblies.
	/// </summary>
	[WireupDependency(typeof(IndependentWireupCommand))]
	public class DependentWireupCommand : WireupCommand
	{
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
			// We can also use the given coordinator to add another dependency,
			// This call is redundant and benign due to the WireupDependencyAttribute
			// applied above.
			// NOTE: The wireup coordinator ensures that things are wired up exactly
			//       once, regardless of how many times they are referred to during
			//       the process of wiring up.
			coordinator.WireupDependency(typeof(IndependentWireupCommand));

			Coordinator = coordinator;
			WireupCount += 1;
		}

		public static IWireupCoordinator Coordinator { get; private set; }
		public static int WireupCount { get; set; }
	}

	public class IndependentWireupCommand : WireupCommand
	{
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
			Coordinator = coordinator;
			WireupCount += 1;
		}

		public static IWireupCoordinator Coordinator { get; private set; }
		public static int WireupCount { get; set; }
	}


	[TestClass]
	public class WireupCoordinatorTests
	{
		[TestMethod]
		public void WireupWillOnlyHappenOnceForEachCommand()
		{
			Assert.IsTrue(DependentWireupCommand.WireupCount <= 1, "coordination of each wireup command happens at most once.");
			Assert.IsTrue(IndependentWireupCommand.WireupCount == DependentWireupCommand.WireupCount, "independent wireup command must only be wired if the dependent wireup command is wired.");

			// Any application should make this call to initiate wireup.
			// Note that if there aren't dependencies or interdependencies then
			// each independent assembly will need to be wired separately.
			IWireupCoordinator coordinator = WireupCoordinator.Instance;
			coordinator.WireupDependencies(Assembly.GetExecutingAssembly());

			Assert.AreEqual(coordinator, DependentWireupCommand.Coordinator, "there should be only one coordinator");
			Assert.AreEqual(1, DependentWireupCommand.WireupCount, "coordination should only happen once per command");

			Assert.AreEqual(coordinator, IndependentWireupCommand.Coordinator, "there should be only one coordinator");
			Assert.AreEqual(1, IndependentWireupCommand.WireupCount, "coordination should only happen once per command");
		}

		[TestMethod]
		public void WireupWillNotHappenForIgnoredAssemblies()
		{
			IWireupCoordinator coordinator = WireupCoordinator.Instance;
			var result = coordinator.WireupDependencies(typeof(string).Assembly);

			Assert.IsFalse(result.Any());
		}
	}
}
