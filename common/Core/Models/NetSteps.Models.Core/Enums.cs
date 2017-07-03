using System;

namespace NetSteps.Models.Core
{
	/// <summary>
	/// Enum indicating kinds of mutations.
	/// </summary>
	[Flags]
	public enum MutationKinds
	{
		/// <summary>
		/// Indicates none.
		/// </summary>
		None = 0,
		/// <summary>
		/// Indicates a direct mutation; for classes it means
		/// the reference was written during a mutation, for properties
		/// it means the property was written.
		/// </summary>
		Direct = 1,
		/// <summary>
		/// Indicates an item was mutated via composition; for classes
		/// it means a child (referenced class) changed, for properties
		/// it indicates the target of the property changed.
		/// </summary>
		ByComposition = 2
	}

	/// <summary>
	/// Indicates a model's states.
	/// </summary>
	[Flags]
	public enum ModelStates
	{
		/// <summary>
		/// Indicates the model is immutable. (Default)
		/// </summary>
		Immutable = 0,
		/// <summary>
		/// Indicates the framework is writing values to the immutable instance.
		/// </summary>
		Writing = 1 << 1,
		/// <summary>
		/// Indicates the model has mutated since it was created/loaded by the framework.
		/// </summary>
		Dirty = 1 << 2,
		/// <summary>
		/// Indicates the model is dirty by composition.
		/// </summary>
		DirtyComposition = 1 << 3
	}
}
