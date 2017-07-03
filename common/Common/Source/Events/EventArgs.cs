using System;

namespace NetSteps.Common.Events
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Generic args class.
	/// Created: 08-13-2010
	/// </summary>
	public class EventArgs<T> : EventArgs
	{
		public T Item { get; set; }

		public EventArgs()
		{
		}

		public EventArgs(T item)
		{
			Item = item;
		}
	}
}
