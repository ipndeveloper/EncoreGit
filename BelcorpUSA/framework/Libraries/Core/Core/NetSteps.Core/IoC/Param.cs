using System;
using System.Reflection;
using NetSteps.Encore.Core.Reflection;
using System.Diagnostics.Contracts;

namespace NetSteps.Encore.Core.IoC
{
	/// <summary>
	/// Enum of param kinds
	/// </summary>
	[Flags]
	public enum ParamKind
	{
		/// <summary>
		/// Indicates user supplied.
		/// </summary>
		UserSupplied = 0,
		/// <summary>
		/// Indicates container supplied.
		/// </summary>
		ContainerSupplied = 1,
		/// <summary>
		/// Indicates declared as the default.
		/// </summary>
		DeclaredDefault = 2,
		/// <summary>
		/// Indicates container supplied default
		/// </summary>
		ContainerDefault = 4,
		/// <summary>
		/// Indicates the param is named
		/// </summary>
		Named = 8,
		/// <summary>
		/// Indicates the param is default, named, and container supplied
		/// </summary>
		DefaultNamed = ContainerDefault | Named,
		/// <summary>
		/// Indicates the param is missing
		/// </summary>
		Missing = 0x40000000,
	}

	/// <summary>
	/// Abstract class for parameters used with a container.
	/// </summary>
	public abstract class Param
	{
		/// <summary>
		/// An empty param array.
		/// </summary>
		public static readonly Param[] EmptyParams = new Param[0];

		/// <summary>
		/// Creates a param on a value.
		/// </summary>
		/// <typeparam name="T">value type T</typeparam>
		/// <param name="value">the value</param>
		/// <returns>a param</returns>
		public static Param Value<T>(T value)
		{
			return new ParamValue<T>(ParamKind.UserSupplied, value);
		}
		
		/// <summary>
		/// Creates a param that will resolve type T from the container.
		/// </summary>
		/// <typeparam name="T">type T</typeparam>
		/// <returns>a param</returns>
		public static Param Resolve<T>()
		{
			return new ParamFromContainer<T>();
		}

		/// <summary>
		/// Creates a param that will resolve type T from the container.
		/// </summary>
		/// <typeparam name="T">type T</typeparam>
		/// <returns>a param</returns>
		public static Param ResolveNamed<T>(string name)
		{
			return new ParamFromContainerNamed<T>(name);
		}

		/// <summary>
		/// Creates a param with a name and value.
		/// </summary>
		/// <typeparam name="T">type T</typeparam>
		/// <param name="name">the name</param>
		/// <param name="value">the value</param>
		/// <returns>a param</returns>
		public static Param Named<T>(string name, T value)
		{
			return new NamedParamValue<T>(ParamKind.UserSupplied | ParamKind.Named, name, value);
		}

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="kind"></param>
		/// <param name="typeofValue"></param>
		protected Param(ParamKind kind, Type typeofValue)
		{
			this.Kind = kind;
			this.TypeofValue = typeofValue;
		}

		/// <summary>
		/// Gets the param's kind.
		/// </summary>
		public ParamKind Kind { get; private set; }
		/// <summary>
		/// Gets the type of the param's value.
		/// </summary>
		public Type TypeofValue { get; private set; }
		/// <summary>
		/// Gets the parameter's value.
		/// </summary>
		/// <param name="container">scoping container</param>
		/// <returns>the param's value</returns>
		public abstract object GetValue(IContainer container);
		/// <summary>
		/// Indicates whether the param is named.
		/// </summary>
		public bool HasName { get { return Kind.HasFlag(ParamKind.Named); } }
		/// <summary>
		/// Gets the param's name.
		/// </summary>
		public string Name { get; protected set; }

		internal static Param Missing(string name, int position, Type type)
		{
			return new ParamMissing(name, position, type);
		}

		internal static Param Declared<T>(T value)
		{
			return new ParamValue<T>(ParamKind.DeclaredDefault, value);
		}

		internal static Param MakeValueUsingReflection(ParamKind kind, Type type, object value)
		{
			Contract.Requires<ArgumentException>(typeof(ParamValue<>).GetGenericArguments().Length == 1);
			Type tt = typeof(ParamValue<>).MakeGenericType(type);
			return (Param)Activator.CreateInstance(tt, kind, value);
		}

		internal static Param MakeContainterSupplied(Type type)
		{
			Type tt = typeof(ParamFromContainer<>).MakeGenericType(type);
			return (Param)Activator.CreateInstance(tt);
		}

