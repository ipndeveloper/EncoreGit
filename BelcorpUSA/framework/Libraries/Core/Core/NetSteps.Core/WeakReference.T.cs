using System;
using System.Runtime.Serialization;

namespace NetSteps.Encore.Core
{
	/// <summary>
	/// Strongly typed weak reference.
	/// </summary>
	/// <typeparam name="T">referenced type T</typeparam>
	[Serializable]
	public class WeakReference<T> : WeakReference
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="target">a reference target.</param>
		public WeakReference(T target)
			: base(target)
		{
		}
		/// <summary>
		/// Creates a new instance (from serialization)
		/// </summary>
		/// <param name="info">serialization info</param>
		/// <param name="context">serialization context</param>
		protected WeakReference(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ 
		}
		/// <summary>
		/// Gets the referenced target as type T.
		/// </summary>
		public T StrongTarget { get { return (T)this.Target; } }
	}

	/// <summary>
	/// Extends the weak reference type.
	/// </summary>
	public static class WeakReferenceExtensions
	{
		/// <summary>
		/// Tries to get the target of the reference.
		/// </summary>
		/// <typeparam name="T">type T of the referenced object</typeparam>
		/// <param name="weakRef">the target reference</param>
		/// <param name="target">reference to a variable that will recieve the target if successful</param>
		/// <returns><em>true</em> if the reference is alive and has a valid value of type T; otherwise <em>false</em></returns>
		/// <exception cref="InvalidCastException">thrown if the target of the reference cannot be cast to type T</exception>
		public static bool TryGetStrongTarget<T>(this WeakReference weakRef, out T target)
		{
			if (weakRef.IsAlive)
			{
				target = (T)weakRef.Target;
				return true;
			}
			target = default(T);
			return false;
		}
	}
}
