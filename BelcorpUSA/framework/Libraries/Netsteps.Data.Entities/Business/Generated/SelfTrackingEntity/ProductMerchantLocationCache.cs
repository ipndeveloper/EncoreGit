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
    public partial class ProductMerchantLocationCache: IObjectWithChangeTracker, INotifyPropertyChanged
    {
        #region Primitive Properties
    	partial void ProductMerchantLocationCacheIDChanged();
    	public int ProductMerchantLocationCacheID
    	{
    		get { return _productMerchantLocationCacheID; }
    		set
    		{
    			if (_productMerchantLocationCacheID != value)
    			{
    				if (ChangeTracker.ChangeTrackingEnabled && ChangeTracker.State != ObjectState.Added)
    				{
    					throw new InvalidOperationException("The property 'ProductMerchantLocationCacheID' is part of the object's key and cannot be changed. Changes to key properties can only be made when the object is not being tracked or is in the Added state.");
    				}
    				_productMerchantLocationCacheID = value;
    				ProductMerchantLocationCacheIDChanged();
    				OnPropertyChanged("ProductMerchantLocationCacheID");
    			}
    		}
    	}
    	private int _productMerchantLocationCacheID;
    	partial void ProductIDChanged();
    	public int ProductID
    	{
    		get { return _productID; }
    		set
    		{
    			if (_productID != value)
    			{
    				ChangeTracker.RecordOriginalValue("ProductID", _productID);
    				_productID = value;
    				ProductIDChanged();
    				OnPropertyChanged("ProductID");
    			}
    		}
    	}
    	private int _productID;
    	partial void CategoryIDChanged();
    	public int CategoryID
    	{
    		get { return _categoryID; }
    		set
    		{
    			if (_categoryID != value)
    			{
    				ChangeTracker.RecordOriginalValue("CategoryID", _categoryID);
    				_categoryID = value;
    				CategoryIDChanged();
    				OnPropertyChanged("CategoryID");
    			}
    		}
    	}
    	private int _categoryID;
    	partial void MerchantNameChanged();
    	public string MerchantName
    	{
    		get { return _merchantName; }
    		set
    		{
    			if (_merchantName != value)
    			{
    				ChangeTracker.RecordOriginalValue("MerchantName", _merchantName);
    				_merchantName = value;
    				MerchantNameChanged();
    				OnPropertyChanged("MerchantName");
    			}
    		}
    	}
    	private string _merchantName;
    	partial void LongProductDescriptionChanged();
    	public string LongProductDescription
    	{
    		get { return _longProductDescription; }
    		set
    		{
    			if (_longProductDescription != value)
    			{
    				ChangeTracker.RecordOriginalValue("LongProductDescription", _longProductDescription);
    				_longProductDescription = value;
    				LongProductDescriptionChanged();
    				OnPropertyChanged("LongProductDescription");
    			}
    		}
    	}
    	private string _longProductDescription;
    	partial void LogoFileNameChanged();
    	public string LogoFileName
    	{
    		get { return _logoFileName; }
    		set
    		{
    			if (_logoFileName != value)
    			{
    				ChangeTracker.RecordOriginalValue("LogoFileName", _logoFileName);
    				_logoFileName = value;
    				LogoFileNameChanged();
    				OnPropertyChanged("LogoFileName");
    			}
    		}
    	}
    	private string _logoFileName;
    	partial void AwardChanged();
    	public string Award
    	{
    		get { return _award; }
    		set
    		{
    			if (_award != value)
    			{
    				ChangeTracker.RecordOriginalValue("Award", _award);
    				_award = value;
    				AwardChanged();
    				OnPropertyChanged("Award");
    			}
    		}
    	}
    	private string _award;
    	partial void MinimumPurchaseChanged();
    	public string MinimumPurchase
    	{
    		get { return _minimumPurchase; }
    		set
    		{
    			if (_minimumPurchase != value)
    			{
    				ChangeTracker.RecordOriginalValue("MinimumPurchase", _minimumPurchase);
    				_minimumPurchase = value;
    				MinimumPurchaseChanged();
    				OnPropertyChanged("MinimumPurchase");
    			}
    		}
    	}
    	private string _minimumPurchase;
    	partial void MaximumAwardPPChanged();
    	public string MaximumAwardPP
    	{
    		get { return _maximumAwardPP; }
    		set
    		{
    			if (_maximumAwardPP != value)
    			{
    				ChangeTracker.RecordOriginalValue("MaximumAwardPP", _maximumAwardPP);
    				_maximumAwardPP = value;
    				MaximumAwardPPChanged();
    				OnPropertyChanged("MaximumAwardPP");
    			}
    		}
    	}
    	private string _maximumAwardPP;
    	partial void AwardRatingChanged();
    	public string AwardRating
    	{
    		get { return _awardRating; }
    		set
    		{
    			if (_awardRating != value)
    			{
    				ChangeTracker.RecordOriginalValue("AwardRating", _awardRating);
    				_awardRating = value;
    				AwardRatingChanged();
    				OnPropertyChanged("AwardRating");
    			}
    		}
    	}
    	private string _awardRating;
    	partial void ExpressionTypeChanged();
    	public string ExpressionType
    	{
    		get { return _expressionType; }
    		set
    		{
    			if (_expressionType != value)
    			{
    				ChangeTracker.RecordOriginalValue("ExpressionType", _expressionType);
    				_expressionType = value;
    				ExpressionTypeChanged();
    				OnPropertyChanged("ExpressionType");
    			}
    		}
    	}
    	private string _expressionType;
    	partial void KeywordsChanged();
    	public string Keywords
    	{
    		get { return _keywords; }
    		set
    		{
    			if (_keywords != value)
    			{
    				ChangeTracker.RecordOriginalValue("Keywords", _keywords);
    				_keywords = value;
    				KeywordsChanged();
    				OnPropertyChanged("Keywords");
    			}
    		}
    	}
    	private string _keywords;
    	partial void LatitudeChanged();
    	public Nullable<double> Latitude
    	{
    		get { return _latitude; }
    		set
    		{
    			if (_latitude != value)
    			{
    				ChangeTracker.RecordOriginalValue("Latitude", _latitude);
    				_latitude = value;
    				LatitudeChanged();
    				OnPropertyChanged("Latitude");
    			}
    		}
    	}
    	private Nullable<double> _latitude;
    	partial void LongitudeChanged();
    	public Nullable<double> Longitude
    	{
    		get { return _longitude; }
    		set
    		{
    			if (_longitude != value)
    			{
    				ChangeTracker.RecordOriginalValue("Longitude", _longitude);
    				_longitude = value;
    				LongitudeChanged();
    				OnPropertyChanged("Longitude");
    			}
    		}
    	}
    	private Nullable<double> _longitude;
    	partial void MerchantIDChanged();
    	public int MerchantID
    	{
    		get { return _merchantID; }
    		set
    		{
    			if (_merchantID != value)
    			{
    				ChangeTracker.RecordOriginalValue("MerchantID", _merchantID);
    				_merchantID = value;
    				MerchantIDChanged();
    				OnPropertyChanged("MerchantID");
    			}
    		}
    	}
    	private int _merchantID;
    	partial void AddressNumberChanged();
    	public string AddressNumber
    	{
    		get { return _addressNumber; }
    		set
    		{
    			if (_addressNumber != value)
    			{
    				ChangeTracker.RecordOriginalValue("AddressNumber", _addressNumber);
    				_addressNumber = value;
    				AddressNumberChanged();
    				OnPropertyChanged("AddressNumber");
    			}
    		}
    	}
    	private string _addressNumber;

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