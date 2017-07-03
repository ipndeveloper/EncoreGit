using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Modules.Autoship.Common;
using NetSteps.Encore.Core.IoC;
using Moq;
using NetSteps.Diagnostics.Logging.Common;
using NetSteps.Content.Common;
using NetSteps.Web.API.Autoship.Common;


namespace NetSteps.Web.API.Autoship.Tests
{
	[TestClass]
	public class AutoshipTests
	{
		private IList<ISiteAutoship> CreateAutoshipSearchResultCollection(int accountID, int autoshipScheduleID)
		{
			IList<ISiteAutoship> result = new List<ISiteAutoship>();

			for (int i = 0; i < 3; i++)
			{
				var autoship = Create.New<ISiteAutoship>();
				autoship.AccountID = accountID;
				autoship.OrderID = i;
				autoship.OrderDate = DateTime.Today;
				autoship.BonusVolume = Convert.ToInt32(99M);
				autoship.OrderTotal = Convert.ToInt32(300M);
				autoship.AutoShipScheduleID = autoshipScheduleID;

				result.Add(autoship);
			}

			return result;
		}

		private IAutoshipCancelResult CreateAutoshipCancelResult(int accountID, int autoshipID)
		{
			var result = Create.New<IAutoshipCancelResult>();

			result.AccountID = accountID;
			result.AutoshipID = autoshipID;
			result.Success = true;
			
			return result;
		}
		
		[TestMethod]
		public void Autoship_Search_Json()
		{
			int accountID = 2300;
			int autoshipScheduleID = 2;
			var auto = new Mock<IAutoship>();
			var log = new Mock<ILogResolver>();
			var term = new Mock<ITermResolver>();
			var autoResult = Create.New<IAutoshipSearchResult>();
			var autoships = CreateAutoshipSearchResultCollection(accountID, autoshipScheduleID);
			var controller = new AutoshipController(auto.Object, term.Object, log.Object);

			autoships[0].AccountID = accountID;
			autoships[0].AutoShipScheduleID = autoshipScheduleID;
			autoships[0].OrderID = 1;
			autoships[0].OrderDate = DateTime.Today;
			autoships[0].BonusVolume = Convert.ToInt32(99M);
			autoships[0].OrderTotal = Convert.ToInt32(300M);

			auto.Setup<IAutoshipSearchResult>(x => x.Search(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>())).Returns(autoResult);

			var result = controller.Search(accountID, autoshipScheduleID);
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(NetSteps.Web.API.Base.Common.JsonResult));
		}

		[TestMethod]
		public void Autoship_Cancel_Json()
		{
			int accountID = 2300;
			int autoshipID = 2;
			var auto = new Mock<IAutoship>();
			var log = new Mock<ILogResolver>();
			var term = new Mock<ITermResolver>();
			var autoships = CreateAutoshipCancelResult(accountID, autoshipID);
			var controller = new AutoshipController(auto.Object, term.Object, log.Object);

			auto.Setup<IAutoshipCancelResult>(x => x.Cancel(It.IsAny<int>(), It.IsAny<int>())).Returns(autoships);

			var result = controller.Cancel(accountID, autoshipID);
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(NetSteps.Web.API.Base.Common.JsonResult));
		}
	}
}
