
using System;
using System.Linq;
using System.Diagnostics.Contracts;

namespace NetSteps.Encore.Core.IoC.Constructors
{
	/// <summary>
	/// Base class for command bindings.
	/// </summary>
	/// <typeparam name="T">target type T</typeparam>
	public class CommandBinding<T>
	{
		/// <summary>
		/// Gets the constructor adapter for target type T
		/// </summary>
		protected ConstructorAdapter<T> Adapter { get; private set; }

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="adapter">A constructor adapter for type T</param>
		protected CommandBinding(ConstructorAdapter<T> adapter)
		{
			this.Adapter = adapter;
		}

		/// <summary>
		/// Executes the constructor adapter and returns the resulting instance.
		/// </summary>
		/// <param name="container">scoping container</param>
		/// <param name="name">the registered name or null</param>
		/// <returns>the resulting instance</returns>
		public virtual T Execute(IContainer container, string name)
		{
			return Adapter.Execute(container, name, null);
		}

		internal static CommandBinding<T> Create(ConstructorAdapter<T> adapter, Param[] parameters)
		{
			Contract.Requires<ArgumentNullException>(parameters != null);

			switch (parameters.Length)
			{
				case 0:
					return new CommandBinding<T>(adapter);
				case 1:
					return new CommandBinding_1<T>(adapter, parameters[0]);
				case 2:
					return new CommandBinding_2<T>(adapter, parameters);
				case 3:
					return new CommandBinding_3<T>(adapter, parameters);
				case 4:
					return new CommandBinding_4<T>(adapter, parameters);
				case 5:
					return new CommandBinding_5<T>(adapter, parameters);
				case 6:
					return new CommandBinding_6<T>(adapter, parameters);
				case 7:
					return new CommandBinding_7<T>(adapter, parameters);
				case 8:
					return new CommandBinding_8<T>(adapter, parameters);
				case 9:
					return new CommandBinding_9<T>(adapter, parameters);
				default:
					return new CommandBinding_N<T>(adapter, parameters);
			}
		}
	}

	internal sealed class CommandBinding_1<T>: CommandBinding<T>
	{
		Param _parameter;

		internal CommandBinding_1(ConstructorAdapter<T> adapter, Param parameter)
			: base(adapter)
		{
			if (parameter == null)
				throw new ArgumentNullException("parameter");
			_parameter = parameter;
		}
		public override T Execute(IContainer container, string name)
		{
			return Adapter.Execute(container, name, new object[] { _parameter.GetValue(container) });
		}
	}
	internal sealed class CommandBinding_2<T>: CommandBinding<T>
	{
		Param[] _parameters;

		internal CommandBinding_2(ConstructorAdapter<T> adapter, Param[] parameters)
			: base(adapter)
		{
			if (parameters == null)
				throw new ArgumentNullException("parameters");
			if (parameters.Length != 2)
				throw new ArgumentException("Invalid number of parameters", "parameters");
			_parameters = parameters;
		}
		public override T Execute(IContainer container, string name)
		{
			object[] parameters = new object[] {
							_parameters[0].GetValue(container),
							_parameters[1].GetValue(container)
			};
			return Adapter.Execute(container, name, parameters);
		}
	}
	internal sealed class CommandBinding_3<T>: CommandBinding<T>
	{
		Param[] _parameters;

		internal CommandBinding_3(ConstructorAdapter<T> adapter, Param[] parameters)
			: base(adapter)
		{
			if (parameters == null)
				throw new ArgumentNullException("parameters");
			if (parameters.Length != 3)
				throw new ArgumentException("Invalid number of parameters", "parameters");
			_parameters = parameters;
		}
		public override T Execute(IContainer container, string name)
		{
			object[] parameters = new object[] {
							_parameters[0].GetValue(container),
							_parameters[1].GetValue(container),
							_parameters[2].GetValue(container),
			};
			return Adapter.Execute(container, name, parameters);
		}
	}
	internal sealed class CommandBinding_4<T>: CommandBinding<T>
	{
		Param[] _parameters;

		internal CommandBinding_4(ConstructorAdapter<T> adapter, Param[] parameters)
			: base(adapter)
		{
			if (parameters == null)
				throw new ArgumentNullException("parameters");
			if (parameters.Length != 4)
				throw new ArgumentException("Invalid number of parameters", "parameters");
			_parameters = parameters;
		}
		public override T Execute(IContainer container, string name)
		{
			object[] parameters = new object[] {
							_parameters[0].GetValue(container),
							_parameters[1].GetValue(container),
							_parameters[2].GetValue(container),
							_parameters[3].GetValue(container),
			};
			return Adapter.Execute(container, name, parameters);
		}
	}
	internal sealed class CommandBinding_5<T>: CommandBinding<T>
	{
		Param[] _parameters;

		internal CommandBinding_5(ConstructorAdapter<T> adapter, Param[] parameters)
			: base(adapter)
		{
			if (parameters == null)
				throw new ArgumentNullException("parameters");
			if (parameters.Length != 5)
				throw new ArgumentException("Invalid number of parameters", "parameters");
			_parameters = parameters;
		}
		public override T Execute(IContainer container, string name)
		{
			object[] parameters = new object[] {
							_parameters[0].GetValue(container),
							_parameters[1].GetValue(container),
							_parameters[2].GetValue(container),
							_parameters[3].GetValue(container),
							_parameters[4].GetValue(container)
			};
			return Adapter.Execute(container, name, parameters);
		}
	}
	internal sealed class CommandBinding_6<T>: CommandBinding<T>
	{
		Param[] _parameters;

