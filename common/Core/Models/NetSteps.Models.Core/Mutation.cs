using System;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core;

namespace NetSteps.Models.Core
{
	/// <summary>
	/// Structure recording a mutation.
	/// </summary>
	public struct Mutation
	{
		static readonly int CHashCodeSeed = typeof(Mutation).GetKeyForType().GetHashCode();

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="path">path to the mutation from the origin</param>
		/// <param name="kind">the kind of mutation</param>
		public Mutation(string path, MutationKinds kind)
		{
			Contract.Requires<ArgumentNullException>(path != null);
			Contract.Requires<ArgumentException>(path.Length > 0);

			_path = path;
			_kinds = kind;
		}
		/// <summary>
		/// Gets the path to the mutation form the origin.
		/// </summary>
		public string Path { get { return _path; } }
		/// <summary>
		/// Gets the kind of mutation at the path's endpoint.
		/// </summary>
		public MutationKinds Kinds { get { return _kinds; } }

		string _path;
		MutationKinds _kinds;

		/// <summary>
		/// Determines if the mutation is equal to another.
		/// </summary>
		/// <param name="other">the other</param>
		/// <returns><em>true</em> if equal; otherwise <em>false</em></returns>
		public bool Equals(Mutation other)
		{
			return String.Equals(_path, other.Path)
				&& _kinds == other._kinds;
		}
		/// <summary>
		/// Determines if the mutation is equal to another object.
		/// </summary>
		/// <param name="obj">the other object</param>
		/// <returns><em>true</em> if equal; otherwise <em>false</em></returns>
		public override bool Equals(object obj)
		{
			return (obj is Mutation) && Equals((Mutation)obj);
		}
		/// <summary>
		/// Gets the mutation's hashcode.
		/// </summary>
		/// <returns>a hashcode</returns>
		public override int GetHashCode()
		{
			int prime = Constants.RandomPrime;
			var code = CHashCodeSeed * prime;
			if (_path != null)
			{
				code ^= this._path.GetHashCode() * prime;
			}
			code ^= ((int)this._kinds) * prime;
			return code;
		}
		/// <summary>
		/// Gets a string representation of the mutation.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Concat("{ \"Path\": \"", Path, "\", \"Kinds\": \"", Kinds, "\" }");
		}
	}

}
