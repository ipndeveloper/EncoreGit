using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Encore.Core.Parallel
{
	/// <summary>
	/// Indicates the Reactor's state.
	/// </summary>
	public enum ReactorState
	{
		/// <summary>
		/// Indicates the reactor is idle.
		/// </summary>
		Idle = 0,
		/// <summary>
		/// Indicates the reactor is active.
		/// </summary>
		Active = 1,
		/// <summary>
		/// Indicates the reactor is suspending.
		/// </summary>
		SuspendSignaled = 2,
		/// <summary>
		/// Indicates the reactor is suspended.
		/// </summary>
		Suspended = 3,
		/// <summary>
		/// Indicates the reactor is stopping.
		/// </summary>
		StopSignaled = 4,
		/// <summary>
		/// Indicates the reactor has stopped.
		/// </summary>
		Stopped = 5,
	}
}