		internal static Param[] GetDefaultParamsUsingReflection(ConstructorInfo ci)
		{
			var defined = ci.GetParameters();
			Param[] result = new Param[defined.Length];

			for (var i = 0; i < defined.Length; i++)
			{
				var p = defined[i];
				// Make a default for the parameter...
				if (p.Attributes.HasFlag(ParameterAttributes.HasDefault))
				{
					result[i] = Param.MakeValueUsingReflection(ParamKind.DeclaredDefault, p.ParameterType, p.DefaultValue);
				}
				else if (p.ParameterType.IsClass || p.ParameterType.IsInterface)
				{
					result[i] = Param.MakeContainterSupplied(p.ParameterType);
				}
				else
				{
					result[i] = Param.Missing(p.Name, p.Position, p.ParameterType);
				}
			}
			return result;
		}

		internal static bool TryBindSuppliedDefaults(ConstructorInfo ci, Param[] defaults, out Param[] bound)
		{
			Contract.Requires<ArgumentNullException>(ci != null);

			var defined = ci.GetParameters();
			Param[] result = new Param[defined.Length];
			if (defaults != null)
			{
				if (defaults.Length == defined.Length)
				{
					var matched = 0;
					for (int i = 0; i < defined.Length; i++)
					{
						if (defaults[i].Kind.HasFlag(ParamKind.Named)
							&& defaults[i].Name != defined[i].Name) break;

						if (!defined[i].ParameterType.IsAssignableFrom(defaults[i].TypeofValue)) break;

						result[i] = defaults[i];
						matched++;
					}
					if (matched == defined.Length)
					{
						bound = result;
						return true;
					}
				}
			}
			bound = null;
			return false;
		}
	}

	internal class ParamValue<T> : Param
	{
		public ParamValue(ParamKind kind, T value)
			: base(kind, typeof(T))
		{
			Value = value;
		}

		public override object GetValue(IContainer container)
		{
			return Value;
		}

		public T Value { get; private set; }
	}

	internal sealed class ParamFromContainer<T> : Param
	{
		public ParamFromContainer()
			: base(ParamKind.ContainerSupplied, typeof(T))
		{
		}

		public override object GetValue(IContainer container)
		{
			return container.New<T>();
		}
	}

	internal sealed class ParamFromContainerNamed<T> : Param
	{
		private string RegistrationName { get; set; }

		public ParamFromContainerNamed(string registrationName)
			: base(ParamKind.ContainerSupplied, typeof(T))
		{
			RegistrationName = registrationName;
		}

		public override object GetValue(IContainer container)
		{
			return container.NewNamed<T>(RegistrationName);
		}
	}

	internal sealed class ParamMissing : Param
	{
		string _name;
		int _position;

		public ParamMissing(string name, int position, Type type)
			: base(ParamKind.Missing, type)
		{
			_name = name;
			_position = position;
		}

		public override object GetValue(IContainer container)
		{
			throw new MissingParameterException(String.Concat("Required parameter missing: {Name: '", _name,
				"', Position: ", _position, ", Type: '", TypeofValue.GetReadableFullName(), "'}"));
		}
	}

	internal sealed class NamedParamValue<T> : ParamValue<T>
	{
		public NamedParamValue(ParamKind kind, string name, T value)
			: base(kind, value)
		{
			Name = name;
		}
	}

	/// <summary>
	/// extensions for the Param class
	/// </summary>
	public static class ParamExtensions
	{		
		/// <summary>
		/// Gets a value from the first parameter of type T.
		/// </summary>
		/// <typeparam name="T">type T</typeparam>
		/// <param name="pp">array of parameters</param>
		/// <param name="c">a container</param>
		/// <returns>the parameter's value</returns>
		public static T OfType<T>(this Param[] pp, IContainer c)
		{
			var t = typeof(T);
			var v = First(pp, p => t == p.TypeofValue);
			return (T)v.GetValue(c);
		}

		/// <summary>
		/// Gets a value from the first parameter assignable to type T.
		/// </summary>
		/// <typeparam name="T">type T</typeparam>
		/// <param name="pp">array of parameters</param>
		/// <param name="c">a container</param>
		/// <returns>the parameter's value</returns>
		public static T AssignableTo<T>(this Param[] pp, IContainer c)
		{
			var t = typeof(T);
			var v = First(pp, p => t.IsAssignableFrom(p.TypeofValue));
			return (T)v.GetValue(c);
		}

		/// <summary>
		/// Gets a value from the first parameter assignable to T and with the name given.
		/// </summary>
		/// <typeparam name="T">type T</typeparam>
		/// <param name="pp">array of parameters</param>
		/// <param name="name">the parameter's name</param>
		/// <param name="c">a container</param>
		/// <returns>the parameter's value</returns>
		public static T Named<T>(this Param[] pp, string name, IContainer c)
		{
			if (name == null) throw new ArgumentNullException(name);
			if (name.Length == 0) throw new ArgumentException("name cannot be empty", "name");

			var t = typeof(T);
			var v = First(pp, p => t.IsAssignableFrom(p.TypeofValue) && name == p.Name);
			return (T)v.GetValue(c);
		}

