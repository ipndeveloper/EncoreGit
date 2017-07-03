using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Collections;

namespace TestMasterHelpProvider.Logging
{
	public class XmlLogWriter : ILogWriter
	{
		#region Fields

		private static int __hashCode;
		private static bool _truncateLogFileOnInit = false;

		private string _filePath;
		private XDocument _document;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets whether or not to truncate the log file when the LogWriter is instantiated.
		/// </summary>
		public static bool TruncateLogFileOnInit
		{
			get { return _truncateLogFileOnInit; }
			set { _truncateLogFileOnInit = value; }
		}

		/// <summary>
		/// Gets the path to the log file.
		/// </summary>
		public string FilePath
		{
			get { return _filePath; }
		}

		#endregion

		#region Constructors

		public XmlLogWriter(string filePath)
		{
			_filePath = filePath;

			XmlLogWriter.__hashCode = GetHashCode();

			if (XmlLogWriter.TruncateLogFileOnInit)
			{
				_document = new XDocument();
				_document.Add(new XElement(XName.Get("logs")));
				_document.Root.Add(CreateNewSessionNode());

				using (FileStream stream = new FileStream(FilePath, FileMode.Create, FileAccess.Write))
				{
					_document.Save(stream);
				}
			}
			else
			{
				using (StreamReader streamReader = new StreamReader(new FileStream(FilePath, FileMode.Open, FileAccess.Read)))
				{
					_document = XDocument.Load(streamReader);
				}

				_document.Root.Add(CreateNewSessionNode());

				using (FileStream stream = new FileStream(FilePath, FileMode.Create, FileAccess.Write))
				{
					_document.Save(stream);
				}
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Writes a message to the log.
		/// </summary>
		/// <param name="logMessageType"></param>
		/// <param name="message"></param>
		public void WriteLogMessage(LogMessageType logMessageType, string message)
		{
			using (StreamReader streamReader = new StreamReader(new FileStream(FilePath, FileMode.Open, FileAccess.Read)))
			{
				_document = XDocument.Load(streamReader);
			}

			IEnumerable<XElement> elements = from el in _document.Root.Elements("session") where (int)el.Attribute("hashCode") == XmlLogWriter.__hashCode select el;

			if (elements.Count() > 0)
			{
				XElement sessionNode = elements.FirstOrDefault();
				sessionNode.Add(CreateMessageNode(logMessageType, message));
			}

			using (FileStream stream = new FileStream(FilePath, FileMode.Create, FileAccess.Write))
			{
				_document.Save(stream);
			}
		}

		/// <summary>
		/// Creates a session node.
		/// </summary>
		/// <returns></returns>
		private XElement CreateNewSessionNode()
		{
			XElement sessionNode = new XElement(XName.Get("session"));
			sessionNode.SetAttributeValue(XName.Get("hashCode"), XmlLogWriter.__hashCode);
			sessionNode.SetAttributeValue(XName.Get("timestamp"), DateTime.Now.ToString(TestMasterHelpProviderConstants.FullTimestampToStringPattern));

			return sessionNode;
		}

		/// <summary>
		/// Creates a message node.
		/// </summary>
		/// <param name="logMessageType"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		private XElement CreateMessageNode(LogMessageType logMessageType, string message)
		{
			XElement messageNode = new XElement(XName.Get("message"));
			messageNode.SetAttributeValue(XName.Get("timestamp"), DateTime.Now.ToString(TestMasterHelpProviderConstants.FullTimestampToStringPattern));
			messageNode.SetAttributeValue(XName.Get("type"), logMessageType.ToString());
			messageNode.SetValue(message);

			return messageNode;
		}

		#endregion
	}
}
