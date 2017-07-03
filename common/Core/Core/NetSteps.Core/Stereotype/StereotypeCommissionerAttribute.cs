using System;
using System.Diagnostics.Contracts;


namespace NetSteps.Encore.Core.Stereotype
{
	/// <summary>
	/// Enum of stereotype behaviors.
	/// </summary>
	public enum StereotypeCommissionerBehavior
	{
		/// <summary>
		/// Default behavior.
		/// </summary>
		Default = 0,
		/// <summary>
		/// Indicates the commissioner emits missing implementations.
		/// </summary>
		EmitsImplementations = 1,
		/// <summary>
		/// Indicates the commissioner supplements emitted implementations.
		/// </summary>
		SupplementsEmittedImplementations = 2,		
		/// <summary>
		/// Indicates that overrides have been disallowed for the stereotype.
		/// </summary>
		OverridesDisallowed = 3
	}

	/// <summary>
	/// Marks an interface or implementation as a stereotype commissioner.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
	public sealed class StereotypeCommissionerAttribute : Attribute
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="name">stereotype name</param>
		public StereotypeCommissionerAttribute(string name)
			: this(name, StereotypeCommissionerBehavior.Default)
		{
			Contract.Requires<ArgumentNullException>(name != null);
			Contract.Requires<ArgumentException>(name.Length > 0);
		}

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="name">stereotype name</param>
		/// <param name="behavior">the commissioner's behavior</param>
		public StereotypeCommissionerAttribute(string name, StereotypeCommissionerBehavior behavior)
		{
			Contract.Requires<ArgumentNullException>(name != null);
			Contract.Requires<ArgumentException>(name.Length > 0);

			this.Name = name;
			this.Behavior = behavior;
		}
		/// <summary>
		/// Gets the stereotype's name.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Indicates the streotype commissioner's behavior.
		/// </summary>
		public StereotypeCommissionerBehavior Behavior { get; private set; }
	}
}
