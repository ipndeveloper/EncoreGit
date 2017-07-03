
using System;
using System.Text;
namespace NetSteps.Common.Ach
{
    /// <summary>
    /// This EntryDetailRecord is for WEB entries...
    /// 
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
    public class EntryDetailRecord : BasicRecord
    {
        /// <summary>
        /// Two digit code identifying the account type at the
        /// receiving financial insitution:
        /// 22 - Deposit destined for a Checking Account 
        /// 23 - Prenotification for a checking credit 
        /// 24 - Zero dollar with remittance into Checking Account 
        /// 27 - Debit destined for a Checking Account 
        /// 28 - Prenotification for a checking debit 
        /// 29 - Zero dollar with remittance into Checking Account 
        /// 32 - Deposit destined for a Savings Account 
        /// 33 - Prenotification for a savings credit 
        /// 34 - Zero dollar with remittance into Savings Account 
        /// 37 - Debit destined for a Savings Account 
        /// 38 - Prenotification for a Savings debit 
        /// 39 - Zero dollar with remittance into Savings Account
        /// Numeric
        /// Length: 2
        /// </summary>
        public string TransactionCode;

        /// <summary>
        /// Transit routing number of the receiver's financial institution.
        /// TTTTAAAA
        /// Length: 8
        /// </summary>
        public string ReceivingDFIIdentification;

        /// <summary>
        /// The ninth digits of the receiving financial institutions transit routing number.
        /// Numeric
        /// Length: 1
        /// </summary>
        public string CheckDigit;

        private string dFIAccountNumber;
        /// <summary>
        /// Receiver's account number at their financial institution. Left justify.
        /// Alpha-Numeric
        /// Length: 17
        /// </summary>
        public string DFIAccountNumber
        {
            get
            {
                return PadItemLength(dFIAccountNumber, 17, false, ' ');
            }
            set
            {
                dFIAccountNumber = value;
            }
        }

        private string amount;
        /// <summary>
        /// Transaction amount in dollars with two decimal places. Left zero fill
        /// if necessary. Enter 10 zeros for prenotes.
        /// $$$$$$$$cc
        /// Length: 10
        /// </summary>
        public string Amount
        {
            get
            {
                return PadItemLength(amount, 10, true, '0');
            }
            set
            {
                if (value.Contains("."))
                {
                    value = value.Replace(".", String.Empty);
                }
                amount = value;
            }
        }

        private string individualIdentificationNumber;
        /// <summary>
        /// Receiver's identification number. This number may be printed on the receiver's
        /// bank statement by the receiving financial insititution.
        /// Alpha-Numeric
        /// Length: 15
        /// </summary>
        public string IndividualIdentificationNumber
        {
            get
            {
                return PadItemLength(individualIdentificationNumber, 15, false, ' ');
            }
            set
            {
                individualIdentificationNumber = value;
            }
        }

        private string individualName;
        /// <summary>
        /// Name of receiver
        /// Alpha-Numeric
        /// Length: 22
        /// </summary>
        public string IndividualName
        {
            get
            {
                return PadItemLength(individualName, 22, false, ' ');
            }
            set
            {
                individualName = value;
            }
        }

        private string paymentTypeCode;
        /// <summary>
        /// Input 'R' for recuring payments, and 'S' for single-Entry payment
        /// Alpha-Numeric
        /// Length: 2
        /// </summary>
        public string PaymentTypeCode
        {
            get
            {
                return PadItemLength(paymentTypeCode, 2, false, ' ');
            }
            set
            {
                paymentTypeCode = value;
            }
        }

        /// <summary>
        /// If there is no addenda accompanying this transaction enter '0'.
        /// If addenda are accompanying the transaction enter '1'.
        /// Numeric
        /// Length: 1
        /// </summary>
        public string AddendaRecordIndicator;

        private string traceNumber;
        /// <summary>
        /// The bank will assign a trace number. This number will be unique
        /// to the transaction and will help identify the transaction in case
        /// of an inquiry.
        /// Numeric
        /// Length: 15
        /// </summary>
        public string TraceNumber
        {
            get
            {
                return PadItemLength(traceNumber, 15, true, '0');
            }
            set
            {
                traceNumber = value;
            }
        }

        public override string ToString()
        {
            StringBuilder entryDetailRecord = new StringBuilder();

            entryDetailRecord.Append(RecordTypeCode);
            entryDetailRecord.Append(TransactionCode);
            entryDetailRecord.Append(ReceivingDFIIdentification);
            entryDetailRecord.Append(CheckDigit);
            entryDetailRecord.Append(DFIAccountNumber);
            entryDetailRecord.Append(Amount);
            entryDetailRecord.Append(IndividualIdentificationNumber);
            entryDetailRecord.Append(IndividualName);
            entryDetailRecord.Append(PaymentTypeCode);
            entryDetailRecord.Append(AddendaRecordIndicator);
            entryDetailRecord.Append(TraceNumber);
            entryDetailRecord.Append("\r\n");

            return entryDetailRecord.ToString();
        }
    }
}
