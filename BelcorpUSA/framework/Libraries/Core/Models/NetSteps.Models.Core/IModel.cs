using System.Collections.Generic;

namespace NetSteps.Models.Core
{
	/// <summary>
	/// Marker interface for models.
	/// </summary>
	public interface IModel { }

	namespace SPI
	{
		/// <summary>
		/// Model's service provider interface.
		/// </summary>
		/// <typeparam name="M"></typeparam>
		public interface IModelSPI<out M> // : IComposable
		{
			/// <summary>
			/// Gets the model's states.
			/// </summary>
			/// <returns></returns>
			ModelStates GetModelState();

			/// <summary>
			/// Indicates whether state has changed via mutation.
			/// </summary>
			/// <returns></returns>
			bool HasMutations();
			/// <summary>
			/// Gets the mutation record for a single property.
			/// </summary>
			/// <param name="property"></param>
			/// <returns></returns>
			MutationKinds GetMutationForProperty(string property);
			/// <summary>
			/// Gets all mutation records related to the current instance.
			/// </summary>
			/// <returns>mutation records describing mutations since
			/// the last clean copy.</returns>
			IEnumerable<Mutation> GetMutations();

			/// <summary>
			/// Marks an instance as clean; clearing all mutation records.
			/// </summary>
			/// <param name="context">a marking context</param>
			void MarkClean(IMarkingContext context);

			/// <summary>
			/// Marks an instance as mutated.
			/// </summary>
			/// <param name="context">a marking context</param>
			/// <param name="kind">the kind of mutation</param>
			void MarkMutated(IMarkingContext context, MutationKinds kind);

			/// <summary>
			/// Produces a copy of the current instance.
			/// </summary>
			/// <param name="context">a copy context</param>
			/// <returns>a copy of the current instance</returns>
			M Copy(ICopyContext context);

			/// <summary>
			/// Mutates the current instance; resulting in a new instance
			/// reflecting the mutation.
			/// </summary>
			/// <typeparam name="MD">mutation descriptor type M</typeparam>
			/// <param name="context">a mutation context</param>
			/// <param name="mutationDescriptor">mutation descriptor</param>
			/// <returns>a new instance reflecting the mutation</returns>
			M Mutate<MD>(IMutationContext context, MD mutationDescriptor);

			/// <summary>
			/// Gets the identity key for a referent.
			/// </summary>
			/// <typeparam name="IK">identity key type IK</typeparam>
			/// <param name="referentName">referent's name (according to model type M)</param>
			/// <returns>The identity key of the referent. If the referent's identity key is not known the result will be equal to default(IK).</returns>
			/// <exception cref="UndefinedReferentException">thrown if <paramref name="referentName"/> is undefined. A referent is the target of
			/// a reference to another model and the referent names are identical to the property name defined on model type M refering to the
			/// referenced model.</exception>
			IK GetReferentID<IK>(string referentName);

			/// <summary>
			/// Gets the reference to a referent.
			/// </summary>
			/// <typeparam name="R">referent type R</typeparam>
			/// <param name="referentName">referent's name (according to model type M)</param>
			/// <returns>a reference to the referent</returns>
			/// <exception cref="UndefinedReferentException">thrown if <paramref name="referentName"/> is undefined. A referent is the target of
			/// a reference to another model and the referent names are identical to the property name defined on model type M refering to the
			/// referenced model.</exception>
			IModelReference<R> GetReference<R>(string referentName);

			/// <summary>
			/// Cascades a mutation to a referent.
			/// </summary>
			/// <typeparam name="R">referent type R</typeparam>
			/// <typeparam name="MD">mutation type MU</typeparam>
			/// <param name="referentName">referent's name (according to model type M)</param>
			/// <param name="context">a mutation context</param>
			/// <param name="mutationDescriptor">the mutation descriptor</param>
			/// <returns>the mutated referent</returns>
			/// <exception cref="UndefinedReferentException">thrown if <paramref name="referentName"/> is undefined. A referent is the target of
			/// a reference to another model and the referent names are identical to the property name defined on model type M refering to the
			/// referenced model.</exception>
			R CascadeReferentMutation<R, MD>(string referentName, IMutationContext context, MD mutationDescriptor);

			/// <summary>
			/// Modifies the model's state by including the states given.
			/// </summary>
			/// <param name="states">the states to include</param>
			/// <param name="cascade">indicates whether the state should cascade to referents</param>
			/// <param name="context">a marking context</param>
			void IncludeModelStates(ModelStates states, bool cascade, IMarkingContext context);

			/// <summary>
			/// Modifies the model's state by excluding the states given.
			/// </summary>
			/// <param name="states">the states to excluded</param>
			/// <param name="cascade">indicates whether the state should cascade to referents</param>
			/// <param name="context">a marking context</param>
			void ExcludeModelStates(ModelStates states, bool cascade, IMarkingContext context);

			/// <summary>
			/// Modifies the model's state to the states given.
			/// </summary>
			/// <param name="states">the states</param>
			/// <param name="cascade">indicates whether the state should cascade to referents</param>
			/// <param name="context">a marking context</param>
			void ChangeModelStates(ModelStates states, bool cascade, IMarkingContext context);
		}
	}
}
