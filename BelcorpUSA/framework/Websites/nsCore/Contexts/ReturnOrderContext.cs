using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Context;
using NetSteps.Encore.Core.IoC;

namespace nsCore.Contexts
{
	public interface IReturnOrderContext : IOrderContext
	{
		IOrder OriginalOrder { get; set; }
	}

	[Serializable]
	[ContainerRegister(typeof(IReturnOrderContext), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Default)]
	public class ReturnOrderContext : OrderContext, IReturnOrderContext
	{
		public IOrder OriginalOrder { get; set; }
	}
}