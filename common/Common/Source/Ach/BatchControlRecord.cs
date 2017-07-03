
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
    public class BatchControlRecord : BasicRecord
    {
        /// <summary>
        /// Identifies the type of entries in the batch:
        /// 200 - ACH Entries mixed Debits and Credits
        /// 220 - ACH Credits Only
        /// 225 - ACH Debits Only
        /// Numeric
        /// Length: 3
        /// </summary>
        public string ServiceClassCode;

        private string entryAddendaCount;
        /// <summary>
        /// Total number of entry detail and addenda records processed
        /// within the batch. This field requires six positions;
        /// right justify and using leading zeros.
        /// Numeric
        /// Length: 6
        /// </summary>
        public string EntryAddendaCount
        {
            get
            {
                return PadItemLength(entryAddendaCount, 6, true, '0');
            }
            set
            {
                entryAddendaCount = value;
            }
        }

        private string entryHash;
        /// <summary>
        /// Total of all positions 4-11 on each 6 record(detail).
        /// Only use the final 10 positions in the entry.
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

        private string totalDebitEntryDollarAmount;
        /// <summary>
        /// Dollar totals of debit entries within the batch. If none, zero fill the field
        /// $$$$$$$$$$cc
        /// Length: 12
        /// </summary>
        public string TotalDebitEntryDollarAmount
        {
            get
            {
                return PadItemLength(totalDebitEntryDollarAmount, 12, true, '0');
            }
            set
            {
                totalDebitEntryDollarAmount = value;
            }
        }

        private string totalCreditEntryDollarAmount;
        /// <summary>
        /// Dollar totals of credit entries with the batch. if none, zero fill the field.
        /// $$$$$$$$$$cc
        /// Length: 12
        /// </summary>
        public string TotalCreditEntryDollarAmount
        {
            get
            {
                return PadItemLength(totalCreditEntryDollarAmount, 12, true, '0');
            }
            set
            {
                totalCreditEntryDollarAmount = value;
            }
        }

        private string companyIdentification;
        /// <summary>
        /// This should match the company identification number used in the corresponding 
        /// batch header record, field 5.
        /// NNNNNNNNNN
        /// Length: 10
        /// </summary>
        public string CompanyIdentification
        {
            get
            {
                return PadItemLength(companyIdentification, 10, true, '0');
            }
            set
            {
                companyIdentification = value;
            }
        }

        private string messageAuthenticationCode;
        /// <summary>
        /// This is an optional field. Please leave this field blank.
        /// Blank
        /// Length: 19
        /// </summary>
        public string MessageAuthenticationCode
        {
            get
            {
                return PadItemLength(messageAuthenticationCode, 19, true, ' ');
            }
            set
            {
                messageAuthenticationCode = value;
            }
        }

        private string reserved;
        /// <summary>
        /// This field is reserved for Federal Reserve use. Please leave this field blank.
        /// Blank
        /// Length: 6
        /// </summary>
        public string Reserved
        {
            get
            {
                return PadItemLength(reserved, 6, true, ' ');
            }
            set
            {
                reserved = value;
            }
        }

        /// <summary>
        /// Enter bank routing number of originator
        /// TTTTAAAA
        /// Length: 8
        /// </summary>
        public string OriginatingFinancialInstitutionID;

        private string batchNumber;
        /// <summary>
        /// Number of the batch associated with this control record.
        /// Numeric
        /// Length: 7
        /// </summary>
        public string BatchNumber
        {
            get
            {
                return PadItemLength(batchNumber, 7, true, '0');
            }
            set
            {
                batchNumber = value;
            }
        }

        public override string ToString()
        {
            StringBuilder BatchControlRecord = new StringBuilder();

            BatchControlRecord.Append(RecordTypeCode);
            BatchControlRecord.Append(ServiceClassCode);
            BatchControlRecord.Append(EntryAddendaCount);
            BatchControlRecord.Append(EntryHash);
            BatchControlRecord.Append(TotalDebitEntryDollarAmount);
            BatchControlRecord.Append(TotalCreditEntryDollarAmount);
            BatchControlRecord.Append(CompanyIdentification);
            BatchControlRecord.Append(MessageAuthenticationCode);
            BatchControlRecord.Append(Reserved);
            BatchControlRecord.Append(OriginatingFinancialInstitutionID);
            BatchControlRecord.Append(BatchNumber);
            BatchControlRecord.Append("\r\n");

            return BatchControlRecord.ToString();
        }
    }
}
