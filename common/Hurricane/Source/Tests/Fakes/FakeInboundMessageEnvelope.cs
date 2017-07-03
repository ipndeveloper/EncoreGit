using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HurricaneServer.Plugins.Inbound;

namespace NetSteps.Hurricane.Plugin.Tests.Fakes
{
    public class FakeInboundMessageEnvelope : IInboundMessageEnvelope
    {
        private byte[] _messageBytes;

        public string CustomMessageId
        {
            get; set;
        }

        public System.IO.Stream GetMessage(Dictionary<string, object> options)
        {
            return GetMessage();
        }

        public System.IO.Stream GetMessage()
        {
            return new MemoryStream(_messageBytes);
        }

        public string MailFrom
        {
            get; set;
        }

        public string MailingId
        {
            get; set;
        }

        public int MessageSize
        {
            get; set;
        }

        public string Recipients
        {
            get; set;
        }

        public void SetMessage(System.IO.Stream message, int Length, Dictionary<string, object> options)
        {
            SetMessage(message, Length);
        }

        public void SetMessage(System.IO.Stream message, int Length)
        {
            _messageBytes = new byte[Length];
            message.Read(_messageBytes, 0, _messageBytes.Length);
        }

        public string SystemMessageId
        {
            get; set;
        }
    }
}
