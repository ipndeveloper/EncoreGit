using NetSteps.Encore.Core;

namespace NetSteps.Models.Core.ModelReferences
{
	internal sealed class EmptyModelReference<M, IK> : IModelReference<M, IK>
	{
		static readonly int CHashCodeSeed = typeof(EmptyModelReference<M, IK>).GetKeyForType().GetHashCode();

		internal EmptyModelReference()
		{
		}

		public bool IsEmpty { get { return true; } }

		public bool HasModel { get { return false; } }

		public bool HasIdentityKey { get { return false; } }

		public M Model { get { return default(M); } }

		public object Clone()
		{
			return base.MemberwiseClone();
		}

		public IModelReference<M> ResolveModel()
		{
			throw new UnresolvableModelException("No resolver.");
		}

		public IModelResolver<M> Resolver
		{
			get { return null; }
		}

		public IK IdentityKey
		{
			get { return default(IK); }
		}

		public bool Equals(EmptyModelReference<M, IK> other)
		{
			return other != null;
		}

		public override bool Equals(object obj)
		{
			return obj is EmptyModelReference<M, IK>
				&& Equals((EmptyModelReference<M, IK>)obj);
		}

		public override int GetHashCode()
		{
			int prime = Constants.RandomPrime;
			return CHashCodeSeed * prime;
		}
	}
}
