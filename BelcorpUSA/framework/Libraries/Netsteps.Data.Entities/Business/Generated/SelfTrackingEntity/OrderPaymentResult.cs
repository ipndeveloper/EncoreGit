//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;

namespace NetSteps.Data.Entities
{
    [KnownType(typeof(Currency))]
    [KnownType(typeof(OrderCustomer))]
    [KnownType(typeof(OrderPayment))]
    [KnownType(typeof(Order))]
    [KnownType(typeof(User))]
    [KnownType(typeof(PaymentGateway))]
    [Serializable]
    public partial class OrderPaymentResult: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void OrderPaymentResultIDChanged();
    	public int OrderPaymentResultID
    	{
    		get { return _orderPaymentResultID; }
    		set
    		{
    			if (_orderPaymentResultID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'OrderPaymentResultID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_orderPaymentResultID = value;
    				OrderPaymentResultIDChanged();
    				OnPropertyChanged("OrderPaymentResultID");
    			}
    		}
    	}
    	private int _orderPaymentResultID;
    	partial void OrderPaymentIDChanged();
    	public int OrderPaymentID
    	{
    		get { return _orderPaymentID; }
    		set
    		{
    			if (_orderPaymentID != value)
    			{
    				ChangeTracker.RecordOriginalValue("OrderPaymentID", _orderPaymentID);
    				if (!IsDeserializing)
    				{
    					if (OrderPayment != null && OrderPayment.OrderPaymentID != value)
    					{
    						OrderPayment = null;
    					}
    				}
    				_orderPaymentID = value;
    				OrderPaymentIDChanged();
    				OnPropertyChanged("OrderPaymentID");
    			}
    		}
    	}
    	private int _orderPaymentID;
    	partial void OrderIDChanged();
    	public int OrderID
    	{
    		get { return _orderID; }
    		set
    		{
    			if (_orderID != value)
    			{
    				ChangeTracker.RecordOriginalValue("OrderID", _orderID);
    				if (!IsDeserializing)
    				{
    					if (Order != null && Order.OrderID != value)
    					{
    						Order = null;
    					}
    				}
    				_orderID = value;
    				OrderIDChanged();
    				OnPropertyChanged("OrderID");
    			}
    		}
    	}
    	private int _orderID;
    	partial void OrderCustomerIDChanged();
    	public Nullable<int> OrderCustomerID
    	{
    		get { return _orderCustomerID; }
    		set
    		{
    			if (_orderCustomerID != value)
    			{
    				ChangeTracker.RecordOriginalValue("OrderCustomerID", _orderCustomerID);
    				if (!IsDeserializing)
    				{
    					if (OrderCustomer != null && OrderCustomer.OrderCustomerID != value)
    					{
    						OrderCustomer = null;
    					}
    				}
    				_orderCustomerID = value;
    				OrderCustomerIDChanged();
    				OnPropertyChanged("OrderCustomerID");
    			}
    		}
    	}
    	private Nullable<int> _orderCustomerID;
    	partial void CurrencyIDChanged();
    	public int CurrencyID
    	{
    		get { return _currencyID; }
    		set
    		{
    			if (_currencyID != value)
    			{
    				ChangeTracker.RecordOriginalValue("CurrencyID", _currencyID);
    				if (!IsDeserializing)
    				{
    					if (Currency != null && Currency.CurrencyID != value)
    					{
    						Currency = null;
    					}
    				}
    				_currencyID = value;
    				CurrencyIDChanged();
    				OnPropertyChanged("CurrencyID");
    			}
    		}
    	}
    	private int _currencyID;
    	partial void ModifiedByUserIDChanged();
    	public Nullable<int> ModifiedByUserID
    	{
    		get { return _modifiedByUserID; }
    		set
    		{
    			if (_modifiedByUserID != value)
    			{
    				ChangeTracker.RecordOriginalValue("ModifiedByUserID", _modifiedByUserID);
    				if (!IsDeserializing)
    				{
    					if (User != null && User.UserID != value)
    					{
    						User = null;
    					}
    				}
    				_modifiedByUserID = value;
    				ModifiedByUserIDChanged();
    				OnPropertyChanged("ModifiedByUserID");
    			}
    		}
    	}
    	private Nullable<int> _modifiedByUserID;
    	partial void DateAuthorizedUTCChanged();
    	public Nullable<System.DateTime> DateAuthorizedUTC
    	{
    		get { return _dateAuthorizedUTC; }
    		set
    		{
    			if (_dateAuthorizedUTC != value)
    			{
    				ChangeTracker.RecordOriginalValue("DateAuthorizedUTC", _dateAuthorizedUTC);
    				_dateAuthorizedUTC = value;
    				DateAuthorizedUTCChanged();
    				OnPropertyChanged("DateAuthorizedUTC");
    			}
    		}
    	}
    	private Nullable<System.DateTime> _dateAuthorizedUTC;
    	partial void AuthorizeTypeChanged();
    	public string AuthorizeType
    	{
    		get { return _authorizeType; }
    		set
    		{
    			if (_authorizeType != value)
    			{
    				ChangeTracker.RecordOriginalValue("AuthorizeType", _authorizeType);
    				_authorizeType = value;
    				AuthorizeTypeChanged();
    				OnPropertyChanged("AuthorizeType");
    			}
    		}
    	}
    	private string _authorizeType;
    	partial void RoutingNumberChanged();
    	public string RoutingNumber
    	{
    		get { return _routingNumber; }
    		set
    		{
    			if (_routingNumber != value)
    			{
    				ChangeTracker.RecordOriginalValue("RoutingNumber", _routingNumber);
    				_routingNumber = value;
    				RoutingNumberChanged();
    				OnPropertyChanged("RoutingNumber");
    			}
    		}
    	}
    	private string _routingNumber;
    	partial void AccountNumberChanged();
    	public string AccountNumber
    	{
    		get { return _accountNumber; }
    		set
    		{
    			if (_accountNumber != value)
    			{
    				ChangeTracker.RecordOriginalValue("AccountNumber", _accountNumber);
    				_accountNumber = value;
    				AccountNumberChanged();
    				OnPropertyChanged("AccountNumber");
    			}
    		}
    	}
    	private string _accountNumber;
    	partial void BankNameChanged();
    	public string BankName
    	{
    		get { return _bankName; }
    		set
    		{
    			if (_bankName != value)
    			{
    				ChangeTracker.RecordOriginalValue("BankName", _bankName);
    				_bankName = value;
    				BankNameChanged();
    				OnPropertyChanged("BankName");
    			}
    		}
    	}
    	private string _bankName;
    	partial void ExpirationDateUTCChanged();
    	public Nullable<System.DateTime> ExpirationDateUTC
    	{
    		get { return _expirationDateUTC; }
    		set
    		{
    			if (_expirationDateUTC != value)
    			{
    				ChangeTracker.RecordOriginalValue("ExpirationDateUTC", _expirationDateUTC);
    				_expirationDateUTC = value;
    				ExpirationDateUTCChanged();
    				OnPropertyChanged("ExpirationDateUTC");
    			}
    		}
    	}
    	private Nullable<System.DateTime> _expirationDateUTC;
    	partial void AmountChanged();
    	public Nullable<decimal> Amount
    	{
    		get { return _amount; }
    		set
    		{
    			if (_amount != value)
    			{
    				ChangeTracker.RecordOriginalValue("Amount", _amount);
    				_amount = value;
    				AmountChanged();
    				OnPropertyChanged("Amount");
    			}
    		}
    	}
    	private Nullable<decimal> _amount;
    	partial void ErrorLevelChanged();
    	public Nullable<int> ErrorLevel
    	{
    		get { return _errorLevel; }
    		set
    		{
    			if (_errorLevel != value)
    			{
    				ChangeTracker.RecordOriginalValue("ErrorLevel", _errorLevel);
    				_errorLevel = value;
    				ErrorLevelChanged();
    				OnPropertyChanged("ErrorLevel");
    			}
    		}
    	}
    	private Nullable<int> _errorLevel;
    	partial void ErrorMessageChanged();
    	public string ErrorMessage
    	{
    		get { return _errorMessage; }
    		set
    		{
    			if (_errorMessage != value)
    			{
    				ChangeTracker.RecordOriginalValue("ErrorMessage", _errorMessage);
    				_errorMessage = value;
    				ErrorMessageChanged();
    				OnPropertyChanged("ErrorMessage");
    			}
    		}
    	}
    	private string _errorMessage;
    	partial void ResponseCodeChanged();
    	public string ResponseCode
    	{
    		get { return _responseCode; }
    		set
    		{
    			if (_responseCode != value)
    			{
    				ChangeTracker.RecordOriginalValue("ResponseCode", _responseCode);
    				_responseCode = value;
    				ResponseCodeChanged();
    				OnPropertyChanged("ResponseCode");
    			}
    		}
    	}
    	private string _responseCode;
    	partial void ResponseSubCodeChanged();
    	public string ResponseSubCode
    	{
    		get { return _responseSubCode; }
    		set
    		{
    			if (_responseSubCode != value)
    			{
    				ChangeTracker.RecordOriginalValue("ResponseSubCode", _responseSubCode);
    				_responseSubCode = value;
    				ResponseSubCodeChanged();
    				OnPropertyChanged("ResponseSubCode");
    			}
    		}
    	}
    	private string _responseSubCode;
    	partial void ResponseReasonCodeChanged();
    	public string ResponseReasonCode
    	{
    		get { return _responseReasonCode; }
    		set
    		{
    			if (_responseReasonCode != value)
    			{
    				ChangeTracker.RecordOriginalValue("ResponseReasonCode", _responseReasonCode);
    				_responseReasonCode = value;
    				ResponseReasonCodeChanged();
    				OnPropertyChanged("ResponseReasonCode");
    			}
    		}
    	}
    	private string _responseReasonCode;
    	partial void ResponseReasonTextChanged();
    	public string ResponseReasonText
    	{
    		get { return _responseReasonText; }
    		set
    		{
    			if (_responseReasonText != value)
    			{
    				ChangeTracker.RecordOriginalValue("ResponseReasonText", _responseReasonText);
    				_responseReasonText = value;
    				ResponseReasonTextChanged();
    				OnPropertyChanged("ResponseReasonText");
    			}
    		}
    	}
    	private string _responseReasonText;
    	partial void AVSResultChanged();
    	public string AVSResult
    	{
    		get { return _aVSResult; }
    		set
    		{
    			if (_aVSResult != value)
    			{
    				ChangeTracker.RecordOriginalValue("AVSResult", _aVSResult);
    				_aVSResult = value;
    				AVSResultChanged();
    				OnPropertyChanged("AVSResult");
    			}
    		}
    	}
    	private string _aVSResult;
    	partial void CardCodeResponseChanged();
    	public string CardCodeResponse
    	{
    		get { return _cardCodeResponse; }
    		set
    		{
    			if (_cardCodeResponse != value)
    			{
    				ChangeTracker.RecordOriginalValue("CardCodeResponse", _cardCodeResponse);
    				_cardCodeResponse = value;
    				CardCodeResponseChanged();
    				OnPropertyChanged("CardCodeResponse");
    			}
    		}
    	}
    	private string _cardCodeResponse;
    	partial void ApprovalCodeChanged();
    	public string ApprovalCode
    	{
    		get { return _approvalCode; }
    		set
    		{
    			if (_approvalCode != value)
    			{
    				ChangeTracker.RecordOriginalValue("ApprovalCode", _approvalCode);
    				_approvalCode = value;
    				ApprovalCodeChanged();
    				OnPropertyChanged("ApprovalCode");
    			}
    		}
    	}
    	private string _approvalCode;
    	partial void ResponseChanged();
    	public string Response
    	{
    		get { return _response; }
    		set
    		{
    			if (_response != value)
    			{
    				ChangeTracker.RecordOriginalValue("Response", _response);
    				_response = value;
    				ResponseChanged();
    				OnPropertyChanged("Response");
    			}
    		}
    	}
    	private string _response;
    	partial void TransactionIDChanged();
    	public string TransactionID
    	{
    		get { return _transactionID; }
    		set
    		{
    			if (_transactionID != value)
    			{
    				ChangeTracker.RecordOriginalValue("TransactionID", _transactionID);
    				_transactionID = value;
    				TransactionIDChanged();
    				OnPropertyChanged("TransactionID");
    			}
    		}
    	}
    	private string _transactionID;
    	partial void IsTestingChanged();
    	public Nullable<bool> IsTesting
    	{
    		get { return _isTesting; }
    		set
    		{
    			if (_isTesting != value)
    			{
    				ChangeTracker.RecordOriginalValue("IsTesting", _isTesting);
    				_isTesting = value;
    				IsTestingChanged();
    				OnPropertyChanged("IsTesting");
    			}
    		}
    	}
    	private Nullable<bool> _isTesting;
    	partial void PaymentGatewayIDChanged();
    	public Nullable<short> PaymentGatewayID
    	{
    		get { return _paymentGatewayID; }
    		set
    		{
    			if (_paymentGatewayID != value)
    			{
    				ChangeTracker.RecordOriginalValue("PaymentGatewayID", _paymentGatewayID);
    				if (!IsDeserializing)
    				{
    					if (PaymentGateway != null && PaymentGateway.PaymentGatewayID != value)
    					{
    						PaymentGateway = null;
    					}
    				}
    				_paymentGatewayID = value;
    				PaymentGatewayIDChanged();
    				OnPropertyChanged("PaymentGatewayID");
    			}
    		}
    	}
    	private Nullable<short> _paymentGatewayID;
    	partial void BalanceOnCardChanged();
    	public Nullable<decimal> BalanceOnCard
    	{
    		get { return _balanceOnCard; }
    		set
    		{
    			if (_balanceOnCard != value)
    			{
    				ChangeTracker.RecordOriginalValue("BalanceOnCard", _balanceOnCard);
    				_balanceOnCard = value;
    				BalanceOnCardChanged();
    				OnPropertyChanged("BalanceOnCard");
    			}
    		}
    	}
    	private Nullable<decimal> _balanceOnCard;
    	partial void DateCreatedUTCChanged();
    	public System.DateTime DateCreatedUTC
    	{
    		get { return _dateCreatedUTC; }
    		set
    		{
    			if (_dateCreatedUTC != value)
    			{
    				ChangeTracker.RecordOriginalValue("DateCreatedUTC", _dateCreatedUTC);
    				_dateCreatedUTC = value;
    				DateCreatedUTCChanged();
    				OnPropertyChanged("DateCreatedUTC");
    			}
    		}
    	}
    	private System.DateTime _dateCreatedUTC;

