using NetSteps.Encore.Core;

namespace NetSteps.Models.Core.ModelReferences
{
	internal sealed class EmptyModelReference<M> : IModelReference<M>
	{
		static readonly int CHashCodeSeed = typeof(EmptyModelReference<M>).GetKeyForType().GetHashCode();

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

		public bool Equals(EmptyModelReference<M> other)
		{
			return other != null;
		}

		public override bool Equals(object obj)
		{
			return obj is EmptyModelReference<M>
				&& Equals((EmptyModelReference<M>)obj);
		}

		public override int GetHashCode()
		{
			int prime = Constants.RandomPrime;
			return CHashCodeSeed * prime;
		}
	}

}
