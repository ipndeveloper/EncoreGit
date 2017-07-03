using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Common.Ach;

namespace NetSteps.Common.Tests.Ach
{
    [TestClass]
    public class BatchControlRecordTests
    {
        [TestMethod]
        public void TestToString()
        {
            BatchControlRecord batchControl = new BatchControlRecord();

            batchControl.RecordTypeCode = "8";
            batchControl.ServiceClassCode = "225";
            batchControl.EntryAddendaCount = "1";
            batchControl.EntryHash = "2";
            batchControl.TotalDebitEntryDollarAmount = "5";
            batchControl.TotalCreditEntryDollarAmount = "0";
            batchControl.CompanyIdentification = "7";
            batchControl.MessageAuthenticationCode = " ";
            batchControl.Reserved = " ";
            batchControl.OriginatingFinancialInstitutionID = "12345678";
            batchControl.BatchNumber = "1";

            string line = batchControl.ToString();

            // 94 is the fixed length of each line. + \r \n makes it 96 in length
            Assert.IsTrue(line.Length == 96);

            Assert.IsTrue(batchControl.RecordTypeCode.Length == 1);
            Assert.IsTrue(batchControl.ServiceClassCode.Length == 3);
            Assert.IsTrue(batchControl.EntryAddendaCount.Length == 6);
            Assert.IsTrue(batchControl.EntryHash.Length == 10);
            Assert.IsTrue(batchControl.TotalDebitEntryDollarAmount.Length == 12);
            Assert.IsTrue(batchControl.TotalCreditEntryDollarAmount.Length == 12);
            Assert.IsTrue(batchControl.CompanyIdentification.Length == 10);
            Assert.IsTrue(batchControl.MessageAuthenticationCode.Length == 19);
            Assert.IsTrue(batchControl.Reserved.Length == 6);
            Assert.IsTrue(batchControl.OriginatingFinancialInstitutionID.Length == 8);
            Assert.IsTrue(batchControl.BatchNumber.Length == 7);
        }
    }
}