        #endregion
        #region Navigation Properties
    
    	public Currency Currency
    	{
    		get { return _currency; }
    		set
    		{
    			if (!ReferenceEquals(_currency, value))
    			{
    				var previousValue = _currency;
    				_currency = value;
    				FixupCurrency(previousValue);
    				OnNavigationPropertyChanged("Currency");
    			}
    		}
    	}
    	private Currency _currency;
    
    	public OrderCustomer OrderCustomer
    	{
    		get { return _orderCustomer; }
    		set
    		{
    			if (!ReferenceEquals(_orderCustomer, value))
    			{
    				var previousValue = _orderCustomer;
    				_orderCustomer = value;
    				FixupOrderCustomer(previousValue);
    				OnNavigationPropertyChanged("OrderCustomer");
    			}
    		}
    	}
    	private OrderCustomer _orderCustomer;
    
    	public OrderPayment OrderPayment
    	{
    		get { return _orderPayment; }
    		set
    		{
    			if (!ReferenceEquals(_orderPayment, value))
    			{
    				var previousValue = _orderPayment;
    				_orderPayment = value;
    				FixupOrderPayment(previousValue);
    				OnNavigationPropertyChanged("OrderPayment");
    			}
    		}
    	}
    	private OrderPayment _orderPayment;
    
