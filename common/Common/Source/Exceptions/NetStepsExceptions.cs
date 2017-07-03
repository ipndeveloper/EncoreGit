using System;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using NetSteps.Common.Configuration;
using NetSteps.Common.Interfaces;

namespace NetSteps.Common.Exceptions
{
	#region Global Exceptions

	[Serializable]
	public class NetStepsException : Exception, IHasBeenLogged
	{
		private string _publicMessage = null;
		private string _internalMessage = null;
        private bool _includeErrorLogMessage = true;
        private string _longStackTrace = string.Empty;

		public int? AccountID { get; set; }
		public int? OrderID { get; set; } // Set this contextually where the error occurs in order to log certain exception associates to the orders they occurred on. - JHE
		public bool HasBeenLogged { get; set; }
		public Object ErrorLog { get; set; }
		public string PublicMessage
		{
			get
			{
				string message = string.IsNullOrEmpty(_publicMessage) ? base.Message : _publicMessage;

                message = string.IsNullOrEmpty(message) ? string.Empty : Regex.Replace(message, @"\r?\n", ApplicationContextCommon.Instance.NewLine);

				bool showErrorLogID = ConfigurationManager.GetAppSetting<bool>("ShowErrorLogID", true);
				if (showErrorLogID)
				{
					IListValue ex = ErrorLog as IListValue;
					if (ex != null)
					{
						if (IncludeErrorLogMessage && ex.ID > 0)
                            message = string.Format("{0}{1}{2}{3}", "Error log#: ", ex.ID, ApplicationContextCommon.Instance.NewLine, message);
					}
				}
				return message;
			}
			set
			{
				_publicMessage = value;
			}
		}

		public string ExceptionMessage
		{
			get
			{
				return base.Message;
			}
		}

		public string InternalMessage
		{
			get
			{
				return _internalMessage;
			}
			set
			{
				_internalMessage = value;
			}
		}

