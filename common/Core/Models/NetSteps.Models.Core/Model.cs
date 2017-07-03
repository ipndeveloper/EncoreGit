using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using NetSteps.Encore.Core;
using NetSteps.Encore.Core.IoC;
using NetSteps.Models.Core.SPI;

namespace NetSteps.Models.Core
{
	/// <summary>
	/// Contains utility methods for working with models.
	/// </summary>
	public static partial class Model
	{
		/// <summary>
		/// Gets the model's states.
		/// </summary>
		/// <typeparam name="M">model type M</typeparam>
		/// <param name="model">a model</param>
		/// <returns>the model's state</returns>
		public static ModelStates GetModelState<M>(this M model)
			where M : IModel
		{
			Contract.Requires<ArgumentNullException>(model != null);

			var spi = model as IModelSPI<M>;
			if (spi == null) throw new NotImplementedException("Cannot access the service provider interface.");
			return spi.GetModelState();
		}

		/// <summary>
		/// Indicates whether state has changed via mutation.
		/// </summary>
		/// <typeparam name="M">model type M</typeparam>
		/// <param name="model">a model</param>
		/// <returns><em>true</em> if the model has mutated since being created/loaded by the framework; otherwise <em>false</em></returns>
		public static bool HasMutations<M>(this M model)
			where M : IModel
		{
			Contract.Requires<ArgumentNullException>(model != null);

			var spi = model as IModelSPI<M>;
			if (spi == null) throw new NotImplementedException("Cannot access the service provider interface.");
			return spi.HasMutations();
		}

		/// <summary>
		/// Gets the mutation record for a single property.
		/// </summary>
		/// <typeparam name="M">model type M</typeparam>
		/// <param name="model">a model</param>
		/// <param name="property"></param>
		/// <returns></returns>
		public static MutationKinds GetMutationForProperty<M>(this M model, string property)
			where M : IModel
		{
			Contract.Requires<ArgumentNullException>(model != null);

			var spi = model as IModelSPI<M>;
			if (spi == null) throw new NotImplementedException("Cannot access the service provider interface.");
			return spi.GetMutationForProperty(property);
		}

		/// <summary>
		/// Gets mutation kinds for a model's property.
		/// </summary>
		/// <typeparam name="M">model type M</typeparam>
		/// <param name="model">the model</param>
		/// <param name="expression">an expresion identifying the property</param>
		/// <returns>the mutation kinds recorded for the property</returns>
		public static MutationKinds GetMutationForProperty<M>(this M model, Expression<Func<M, object>> expression)
		{
			Contract.Requires(expression != null);

			var spi = model as IModelSPI<M>;
			if (spi == null) throw new NotImplementedException("Cannot access the service provider interface.");

			MemberInfo member = expression.GetMemberFromExpression();
			Contract.Assert(member != null, "Expression must reference a property member");

			var memberType = member.MemberType;
			Contract.Assert(memberType == MemberTypes.Property, "Expression must reference a property member");

			return spi.GetMutationForProperty(member.Name);
		}

		/// <summary>
		/// Gets all mutation records related to the current instance.
		/// </summary>
		/// <typeparam name="M">model type M</typeparam>
		/// <param name="model">the model</param>
		/// <returns>mutation records describing mutations since
		/// the last clean copy.</returns>
		public static IEnumerable<Mutation> GetMutations<M>(this M model)
			where M : IModel
		{
			Contract.Requires<ArgumentNullException>(model != null);

			var spi = model as IModelSPI<M>;
			if (spi == null) throw new NotImplementedException("Cannot access the service provider interface.");
			return spi.GetMutations();
		}

		/// <summary>
		/// Produces a copy of the current instance.
		/// </summary>
		/// <typeparam name="M">model type M</typeparam>
		/// <param name="model">the model</param>
		/// <param name="context">a copy context</param>
		/// <returns>a copy of the current instance</returns>
		public static M Copy<M>(this M model, ICopyContext context)
		{
			Contract.Requires<ArgumentNullException>(model != null);

			var spi = model as IModelSPI<M>;
			if (spi == null) throw new NotImplementedException("Cannot access the service provider interface.");
			return spi.Copy(context);
		}

		/// <summary>
		/// Produces a copy of the current instance.
		/// </summary>
		/// <typeparam name="M">model type M</typeparam>
		/// <param name="model">the model</param>
		/// <returns>a copy of the current instance</returns>
		public static M Copy<M>(M model)
			where M : IModel
		{
			Contract.Requires<ArgumentNullException>(model != null);

			var spi = model as IModelSPI<M>;
			if (spi == null) throw new NotImplementedException("Cannot access the service provider interface.");
			using (var container = Create.SharedOrNewContainer())
			{
				return spi.Copy(new CopyContext(container));
			}
		}

		/// <summary>
		/// Mutates the current instance; resulting in a new instance
		/// reflecting the mutation.
		/// </summary>
		/// <typeparam name="M">model type M</typeparam>
		/// <typeparam name="MU">mutation type MU</typeparam>
		/// <param name="model">the model</param>
		/// <param name="context">a mutation context</param>
		/// <param name="mutation">mutation descriptor</param>
		/// <returns>a new instance reflecting the mutation</returns>
		public static M Mutate<M, MU>(this M model, IMutationContext context, MU mutation)
		{
			Contract.Requires<ArgumentNullException>(model != null);

			var spi = model as IModelSPI<M>;
			if (spi == null) throw new NotImplementedException("Cannot access the service provider interface.");
			return spi.Mutate(context, mutation);
		}

		/// <summary>
		/// Mutates the current instance; resulting in a new instance
		/// reflecting the mutation.
		/// </summary>
		/// <typeparam name="M">model type M</typeparam>
		/// <typeparam name="MU">mutation type MU</typeparam>
		/// <param name="model">the model</param>
		/// <param name="mutation">mutation descriptor</param>
		/// <returns>a new instance reflecting the mutation</returns>
		public static M Mutate<M, MU>(this M model, MU mutation)
		{
			Contract.Requires<ArgumentNullException>(model != null);

			var spi = model as IModelSPI<M>;
			if (spi == null) throw new NotImplementedException("Cannot access the service provider interface.");

			using (var container = Create.SharedOrNewContainer())
			{
				return spi.Mutate(new MutationContext(container), mutation);
			}
		}
	}
}
