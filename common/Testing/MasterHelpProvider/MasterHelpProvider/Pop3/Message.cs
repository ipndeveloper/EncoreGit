#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace Utilities.Email.Pop3
{
    /// <summary>
    /// Class for containing the messages
    /// returned by the Pop3Client class.
    /// </summary>
    public class Message
    {
        #region Properties
        /// <summary>
        /// The number associated with the message
        /// </summary>
        public long MessageNumber
        {
            get { return _MessageNumber; }
            set { _MessageNumber = value; }
        }
        private long _MessageNumber = 0;

        /// <summary>
        /// Size of the message in bytes
        /// </summary>
        public long MessageSize
        {
            get { return _MessageSize; }
            set { _MessageSize = value; }
        }
        private long _MessageSize = 0;

        /// <summary>
        /// If true, we've retrieved this message from the server
        /// </summary>
        public bool Retrieved
        {
            get { return _Retrieved; }
            set { _Retrieved = value; }
        }
        private bool _Retrieved = false;

        /// <summary>
        /// The actual MIME message returned by the server
        /// </summary>
        public MIME.MIMEMessage MessageBody
        {
            get { return _Message; }
            set { _Message = value; }
        }
        private MIME.MIMEMessage _Message = null;
        #endregion
    }
}