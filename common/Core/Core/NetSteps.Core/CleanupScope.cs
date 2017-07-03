using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Threading;
using NetSteps.Encore.Core.Log;
using NetSteps.Encore.Core.Properties;

namespace NetSteps.Encore.Core
{
	/// <summary>
	/// Provides information about cleanup scope events.
	/// </summary>
	public sealed class CleanupScopeItemEventArgs : EventArgs
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="item">the item that caused the event</param>
		public CleanupScopeItemEventArgs(object item)
		{
			Item = item;
		}
		/// <summary>
		/// The item that caused the event.
		/// </summary>
		public object Item { get; private set; }
	}

	/// <summary>
	/// Utility class for collecting actions and disposable items for cleanup. Actions and 
	/// disposable items, at dispose time, are either disposed (IDisposables) 
	/// or invoked (Actions) in the reverse order in which they are added to the scope.
	/// </summary>
	public partial class CleanupScope : Disposable, ICleanupScope
	{
		internal static class Ambient
		{
			[ThreadStatic]
			static Stack<ICleanupScope> __stack;

			internal static bool TryPeek(out ICleanupScope ambient)
			{
				if (__stack != null && __stack.Count > 0)
				{
					ambient = __stack.Peek();
					return true;
				}
				ambient = default(ICleanupScope);
				return false;
			}

			internal static bool TryPop(ICleanupScope comparand)
			{
				if (__stack == null || __stack.Count == 0) return false;

				var ambient = __stack.Peek();
				if (Object.ReferenceEquals(ambient, comparand))
				{
					__stack.Pop();
					return true;
				}
				return false;
			}

			internal static void Push(ICleanupScope ambient)
			{
				if (__stack == null) __stack = new Stack<ICleanupScope>();
				__stack.Push(ambient);
			}

		}

		/// <summary>
		/// Gets the current "ambient" cleanup scope. This is the nearest
		/// cleanup scope in the call stack.
		/// </summary>
		public static ICleanupScope Current
		{
			get
			{
				ICleanupScope ambient;
				return (Ambient.TryPeek(out ambient)) ? ambient : default(ICleanupScope);
			}
		}

		internal static ICleanupScope ForkAmbient()
		{
			ICleanupScope ambient;
			return (Ambient.TryPeek(out ambient)) ? ambient.ShareScope() : default(ICleanupScope);
		}

		internal static ICleanupScope EnsureAmbient(ICleanupScope ambient)
		{
			var scope = ambient ?? new CleanupScope();
			Ambient.Push(scope);
			return scope;
		}
		/// <summary>
		/// Shares the ambient scope if it exists; otherwise, creates a new scope.
		/// </summary>
		/// <returns>a cleanup scope</returns>
		public static ICleanupScope NewOrSharedScope()
		{
			ICleanupScope ambient;
			return (Ambient.TryPeek(out ambient)) ? ambient.ShareScope() : new CleanupScope();
		}

		readonly ConcurrentStack<StackItem> _items = new ConcurrentStack<StackItem>();
		readonly bool _independent;

		EventHandler<CleanupScopeItemEventArgs> _itemAdded;
		EventHandler<CleanupScopeItemEventArgs> _itemDisposed;
		// Reference counts disposing threads. We always have one.
		int _disposers = 1; 
		object _ownerNotifier;

		/// <summary>
		/// Creates a new scope.
		/// </summary>
		public CleanupScope() : this(false)
		{
		}

		/// <summary>
		/// Creates a new scope.
		/// </summary>
		/// <param name="independent">indicates whether the scope is independent of the stack</param>
		public CleanupScope(bool independent)
		{
			this._independent = independent;
			if (!_independent)
			{
				Ambient.Push(this);
			}
		}

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="independent">indicates whether the scope is independent of the stack</param>
		/// <param name="ownerNotifier">the owner, notifier</param>
		public CleanupScope(bool independent, object ownerNotifier)
			: this(independent)
		{
			_ownerNotifier = ownerNotifier;
		}

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="ownerNotifier">the owner, notifier</param>
		public CleanupScope(object ownerNotifier) : this(false, ownerNotifier)
		{
		}
		
		/// <summary>
		/// Creates a new scope and adds to it the disposable item given.
		/// </summary>
		/// <param name="independent">indicates whether the scope is independent of the stack</param>
		/// <param name="items">Items to be disposed when the scope is cleaned up.</param>
		public CleanupScope(bool independent, params IDisposable[] items) : this(independent)
		{
			Contract.Assume(items != null);
			foreach (IDisposable item in items)
			{
				if (item != null)
				{
					_items.Push(new StackItem(item));
				}
			}
		}

		/// <summary>
		/// Creates a new scope and adds to it the disposable item given.
		/// </summary>
		/// <param name="items">Items to be disposed when the scope is cleaned up.</param>
		public CleanupScope(params IDisposable[] items)
			: this(false, items)
		{			
		}

		/// <summary>
		/// Creates a new scope and adds an action to be performed when the scope is cleaned up.
		/// </summary>
		/// <param name="independent">indicates whether the scope is independent of the stack</param>
		/// <param name="actions">an array of actions to be performed when the scope is cleaned up.</param>
		public CleanupScope(bool independent, params Action[] actions) : this(independent)
		{
			foreach (Action action in actions)
			{
				_items.Push(new StackItem(action));
			}
		}

		/// <summary>
		/// Creates a new scope and adds an action to be performed when the scope is cleaned up.
		/// </summary>
		/// <param name="actions">an array of actions to be performed when the scope is cleaned up.</param>
		public CleanupScope(params Action[] actions)
			: this(false, actions)
		{			
		}
		
		/// <summary>
		/// Adds a disposable item to the cleanup scope. Actions and disposable items are collected 
		/// and at cleanup whill be either disposed (IDisposables) or invoked (Actions) in the reverse 
		/// order in which they are added.
		/// </summary>
		/// <typeparam name="T">Type of the item being added; ensures IDisposable by inference.</typeparam>
		/// <param name="item">An item to be disposed when the scope is cleaned up.</param>
		/// <returns>Returns the item given.</returns>
		public T Add<T>(T item)
			where T : IDisposable
		{
			if (item == null) throw new ArgumentNullException("item");
			if (IsDisposed) throw new ObjectDisposedException(typeof(CleanupScope).FullName);

			_items.Push(new StackItem(item));
			return item;
		}

		/// <summary>
		/// Adds an action to the cleanup scope. Actions and IDisposables collected in the same queue and
		/// are either disposed (IDisposables) or invoked (Actions) in the reverse order in which they are
		/// added.
		/// </summary>
		/// <param name="action">An action to be performed when the scope is cleaned up.</param>
		public void AddAction(Action action)
		{
			if (action == null) throw new ArgumentNullException("action");
			if (IsDisposed) throw new ObjectDisposedException(typeof(CleanupScope).FullName);


			_items.Push(new StackItem(action));
		}

		/// <summary>
		/// Shares the scope. Callers must guarantee that there is a matching call to Dispose
		/// for every call to share. Preferrably by wrapping it in a using clause.
		/// </summary>
		/// <returns>the shared scope</returns>
		public ICleanupScope ShareScope()
		{
			if (IsDisposed) throw new ObjectDisposedException(typeof(CleanupScope).FullName);

			Interlocked.Increment(ref _disposers);
			return this;
		}

		/// <summary>
		/// Event fired when items are added to he scope.
		/// </summary>
		public event EventHandler<CleanupScopeItemEventArgs> ItemAdded
		{
			add { _itemAdded += value; }
			remove { _itemAdded -= value; }
		}
		/// <summary>
		/// Event fired when items are disposed by the scope.
		/// </summary>
		public event EventHandler<CleanupScopeItemEventArgs> ItemDisposed
		{
			add { _itemDisposed += value; }
			remove { _itemDisposed -= value; }
		}

		/// <summary>
		/// Performs the scope's disposal.
		/// </summary>
		/// <param name="disposing">indicates whether the scope is disposing</param>
		/// <returns><em>true</em> if disposed as a result of the call; otherwise <em>false</em></returns>
		protected override bool PerformDispose(bool disposing)
		{			
			if (!_independent && !Ambient.TryPop(this))
			{
				// Notify the caller that they are calling dispose out of order.
				// This never happens if the caller uses a 'using' 
				var message = "Cleanup scope disposed out of order. To eliminate this possibility always wrap the scope in a 'using clause'.";
				try
				{
					Trace.TraceWarning(message);
				}
				catch (Exception)
				{ // don't surface if we're in a finalizer...
					if (!disposing) throw;
				}
				if (disposing) throw new InvalidOperationException(message);
			}
			if (disposing && Interlocked.Decrement(ref _disposers) > 0)
			{
				return false;
			}
			StackItem item;			
			while (_items.TryPop(out item))
			{
				try
				{
					if (item.Disposable != null)
					{
						item.Disposable.Dispose();
						NotifyItemDisposed(item.Disposable);
					}
					else if (item.Action != null) item.Action();
				}
				catch (Exception e)
				{
					if (disposing) throw;

					// We may be in the GC, trace as a warning and eat any exception
					// thrown by trace logic...
					try
					{
						Trace.TraceWarning(String.Concat(Resources.Warn_ErrorWhileDisposingCleanupScope,
							": ", (item.Disposable == null) ? item.Action.GetFullName() : item.Disposable.GetType().FullName,
							"; ", e.FormatForLogging())
								);
					}
					catch (Exception) { /* safety net, intentionally eat the since we might be in GC thread */ }
				}
			}
			return true;
		}
	
		void NotifyItemAdded(object item)
		{
			if (_itemAdded != null)
			{
				var sender = _ownerNotifier ?? this;
				_itemAdded(sender, new CleanupScopeItemEventArgs(item));
			}
		}

		void NotifyItemDisposed(object item)
		{
			if (_itemDisposed != null)
			{
				var sender = _ownerNotifier ?? this;
				object args = new object[] { sender, new CleanupScopeItemEventArgs(item) };
				foreach (var handler in _itemDisposed.GetInvocationList())
				{
					try
					{
						handler.DynamicInvoke(args);
					}
					catch (Exception) { /* Ouch!, we're in the GC - nothing to do here. */ }
				}
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		struct StackItem
		{
			public Action Action;
			public IDisposable Disposable;

			public StackItem(IDisposable d)
			{
				this.Action = null; // keep compiler happy
				this.Disposable = d;
			}

			public StackItem(Action a)
			{
				this.Disposable = null; // keep compiler happy
				this.Action = a;
			}
		}
	}

}
