using System;
using System.Threading;

namespace NetSteps.Encore.Core.Parallel
{
	/// <summary>
	/// Parallelism using thread pool.
	/// </summary>
	public static class Go
	{
		/// <summary>
		/// Performs an action in parallel.
		/// </summary>
		/// <param name="action">an action</param>
		/// <returns>a completion</returns>
		public static Completion Parallel(Action action)
		{
			if (action == null) throw new ArgumentNullException("action");
			Completion waitable = new Completion(action.Target);

			var ambient = CleanupScope.ForkAmbient();
			ThreadPool.QueueUserWorkItem(new WaitCallback(
				ignored =>
				{
					using (var scope = CleanupScope.EnsureAmbient(ambient))
					{
						var caught = default(Exception);
						try
						{
							action();
							waitable.MarkCompleted();
						}
						catch (Exception e)
						{
							caught = e;
							waitable.MarkFaulted(caught);
							NotifyUncaughtException(action.Target, e);
						}
					}
				}
				));
			return waitable;
		}

		/// <summary>
		/// Performs an action in parallel, and when complete, 
		/// invokes the given continuation.
		/// </summary>
		/// <param name="action">an action</param>
		/// <param name="continuation">a continuation</param>
		/// <returns></returns>
		public static Completion Parallel(Action action, Continuation continuation)
		{
			if (action == null) throw new ArgumentNullException("action");
			if (continuation == null) throw new ArgumentNullException("continuation");

			Completion waitable = new Completion(action.Target);
			var ambient = CleanupScope.ForkAmbient();
			ThreadPool.QueueUserWorkItem(new WaitCallback(
				ignored =>
				{
					using (var scope = CleanupScope.EnsureAmbient(ambient))
					{
						var caught = default(Exception);
						try
						{
							action();
						}
						catch (Exception e)
						{
							caught = e;
							try
							{
								continuation(caught);
							}
							catch (Exception ee)
							{
								NotifyUncaughtException(action.Target, ee);
							}
							try
							{
								waitable.MarkFaulted(caught);
							}
							catch (Exception ee)
							{
								NotifyUncaughtException(action.Target, ee);
							}
							return;
						}
						try
						{
							continuation(caught);
						}
						catch (Exception e)
						{
							caught = e;
							try
							{
								waitable.MarkFaulted(caught);
							}
							catch (Exception ee)
							{
								NotifyUncaughtException(action.Target, ee);
							}
							return;
						}
						try
						{
							waitable.MarkCompleted();
						}
						catch (Exception ee)
						{
							NotifyUncaughtException(action.Target, ee);
						}
					}
				}
				));
			return waitable;
		}

		/// <summary>
		/// Performs an action in parallel, and when complete, 
		/// invokes the given continuation.
		/// </summary>
		/// <typeparam name="T">value type T</typeparam>
		/// <param name="value">a value to be passed to the action</param>
		/// <param name="action">an action</param>
		/// <param name="continuation">a continuation</param>
		/// <returns></returns>
		public static Completion Parallel<T>(T value, Action<T> action, Continuation continuation)
		{
			if (action == null) throw new ArgumentNullException("action");
			if (continuation == null) throw new ArgumentNullException("continuation");

			Completion waitable = new Completion(action.Target);
			var ambient = CleanupScope.ForkAmbient();
			ThreadPool.QueueUserWorkItem(new WaitCallback(
				ignored =>
				{
					using (var scope = CleanupScope.EnsureAmbient(ambient))
					{
						var caught = default(Exception);
						try
						{
							action(value);
						}
						catch (Exception e)
						{
							caught = e;
							try
							{
								continuation(caught);
							}
							catch (Exception ee)
							{
								NotifyUncaughtException(action.Target, ee);
							}
							try
							{
								waitable.MarkFaulted(caught);
							}
							catch (Exception ee)
							{
								NotifyUncaughtException(action.Target, ee);
							}
							return;
						}
						try
						{
							continuation(caught);
						}
						catch (Exception e)
						{
							caught = e;
							try
							{
								waitable.MarkFaulted(caught);
							}
							catch (Exception ee)
							{
								NotifyUncaughtException(action.Target, ee);
							}
							return;
						}
						try
						{
							waitable.MarkCompleted();
						}
						catch (Exception ee)
						{
							NotifyUncaughtException(action.Target, ee);
						}
					}
				}
				));
			return waitable;
		}

