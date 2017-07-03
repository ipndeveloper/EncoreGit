﻿using System;
using System.Diagnostics.Contracts;


namespace NetSteps.Encore.Core.Reflection.Emit
{	
	/// <summary>
	/// Helper class for working with class members in the IL stream.
	/// </summary>
	public abstract class EmittedMember
	{		
		/// <summary>
		/// Creates a new instance
		/// </summary>
		/// <param name="type">the emitted type</param>
		/// <param name="name">the name of the member</param>
		protected EmittedMember(EmittedClass type, string name)
		{
			Contract.Requires<ArgumentNullException>(name != null);
			Contract.Requires<ArgumentNullException>(name.Length > 0);

			this.TargetClass = type;
			this.Name = name;
		}
		/// <summary>
		/// Indicates whether the member has been compiled.
		/// </summary>
		public bool IsCompiled { get; private set; }
		/// <summary>
		/// Indicates whether the member is a static member.
		/// </summary>
		public bool IsStatic { get; protected set; }
		/// <summary>
		/// Gets the member's name.
		/// </summary>
		public string Name { get; protected set; }
		/// <summary>
		/// Gets the emitted class on which this member resides.
		/// </summary>
		public EmittedClass TargetClass { get; private set; }

		/// <summary>
		/// Compiles the member.
		/// </summary>
		public void Compile()
		{
			if (!this.IsCompiled)
			{
				OnCompile();
				this.IsCompiled = true;
			}
		}

		/// <summary>
		/// Called by the framework when the member is being compiled.
		/// </summary>
		protected internal virtual void OnCompile()
		{
			throw new NotImplementedException();
		}
	}
}