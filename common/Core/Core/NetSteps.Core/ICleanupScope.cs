using System;

namespace NetSteps.Encore.Core
{
	/// <summary>
	/// Deliniates a cleanup scope.
	/// </summary>
	public interface ICleanupScope : IDisposable
	{
		/// <summary>
		/// Adds a disposable item to the scope. When the scope
		/// is disposed all added items are guaranteed to also be
		/// disposed.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="item"></param>
		/// <returns></returns>
		T Add<T>(T item) where T: IDisposable;
		/// <summary>
		/// Adds an action to be performed upon scope
		/// completion (on dispose).
		/// </summary>
		/// <param name="action"></param>
		void AddAction(Action action);

		/// <summary>
		/// Prepares the scope for use in multiple threads.
		/// </summary>
		/// <returns>An equivalent scope.</returns>
		ICleanupScope ShareScope();
	}
}
