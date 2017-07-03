using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core.IoC;
using nsCore.Areas.Products.Models.Promotions.Interfaces;

namespace nsCore.Areas.Products.Models.Promotions
{
	public interface ICombinationOfProductsCartCondition : ICartConditionModel
	{
		IList<int> RequiredProductIDs { get; set; }
	}

	[ContainerRegister(typeof(ICombinationOfProductsCartCondition), RegistrationBehaviors.Default, ScopeBehavior=ScopeBehavior.InstancePerRequest)]
	public class CombinationOfProductsCartConditionModel : ICombinationOfProductsCartCondition
	{
		public IList<int> RequiredProductIDs { get; set; }

		public CombinationOfProductsCartConditionModel()
		{
			RequiredProductIDs = new List<int>();
		}

		[ContractInvariantMethod]
		private void ObjectInvariant()
		{
			Contract.Invariant(RequiredProductIDs != null);
		}
	}
}