    	public Order Order
    	{
    		get { return _order; }
    		set
    		{
    			if (!ReferenceEquals(_order, value))
    			{
    				var previousValue = _order;
    				_order = value;
    				FixupOrder(previousValue);
    				OnNavigationPropertyChanged("Order");
    			}
    		}
    	}
    	private Order _order;
    
    	public User User
    	{
    		get { return _user; }
    		set
    		{
    			if (!ReferenceEquals(_user, value))
    			{
    				var previousValue = _user;
    				_user = value;
    				FixupUser(previousValue);
    				OnNavigationPropertyChanged("User");
    			}
    		}
    	}
    	private User _user;
    
    	public PaymentGateway PaymentGateway
    	{
    		get { return _paymentGateway; }
    		set
    		{
    			if (!ReferenceEquals(_paymentGateway, value))
    			{
    				var previousValue = _paymentGateway;
    				_paymentGateway = value;
    				FixupPaymentGateway(previousValue);
    				OnNavigationPropertyChanged("PaymentGateway");
    			}
    		}
    	}
    	private PaymentGateway _paymentGateway;

        #endregion
        #region ChangeTracking
    
    	protected virtual void OnPropertyChanged(String propertyName)
    	{
    		if (ChangeTracker.State != ObjectState.Added && ChangeTracker.State != ObjectState.Deleted)
    		{
    			ChangeTracker.State = ObjectState.Modified;
    		}
    		if (_propertyChanged != null)
    		{
    			_propertyChanged(this, new PropertyChangedEventArgs(propertyName));
    		}
    	}
    