		internal CommandBinding_6(ConstructorAdapter<T> adapter, Param[] parameters)
			: base(adapter)
		{
			if (parameters == null)
				throw new ArgumentNullException("parameters");
			if (parameters.Length != 6)
				throw new ArgumentException("Invalid number of parameters", "parameters");
			_parameters = parameters;
		}
		public override T Execute(IContainer container, string name)
		{
			object[] parameters = new object[] {
							_parameters[0].GetValue(container),
							_parameters[1].GetValue(container),
							_parameters[2].GetValue(container),
							_parameters[3].GetValue(container),
							_parameters[4].GetValue(container),
							_parameters[5].GetValue(container),
			};
			return Adapter.Execute(container, name, parameters);
		}
	}
	internal sealed class CommandBinding_7<T>: CommandBinding<T>
	{
		Param[] _parameters;

		internal CommandBinding_7(ConstructorAdapter<T> adapter, Param[] parameters)
			: base(adapter)
		{
			if (parameters == null)
				throw new ArgumentNullException("parameters");
			if (parameters.Length != 7)
				throw new ArgumentException("Invalid number of parameters", "parameters");
			_parameters = parameters;
		}
		public override T Execute(IContainer container, string name)
		{
			object[] parameters = new object[] {
							_parameters[0].GetValue(container),
							_parameters[1].GetValue(container),
							_parameters[2].GetValue(container),
							_parameters[3].GetValue(container),
							_parameters[4].GetValue(container),
							_parameters[5].GetValue(container),
							_parameters[6].GetValue(container)
			};
			return Adapter.Execute(container, name, parameters);
		}
	}
	internal sealed class CommandBinding_8<T>: CommandBinding<T>
	{
		Param[] _parameters;

		internal CommandBinding_8(ConstructorAdapter<T> adapter, Param[] parameters)
			: base(adapter)
		{
			if (parameters == null)
				throw new ArgumentNullException("parameters");
			if (parameters.Length != 8)
				throw new ArgumentException("Invalid number of parameters", "parameters");
			_parameters = parameters;
		}
		public override T Execute(IContainer container, string name)
		{
			object[] parameters = new object[] {
							_parameters[0].GetValue(container),
							_parameters[1].GetValue(container),
							_parameters[2].GetValue(container),
							_parameters[3].GetValue(container),
							_parameters[4].GetValue(container),
							_parameters[5].GetValue(container),
							_parameters[6].GetValue(container),
							_parameters[7].GetValue(container)
			};
			return Adapter.Execute(container, name, parameters);
		}
	}
	internal sealed class CommandBinding_9<T>: CommandBinding<T>
	{
		Param[] _parameters;

		internal CommandBinding_9(ConstructorAdapter<T> adapter, Param[] parameters)
			: base(adapter)
		{
			if (parameters == null)
				throw new ArgumentNullException("parameters");
			if (parameters.Length != 9)
				throw new ArgumentException("Invalid number of parameters", "parameters");
			_parameters = parameters;
		}
		public override T Execute(IContainer container, string name)
		{
			object[] parameters = new object[] {
							_parameters[0].GetValue(container),
							_parameters[1].GetValue(container),
							_parameters[2].GetValue(container),
							_parameters[3].GetValue(container),
							_parameters[4].GetValue(container),
							_parameters[5].GetValue(container),
							_parameters[6].GetValue(container),
							_parameters[7].GetValue(container),
							_parameters[8].GetValue(container),
			};
			return Adapter.Execute(container, name, parameters);
		}
	}
	internal sealed class CommandBinding_10<T>: CommandBinding<T>
	{
		Param[] _parameters;

		internal CommandBinding_10(ConstructorAdapter<T> adapter, Param[] parameters)
			: base(adapter)
		{
			if (parameters == null)
				throw new ArgumentNullException("parameters");
			if (parameters.Length != 10)
				throw new ArgumentException("Invalid number of parameters", "parameters");
			_parameters = parameters;
		}
		public override T Execute(IContainer container, string name)
		{
			object[] parameters = new object[] {
							_parameters[0].GetValue(container),
							_parameters[1].GetValue(container),
							_parameters[2].GetValue(container),
							_parameters[3].GetValue(container),
							_parameters[4].GetValue(container),
							_parameters[5].GetValue(container),
							_parameters[6].GetValue(container),
							_parameters[7].GetValue(container),
							_parameters[8].GetValue(container),
							_parameters[9].GetValue(container),
			};
			return Adapter.Execute(container, name, parameters);
		}
	}
	internal sealed class CommandBinding_N<T>: CommandBinding<T>
	{
		Param[] _parameters;

		internal CommandBinding_N(ConstructorAdapter<T> adapter, Param[] parameters)
			: base(adapter)
		{
			if (parameters == null)
				throw new ArgumentNullException("parameters");
			_parameters = parameters;
		}
		public override T Execute(IContainer container, string name)
		{
			return Adapter.Execute(container, name, (from p in _parameters
																				 select p.GetValue(container)).ToArray());
		}


	}
}
