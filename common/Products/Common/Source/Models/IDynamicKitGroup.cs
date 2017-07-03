using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NetSteps.Content.Common.Models;

namespace NetSteps.Products.Common.Models
{
	/// <summary>
	/// Common interface for DynamicKitGroup.
	/// </summary>
	[ContractClass(typeof(Contracts.DynamicKitGroupContracts))]
	public interface IDynamicKitGroup
	{
	    #region Primitive properties
	
		/// <summary>
		/// The DynamicKitGroupID for this DynamicKitGroup.
		/// </summary>
		int DynamicKitGroupID { get; set; }
	
		/// <summary>
		/// The DynamicKitID for this DynamicKitGroup.
		/// </summary>
		int DynamicKitID { get; set; }
	
		/// <summary>
		/// The MinimumProductCount for this DynamicKitGroup.
		/// </summary>
		int MinimumProductCount { get; set; }
	
		/// <summary>
		/// The MaximumProductCount for this DynamicKitGroup.
		/// </summary>
		int MaximumProductCount { get; set; }
	
		/// <summary>
		/// The SortIndex for this DynamicKitGroup.
		/// </summary>
		int SortIndex { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The DynamicKitGroupRules for this DynamicKitGroup.
		/// </summary>
		IEnumerable<IDynamicKitGroupRule> DynamicKitGroupRules { get; }
	
		/// <summary>
		/// Adds an <see cref="IDynamicKitGroupRule"/> to the DynamicKitGroupRules collection.
		/// </summary>
		/// <param name="item">The <see cref="IDynamicKitGroupRule"/> to add.</param>
		void AddDynamicKitGroupRule(IDynamicKitGroupRule item);
	
		/// <summary>
		/// Removes an <see cref="IDynamicKitGroupRule"/> from the DynamicKitGroupRules collection.
		/// </summary>
		/// <param name="item">The <see cref="IDynamicKitGroupRule"/> to remove.</param>
		void RemoveDynamicKitGroupRule(IDynamicKitGroupRule item);
	
		/// <summary>
		/// The Translations for this DynamicKitGroup.
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
		[ContractClassFor(typeof(IDynamicKitGroup))]
		internal abstract class DynamicKitGroupContracts : IDynamicKitGroup
		{
		    #region Primitive properties
		
			int IDynamicKitGroup.DynamicKitGroupID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IDynamicKitGroup.DynamicKitID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IDynamicKitGroup.MinimumProductCount
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IDynamicKitGroup.MaximumProductCount
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IDynamicKitGroup.SortIndex
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IDynamicKitGroupRule> IDynamicKitGroup.DynamicKitGroupRules
			{
				get { throw new NotImplementedException(); }
			}
		
			void IDynamicKitGroup.AddDynamicKitGroupRule(IDynamicKitGroupRule item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IDynamicKitGroup.RemoveDynamicKitGroupRule(IDynamicKitGroupRule item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IDescriptionTranslation> IDynamicKitGroup.Translations
			{
				get { throw new NotImplementedException(); }
			}
		
			void IDynamicKitGroup.AddTranslation(IDescriptionTranslation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IDynamicKitGroup.RemoveTranslation(IDescriptionTranslation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
