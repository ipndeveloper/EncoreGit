﻿using System.Reflection.Emit;
using System;

namespace NetSteps.Encore.Core.Reflection.Emit
{
	/// <summary>
	/// Interface for objects that have a value that can be loaded on the stack.
	/// </summary>
	public interface IValueRef
	{
		/// <summary>
		/// Name of the value.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Gets the value's type.
		/// </summary>
		Type TargetType { get; }

		/// <summary>
		/// Loads the address of the value by pushing it onto the stack.
		/// </summary>
		/// <param name="il">the il generator.</param>
		void LoadAddress(ILGenerator il);

		/// <summary>
		/// Loads the value by pushing it onto the stack.
		/// </summary>
		/// <param name="il">the il generator.</param>
		void LoadValue(ILGenerator il);

		/// <summary>
		/// Stores the value by popping it off of the stack.
		/// </summary>
		/// <param name="il">the il generator.</param>
		void StoreValue(ILGenerator il);
	}

}