    	protected virtual void OnNavigationPropertyChanged(String propertyName)
    	{
    		if (_propertyChanged != null)
    		{
    			_propertyChanged(this, new PropertyChangedEventArgs(propertyName));
    		}
    	}
    
    	event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged{ add { _propertyChanged += value; } remove { _propertyChanged -= value; } }
    	private event PropertyChangedEventHandler _propertyChanged;
    	private ObjectChangeTracker _changeTracker;
    
    	public ObjectChangeTracker ChangeTracker
    	{
    		get
    		{
    			if (_changeTracker == null)
    			{
    				_changeTracker = new ObjectChangeTracker();
    				_changeTracker.ObjectStateChanging += HandleObjectStateChanging;
    			}
    			return _changeTracker;
    		}
    		set
    		{
    			if(_changeTracker != null)
    			{
    				_changeTracker.ObjectStateChanging -= HandleObjectStateChanging;
    			}
    			_changeTracker = value;
    			if(_changeTracker != null)
    			{
    				_changeTracker.ObjectStateChanging += HandleObjectStateChanging;
    			}
    		}
    	}
    
    	private void HandleObjectStateChanging(object sender, ObjectStateChangingEventArgs e)
    	{
    		if (e.NewState == ObjectState.Deleted)
    		{
    			ClearNavigationProperties();
    		}
    	}
    
