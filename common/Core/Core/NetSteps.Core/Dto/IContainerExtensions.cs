using System;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Properties;

namespace NetSteps.Encore.Core.Dto
{
	/// <summary>
	/// Container extensions for DataTransferObjects
	/// </summary>
	public static class IContainerExtensions
	{
		/// <summary>
		/// Creates a data transfer object by mutating the dto given.
		/// </summary>
		/// <typeparam name="T">dto type T</typeparam>
		/// <param name="container">a container</param>
		/// <param name="item">an existing dto object to mutate</param>
		/// <param name="mutator">an action used to mutate the item</param>
		/// <returns>a new dto instance with the mutated values</returns>
		public static T Mutation<T>(this IContainer container, T item, Action<T> mutator)
		{
			return Mutation<T>(container, item, mutator, true);
		}
		/// <summary>
		/// Creates a data transfer object by mutating the dto given.
		/// </summary>
		/// <typeparam name="T">dto type T</typeparam>
		/// <param name="container">a container</param>
		/// <param name="item">an existing dto object to mutate</param>
		/// <param name="mutator">an action used to mutate the item</param>
		/// <param name="readonly">indicates whether the result should be marked readonly</param>
		/// <returns>a new dto instance with the mutated values</returns>
		public static T Mutation<T>(this IContainer container, T item, Action<T> mutator, bool @readonly)
		{
			Contract.Requires<ArgumentNullException>(container != null, Resources.Chk_CannotBeNull);
			Contract.Requires<ArgumentNullException>(mutator != null, Resources.Chk_CannotBeNull);
			T result = DataTransferObject<T>.Mutate(container, item, mutator);
			if (@readonly) (result as DataTransferObject<T>).MarkReadonly();
			return result;
		}

		/// <summary>
		/// Creates a copy of a data transfer object.
		/// </summary>
		/// <typeparam name="T">dto type T</typeparam>
		/// <param name="container">a container</param>
		/// <param name="item">an existing dto object to mutate</param>
		/// <returns>a readonly copy of <paramref name="item"/></returns>
		public static T DtoCopy<T>(this IContainer container, T item)
		{
			return DtoCopy<T>(container, item, true);
		}
		/// <summary>
		/// Creates a copy of a data transfer object.
		/// </summary>
		/// <typeparam name="T">dto type T</typeparam>
		/// <param name="container">a container</param>
		/// <param name="item">an existing dto object to mutate</param>
		/// <param name="readonly">indicates whether the result should be marked readonly</param>
		/// <returns>a copy of <paramref name="item"/></returns>
		public static T DtoCopy<T>(this IContainer container, T item, bool @readonly)
		{
			Contract.Requires<ArgumentNullException>(container != null, Resources.Chk_CannotBeNull);

			T result = DataTransferObject<T>.Copy(container, item);
			if (@readonly) (result as DataTransferObject<T>).MarkReadonly();
			return result;
		}

		/// <summary>
		/// Creates an instance of an interface for use as a data transfer object.
		/// </summary>
		/// <typeparam name="T">dto type T</typeparam>
		/// <param name="container">a container</param>
		/// <returns>a new dto instance</returns>
		public static T NewDto<T>(this IContainer container)
		{
			return container.NewImplementationOf<T>(LifespanTracking.External, DataTransfer.ConcreteType<T>());
		}

		/// <summary>
		/// Creates an instance of an interface for use as a data transfer object.
		/// </summary>
		/// <typeparam name="T">dto type T</typeparam>
		/// <param name="container">a container</param>
		/// <param name="tracking">lifespan tracking of the new instance</param>
		/// <returns>a new dto instance</returns>
		public static T NewDto<T>(this IContainer container, LifespanTracking tracking)
		{
			return container.NewImplementationOf<T>(tracking, DataTransfer.ConcreteType<T>());
		}
	}
}
