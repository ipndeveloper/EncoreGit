using System;
using System.Collections.Generic;
using NetSteps.Encore.Core.Properties;

namespace NetSteps.Encore.Core.Wireup.Meta
{
	/// <summary>
	/// Attribute declaring a wireup command for an assembly.
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module, AllowMultiple = true)]
	public sealed class WireupAttribute : Attribute
	{
		Type[] _commands;

		/// <summary>
		/// Creates a new WireupAttribute and initializes its behavior.
		/// </summary>
		/// <param name="behaviors">the assembly's wireup behavior</param>
		public WireupAttribute(WireupBehaviors behaviors)
		{
			Behaviors = behaviors;
			this._commands = new Type[0];
		}

		/// <summary>
		/// Creates a new WireupAttribute and initializes the command type.
		/// </summary>
		/// <param name="commandType">command type</param>
		public WireupAttribute(Type commandType)
		{
			var commands = new List<Type>();
			if (typeof(IWireupCommand).IsAssignableFrom(commandType))
			{
				commands.Add(commandType);
			}
			else
			{
				throw new ArgumentException(Resources.Chk_TypeMustBeAssignableToIWireupCommand);
			}
			this._commands = commands.ToArray();
		}

		/// <summary>
		/// Creates a new WireupAttribute and initializes its behavior and command type.
		/// </summary>
		/// <param name="behaviors">the assembly's wireup behavior</param>
		/// <param name="commandType">command type</param>
		public WireupAttribute(WireupBehaviors behaviors, Type commandType)
		{
			Behaviors = behaviors;
			var commands = new List<Type>();
			if (typeof(IWireupCommand).IsAssignableFrom(commandType))
			{
				commands.Add(commandType);
			}
			else
			{
				throw new ArgumentException(Resources.Chk_TypeMustBeAssignableToIWireupCommand);
			}
			this._commands = commands.ToArray();
		}

		/// <summary>
		/// Creates a new WireupAttribute and initializes its behavior and command types.
		/// </summary>
		/// <param name="behaviors">the assembly's wireup behavior</param>
		/// <param name="commandTypes">command types</param>
		public WireupAttribute(WireupBehaviors behaviors, params Type[] commandTypes)
		{
			Behaviors = behaviors;
			var commands = new List<Type>();
			foreach (var t in commandTypes)
			{
				if (typeof(IWireupCommand).IsAssignableFrom(t))
				{
					commands.Add(t);
				}
				else
				{
					throw new ArgumentException(Resources.Chk_TypeMustBeAssignableToIWireupCommand);
				}
			}
			this._commands = commands.ToArray();
		}

		/// <summary>
		/// Indicates the assembly's wireup behaviors.
		/// </summary>
		public WireupBehaviors Behaviors { get; private set; }

		/// <summary>
		/// The command types to be invoked during wireup.
		/// </summary>
		public IEnumerable<Type> CommandType
		{
			get { return _commands.ToReadOnly(); }
		}
	}
}