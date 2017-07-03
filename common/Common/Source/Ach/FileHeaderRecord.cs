using System.Collections.Generic;
using System.Text;

namespace NetSteps.Common.Ach
{
    /// <summary>
    /// Author: Lenni Uitto
    /// 
    /// Basic structure of NACHA FILE by layers...Pay attention to tabs in this comment...they show the structure of the file.
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
    public class FileHeaderRecord : BasicRecord
    {
        private string priorityCode;
        /// <summary>
        /// The lower the number, the higher processing priority.
        /// '01'
        /// Length: 2
        /// </summary>
        public string PriorityCode
        {
            get
            {
                return PadItemLength(priorityCode, 2, true, '0');
            }
            set
            {
                priorityCode = value;
            }
        }

        private string immediateDestination;
        /// <summary>
        /// Destination Routing number
        /// it should be proceded by a blank
        /// b123456789
        /// Length: 10
        /// </summary>
        public string ImmediateDestination
        {
            get
            {
                return PadItemLength(immediateDestination, 10, true, ' ');
            }
            set
            {
                immediateDestination = value;
            }
        }

        private string immediateOrigin;
        /// <summary>
        /// 10-digit company number. The use of an IRS Federal TAx
        /// Identification Number as a company identification is
        /// recommended.
        /// 1234567890
        /// Length: 10
        /// </summary>
        public string ImmediateOrigin
        {
            get
            {
                return PadItemLength(immediateOrigin, 10, true, '0');
            }
            set
            {
                immediateOrigin = value;
            }
        }

        /// <summary>
        /// The date you created the input file.
        /// YYMMDD
        /// Length: 6
        /// </summary>
        public string FileCreationDate;

        /// <summary>
        /// Time of day you created the input file. This field is used
        /// to distinguish between input files if you submit more than
        /// one per day.
        /// HHMM
        /// Length: 4
        /// </summary>
        public string FileCreationTime;

        /// <summary>
        /// Code to distinguish among multiple input files. Label the first
        /// (or only) file "A", and continue in sequence (A-Z). If more than
        /// one file is delivered, they must have different modifiers.
        /// A-Z 0-9
        /// Length: 1
        /// </summary>
        public string FileIDModifier;

        /// <summary>
        /// Number of bytes per record.
        /// '094'
        /// Length: 3
        /// </summary>
        public string RecordSize;

        /// <summary>
        /// Block at 10
        /// '10'
        /// Length: 2
        /// </summary>
        public string BlockingFactor;

        /// <summary>
        /// Currently there is only one code.
        /// '1'
        /// Length: 1
        /// </summary>
        public string FormatCode;

        private string immediateDestinationName;
        /// <summary>
        /// Name of destination bank
        /// Alpha-Numeric
        /// Length: 23
        /// </summary>
        public string ImmediateDestinationName
        {
            get
            {
                return PadItemLength(immediateDestinationName, 23, false, ' ');
            }
            set
            {
                immediateDestinationName = value;
            }
        }

        private string immediateOriginName;
        /// <summary>
        /// Your companies name, up to 23 characters
        /// Alpha-Numeric
        /// Length: 23
        /// </summary>
        public string ImmediateOriginName
        {
            get
            {
                return PadItemLength(immediateOriginName, 23, false, ' ');
            }
            set
            {
                immediateOriginName = value;
            }
        }

        private string referenceCode;
        /// <summary>
        /// Optional Field you may use to describe input file for internal 
        /// accounting purposes.
        /// Alpha-Numeric
        /// Length: 8
        /// </summary>
        public string ReferenceCode
        {
            get
            {
                return PadItemLength(referenceCode, 8, false, ' ');
            }
            set
            {
                referenceCode = value;
            }
        }

        /// <summary>
        /// list of batch headers
        /// </summary>
        public List<BatchHeaderRecord> BatchHeader;

        /// <summary>
        /// each file header has a filecontrol
        /// </summary>
        public FileControlRecord FileControl;

        /// <summary>
        /// Generates NACHA FILE
        /// </summary>
        /// <returns>returns string that is the nacha file</returns>
        public override string ToString()
        {
            StringBuilder NachaFile = new StringBuilder();

            NachaFile.Append(RecordTypeCode);
            NachaFile.Append(PriorityCode);
            NachaFile.Append(ImmediateDestination);
            NachaFile.Append(ImmediateOrigin);
            NachaFile.Append(FileCreationDate);
            NachaFile.Append(FileCreationTime);
            NachaFile.Append(FileIDModifier);
            NachaFile.Append(RecordSize);
            NachaFile.Append(BlockingFactor);
            NachaFile.Append(FormatCode);
            NachaFile.Append(ImmediateDestinationName);
            NachaFile.Append(ImmediateOriginName);
            NachaFile.Append(ReferenceCode);
            NachaFile.Append("\r\n");

            foreach (BatchHeaderRecord batchHeaderRecord in BatchHeader)
            {
                NachaFile.Append(batchHeaderRecord.ToString());
                NachaFile.Append(batchHeaderRecord.BatchControl.ToString());
            }

            NachaFile.Append(FileControl.ToString());

            return NachaFile.ToString();
        }
    }
}
