using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Common.Ach;

namespace NetSteps.Common.Tests.Ach
{
    [TestClass]
    public class FileHeaderRecordTests
    {
        [TestMethod]
        public void TestToString()
        {
            FileHeaderRecord fileHeader = new FileHeaderRecord();

            fileHeader.RecordTypeCode = "1";
            fileHeader.PriorityCode = "01";
            fileHeader.ImmediateDestination = "123456789";
            fileHeader.ImmediateOrigin = "";
            fileHeader.FileCreationDate = "120312";
            fileHeader.FileCreationTime = "1255";
            fileHeader.FileIDModifier = "A";
            fileHeader.RecordSize = "094";
            fileHeader.BlockingFactor = "10";
            fileHeader.FormatCode = "1";
            fileHeader.ImmediateDestinationName = "BankName";
            fileHeader.ImmediateOriginName = "GoldCanyonTest";
            fileHeader.ReferenceCode = "1";

            fileHeader.BatchHeader = new List<BatchHeaderRecord>();
            fileHeader.BatchHeader.Add(new BatchHeaderRecord());
            fileHeader.BatchHeader.FirstOrDefault().BatchControl = new BatchControlRecord();
            fileHeader.BatchHeader.FirstOrDefault().EntryDetail = new List<EntryDetailRecord>();
            fileHeader.FileControl = new FileControlRecord();

            string line = fileHeader.ToString();

            Assert.IsTrue(fileHeader.RecordTypeCode.Length == 1);
            Assert.IsTrue(fileHeader.PriorityCode.Length == 2);
            Assert.IsTrue(fileHeader.ImmediateDestination.Length == 10);
            Assert.IsTrue(fileHeader.ImmediateOrigin.Length == 10);
            Assert.IsTrue(fileHeader.FileCreationDate.Length == 6);
            Assert.IsTrue(fileHeader.FileCreationTime.Length == 4);
            Assert.IsTrue(fileHeader.FileIDModifier.Length == 1);
            Assert.IsTrue(fileHeader.RecordSize.Length == 3);
            Assert.IsTrue(fileHeader.BlockingFactor.Length == 2);
            Assert.IsTrue(fileHeader.FormatCode.Length == 1);
            Assert.IsTrue(fileHeader.ImmediateDestinationName.Length == 23);
            Assert.IsTrue(fileHeader.ImmediateOriginName.Length == 23);
            Assert.IsTrue(fileHeader.ReferenceCode.Length == 8);
        }
    }
}
