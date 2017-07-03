using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using Cart.Common.Service;

namespace Cart.Common.Tests
{
	[TestClass]
	public class CartServiceTests
	{
		[TestMethod]
		public void CartService_GetCarts_ReturnsSomething()
		{
			var carts = Create.New<ICartService>().GetCarts();
			Assert.IsTrue(carts != null && carts.Any());
		}
	}
}
