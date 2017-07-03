using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Common.Ach;

namespace NetSteps.Common.Tests.Ach
{
    [TestClass]
    public class FileControlRecordTests
    {
        [TestMethod]
        public void TestToString()
        {
            FileControlRecord fileControl = new FileControlRecord();

            fileControl.RecordTypeCode = "8";
            fileControl.BatchCount = "1";
            fileControl.BlockCount = "4";
            fileControl.EntryAddendaCount = "0";
            fileControl.EntryHash = "15";
            fileControl.TotalDebitEntryDollarAmountInFile = "12.35";
            fileControl.TotalCreditEntryDollarAmountInFile = "0";
            fileControl.Reserved = " ";

            string line = fileControl.ToString();

            // 94 is the fixed length of each line. + \n makes it 95 in length
            Assert.IsTrue(line.Length == 96);

            Assert.IsTrue(fileControl.RecordTypeCode.Length == 1);
            Assert.IsTrue(fileControl.BatchCount.Length == 6);
            Assert.IsTrue(fileControl.BlockCount.Length == 6);
            Assert.IsTrue(fileControl.EntryAddendaCount.Length == 8);
            Assert.IsTrue(fileControl.EntryHash.Length == 10);
            Assert.IsTrue(fileControl.TotalDebitEntryDollarAmountInFile.Length == 12);
            Assert.IsTrue(fileControl.TotalCreditEntryDollarAmountInFile.Length == 12);
            Assert.IsTrue(fileControl.Reserved.Length == 39);
        }
    }
}
