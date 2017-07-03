using System;

namespace NetSteps.Encore.Core.IoC
{
	/// <summary>
	/// Interface for objects owned by a container.
	/// </summary>
	public interface IContainerOwned : IDisposable
	{
		/// <summary>
		/// Gets the container owner.
		/// </summary>
		IContainer Container { get; }
	}

	/// <summary>
	/// abstract implementation of the IContainerOwned interface
	/// </summary>
	public abstract class ContainerOwned : Disposable, IContainerOwned
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="container">the container, owner</param>
		protected ContainerOwned(IContainer container)
		{
			if (container == null)
				throw new ArgumentNullException("container");
			this.Container = container;
		}

		/// <summary>
		/// Returns the container, owner.
		/// </summary>
		public IContainer Container { get; private set; }
	}
}
