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
    [Serializable]
    public partial class MailDomain: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void MailDomainIDChanged();
    	public int MailDomainID
    	{
    		get { return _mailDomainID; }
    		set
    		{
    			if (_mailDomainID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'MailDomainID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_mailDomainID = value;
    				MailDomainIDChanged();
    				OnPropertyChanged("MailDomainID");
    			}
    		}
    	}
    	private int _mailDomainID;
    	partial void DomainNameChanged();
    	public string DomainName
    	{
    		get { return _domainName; }
    		set
    		{
    			if (_domainName != value)
    			{
    				ChangeTracker.RecordOriginalValue("DomainName", _domainName);
    				_domainName = value;
    				DomainNameChanged();
    				OnPropertyChanged("DomainName");
    			}
    		}
    	}
    	private string _domainName;
    	partial void ServiceUriChanged();
    	public string ServiceUri
    	{
    		get { return _serviceUri; }
    		set
    		{
    			if (_serviceUri != value)
    			{
    				ChangeTracker.RecordOriginalValue("ServiceUri", _serviceUri);
    				_serviceUri = value;
    				ServiceUriChanged();
    				OnPropertyChanged("ServiceUri");
    			}
    		}
    	}
    	private string _serviceUri;
    	partial void UserNameChanged();
    	public string UserName
    	{
    		get { return _userName; }
    		set
    		{
    			if (_userName != value)
    			{
    				ChangeTracker.RecordOriginalValue("UserName", _userName);
    				_userName = value;
    				UserNameChanged();
    				OnPropertyChanged("UserName");
    			}
    		}
    	}
    	private string _userName;
    	partial void PasswordChanged();
    	public string Password
    	{
    		get { return _password; }
    		set
    		{
    			if (_password != value)
    			{
    				ChangeTracker.RecordOriginalValue("Password", _password);
    				_password = value;
    				PasswordChanged();
    				OnPropertyChanged("Password");
    			}
    		}
    	}
    	private string _password;
    	partial void ServerChanged();
    	public string Server
    	{
    		get { return _server; }
    		set
    		{
    			if (_server != value)
    			{
    				ChangeTracker.RecordOriginalValue("Server", _server);
    				_server = value;
    				ServerChanged();
    				OnPropertyChanged("Server");
    			}
    		}
    	}
    	private string _server;
    	partial void PortChanged();
    	public int Port
    	{
    		get { return _port; }
    		set
    		{
    			if (_port != value)
    			{
    				ChangeTracker.RecordOriginalValue("Port", _port);
    				_port = value;
    				PortChanged();
    				OnPropertyChanged("Port");
    			}
    		}
    	}
    	private int _port;
    	partial void ServerUriChanged();
    	public string ServerUri
    	{
    		get { return _serverUri; }
    		set
    		{
    			if (_serverUri != value)
    			{
    				ChangeTracker.RecordOriginalValue("ServerUri", _serverUri);
    				_serverUri = value;
    				ServerUriChanged();
    				OnPropertyChanged("ServerUri");
    			}
    		}
    	}
    	private string _serverUri;
    	partial void IsDefaultForInternalMailAccountsChanged();
    	public bool IsDefaultForInternalMailAccounts
    	{
    		get { return _isDefaultForInternalMailAccounts; }
    		set
    		{
    			if (_isDefaultForInternalMailAccounts != value)
    			{
    				ChangeTracker.RecordOriginalValue("IsDefaultForInternalMailAccounts", _isDefaultForInternalMailAccounts);
    				_isDefaultForInternalMailAccounts = value;
    				IsDefaultForInternalMailAccountsChanged();
    				OnPropertyChanged("IsDefaultForInternalMailAccounts");
    			}
    		}
    	}
    	private bool _isDefaultForInternalMailAccounts;

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
    	}

        #endregion
    }
}
