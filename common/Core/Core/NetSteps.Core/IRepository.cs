using System.Collections.Generic;
using NetSteps.Encore.Core.IoC;
using IoC = NetSteps.Encore.Core.IoC;

namespace NetSteps.Encore.Core
{
	/// <summary>
	/// Basic repository interface for performing CRUD for 
	/// model type M.
	/// </summary>
	/// <typeparam name="M">model type M</typeparam>
	/// <typeparam name="I">model's identity type I</typeparam>
	public interface IRepository<M, I>
	{
		/// <summary>
		/// Creates a new model M within the repository.
		/// </summary>
		/// <param name="container">current container</param>
		/// <param name="model">the model instance</param>
		/// <returns>the stored instance</returns>
		M Create(IContainer container, M model);

		/// <summary>
		/// Reads an instance by identity from the repository.
		/// </summary>
		/// <param name="container">current container</param>
		/// <param name="id">the instance's identity</param>
		/// <returns>the instance</returns>
		M Read(IContainer container, I id);
		
		/// <summary>
		/// Reads all instances of model type M from the repository.
		/// </summary>
		/// <param name="container">current container</param>
		/// <returns>an enumerable over all instances of M</returns>
		IEnumerable<M> All(IContainer container);

		/// <summary>
		/// Updates the repository's copy of an instance M
		/// </summary>
		/// <param name="container">current container</param>
		/// <param name="model">the model instance</param>
		/// <returns>the updated instance</returns>
		M Update(IContainer container, M model);

		/// <summary>
		/// Deletes the repository's copy of an instance by it's identity.
		/// </summary>
		/// <param name="container">current container</param>
		/// <param name="id">the instance's identity</param>
		void Delete(IContainer container, I id);
	}

	/// <summary>
	/// Extensions for IRepository&lt;M>
	/// </summary>
	public static class IRepositoryExtensions
	{
		/// <summary>
		/// Creates a new model M within the repository.
		/// </summary>
		/// <param name="repo">the repository</param>
		/// <param name="model">the model instance</param>
		/// <returns>the stored instance</returns>
		public static M Create<M, I>(this IRepository<M, I> repo, M model)
		{
			using (var c = IoC.Create.SharedOrNewContainer())
			{
				return repo.Create(c, model);
			}
		}

		/// <summary>
		/// Reads an instance by identity from the repository.
		/// </summary>
		/// <param name="repo">the repository</param>
		/// <param name="id">the instance's identity</param>
		/// <returns>the instance</returns>
		public static M Read<M, I>(this IRepository<M, I> repo, I id)
		{
			using (var c = IoC.Create.SharedOrNewContainer())
			{
				return repo.Read(c, id);
			}
		}

		/// <summary>
		/// Reads all instances of model type M from the repository.
		/// </summary>
		/// <param name="repo">the repository</param>
		/// <returns>an enumerable over all instances of M</returns>
		public static IEnumerable<M> All<M, I>(this IRepository<M, I> repo)
		{
			using (var c = IoC.Create.SharedOrNewContainer())
			{
				return repo.All(c);
			}
		}

		/// <summary>
		/// Updates the repository's copy of an instance M
		/// </summary>
		/// <param name="repo">the repository</param>
		/// <param name="model">the model instance</param>
		/// <returns>the updated instance</returns>
		public static M Update<M, I>(this IRepository<M, I> repo, M model)
		{
			using (var c = IoC.Create.SharedOrNewContainer())
			{
				return repo.Update(c, model);
			}
		}

		/// <summary>
		/// Deletes the repository's copy of an instance by it's identity.
		/// </summary>
		/// <param name="repo">the repository</param>
		/// <param name="id">the instance's identity</param>
		public static void Delete<M, I>(this IRepository<M, I> repo, I id)
		{
			using (var c = IoC.Create.SharedOrNewContainer())
			{
				repo.Delete(c, id);
			}
		}
	}
}