		// Show PublicMessage if set to hide unfriendly messages from users. - JHE
		public new string Message
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(PublicMessage))
					return this.PublicMessage;
				else
					return base.Message;
			}
		}

		public bool IncludeErrorLogMessage
		{
			get
			{
				return _includeErrorLogMessage;
			}
			set
			{
				_includeErrorLogMessage = value;
			}
		}

        /// <summary>
        /// Overriden to provide a longer stack trace than the default
        /// </summary>
        public override string StackTrace { get { return string.Format("{1}{0}LONG STACK TRACE:{0}{2}", Environment.NewLine, base.StackTrace ?? string.Empty, _longStackTrace ?? string.Empty); } }

		public NetStepsException()
		{
            _longStackTrace = Environment.StackTrace;
		}

		public NetStepsException(Exception ex)
			: base(ex.Message, ex)
		{
            _longStackTrace = Environment.StackTrace;
		}

		public NetStepsException(string message)
			: base(message)
        {
            _longStackTrace = Environment.StackTrace;
		}
		public NetStepsException(string message, Exception innerException)
			: base(message, innerException)
        {
            _longStackTrace = Environment.StackTrace;
		}

		protected NetStepsException(
			  SerializationInfo info,
			  StreamingContext context)
			: base(info, context)
        {
            _longStackTrace = Environment.StackTrace;
		}
	}

	[Serializable]
	public class NetStepsDataException : NetStepsException
	{
		public string StoredProcedure { get; set; }

		public NetStepsDataException()
			: base()
		{
		}

		public NetStepsDataException(Exception ex)
			: base(ex.Message, ex)
		{
		}

		public NetStepsDataException(string message)
			: base(message)
		{
		}
		public NetStepsDataException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	[Serializable]
	public class NetStepsBusinessException : NetStepsException
	{
		public NetStepsBusinessException()
			: base()
		{
		}

		public NetStepsBusinessException(Exception ex)
			: base(ex.Message, ex)
		{
		}

		public NetStepsBusinessException(string message)
			: base(message)
		{
		}
		public NetStepsBusinessException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	[Serializable]
	public class NetStepsOptimisticConcurrencyException : NetStepsException
	{
		public NetStepsOptimisticConcurrencyException()
			: base()
		{
		}

		public NetStepsOptimisticConcurrencyException(Exception ex)
			: base(ex.Message, ex)
		{
		}

		public NetStepsOptimisticConcurrencyException(string message)
			: base(message)
		{
		}
		public NetStepsOptimisticConcurrencyException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	[Serializable]
	public class NetStepsEntityFrameworkException : NetStepsException
	{
		public NetStepsEntityFrameworkException()
			: base()
		{
		}

		public NetStepsEntityFrameworkException(Exception ex)
			: base(ex.Message, ex)
		{
		}

		public NetStepsEntityFrameworkException(string message)
			: base(message)
		{
		}
		public NetStepsEntityFrameworkException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	[Serializable]
	public class NetStepsPaymentGatewayException : NetStepsException
	{
		public NetStepsPaymentGatewayException()
			: base()
		{
		}

		public NetStepsPaymentGatewayException(Exception ex)
			: base(ex.Message, ex)
		{
		}

		public NetStepsPaymentGatewayException(string message)
			: base(message)
		{
		}
		public NetStepsPaymentGatewayException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	[Serializable]
	public class NetStepsApplicationException : NetStepsException
	{
		public NetStepsApplicationException()
			: base()
		{
		}

		public NetStepsApplicationException(Exception ex)
			: base(ex.Message, ex)
		{
		}

		public NetStepsApplicationException(string message)
			: base(message)
		{
		}
		public NetStepsApplicationException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
	#endregion

	#region CRUD Data Exceptions

	[Serializable]
	public class InsertDataException : NetStepsDataException
	{
		public InsertDataException()
			: base()
		{
		}

		public InsertDataException(string message)
			: base(message)
		{
		}
		public InsertDataException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	[Serializable]
	public class UpdateDataException : NetStepsDataException
	{
		public UpdateDataException()
			: base()
		{
		}

		public UpdateDataException(string message)
			: base(message)
		{
		}
		public UpdateDataException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	[Serializable]
	public class DeleteDataException : NetStepsDataException
	{
		public DeleteDataException()
			: base()
		{
		}

		public DeleteDataException(string message)
			: base(message)
		{
		}
		public DeleteDataException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	[Serializable]
	public class LoadDataException : NetStepsDataException
	{
		public LoadDataException()
			: base()
		{
		}

		public LoadDataException(string message)
			: base(message)
		{
		}
		public LoadDataException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	#endregion

	#region CRUD Business Exceptions

	[Serializable]
	public class InsertBusinessException : NetStepsBusinessException
	{
		public InsertBusinessException()
			: base()
		{
		}

		public InsertBusinessException(string message)
			: base(message)
		{
		}
		public InsertBusinessException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	[Serializable]
	public class UpdateBusinessException : NetStepsBusinessException
	{
		public UpdateBusinessException()
			: base()
		{
		}

		public UpdateBusinessException(string message)
			: base(message)
		{
		}
		public UpdateBusinessException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	[Serializable]
	public class DeleteBusinessException : NetStepsBusinessException
	{
		public DeleteBusinessException()
			: base()
		{
		}

		public DeleteBusinessException(string message)
			: base(message)
		{
		}
		public DeleteBusinessException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	[Serializable]
	public class LoadBusinessException : NetStepsBusinessException
	{
		public LoadBusinessException()
			: base()
		{
		}

		public LoadBusinessException(string message)
			: base(message)
		{
		}
		public LoadBusinessException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	#endregion

	[Serializable]
	public class InvalidConversionException : NetStepsException
	{
		public InvalidConversionException()
		{
		}

		public InvalidConversionException(string message)
			: base(message)
		{
		}

		public InvalidConversionException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected InvalidConversionException(
			SerializationInfo info,
			StreamingContext context)
			: base(info, context)
		{
		}
	}

	[Serializable]
	public class SiteNotFoundByUrlException : Exception
	{
		public SiteNotFoundByUrlException(string message)
			: base(message)
		{ }
	}

	[Serializable]
	public class ContentEditLinkNotSetException : Exception
	{
		public ContentEditLinkNotSetException(string message)
			: base(message)
		{

		}
	}

	[Serializable]
	public class ContentEditInternalPageException : Exception
	{
		public ContentEditInternalPageException(string message)
			: base(message)
		{

		}
	}

	[Serializable]
	public class ImageSizeException : Exception
	{
		public ImageSizeException(string message)
			: base(message)
		{

		}
	}

	[Serializable]
	public class TaxesNotFoundForAddressException : NetStepsDataException
	{
		public TaxesNotFoundForAddressException()
			: base()
		{
		}

		public TaxesNotFoundForAddressException(Exception ex)
			: base(ex.Message, ex)
		{
		}

		public TaxesNotFoundForAddressException(string message)
			: base(message)
		{
		}
		public TaxesNotFoundForAddressException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	[Serializable]
	public class ShippingMethodNotAvailableException : NetStepsException
	{
		public ShippingMethodNotAvailableException()
			: base()
		{
		}

		public ShippingMethodNotAvailableException(Exception ex)
			: base(ex.Message, ex)
		{
		}

		public ShippingMethodNotAvailableException(string message)
			: base(message)
		{
		}
		public ShippingMethodNotAvailableException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

    [Serializable]
    public class AsyncReloadLoadException : NetStepsException
    {
        public AsyncReloadLoadException()
			: base()
		{
		}

		public AsyncReloadLoadException(Exception ex)
			: base(ex.Message, ex)
		{
		}

		public AsyncReloadLoadException(string message)
			: base(message)
		{
		}
        public AsyncReloadLoadException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
    }
}