		/// <summary>
		/// Gets a value from the parameter of type T and at the index given.
		/// </summary>
		/// <typeparam name="T">type T</typeparam>
		/// <param name="pp">array of paramters</param>
		/// <param name="position">the parameter's index</param>
		/// <param name="c">a container</param>
		/// <returns>the parameter's value</returns>
		public static T At<T>(this Param[] pp, int position, IContainer c)
		{
			var v = pp[position];
			if (typeof(T).IsAssignableFrom(v.TypeofValue))
			{
				return (T)v.GetValue(c);
			}
			throw new InvalidOperationException(String.Concat("Value not assignable: ", pp[position].TypeofValue.GetReadableFullName()));
		}
		
		/// <summary>
		/// Gets the first parameter from <paramref name="pp"/> that satisfies the predticate.
		/// </summary>
		/// <param name="pp">array of paramters</param>
		/// <param name="predicate">the predicate</param>
		/// <returns>the first param succeeding the predicate</returns>
		public static Param First(this Param[] pp, Func<Param, bool> predicate)
		{
			if (predicate == null) throw new ArgumentNullException("predicate");

			#region switch-unrolled - if this seems wierd, search unit tests for 'switch-unrolled'
			var plen = pp.Length;
			switch (plen)
			{
				case 0: break;
				case 1: if (predicate(pp[0])) return pp[0]; break;
				case 2: if (predicate(pp[0])) return pp[0]; if (predicate(pp[1])) return pp[1]; break;
				case 3:	if (predicate(pp[0])) return pp[0]; if (predicate(pp[1])) return pp[1]; if (predicate(pp[2])) return pp[2]; break;
				case 4: if (predicate(pp[0])) return pp[0]; if (predicate(pp[1])) return pp[1]; if (predicate(pp[2])) return pp[2]; if (predicate(pp[3])) return pp[3]; break;
				case 5: if (predicate(pp[0])) return pp[0]; if (predicate(pp[1])) return pp[1]; if (predicate(pp[2])) return pp[2]; if (predicate(pp[3])) return pp[3]; if (predicate(pp[4])) return pp[4]; break;
				case 6: if (predicate(pp[0])) return pp[0]; if (predicate(pp[1])) return pp[1]; if (predicate(pp[2])) return pp[2]; if (predicate(pp[3])) return pp[3]; if (predicate(pp[4])) return pp[4]; if (predicate(pp[5])) return pp[5]; break;
				case 7:
					if (predicate(pp[0])) return pp[0];
					if (predicate(pp[1])) return pp[1];
					if (predicate(pp[2])) return pp[2];
					if (predicate(pp[3])) return pp[3];
					if (predicate(pp[4])) return pp[4];
					if (predicate(pp[5])) return pp[5];
					if (predicate(pp[6])) return pp[6];
					break;
				case 8:
					if (predicate(pp[0])) return pp[0];
					if (predicate(pp[1])) return pp[1];
					if (predicate(pp[2])) return pp[2];
					if (predicate(pp[3])) return pp[3];
					if (predicate(pp[4])) return pp[4];
					if (predicate(pp[5])) return pp[5];
					if (predicate(pp[6])) return pp[6];
					if (predicate(pp[7])) return pp[7];
					break;
				case 9:
					if (predicate(pp[0])) return pp[0];
					if (predicate(pp[1])) return pp[1];
					if (predicate(pp[2])) return pp[2];
					if (predicate(pp[3])) return pp[3];
					if (predicate(pp[4])) return pp[4];
					if (predicate(pp[5])) return pp[5];
					if (predicate(pp[6])) return pp[6];
					if (predicate(pp[7])) return pp[7];
					if (predicate(pp[8])) return pp[8];
					break;
				default:
					if (predicate(pp[0])) return pp[0];
					if (predicate(pp[1])) return pp[1];
					if (predicate(pp[2])) return pp[2];
					if (predicate(pp[3])) return pp[3];
					if (predicate(pp[4])) return pp[4];
					if (predicate(pp[5])) return pp[5];
					if (predicate(pp[6])) return pp[6];
					if (predicate(pp[7])) return pp[7];
					if (predicate(pp[8])) return pp[8];
					if (predicate(pp[9])) return pp[9];
					for (int i = 10; i < plen; i++)
						if (predicate(pp[i])) return pp[i];
					break;
			}
			#endregion
			throw new InvalidOperationException("Parameter not found");
		}
	}
}
