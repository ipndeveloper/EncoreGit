using System;
using System.Threading;
using System.Diagnostics.CodeAnalysis;

namespace NetSteps.Encore.Core
{
	/// <summary>
	/// Abstract logic for disposable objects.
	/// </summary>
	[Serializable]
	public abstract class Disposable : IDisposable
	{
		enum DisposalState
		{
			None = 0,
			Incomplete = 1,
			Disposing = 2,
			Disposed = 3
		}
		Status<DisposalState> _disposal = new Status<DisposalState>();

		/// <summary>
		/// Finalizes the instance.
		/// </summary>
		~Disposable()
		{
			this.Dispose(false);
		}

		/// <summary>
		/// Disposes the instance.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1063", Justification="By design; allows subclasses to determine when GC finalize is suppressed.")]
		public void Dispose()
		{
			this.Dispose(true);
		}

		/// <summary>
		/// Indicates whether the instance has been disposed.
		/// </summary>
		public bool IsDisposed { get { return _disposal.CurrentState == DisposalState.Disposed; } }

		void Dispose(bool disposing)
		{
			while (_disposal.IsLessThan(DisposalState.Disposed))
			{
				if (_disposal.HasState(DisposalState.Disposed))
				{
					throw new ObjectDisposedException(this.GetType().FullName);
				}
				else if (_disposal.HasState(DisposalState.Disposing))
				{
					if (!_disposal.TrySpinWaitForState(DisposalState.Incomplete, state => state == DisposalState.Disposed))
					{
						throw new ObjectDisposedException(this.GetType().FullName);
					}
				}
				else if (_disposal.TryTransition(DisposalState.Disposing, DisposalState.Incomplete, DisposalState.None))
				{
					if (PerformDispose(disposing))
					{
						if (_disposal.ChangeState(DisposalState.Disposed))
						{
							GC.SuppressFinalize(this);
						}						
					}
					else
					{
						_disposal.ChangeState(DisposalState.Incomplete);
					}
					break;
				}
			}
		}

		/// <summary>
		/// Performs the dispose logic.
		/// </summary>
		/// <param name="disposing">Whether the object is disposing (IDisposable.Dispose method was called).</param>
		/// <returns>Implementers should return true if the disposal was successful; otherwise false.</returns>
		protected abstract bool PerformDispose(bool disposing);
	}


}
