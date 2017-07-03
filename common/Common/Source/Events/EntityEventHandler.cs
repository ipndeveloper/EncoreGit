using System;

namespace NetSteps.Common.Events
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Class to pass back a specific object when an event occurred with that object.
	/// Created: 09-21-2009
	/// </summary>
	public delegate void EntityEventHandler<T>(object sender, EntityEventHandlerArgs<T> e);

	public class EntityEventHandlerArgs<T> : EventArgs
	{
		public T Entity { get; set; }

		public EntityEventHandlerArgs()
		{
		}

		public EntityEventHandlerArgs(T entity)
		{
			Entity = entity;
		}
	}
}
