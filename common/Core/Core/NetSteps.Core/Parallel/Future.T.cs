using System;
using System.Diagnostics;
using System.Threading;

namespace NetSteps.Encore.Core.Parallel
{
	/// <summary>
	/// Default waitable implementation.
	/// </summary>
	public sealed class Future<T>
	{
		const int Status_Waiting = 0;
		const int Status_Completed = 1;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		Object _lock = new Object();
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		int _status;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		Exception _fault;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		T _value;

		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		public Future()
		{
			_status = Status_Waiting;
		}

		/// <summary>
		/// Marks the completion.
		/// </summary>
		/// <param name="value"></param>
		public void MarkCompleted(T value)
		{
			lock (_lock)
			{
				// Ensure the wait completes only once...
				if (_status != Status_Waiting)
					throw new InvalidOperationException("Already completed");
				_value = value;
				_status = Status_Completed;
				Monitor.PulseAll(_lock);
			}
		}

		/// <summary>
		/// Marks the completion.
		/// </summary>
		/// <param name="fault"></param>
		public void MarkFaulted(Exception fault)
		{
			lock (_lock)
			{
				// Ensure the wait completes only once...
				if (_status != Status_Waiting)
					throw new InvalidOperationException("Already completed");
				_fault = fault;
				_status = Status_Completed;
				Monitor.PulseAll(_lock);
			}
		}

		/// <summary>
		/// Indicates whether the wait has completed.
		/// </summary>
		public bool IsCompleted
		{
			get { return Thread.VolatileRead(ref _status) == Status_Completed; }
		}

		/// <summary>
		/// Determines if the completion resulted in an error.
		/// </summary>
		public bool IsFaulted
		{
			get
			{
				Thread.MemoryBarrier();
				var fault = _fault;
				Thread.MemoryBarrier();
				return fault != null;
			}
		}

		/// <summary>
		/// Gets the exception that caused the fault.
		/// </summary>
		public Exception Exception
		{
			get
			{
				Thread.MemoryBarrier();
				var fault = _fault;
				Thread.MemoryBarrier();
				return fault;
			}
		}

		/// <summary>
		/// Waits (blocks the current thread) until the value is present or the timeout is exceeded.
		/// </summary>
		/// <param name="timeout">A timespan representing the timeout period.</param>
		/// <returns>The future's value.</returns>
		public bool Wait(TimeSpan timeout)
		{
			if (timeout.Ticks > 0)
			{
				lock (_lock)
				{
					if (!this.IsCompleted)
					{
						Monitor.Wait(_lock, timeout);
					}
				}
			}
			return this.IsCompleted;
		}

		/// <summary>
		/// Gets the future variable's value. Warning! Reading this property
		/// will block your thread indefinitely or until the future variable
		/// has been set; whichever comes sooner.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public T Value
		{
			get
			{
				return AwaitValue();
			}
		}

		/// <summary>
		/// Tries to read the value. This call will not block the calling
		/// thread if the value is not present.
		/// </summary>
		/// <param name="value">A reference where the value will be written if 
		/// it is present.</param>
		/// <returns><em>true</em> if the value was successfully read; otherwise <em>false</em>.</returns>
		public bool TryGetValue(out T value)
		{
			if (this.IsCompleted)
			{
				if (!this.IsFaulted)
				{
					value = this.Value;
					return true;
				}
			}
			value = default(T);
			return false;
		}

		/// <summary>
		/// Tries to read the value. This call will not block the calling
		/// thread for the period of the timeout if the value is not present.
		/// </summary>
		/// <param name="millisecondsTimeout">timeout in milliseconds</param>
		/// <param name="value">A reference where the value will be written if 
		/// it is present.</param>
		/// <returns><em>true</em> if the value was successfully read; otherwise <em>false</em>.</returns>
		public bool TryGetValue(int millisecondsTimeout, out T value)
		{
			if (millisecondsTimeout > 0)
			{
				return TryGetValue(TimeSpan.FromMilliseconds(millisecondsTimeout), out value);
			}
			else
			{
				return TryGetValue(out value);
			}
		}

		/// <summary>
		/// Tries to read the value. This call will not block the calling
		/// thread for the period of the timeout if the value is not present.
		/// </summary>
		/// <param name="timeout">the timeout</param>
		/// <param name="value">A reference where the value will be written if 
		/// it is present.</param>
		/// <returns><em>true</em> if the value was successfully read; otherwise <em>false</em>.</returns>
		public bool TryGetValue(TimeSpan timeout, out T value)
		{
			if (Wait(timeout))
			{
				value = Value;
				return true;
			}
			value = default(T);
			return false;
		}

		/// <summary>
		/// Waits (blocks the current thread) until the value is present.
		/// </summary>
		/// <returns>The future's value.</returns>
		public T AwaitValue()
		{
			lock (_lock)
			{
				if (!this.IsCompleted)
				{
					Monitor.Wait(_lock);
				}
			}

			if (this.IsFaulted)
				throw new ParallelException("Background thread faulted.", this.Exception);

			Thread.MemoryBarrier();
			var val = _value;
			Thread.MemoryBarrier();
			return val;
		}

		/// <summary>
		/// Waits (blocks the current thread) until the value is present or the timeout is exceeded.
		/// </summary>
		/// <param name="millisecondsTimeout">Timeout in milliseconds.</param>
		/// <returns>The future's value.</returns>
		/// <exception cref="ParallelTimeoutException">thrown if the timeout is exceeded before the value becomes available.</exception>
		public T AwaitValue(int millisecondsTimeout)
		{
			if (millisecondsTimeout > 0)
			{
				return AwaitValue(TimeSpan.FromMilliseconds(millisecondsTimeout));
			}
			else if (this.IsCompleted)
			{
				if (this.IsFaulted)
					throw new ParallelException("Background thread faulted.", this.Exception);

				Thread.MemoryBarrier();
				var val = _value;
				Thread.MemoryBarrier();
				return val;
			}

			throw new TimeoutException();
		}

		/// <summary>
		/// Waits (blocks the current thread) until the value is present or the timeout is exceeded.
		/// </summary>
		/// <param name="timeout">A timespan representing the timeout period.</param>
		/// <returns>The future's value.</returns>
		/// <exception cref="TimeoutException">thrown if the timeout is exceeded before the value becomes available.</exception>
		public T AwaitValue(TimeSpan timeout)
		{
			if (Wait(timeout))
			{
				if (this.IsFaulted)
					throw new ParallelException("Background thread faulted.", this.Exception);

				Thread.MemoryBarrier();
				var val = _value;
				Thread.MemoryBarrier();
				return val;
			}

			throw new TimeoutException();
		}
	}
}
