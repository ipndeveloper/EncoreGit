using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NetSteps.Content.Common.Models;

namespace NetSteps.Products.Common.Models
{
	/// <summary>
	/// Common interface for Brand.
	/// </summary>
	[ContractClass(typeof(Contracts.BrandContracts))]
	public interface IBrand
	{
	    #region Primitive properties
	
		/// <summary>
		/// The BrandID for this Brand.
		/// </summary>
		int BrandID { get; set; }
	
		/// <summary>
		/// The BrandNumber for this Brand.
		/// </summary>
		string BrandNumber { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The DescriptionTranslations for this Brand.
		/// </summary>
		IEnumerable<IDescriptionTranslation> DescriptionTranslations { get; }
	
		/// <summary>
		/// Adds an <see cref="IDescriptionTranslation"/> to the DescriptionTranslations collection.
		/// </summary>
		/// <param name="item">The <see cref="IDescriptionTranslation"/> to add.</param>
		void AddDescriptionTranslation(IDescriptionTranslation item);
	
		/// <summary>
		/// Removes an <see cref="IDescriptionTranslation"/> from the DescriptionTranslations collection.
		/// </summary>
		/// <param name="item">The <see cref="IDescriptionTranslation"/> to remove.</param>
		void RemoveDescriptionTranslation(IDescriptionTranslation item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IBrand))]
		internal abstract class BrandContracts : IBrand
		{
		    #region Primitive properties
		
			int IBrand.BrandID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IBrand.BrandNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IDescriptionTranslation> IBrand.DescriptionTranslations
			{
				get { throw new NotImplementedException(); }
			}
		
			void IBrand.AddDescriptionTranslation(IDescriptionTranslation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IBrand.RemoveDescriptionTranslation(IDescriptionTranslation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
