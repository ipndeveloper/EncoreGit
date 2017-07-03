using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Addresses.PickupPoints.Common.Services;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Addresses.PickupPoints.Services.Tests
{
	[TestClass]
	public class PickupPointsServiceTest
	{
		[TestMethod]
		public void Wireup_Should_CreateDefaultPickupPointService()
		{
			var ppService = Create.New<IPickupPointService>();
			Assert.IsTrue(ppService.GetType() == typeof(NetSteps.Addresses.PickupPoints.Services.PickupPointService));
		}

		[TestMethod]
		public void GetPickupPointCode()
		{
			// Arrange
			RegisterDefaultMockPickupPointRepository();
			var service = new PickupPointService();

			// Act
			var result = service.GetPickupPoints(new CultureInfo("en-US"), "79310", "Vouhe");

			// Assert
			Assert.IsTrue(result.Any() && result.First().PickupPointCode.StartsWith("XYZ"));
		}

		private void RegisterDefaultMockPickupPointRepository()
		{
			var root = Container.Root;

			root.ForType<IPickupPointRepository>()
				.Register<PickupPointRepositoryMock>()
				.ResolveAsSingleton()
				.End();
		}
	}
}
