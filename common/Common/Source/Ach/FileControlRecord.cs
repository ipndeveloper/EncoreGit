
using System;
using System.Text;
namespace NetSteps.Common.Ach
{
    /// <summary>
    /// Basic structure of NACHA FILE by layers...tabs mean the following are part of that layer.
    /// 
    /// File Header Record
    ///     Batch Header Record
    ///         First Entry Detail Record
    ///                  ...
    ///         Last Entry Detail Record
    ///     Batch Control Record
    ///     Batch Header Record
    ///         First Entry Detail Record
    ///                  ...
    ///         Last Entry Detail Record
    ///     Batch Control Record
    /// File Control Record
    /// 
    /// There can be multiple batches within the file header record.
    /// 
    /// </summary>
    public class FileControlRecord : BasicRecord
    {
        private string batchCount;
        /// <summary>
        /// The total number of batch header records in the file.
        /// Numeric
        /// Length: 6
        /// </summary>
        public string BatchCount
        {
            get
            {
                return PadItemLength(batchCount, 6, true, '0');
            }
            set
            {
                batchCount = value;
            }
        }

        private string blockCount;
        /// <summary>
        /// The total number of physical blocks on the file, including
        /// the File Header and File Control records
        /// Numeric
        /// Length: 6
        /// </summary>
        public string BlockCount
        {
            get
            {
                return PadItemLength(blockCount, 6, true, '0');
            }
            set
            {
                blockCount = value;
            }
        }

        private string entryAddendaCount;
        /// <summary>
        /// Total number of entry detail and addenda records on the file.
        /// Numeric
        /// Length: 8
        /// </summary>
        public string EntryAddendaCount
        {
            get
            {
                return PadItemLength(entryAddendaCount, 8, true, '0');
            }
            set
            {
                entryAddendaCount = value;
            }
        }

        private string entryHash;
        /// <summary>
        /// Total of all positions 4-11 on each 6 record(Detail. Only use the
        /// final 10 positions in the entry.
        /// Numeric
        /// Length: 10
        /// </summary>
        public string EntryHash
        {
            get
            {
                return PadItemLength(entryHash, 10, true, '0');
            }
            set
            {
                entryHash = value;
            }
        }

        private string totalDebitEntryDollarAmountInFile;
        /// <summary>
        /// Dollar totals of debit entries within the file. If none, zero fill
        /// the field.
        /// $$$$$$$$$$cc
        /// Length: 12
        /// </summary>
        public string TotalDebitEntryDollarAmountInFile
        {
            get
            {
                return PadItemLength(totalDebitEntryDollarAmountInFile, 12, true, '0');
            }
            set
            {
                totalDebitEntryDollarAmountInFile = value;
            }
        }

        private string totalCreditEntryDollarAmountInFile;
        /// <summary>
        /// Dollar totals of credit entries within the file. If none, zero fill
        /// the field.
        /// $$$$$$$$$$cc
        /// Length: 12
        /// </summary>
        public string TotalCreditEntryDollarAmountInFile
        {
            get
            {
                return PadItemLength(totalCreditEntryDollarAmountInFile, 12, true, '0');
            }
            set
            {
                totalCreditEntryDollarAmountInFile = value;
            }
        }

        private string reserved;
        /// <summary>
        /// Leave this field blank
        /// Blank
        /// Length: 39
        /// </summary>
        public string Reserved
        {
            get
            {
                return PadItemLength(reserved, 39, false, ' ');
            }
            set
            {
                reserved = value;
            }
        }

        /// <summary>
        /// Generates the block of file for FileControlRecord.
        /// </summary>
        /// <returns>FileControlRecord block of NACHA File</returns>
        public override string ToString()
        {
            StringBuilder FileControlRecord = new StringBuilder();
            Reserved = String.Empty;

            FileControlRecord.Append(RecordTypeCode);
            FileControlRecord.Append(BatchCount);
            FileControlRecord.Append(BlockCount);
            FileControlRecord.Append(EntryAddendaCount);
            FileControlRecord.Append(EntryHash);
            FileControlRecord.Append(TotalDebitEntryDollarAmountInFile);
            FileControlRecord.Append(TotalCreditEntryDollarAmountInFile);
            FileControlRecord.Append(Reserved);
            FileControlRecord.Append("\r\n");

            return FileControlRecord.ToString();
        }
    }
}

