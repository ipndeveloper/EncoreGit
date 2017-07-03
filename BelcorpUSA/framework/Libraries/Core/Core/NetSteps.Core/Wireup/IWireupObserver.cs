using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Wireup.Meta;

namespace NetSteps.Encore.Core.Wireup
{
	/// <summary>
	/// Interface for wireup observers. Wireup coordinators will notify observers of 
	/// tasks and dependencies having a matching observer key.
	/// </summary>
	public interface IWireupObserver
	{
		/// <summary>
		/// Gets the observer's key.
		/// </summary>
		Guid ObserverKey { get; }
		/// <summary>
		/// Called by coordinators to notify observers of wireup tasks.
		/// </summary>
		/// <param name="coordinator"></param>
		/// <param name="task"></param>
		/// <param name="target"></param>
		void NotifyWireupTask(IWireupCoordinator coordinator, WireupTaskAttribute task, Type target);
	}
}
