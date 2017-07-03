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
    [KnownType(typeof(OrderItem))]
    [Serializable]
    public partial class OrderItemMessage: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void OrderItemMessageIDChanged();
    	public int OrderItemMessageID
    	{
    		get { return _orderItemMessageID; }
    		set
    		{
    			if (_orderItemMessageID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'OrderItemMessageID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_orderItemMessageID = value;
    				OrderItemMessageIDChanged();
    				OnPropertyChanged("OrderItemMessageID");
    			}
    		}
    	}
    	private int _orderItemMessageID;
    	partial void OrderItemIDChanged();
    	public int OrderItemID
    	{
    		get { return _orderItemID; }
    		set
    		{
    			if (_orderItemID != value)
    			{
    				ChangeTracker.RecordOriginalValue("OrderItemID", _orderItemID);
    				if (!IsDeserializing)
    				{
    					if (OrderItem != null && OrderItem.OrderItemID != value)
    					{
    						OrderItem = null;
    					}
    				}
    				_orderItemID = value;
    				OrderItemIDChanged();
    				OnPropertyChanged("OrderItemID");
    			}
    		}
    	}
    	private int _orderItemID;
    	partial void OrderItemMessageDisplayKindIDChanged();
    	public int OrderItemMessageDisplayKindID
    	{
    		get { return _orderItemMessageDisplayKindID; }
    		set
    		{
    			if (_orderItemMessageDisplayKindID != value)
    			{
    				ChangeTracker.RecordOriginalValue("OrderItemMessageDisplayKindID", _orderItemMessageDisplayKindID);
    				_orderItemMessageDisplayKindID = value;
    				OrderItemMessageDisplayKindIDChanged();
    				OnPropertyChanged("OrderItemMessageDisplayKindID");
    			}
    		}
    	}
    	private int _orderItemMessageDisplayKindID;
    	partial void MessageSourceKeyChanged();
    	public string MessageSourceKey
    	{
    		get { return _messageSourceKey; }
    		set
    		{
    			if (_messageSourceKey != value)
    			{
    				ChangeTracker.RecordOriginalValue("MessageSourceKey", _messageSourceKey);
    				_messageSourceKey = value;
    				MessageSourceKeyChanged();
    				OnPropertyChanged("MessageSourceKey");
    			}
    		}
    	}
    	private string _messageSourceKey;
    	partial void MessageChanged();
    	public string Message
    	{
    		get { return _message; }
    		set
    		{
    			if (_message != value)
    			{
    				ChangeTracker.RecordOriginalValue("Message", _message);
    				_message = value;
    				MessageChanged();
    				OnPropertyChanged("Message");
    			}
    		}
    	}
    	private string _message;
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
    	partial void DateLastModifiedUTCChanged();
    	public System.DateTime DateLastModifiedUTC
    	{
    		get { return _dateLastModifiedUTC; }
    		set
    		{
    			if (_dateLastModifiedUTC != value)
    			{
    				ChangeTracker.RecordOriginalValue("DateLastModifiedUTC", _dateLastModifiedUTC);
    				_dateLastModifiedUTC = value;
    				DateLastModifiedUTCChanged();
    				OnPropertyChanged("DateLastModifiedUTC");
    			}
    		}
    	}
    	private System.DateTime _dateLastModifiedUTC;

        #endregion
        #region Navigation Properties
    
    	public OrderItem OrderItem
    	{
    		get { return _orderItem; }
    		set
    		{
    			if (!ReferenceEquals(_orderItem, value))
    			{
    				var previousValue = _orderItem;
    				_orderItem = value;
    				FixupOrderItem(previousValue);
    				OnNavigationPropertyChanged("OrderItem");
    			}
    		}
    	}
    	private OrderItem _orderItem;

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
    		OrderItem = null;
    	}

        #endregion
        #region Association Fixup
    
    	private void FixupOrderItem(OrderItem previousValue)
    	{
    		if (IsDeserializing)
    		{
    			return;
    		}
    
    		if (previousValue != null && previousValue.OrderItemMessages.Contains(this))
    		{
    			previousValue.OrderItemMessages.Remove(this);
    		}
    
    		if (OrderItem != null)
    		{
    			if (!OrderItem.OrderItemMessages.Contains(this))
    			{
    				OrderItem.OrderItemMessages.Add(this);
    			}
    
    			OrderItemID = OrderItem.OrderItemID;
    		}
    		if (ChangeTracker.ChangeTrackingEnabled)
    		{
    			if (ChangeTracker.OriginalValues.ContainsKey("OrderItem")
    				&& (ChangeTracker.OriginalValues["OrderItem"] == OrderItem))
    			{
    				ChangeTracker.OriginalValues.Remove("OrderItem");
    			}
    			else
    			{
    				ChangeTracker.RecordOriginalValue("OrderItem", previousValue);
    			}
    			if (OrderItem != null && !OrderItem.ChangeTracker.ChangeTrackingEnabled)
    			{
    				OrderItem.StartTracking();
    			}
    		}
    	}

        #endregion
    }
}