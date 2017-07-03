using System;

namespace NetSteps.Common.Ach
{
    /// <summary>
    /// This is used by FileHeaderRecord, FileControlRecord, BatchHeaderRecord, BatchControlRecord, and EntryDetailRecord
    /// </summary>
    public abstract class BasicRecord
    {
        private string recordTypeCode;

        /// <summary>
        /// This code identifying the BatchHeader record is 
        /// FileHeader = '1', BatchHeader = '5', EntryRecord = '6', BatchControl = '8', FileControl = '9'
        /// Length: 1
        /// </summary>
        public string RecordTypeCode
        {
            get
            {
                return PadItemLength(recordTypeCode, 1, true, ' ');
            }
            set
            {
                recordTypeCode = value;
            }
        }

        /// <summary>
        /// This function is used to get the right amount of blank spaces in each item block for the nacha file.
        /// Nacha files are fix length so each item has a certain length.
        /// </summary>
        /// <param name="original">original string to pad</param>
        /// <param name="fixedLength">length to be</param>
        /// <returns>returns the new string all padded and ready to go.</returns>
        public string PadItemLength(string original, int fixedLength, bool padLeft = true, char padChar = ' ')
        {
            string fixedString = String.Empty;

            if (original == null)
            {
                original = String.Empty;
            }

            if (fixedLength > original.Length)
            {
                if (padLeft)
                {
                    fixedString = original.PadLeft(fixedLength, padChar);
                }
                else
                {
                    fixedString = original.PadRight(fixedLength, padChar);
                }
            }
            else if (fixedLength < original.Length)
            {
                throw new Exception("The original item length is longer than the fixed length of the item");
            }
            else
            {
                fixedString = original;
            }

            return fixedString;
        }
    }
}
