using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Common.Collections
{
	public static class ListExtensions
	{
		public static IList<TAbstraction> AsAbstract<TAbstraction, TImplementation>(this IList<TImplementation> instance)
			where TImplementation : class, TAbstraction
		{
			return new AbstractList<TImplementation, TAbstraction>(instance);
		}

		public static IList<TConcrete> AsConcrete<TAbstraction, TConcrete>(this IList<TAbstraction> instance)
			where TConcrete : class, TAbstraction
		{
			return new ConcreteList<TAbstraction, TConcrete>(instance);
		}
	}
}