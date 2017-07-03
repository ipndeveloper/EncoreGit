using System;
using System.Diagnostics.Contracts;

namespace NetSteps.Encore.Core.Wireup.Meta
{
	/// <summary>
	/// Base wireup task attribute. Wireup tasks are executed by the wireup coordinator opon discovery.
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
	public abstract class WireupTaskAttribute : Attribute
	{		
		/// <summary>
		/// Createas a new instance.
		/// </summary>
		protected WireupTaskAttribute() : this(WireupPhase.Tasks)
		{
		}

		/// <summary>
		/// Createas a new instance.
		/// </summary>
		/// <param name="phase">the wireup phase in which the task is executed</param>
		protected WireupTaskAttribute(WireupPhase phase)
		{
			this.Phase = phase;
		}

		/// <summary>
		/// Indicates the wireup phase.
		/// </summary>
		public WireupPhase Phase { get; private set; }

		/// <summary>
		/// Called by the framework to execute the task.
		/// </summary>
		internal void ExecuteTask(IWireupCoordinator coordinator)
		{
			Contract.Requires<ArgumentNullException>(coordinator != null);

			PerformTask(coordinator);
		}

		/// <summary>
		/// Called by the base class upon execution. Derived classes should 
		/// provide an implementation that performs the wireup logic.
		/// </summary>
		protected abstract void PerformTask(IWireupCoordinator coordinator);		
	}
}