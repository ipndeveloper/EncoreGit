using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using NetSteps.Encore.Core.Properties;

namespace NetSteps.Encore.Core
{
	/// <summary>
	/// Utility structure for performing and tracking threadsafe state transitions.
	/// </summary>
	/// <typeparam name="E">State type E (should be an enum)</typeparam>
	[Serializable]
	public struct Status<E>
		where E : struct
	{
		static readonly int CHashCodeSeed = typeof(Status<E>).AssemblyQualifiedName.GetHashCode();

		int _status;

		[SuppressMessage("Microsoft.Usage", "CA2207")]
		static Status()
		{
			Contract.Assert(typeof(E).IsEnum, Resources.Err_StatusTypeMustBeEnum);
		}

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="initialState">Initial state</param>
		public Status(E initialState)
		{
			_status = Convert.ToInt32(initialState);
		}

		/// <summary>
		/// Accesses the current state.
		/// </summary>
		public E CurrentState
		{
			get
			{
				Thread.MemoryBarrier();
				int state = _status;
				Thread.MemoryBarrier();
				return (E)Enum.ToObject(typeof(E), state);
			}
		}

		/// <summary>
		/// Determines if the current state is greater than the comparand.
		/// </summary>
		/// <param name="comparand">comparand</param>
		/// <returns><em>true</em> if the current state is greater than <paramref name="comparand"/>; otherwise <em>false</em></returns>
		public bool IsGreaterThan(E comparand)
		{
			Thread.MemoryBarrier();
			int current = _status;
			Thread.MemoryBarrier();
			return current > Convert.ToInt32(comparand);
		}

		/// <summary>
		/// Determines if the current state is less than the comparand.
		/// </summary>
		/// <param name="comparand">comparand</param>
		/// <returns><em>true</em> if the current state is less than <paramref name="comparand"/>; otherwise <em>false</em></returns>
		public bool IsLessThan(E comparand)
		{
			Thread.MemoryBarrier();
			int current = _status;
			Thread.MemoryBarrier();
			return current < Convert.ToInt32(comparand);
		}

		/// <summary>
		/// Transitions to the given state.
		/// </summary>
		/// <param name="value">the target state</param>
		/// <returns><em>true</em> if the state was changes as a result of the call; 
		/// otherwise <em>false</em> (already had the desired state).</returns>
		public bool ChangeState(E value)
		{
			int v = Convert.ToInt32(value);
			int init, fin;
			while (true)
			{
				Thread.MemoryBarrier();
				fin = _status;
				Thread.MemoryBarrier();
				if (fin == v) return false;

				init = fin;
				fin = Interlocked.CompareExchange(ref _status, v, init);
				if (fin == init) return true;
			}
		}

		/// <summary>
		/// Performs a state transition if the current state compares greater than the <paramref name="comparand"/>
		/// </summary>
		/// <param name="value">the target state</param>
		/// <param name="comparand">comparand state</param>
		/// <returns><em>true</em> if the current state compares greater than <paramref name="comparand"/>; 
		/// otherwise <em>false</em></returns>
		public bool SetStateIfGreaterThan(E value, E comparand)
		{
			int c = Convert.ToInt32(comparand);
			int v = Convert.ToInt32(value);

			Thread.MemoryBarrier();
			int init, fin = _status;
			Thread.MemoryBarrier();
			while (true)
			{
				if (fin < c) return false;

				init = fin;
				fin = Interlocked.CompareExchange(ref _status, v, init);
				if (fin == init) return true;
			}
		}

		/// <summary>
		/// Performs a state transition if the current state compares less than the <paramref name="comparand"/>
		/// </summary>
		/// <param name="value">the target state</param>
		/// <param name="comparand">comparand state</param>
		/// <returns><em>true</em> if the current state compares less than <paramref name="comparand"/>; 
		/// otherwise <em>false</em></returns>
		public bool SetStateIfLessThan(E value, E comparand)
		{
			int c = Convert.ToInt32(comparand);
			int v = Convert.ToInt32(value);

			Thread.MemoryBarrier();
			int init, fin = _status;
			Thread.MemoryBarrier();
			while (true)
			{
				if (fin > c) return false;

				init = fin;
				fin = Interlocked.CompareExchange(ref _status, v, init);
				if (fin == init) return true;
			}
		}

		/// <summary>
		/// Performs a state transition if the current state compares less than the <paramref name="comparand"/>
		/// </summary>
		/// <param name="value">the target state</param>
		/// <param name="comparand">comparand state</param>
		/// <param name="action">An action to be performed if the state transition succeeds</param>
		/// <returns><em>true</em> if the current state compares less than <paramref name="comparand"/>; otherwise <em>false</em></returns>
		public bool SetStateIfLessThan(E value, E comparand, Action action)
		{
			Contract.Requires<ArgumentNullException>(action != null);
			if (SetStateIfLessThan(value, comparand))
			{
				action();
				return true;
			}
			return false;
		}

		/// <summary>
		/// Toggles between the toggle state and the desired state - with
		/// a spin-wait if necessary.
		/// </summary>
		/// <param name="desired">desired state</param>
		/// <param name="toggle">state from which the desired state can toggle</param>
		/// <returns><em>true</em> if the state transitions to the desired state from the toggle state; otherwise <em>false</em></returns>
		public bool SpinToggleState(E desired, E toggle)
		{
			int d = Convert.ToInt32(desired);
			int t = Convert.ToInt32(toggle);

		spin:
			int r = Interlocked.CompareExchange(ref _status, d, t);
			// If the state was the toggle state then we're successful and done...
			if (r == t) return true;
			// otherwise if the result is anything but the desired state we're
			// unsuccessful and done...
			if (r != d) return false;
			// otherwise we spin
			goto spin;
		}

		/// <summary>
		/// Performs a spinwait until the current state equals the target state.
		/// </summary>
		/// <param name="targetState">the target state</param>
		/// <param name="loopAction">An action to perform inside the spin cycle</param>
		public void SpinWaitForState(E targetState, Action loopAction)
		{
			int state = Convert.ToInt32(targetState);
			Thread.MemoryBarrier();
			int current = _status;
			Thread.MemoryBarrier();
			while (current != state)
			{
				loopAction();
				Thread.MemoryBarrier();
				current = _status;
				Thread.MemoryBarrier();
			}
		}

		/// <summary>
		/// Performs a spinwait until the current state equals the target state.
		/// </summary>
		/// <param name="targetState">the target state</param>
		/// <param name="loopAction">An action to perform inside the spin cycle; 
		/// waiting continues until either the target state is reached or the loop
		/// action returns false.</param>
		/// <returns><em>true</em> if the target state was reached; otherwise <em>false</em>.</returns>
		public bool TrySpinWaitForState(E targetState, Func<E, bool> loopAction)
		{
			int target = Convert.ToInt32(targetState);
			Thread.MemoryBarrier();
			int current = _status;
			Thread.MemoryBarrier();
			while (current != target)
			{
				if (!loopAction((E)Enum.ToObject(typeof(E), current)))
				{
					// loop signaled to stop waiting...
					return false;
				}
				Thread.MemoryBarrier();
				current = _status;
				Thread.MemoryBarrier();
			}
			// wait completed, state reached...
			return true;
		}

		/// <summary>
		/// Tries to transition the state
		/// </summary>
		/// <param name="value">the target state</param>
		/// <param name="comparand">comparand state must match current state</param>
		/// <returns><em>true</em> if the current state matches <paramref name="comparand"/> and the state is transitioned to <paramref name="value"/>; otherwise <em>false</em></returns>
		public bool TryTransition(E value, E comparand)
		{
			int c = Convert.ToInt32(comparand);
			return Interlocked.CompareExchange(ref _status, Convert.ToInt32(value), c) == c;
		}

		/// <summary>
		/// Tries to transition the state.
		/// </summary>
		/// <param name="value">the target state</param>
		/// <param name="comparand">one or more comparands</param>
		/// <returns>true if the current state matches one of the comparands and is transitioned to <paramref name="value"/>; otherwise false</returns>
		public bool TryTransition(E value, params E[] comparand)
		{
			var comparands = (from c in comparand
												select Convert.ToInt32(c)).ToArray();
			var v = Convert.ToInt32(value);

			Thread.MemoryBarrier();
			int state = _status;
			Thread.MemoryBarrier();
			while (comparands.Contains(state))
			{
				if (Interlocked.CompareExchange(ref _status, v, state) == state)
					return true;
				Thread.MemoryBarrier();
				state = _status;
				Thread.MemoryBarrier();
			}

			return false;
		}

		/// <summary>
		/// Tries to transition the state. Upon success executes the action given.
		/// </summary>
		/// <param name="value">the target state</param>
		/// <param name="comparand">comparand state must match current state</param>
		/// <param name="action">action to perform if the state transition is successful</param>
		/// <returns><em>true</em> if the current state matches <paramref name="comparand"/> and the state is transitioned to <paramref name="value"/>; otherwise <em>false</em></returns>
		public bool TryTransition(E value, E comparand, Action action)
		{
			Contract.Requires<ArgumentNullException>(action != null);
			if (TryTransition(value, comparand))
			{
				action();
				return true;
			}
			return false;
		}

		/// <summary>
		/// Compares the current state to the comparand, if they are equal, replaces the current state with the values
		/// </summary>
		/// <param name="value">the value</param>
		/// <param name="comparand">the comparand</param>
		/// <returns>the status prior</returns>
		public E CompareExchange(E value, E comparand)
		{
			return
				(E)Enum.ToObject(typeof(E), Interlocked.CompareExchange(ref _status, Convert.ToInt32(value), Convert.ToInt32(comparand)));
			;
		}

		/// <summary>
		/// Determines if the current state includes the value given.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool HasState(E value)
		{
			int v = Convert.ToInt32(value);
			Thread.MemoryBarrier();
			int current = _status;
			Thread.MemoryBarrier();
			return (current & v) == v;
		}

		/// <summary>
		/// Tests whethe the status is equal to another.
		/// </summary>
		/// <param name="other">the other</param>
		/// <returns>true if equal; otherwise false</returns>
		public bool Equals(Status<E> other)
		{
			Thread.MemoryBarrier();
			int ours = _status;
			Thread.MemoryBarrier();
			Thread.MemoryBarrier();
			int theirs = other._status;
			Thread.MemoryBarrier();
			return ours == theirs;
		}

		/// <summary>
		/// Tests whether the status is equal to another
		/// </summary>
		/// <param name="obj">the other</param>
		/// <returns>true if equal; otherwise false</returns>
		public override bool Equals(object obj)
		{
			return (obj is Status<E>) && Equals((Status<E>)obj);
		}

		/// <summary>
		/// Gets the hashcode.
		/// </summary>
		/// <returns>the hashcode</returns>
		public override int GetHashCode()
		{
			int prime = Constants.RandomPrime;
			var code = CHashCodeSeed * prime;
			Thread.MemoryBarrier();
			code ^= (int)_status * prime;
			Thread.MemoryBarrier();
			return code;
		}

		/// <summary>
		/// Specialized == operator
		/// </summary>
		/// <param name="lhs">left hand comparand</param>
		/// <param name="rhs">right hand comparand</param>
		/// <returns>true if the comparands are equal</returns>
		public static bool operator ==(Status<E> lhs, Status<E> rhs)
		{
			return lhs.Equals(rhs);
		}

		/// <summary>
		/// Specialized != operator
		/// </summary>
		/// <param name="lhs">left hand comparand</param>
		/// <param name="rhs">right hand comparand</param>
		/// <returns>true if the comparands are not equal</returns>
		public static bool operator !=(Status<E> lhs, Status<E> rhs)
		{
			return !lhs.Equals(rhs);
		}

		/// <summary>
		/// Specialized == operator
		/// </summary>
		/// <param name="lhs">left hand comparand</param>
		/// <param name="rhs">right hand comparand</param>
		/// <returns>true if the comparands are equal</returns>
		public static bool operator ==(Status<E> lhs, E rhs)
		{
			return lhs.Equals(ToStatus(rhs));
		}

		/// <summary>
		/// Specialized != operator
		/// </summary>
		/// <param name="lhs">left hand comparand</param>
		/// <param name="rhs">right hand comparand</param>
		/// <returns>true if the comparands are not equal</returns>
		public static bool operator !=(Status<E> lhs, E rhs)
		{
			return !lhs.Equals(ToStatus(rhs));
		}

		/// <summary>
		/// Specialized == operator
		/// </summary>
		/// <param name="lhs">left hand comparand</param>
		/// <param name="rhs">right hand comparand</param>
		/// <returns>true if the comparands are equal</returns>
		public static bool operator ==(E lhs, Status<E> rhs)
		{
			return ToStatus(lhs).Equals(rhs);
		}

		/// <summary>
		/// Specialized != operator
		/// </summary>
		/// <param name="lhs">left hand comparand</param>
		/// <param name="rhs">right hand comparand</param>
		/// <returns>true if the comparands are not equal</returns>
		public static bool operator !=(E lhs, Status<E> rhs)
		{
			return !ToStatus(lhs).Equals(rhs);
		}
		/// <summary>
		/// Converts Status&lt;E> to E
		/// </summary>
		/// <param name="s">the status</param>
		/// <returns>the equivalent E</returns>
		[SuppressMessage("Microsoft.Design", "CA1000", Justification = "By design.")]
		public static E ToObject(Status<E> s)
		{
			return s.CurrentState;
		}

		/// <summary>
		/// Converts E to Status&lt;E>
		/// </summary>
		/// <param name="s">the value</param>
		/// <returns>the equivalent Status&lt;E></returns>
		[SuppressMessage("Microsoft.Design", "CA1000", Justification = "By design.")]
		public static Status<E> ToStatus(E s)
		{
			return new Status<E>(s);
		}
	}

}
