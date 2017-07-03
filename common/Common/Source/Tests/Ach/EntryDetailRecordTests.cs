using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Common.Ach;

namespace NetSteps.Common.Tests.Ach
{
    [TestClass]
    public class EntryDetailRecordTests
    {
        [TestMethod]
        public void TestToString()
        {
            EntryDetailRecord entryDetail = new EntryDetailRecord();

            entryDetail.RecordTypeCode = "6";
            entryDetail.TransactionCode = "22";
            entryDetail.ReceivingDFIIdentification = "12345678";
            entryDetail.CheckDigit = "9";
            entryDetail.DFIAccountNumber = "123456789";
            entryDetail.Amount = "12.35";
            entryDetail.IndividualIdentificationNumber = "12345678";
            entryDetail.IndividualName = "Tester Man";
            entryDetail.PaymentTypeCode = "S";
            entryDetail.AddendaRecordIndicator = "0";
            entryDetail.TraceNumber = "1";

            string line = entryDetail.ToString();

            // 94 is the fixed length of each line. + \r \n makes it 96 in length
            Assert.IsTrue(line.Length == 96);

            Assert.IsTrue(entryDetail.RecordTypeCode.Length == 1);
            Assert.IsTrue(entryDetail.TransactionCode.Length == 2);
            Assert.IsTrue(entryDetail.ReceivingDFIIdentification.Length == 8);
            Assert.IsTrue(entryDetail.CheckDigit.Length == 1);
            Assert.IsTrue(entryDetail.DFIAccountNumber.Length == 17);
            Assert.IsTrue(entryDetail.Amount.Length == 10);
            Assert.IsTrue(entryDetail.IndividualIdentificationNumber.Length == 15);
            Assert.IsTrue(entryDetail.IndividualName.Length == 22);
            Assert.IsTrue(entryDetail.PaymentTypeCode.Length == 2);
            Assert.IsTrue(entryDetail.AddendaRecordIndicator.Length == 1);
            Assert.IsTrue(entryDetail.TraceNumber.Length == 15);
        }
    }
}
