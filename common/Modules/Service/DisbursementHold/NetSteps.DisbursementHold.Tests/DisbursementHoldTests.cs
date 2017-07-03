using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NetSteps.Content.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.DisbursementHold.Common;
using NetSteps.DisbursementHold.Service;

namespace NetSteps.DisbursementHold.Tests
{
    [TestClass]
    public class DisbursementHoldTests
    {

        private ICheckHold CreateNewCheckHold(int accountID)
        {
            var result = Create.New<ICheckHold>();
            result.AccountID = accountID;

            return result;
        }

        private ICheckHoldResult CreateNewCheckHoldResult(int accountID, DateTime holdDate)
        {
            var result = Create.New<ICheckHoldResult>();
            result.ErrorMessages = new List<string>();
            result.AccountID = accountID;
            result.HoldUntil = holdDate;
            result.Success = true;

            return result;
        }

        private ICheckHoldValues CreateNewCheckHoldValues()
        {
            var result = Create.New<ICheckHoldValues>();            
            result.ApplicationID = 1;
            result.OverrideReasonID = 1;
            result.Notes = "EXPIRED RENEWAL DATE";
            result.UserID = 1;

            return result;
        }
        

        [TestMethod]
        public void SaveDisbursementHold_Returns_New_ICheckHoldResult()
        {
            // Arrange
            int accountID = 1000;
            DateTime holdDate = DateTime.Today.AddYears(1);

            var checkHoldRepository = new Mock<ICheckHoldRepositoryAdapter>();
            var termTranslation = new Mock<ITermResolver>();

            ICheckHold checkHold = CreateNewCheckHold(accountID);
            ICheckHoldValues checkHoldValues = CreateNewCheckHoldValues();
            ICheckHoldResult checkHoldResults = CreateNewCheckHoldResult(accountID, holdDate);

            checkHoldRepository.Setup<ICheckHoldResult>(x => x.LoadDisbursementHold(It.IsAny<int>())).Returns(checkHoldResults);
            checkHoldRepository.Setup<ICheckHoldValues>(x => x.LoadCheckHoldValues()).Returns(checkHoldValues);
            checkHoldRepository.Setup<ICheckHoldResult>(x => x.SaveDisbursementHold(It.IsAny<ICheckHold>())).Returns(checkHoldResults);
            checkHoldRepository.Setup<ICheckHoldResult>(x => x.UpdateDisbursementHold(It.IsAny<ICheckHold>())).Returns(checkHoldResults);

            var adapter = new DisbursementHoldService(checkHoldRepository.Object, termTranslation.Object);

            // Act
            var result = adapter.SaveDisbursementHold(accountID, holdDate);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ICheckHoldResult));
            Assert.AreEqual(holdDate, result.HoldUntil);
        }

    }
}
