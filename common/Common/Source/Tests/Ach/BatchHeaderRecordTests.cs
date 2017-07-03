using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Common.Ach;

namespace NetSteps.Common.Tests.Ach
{
    [TestClass]
    public class BatchHeaderRecordTests
    {
        [TestMethod]
        public void TestToString()
        {
            BatchHeaderRecord batchHeader = new BatchHeaderRecord();

            batchHeader.RecordTypeCode = "8";
            batchHeader.ServiceClassCode = "225";
            batchHeader.CompanyName = "Company";
            batchHeader.DiscretionaryData = "1";
            batchHeader.CompanyIdentification = "2";
            batchHeader.StandardEntryClass = "WEB";
            batchHeader.CompanyEntryDescription = "0";
            batchHeader.CompanyDescriptiveDate = "120312";
            batchHeader.EffectiveEntryDate = "120312";
            batchHeader.SettlementDate = " ";
            batchHeader.OriginatorStatusCode = "8";
            batchHeader.OriginatingFinancialInstitution = "12345678";
            batchHeader.BatchNumber = "1";

            batchHeader.EntryDetail = new List<EntryDetailRecord>();

            string line = batchHeader.ToString();

            // 94 is the fixed length of each line. + \r \n makes it 95 in length
            Assert.IsTrue(line.Length == 96);

            Assert.IsTrue(batchHeader.RecordTypeCode.Length == 1);
            Assert.IsTrue(batchHeader.ServiceClassCode.Length == 3);
            Assert.IsTrue(batchHeader.CompanyName.Length == 16);
            Assert.IsTrue(batchHeader.DiscretionaryData.Length == 20);
            Assert.IsTrue(batchHeader.CompanyIdentification.Length == 10);
            Assert.IsTrue(batchHeader.StandardEntryClass.Length == 3);
            Assert.IsTrue(batchHeader.CompanyEntryDescription.Length == 10);
            Assert.IsTrue(batchHeader.CompanyDescriptiveDate.Length == 6);
            Assert.IsTrue(batchHeader.EffectiveEntryDate.Length == 6);
            Assert.IsTrue(batchHeader.SettlementDate.Length == 3);
            Assert.IsTrue(batchHeader.OriginatorStatusCode.Length == 1);
            Assert.IsTrue(batchHeader.OriginatingFinancialInstitution.Length == 8);
            Assert.IsTrue(batchHeader.BatchNumber.Length == 7);
        }
    }
}
