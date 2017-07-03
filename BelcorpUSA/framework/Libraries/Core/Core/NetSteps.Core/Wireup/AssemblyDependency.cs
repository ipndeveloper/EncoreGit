using System;
using System.Diagnostics.Contracts;

namespace NetSteps.Encore.Core.Wireup
{
	/// <summary>
	/// Structure around a key-value-pair.
	/// </summary>
	public struct AssemblyDependency
	{
		static readonly int CHashCodeSeed = typeof(AssemblyDependency).AssemblyQualifiedName.GetHashCode();

		string _assembly;
		string _version;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="assembly">the assembly's name</param>
		/// <param name="version">the assembly's version</param>
		public AssemblyDependency(string assembly, string version)
		{
			Contract.Requires<ArgumentNullException>(assembly != null);
			Contract.Requires<ArgumentException>(assembly.Length > 0);
			Contract.Requires<ArgumentNullException>(version != null);
			Contract.Requires<ArgumentException>(version.Length > 0);

			_assembly = assembly;
			_version = version;
		}

		/// <summary>
		/// Gets the assembly's name.
		/// </summary>
		public string Name { get { return _assembly; } }
		/// <summary>
		/// Gets the assembly's version.
		/// </summary>
		public string Version { get { return _version; } }

		/// <summary>
		/// Determines if the pair is equal to another.
		/// </summary>
		/// <param name="other">the other</param>
		/// <returns><em>true</em> if equal; otherwise <em>false</em></returns>
		public bool Equals(AssemblyDependency other)
		{
			return String.Equals(_assembly, other._assembly)
				&& String.Equals(_version, other._version);
		}

		/// <summary>
		/// Determines if the pair is equal to another object.
		/// </summary>
		/// <param name="obj">the other object</param>
		/// <returns><em>true</em> if equal; otherwise <em>false</em></returns>
		public override bool Equals(object obj)
		{
			return obj is AssemblyDependency
				&& Equals((AssemblyDependency)obj);
		}

		/// <summary>
		/// Converts the pair to a string representation.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Concat("{ Name=\"", _assembly, ", Version=\"", _version, "\" }");
		}
		
		/// <summary>
		/// Calculates the pair's hashcode.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			int prime = Constants.RandomPrime; // a random prime

			int result = CHashCodeSeed * prime;
			if (_assembly != null)
				result ^= prime * _assembly.GetHashCode();
			if (_version != null)
				result ^= prime * _version.GetHashCode();
			return result;
		}

		/// <summary>
		/// Determines if two pairs are equal.
		/// </summary>
		/// <param name="lhs">left hand operand</param>
		/// <param name="rhs">right hand operand</param>
		/// <returns><em>true</em> if equal; otherwise <em>false</em></returns>
		public static bool operator ==(AssemblyDependency lhs, AssemblyDependency rhs)
		{
			return lhs.Equals(rhs);
		}

		/// <summary>
		/// Determines if two pairs are unequal.
		/// </summary>
		/// <param name="lhs">left hand operand</param>
		/// <param name="rhs">right hand operand</param>
		/// <returns><em>true</em> if unequal; otherwise <em>false</em></returns>
		public static bool operator !=(AssemblyDependency lhs, AssemblyDependency rhs)
		{
			return !lhs.Equals(rhs);
		}
	}	

}
