using System;

namespace NetSteps.Encore.Core.Parallel
{
	/// <summary>
	/// Continuation delegate.
	/// </summary>
	/// <param name="fault">an exception raised by the operation</param>
	/// <remarks>Continuations are invoked when the operation on
	/// which they depend reaches completion. Continuations always
	/// receive an exception as their first argument. In success
	/// cases, the <paramref name="fault"/> argument will be null; otherwise
	/// it is the exception raised by the operation being continued.</remarks>
	public delegate void Continuation(Exception fault);

	/// <summary>
	/// Continuation delegate.
	/// </summary>
	/// <typeparam name="T">item type T, usually the result type of the operation being continued.</typeparam>
	/// <param name="fault">an exception raised by the operation</param>
	/// <param name="item">the result of the continued operation (when the operation is a Fun&lt;>)</param>
	/// <remarks>Continuations are invoked when the operation on
	/// which they depend reaches completion. Continuations always
	/// receive an exception as their first argument. In success
	/// cases, the <paramref name="fault"/> argument will be null; otherwise
	/// it is the exception raised by the operation being continued.</remarks>
	public delegate void Continuation<in T>(Exception fault, T item);

	/// <summary>
	/// Continuation delegate.
	/// </summary>
	/// <typeparam name="R">result type R</typeparam>
	/// <param name="fault">an exception raised by the operation</param>
	/// <returns>a return item</returns>
	/// <remarks>Continuations are invoked when the operation on
	/// which they depend reaches completion. Continuations always
	/// receive an exception as their first argument. In success
	/// cases, the <paramref name="fault"/> argument will be null; otherwise
	/// it is the exception raised by the operation being continued.</remarks>
	public delegate R ContinuationFunc<out R>(Exception fault);

	/// <summary>
	/// Continuation delegate.
	/// </summary>
	/// <typeparam name="T">item type T, usually the result type of the operation being continued.</typeparam>
	/// <typeparam name="R">result type R</typeparam>
	/// <param name="fault">an exception raised by the operation</param>
	/// <param name="item">the result of the continued operation (when the operation is a Fun&lt;>)</param>
	/// <returns>a return item</returns>
	/// <remarks>Continuations are invoked when the operation on
	/// which they depend reaches completion. Continuations always
	/// receive an exception as their first argument. In success
	/// cases, the <paramref name="fault"/> argument will be null; otherwise
	/// it is the exception raised by the operation being continued.</remarks>
	public delegate R ContinuationFunc<in T, out R>(Exception fault, T item);
}
