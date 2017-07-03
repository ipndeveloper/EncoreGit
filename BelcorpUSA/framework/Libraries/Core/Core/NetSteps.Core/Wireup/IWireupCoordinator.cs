
using System;
using System.Reflection;
using System.Collections.Generic;

namespace NetSteps.Encore.Core.Wireup
{
	/// <summary>
	/// Ensures assemblies are wired up according to their declared
	/// wireup dependencies and that all wireup commands are given
	/// a chance to run.
	/// </summary>
	public interface IWireupCoordinator
	{
		/// <summary>
		/// Coordinates the wireup of an assembly.
		/// </summary>
		/// <param name="assembly"></param>
		IEnumerable<AssemblyDependency> WireupDependencies(Assembly assembly);

        /// <summary>
        /// Called by the framework when an assembly is loaded.
        /// </summary>
        /// <param name="assembly"></param>
        void NotifyAssemblyLoaded(Assembly assembly);

		/// <summary>
		/// Directly wires up a dependency (if it is not already wired).
		/// </summary>
		/// <param name="type"></param>
		void WireupDependency(Type type);

		/// <summary>
		/// Exposes the dependencies an assembly has on other assemblies.
		/// </summary>
		/// <param name="assem">the target assembly</param>
		/// <returns>all known dependencies according to wireup declarations</returns>
		IEnumerable<AssemblyDependency> ExposeDependenciesFor(Assembly assem);

		/// <summary>
		/// Registers an observer.
		/// </summary>
		/// <param name="observer"></param>
		void RegisterObserver(IWireupObserver observer);
		
		/// <summary>
		/// Unregisters an observer.
		/// </summary>
		/// <param name="observerKey"></param>
		void UnregisterObserver(Guid observerKey);
	}
}
