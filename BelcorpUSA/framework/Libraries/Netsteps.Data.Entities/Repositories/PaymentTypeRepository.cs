using System;
using System.Linq;
using NetSteps.Data.Entities.Cache;
using NetSteps.Encore.Core.IoC;
using NetSteps.Payments.Common.Models;

namespace NetSteps.Data.Entities.Repositories
{
	[ContainerRegister(typeof(Payments.Common.Repositories.IPaymentTypeRepository), RegistrationBehaviors.IfNoneOther, ScopeBehavior = ScopeBehavior.Singleton)]
	public partial class PaymentTypeRepository : Payments.Common.Repositories.IPaymentTypeRepository
	{
		public IPaymentType[] GetPaymentTypes(Predicate<IPaymentType> predicate)
		{
			return SmallCollectionCache.Instance.PaymentTypes.Where(predicate.Invoke).ToArray();
		}

		public IPaymentType[] GetAllPaymentTypes()
		{
			return SmallCollectionCache.Instance.PaymentTypes.ToArray();
		}
	}
}
