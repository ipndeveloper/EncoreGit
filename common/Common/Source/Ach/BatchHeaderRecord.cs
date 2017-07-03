using System.Collections.Generic;
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
    public class BatchHeaderRecord : BasicRecord
    {
        /// <summary>
        /// Identifies the type of entries in the batch
        /// 200 - ACH Entries mixed Debits and Credits
        /// 220 - ACH Credits Only
        /// 225 - ACH Debits Only
        /// Numeric
        /// Length: 3
        /// </summary>
        public string ServiceClassCode;

        private string companyName;
        /// <summary>
        /// Your company name, up to 16 characters. This name may appear
        /// on the receivers' statements prepared by the Receiving Financial Institution
        /// Alpha-Numeric
        /// Length: 16
        /// </summary>
        public string CompanyName
        {
            get
            {
                return PadItemLength(companyName, 16, false, ' ');
            }
            set
            {
                companyName = value;
            }
        }

        private string discretionaryData;
        /// <summary>
        /// Optional
        /// Alpha-Numeric
        /// Length: 20
        /// </summary>
        public string DiscretionaryData
        {
            get
            {
                return PadItemLength(discretionaryData, 20, false, ' ');
            }
            set
            {
                discretionaryData = value;
            }
        }

        private string companyIdentification;
        /// <summary>
        /// Your 10-digit company number. Identical to the number in field 4 of the 
        /// File Header Record, unless multiple companies/divisions are provided in
        /// one transmission
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

        /// <summary>
        /// Identifies the entries in the batch. Common standard entry class codes are 
        /// PPD(Prearranged Payments, and Deposit entries) for consumer items, 
        /// CCD(Cash Concentration and Disbursement entries) for corporate transactions,
        /// TEL(Telephone initiated entries), and
        /// WEB(Authorization received via the Internet)
        /// Alpha
        /// Length: 3
        /// </summary>
        public string StandardEntryClass;

        private string companyEntryDescription;
        /// <summary>
        /// Your description of the transaction. This may be printed on the receivers' bank
        /// statement by the receiving Financial Institution(i.e. payroll)
        /// Alpha-Numeric
        /// Length: 10
        /// </summary>
        public string CompanyEntryDescription
        {
            get
            {
                return PadItemLength(companyEntryDescription, 10, false, ' ');
            }
            set
            {
                companyEntryDescription = value;
            }
        }

        private string companyDescriptiveDate;
        /// <summary>
        /// The date you choose to identify the transactions. This date may be printed
        /// on the participants' bank statement by the Receiving Financial Institution.
        /// Alpha-Numeric
        /// Length: 6
        /// </summary>
        public string CompanyDescriptiveDate
        {
            get
            {
                return PadItemLength(companyDescriptiveDate, 6, false, ' ');
            }
            set
            {
                companyDescriptiveDate = value;
            }
        }

        /// <summary>
        /// Date transactions are to be posted to the participants' account.
        /// YYMMDD
        /// Length: 6
        /// </summary>
        public string EffectiveEntryDate;

        private string settlementDate;
        /// <summary>
        /// Leave blank, taken care of by ACH
        /// Length: 3
        /// </summary>
        public string SettlementDate
        {
            get
            {
                return PadItemLength(settlementDate, 3, true, ' ');
            }
            set
            {
                settlementDate = value;
            }
        }

        /// <summary>
        /// Originator status code.
        /// '1'
        /// Length: 1
        /// </summary>
        public string OriginatorStatusCode;

        /// <summary>
        /// Originators routing number.
        /// 12345678
        /// Length: 8
        /// </summary>
        public string OriginatingFinancialInstitution;

        private string batchNumber;
        /// <summary>
        /// Number batches sequentially.
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

        public List<EntryDetailRecord> EntryDetail;

        public BatchControlRecord BatchControl;

        public override string ToString()
        {
            StringBuilder BatchHeadRecord = new StringBuilder();

            BatchHeadRecord.Append(RecordTypeCode);
            BatchHeadRecord.Append(ServiceClassCode);
            BatchHeadRecord.Append(CompanyName);
            BatchHeadRecord.Append(DiscretionaryData);
            BatchHeadRecord.Append(CompanyIdentification);
            BatchHeadRecord.Append(StandardEntryClass);
            BatchHeadRecord.Append(CompanyEntryDescription);
            BatchHeadRecord.Append(CompanyDescriptiveDate);
            BatchHeadRecord.Append(EffectiveEntryDate);
            BatchHeadRecord.Append(SettlementDate);
            BatchHeadRecord.Append(OriginatorStatusCode);
            BatchHeadRecord.Append(OriginatingFinancialInstitution);
            BatchHeadRecord.Append(BatchNumber);
            BatchHeadRecord.Append("\r\n");

            foreach (EntryDetailRecord entryDetailRecord in EntryDetail)
            {
                BatchHeadRecord.Append(entryDetailRecord.ToString());
            }

            return BatchHeadRecord.ToString();
        }
    }
}
