using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NetSteps.Content.Common.Models;

namespace NetSteps.Logistics.Common.Models
{
	/// <summary>
	/// Common interface for ShippingMethod.
	/// </summary>
	[ContractClass(typeof(Contracts.ShippingMethodContracts))]
	public interface IShippingMethod
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ShippingMethodID for this ShippingMethod.
		/// </summary>
		int ShippingMethodID { get; set; }
	
		/// <summary>
		/// The Name for this ShippingMethod.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The ShortName for this ShippingMethod.
		/// </summary>
		string ShortName { get; set; }
	
		/// <summary>
		/// The Active for this ShippingMethod.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The IsWillCall for this ShippingMethod.
		/// </summary>
		Nullable<bool> IsWillCall { get; set; }
	
		/// <summary>
		/// The SortIndex for this ShippingMethod.
		/// </summary>
		Nullable<byte> SortIndex { get; set; }
	
		/// <summary>
		/// The TrackingNumberBaseUrl for this ShippingMethod.
		/// </summary>
		string TrackingNumberBaseUrl { get; set; }
	
		/// <summary>
		/// The AllowPoBox for this ShippingMethod.
		/// </summary>
		bool AllowPoBox { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The Translations for this ShippingMethod.
		/// </summary>
		IEnumerable<IDescriptionTranslation> Translations { get; }
	
		/// <summary>
		/// Adds an <see cref="IDescriptionTranslation"/> to the Translations collection.
		/// </summary>
		/// <param name="item">The <see cref="IDescriptionTranslation"/> to add.</param>
		void AddTranslation(IDescriptionTranslation item);
	
		/// <summary>
		/// Removes an <see cref="IDescriptionTranslation"/> from the Translations collection.
		/// </summary>
		/// <param name="item">The <see cref="IDescriptionTranslation"/> to remove.</param>
		void RemoveTranslation(IDescriptionTranslation item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IShippingMethod))]
		internal abstract class ShippingMethodContracts : IShippingMethod
		{
		    #region Primitive properties
		
			int IShippingMethod.ShippingMethodID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IShippingMethod.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IShippingMethod.ShortName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IShippingMethod.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<bool> IShippingMethod.IsWillCall
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<byte> IShippingMethod.SortIndex
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IShippingMethod.TrackingNumberBaseUrl
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IShippingMethod.AllowPoBox
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IDescriptionTranslation> IShippingMethod.Translations
			{
				get { throw new NotImplementedException(); }
			}
		
			void IShippingMethod.AddTranslation(IDescriptionTranslation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IShippingMethod.RemoveTranslation(IDescriptionTranslation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