    	protected bool IsDeserializing { get; private set; }
    
    	[OnDeserializing]
    	public void OnDeserializingMethod(StreamingContext context)
    	{
    		IsDeserializing = true;
    	}
    
    	[OnDeserialized]
    	public void OnDeserializedMethod(StreamingContext context)
    	{
    		IsDeserializing = false;
    		ChangeTracker.ChangeTrackingEnabled = true;
    	}
    
    
    	protected virtual void ClearNavigationProperties()
    	{
    		Currency = null;
    		OrderCustomer = null;
    		OrderPayment = null;
    		Order = null;
    		User = null;
    		PaymentGateway = null;
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupCurrency(Currency previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.OrderPaymentResults.Contains(this))
    		{
    			previousValue.OrderPaymentResults.Remove(this);
    		}
    
    		if (Currency != null)
    		{
    			if (!Currency.OrderPaymentResults.Contains(this))
    			{
    				Currency.OrderPaymentResults.Add(this);
    			}
    
    			CurrencyID = Currency.CurrencyID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("Currency")
    				&& (ChangeTracker.OriginalValues["Currency"] == Currency))
    			{
    				ChangeTracker.OriginalValues.Remove("Currency");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("Currency", previousValue);
    			}
    			if (Currency != null && !Currency.ChangeTracker.ChangeTrackingEnabled)
    			{
    				Currency.StartTracking();
    			}
    		}
    	}
    
    	private void FixupOrderCustomer(OrderCustomer previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.OrderPaymentResults.Contains(this))
    		{
    			previousValue.OrderPaymentResults.Remove(this);
    		}
    
    		if (OrderCustomer != null)
    		{
    			if (!OrderCustomer.OrderPaymentResults.Contains(this))
    			{
    				OrderCustomer.OrderPaymentResults.Add(this);
    			}
    
    			OrderCustomerID = OrderCustomer.OrderCustomerID;
    		}
    		else if (!skipKeys)
    		{
    			OrderCustomerID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("OrderCustomer")
    				&& (ChangeTracker.OriginalValues["OrderCustomer"] == OrderCustomer))
    			{
    				ChangeTracker.OriginalValues.Remove("OrderCustomer");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("OrderCustomer", previousValue);
    			}
    			if (OrderCustomer != null && !OrderCustomer.ChangeTracker.ChangeTrackingEnabled)
    			{
    				OrderCustomer.StartTracking();
    			}
    		}
    	}
    
    	private void FixupOrderPayment(OrderPayment previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.OrderPaymentResults.Contains(this))
    		{
    			previousValue.OrderPaymentResults.Remove(this);
    		}
    
    		if (OrderPayment != null)
    		{
    			if (!OrderPayment.OrderPaymentResults.Contains(this))
    			{
    				OrderPayment.OrderPaymentResults.Add(this);
    			}
    
    			OrderPaymentID = OrderPayment.OrderPaymentID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("OrderPayment")
    				&& (ChangeTracker.OriginalValues["OrderPayment"] == OrderPayment))
    			{
    				ChangeTracker.OriginalValues.Remove("OrderPayment");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("OrderPayment", previousValue);
    			}
    			if (OrderPayment != null && !OrderPayment.ChangeTracker.ChangeTrackingEnabled)
    			{
    				OrderPayment.StartTracking();
    			}
    		}
    	}
    
    	private void FixupOrder(Order previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.OrderPaymentResults.Contains(this))
    		{
    			previousValue.OrderPaymentResults.Remove(this);
    		}
    
    		if (Order != null)
    		{
    			if (!Order.OrderPaymentResults.Contains(this))
    			{
    				Order.OrderPaymentResults.Add(this);
    			}
    
    			OrderID = Order.OrderID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("Order")
    				&& (ChangeTracker.OriginalValues["Order"] == Order))
    			{
    				ChangeTracker.OriginalValues.Remove("Order");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("Order", previousValue);
    			}
    			if (Order != null && !Order.ChangeTracker.ChangeTrackingEnabled)
    			{
    				Order.StartTracking();
    			}
    		}
    	}
    
    	private void FixupUser(User previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.OrderPaymentResults.Contains(this))
    		{
    			previousValue.OrderPaymentResults.Remove(this);
    		}
    
    		if (User != null)
    		{
    			if (!User.OrderPaymentResults.Contains(this))
    			{
    				User.OrderPaymentResults.Add(this);
    			}
    
    			ModifiedByUserID = User.UserID;
    		}
    		else if (!skipKeys)
    		{
    			ModifiedByUserID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("User")
    				&& (ChangeTracker.OriginalValues["User"] == User))
    			{
    				ChangeTracker.OriginalValues.Remove("User");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("User", previousValue);
    			}
    			if (User != null && !User.ChangeTracker.ChangeTrackingEnabled)
    			{
    				User.StartTracking();
    			}
    		}
    	}
    
    	private void FixupPaymentGateway(PaymentGateway previousValue, bool skipKeys = false)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.OrderPaymentResults.Contains(this))
    		{
    			previousValue.OrderPaymentResults.Remove(this);
    		}
    
    		if (PaymentGateway != null)
    		{
    			if (!PaymentGateway.OrderPaymentResults.Contains(this))
    			{
    				PaymentGateway.OrderPaymentResults.Add(this);
    			}
    
    			PaymentGatewayID = PaymentGateway.PaymentGatewayID;
    		}
    		else if (!skipKeys)
    		{
    			PaymentGatewayID = null;
    		}
    
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("PaymentGateway")
    				&& (ChangeTracker.OriginalValues["PaymentGateway"] == PaymentGateway))
    			{
    				ChangeTracker.OriginalValues.Remove("PaymentGateway");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("PaymentGateway", previousValue);
    			}
    			if (PaymentGateway != null && !PaymentGateway.ChangeTracker.ChangeTrackingEnabled)
    			{
    				PaymentGateway.StartTracking();
    			}
    		}
    	}

        #endregion
    }
}