		/// <summary>
		/// Performs an action in parallel, and when complete, 
		/// invokes the given continuation.
		/// </summary>
		/// <typeparam name="R">result type R</typeparam>
		/// <param name="fun">an action</param>
		/// <returns></returns>
		public static Completion<R> Parallel<R>(Func<R> fun)
		{
			if (fun == null) throw new ArgumentNullException("fun");

			Completion<R> waitable = new Completion<R>(fun.Target);
			var ambient = CleanupScope.ForkAmbient();
			ThreadPool.QueueUserWorkItem(new WaitCallback(
				ignored =>
				{
					using (var scope = CleanupScope.EnsureAmbient(ambient))
					{
						R result = default(R);
						try
						{
							result = fun();
						}
						catch (Exception e)
						{
							try
							{
								waitable.MarkFaulted(e);
							}
							catch (Exception ee)
							{
								NotifyUncaughtException(fun.Target, ee);
							}
							return;
						}
						try
						{
							if (!waitable.IsFaulted)
							{
								waitable.MarkCompleted(result);
							}
						}
						catch (Exception ee)
						{
							NotifyUncaughtException(fun.Target, ee);
						}
					}
				}
				));
			return waitable;
		}
		/// <summary>
		/// Performs an action in parallel, and when complete, 
		/// invokes the given continuation.
		/// </summary>
		/// <typeparam name="R">result type R</typeparam>
		/// <param name="fun">an action</param>
		/// <param name="continuation">a continuation</param>
		/// <returns></returns>
		public static Completion<R> Parallel<R>(Func<R> fun, Continuation<R> continuation)
		{
			if (fun == null) throw new ArgumentNullException("fun");
			if (continuation == null) throw new ArgumentNullException("continuation");

			Completion<R> waitable = new Completion<R>(fun.Target);
			var ambient = CleanupScope.ForkAmbient();
			ThreadPool.QueueUserWorkItem(new WaitCallback(
				ignored =>
				{
					using (var scope = CleanupScope.EnsureAmbient(ambient))
					{
						R result = default(R);
						var caught = default(Exception);
						try
						{
							result = fun();
						}
						catch (Exception e)
						{
							caught = e;
							try
							{
								continuation(caught, result);
							}
							catch (Exception ee)
							{
								NotifyUncaughtException(fun.Target, ee);
							}
							try
							{
								waitable.MarkFaulted(caught);
							}
							catch (Exception ee)
							{
								NotifyUncaughtException(fun.Target, ee);
							}
							return;
						}
						try
						{
							continuation(caught, result);
						}
						catch (Exception e)
						{
							caught = e;
							try
							{
								waitable.MarkFaulted(caught);
							}
							catch (Exception ee)
							{
								NotifyUncaughtException(fun.Target, ee);
							}
							return;
						}
						try
						{
							if (!waitable.IsFaulted)
							{
								waitable.MarkCompleted(result);
							}
						}
						catch (Exception ee)
						{
							NotifyUncaughtException(fun.Target, ee);
						}
					}
				}
				));
			return waitable;
		}

		/// <summary>
		/// Performs an action in parallel, and when complete, 
		/// invokes the given continuation.
		/// </summary>
		/// <typeparam name="T">value type T</typeparam>
		/// <typeparam name="R">result type R</typeparam>
		/// <param name="value">a value to be passed to the action</param>
		/// <param name="fun">an action</param>
		/// <returns></returns>
		public static Completion<R> Parallel<T, R>(T value, Func<T, R> fun)
		{
			if (fun == null) throw new ArgumentNullException("fun");

			Completion<R> waitable = new Completion<R>(fun.Target);
			var ambient = CleanupScope.ForkAmbient();
			ThreadPool.QueueUserWorkItem(new WaitCallback(
				ignored =>
				{
					using (var scope = CleanupScope.EnsureAmbient(ambient))
					{
						R result = default(R);
						var caught = default(Exception);
						try
						{
							result = fun(value);
						}
						catch (Exception e)
						{
							caught = e;
							try
							{
								waitable.MarkFaulted(caught);
							}
							catch (Exception ee)
							{
								NotifyUncaughtException(fun.Target, ee);
							}
							return;
						}
						try
						{
							if (!waitable.IsFaulted)
							{
								waitable.MarkCompleted(result);
							}
						}
						catch (Exception ee)
						{
							NotifyUncaughtException(fun.Target, ee);
						}
					}
				}
				));
			return waitable;
		}

		/// <summary>
		/// Performs an action in parallel, and when complete, 
		/// invokes the given continuation.
		/// </summary>
		/// <typeparam name="T">value type T</typeparam>
		/// <typeparam name="R">result type R</typeparam>
		/// <param name="value">a value to be passed to the action</param>
		/// <param name="fun">an action</param>
		/// <param name="continuation">a continuation</param>
		/// <returns></returns>
		public static Completion<R> Parallel<T, R>(T value, Func<T, R> fun, Continuation<R> continuation)
		{
			if (fun == null) throw new ArgumentNullException("fun");
			if (continuation == null) throw new ArgumentNullException("continuation");

			Completion<R> waitable = new Completion<R>(fun.Target);
			var ambient = CleanupScope.ForkAmbient();
			ThreadPool.QueueUserWorkItem(new WaitCallback(
				ignored =>
				{
					using (var scope = CleanupScope.EnsureAmbient(ambient))
					{
						R result = default(R);
						var caught = default(Exception);
						try
						{
							result = fun(value);
						}
						catch (Exception e)
						{
							caught = e;
							try
							{
								continuation(caught, result);
							}
							catch (Exception ee)
							{
								NotifyUncaughtException(fun.Target, ee);
							}
							try
							{
								waitable.MarkFaulted(caught);
							}
							catch (Exception ee)
							{
								NotifyUncaughtException(fun.Target, ee);
							}
							return;
						}
						try
						{
							continuation(caught, result);
						}
						catch (Exception e)
						{
							caught = e;
							try
							{
								waitable.MarkFaulted(caught);
							}
							catch (Exception ee)
							{
								NotifyUncaughtException(fun.Target, ee);
							}
							return;
						}
						try
						{
							if (!waitable.IsFaulted)
							{
								waitable.MarkCompleted(result);
							}
						}
						catch (Exception ee)
						{
							NotifyUncaughtException(fun.Target, ee);
						}
					}
				}
				));
			return waitable;
		}

		static EventHandler<UncaughtExceptionArgs> _onUncaughtException;

		/// <summary>
		/// Event fired when uncaught exceptions are raised by parallel executions.
		/// </summary>
		public static event EventHandler<UncaughtExceptionArgs> OnUncaughtException
		{
			add { _onUncaughtException += value; }
			remove { _onUncaughtException -= value; }
		}

		internal static void NotifyUncaughtException(object sender, Exception e)
		{
			if (_onUncaughtException != null)
			{
				_onUncaughtException(sender, new UncaughtExceptionArgs(e));
			}
		}
	}
}
