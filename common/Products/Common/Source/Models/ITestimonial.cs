using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Products.Common.Models
{
	/// <summary>
	/// Common interface for Testimonial.
	/// </summary>
	[ContractClass(typeof(Contracts.TestimonialContracts))]
	public interface ITestimonial
	{
	    #region Primitive properties
	
		/// <summary>
		/// The TestimonialID for this Testimonial.
		/// </summary>
		int TestimonialID { get; set; }
	
		/// <summary>
		/// The HtmlContentID for this Testimonial.
		/// </summary>
		Nullable<int> HtmlContentID { get; set; }
	
		/// <summary>
		/// The QuotedBy for this Testimonial.
		/// </summary>
		string QuotedBy { get; set; }
	
		/// <summary>
		/// The DateQuotedUTC for this Testimonial.
		/// </summary>
		Nullable<System.DateTime> DateQuotedUTC { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The ProductBases for this Testimonial.
		/// </summary>
		IEnumerable<IProductBase> ProductBases { get; }
	
		/// <summary>
		/// Adds an <see cref="IProductBase"/> to the ProductBases collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductBase"/> to add.</param>
		void AddProductBas(IProductBase item);
	
		/// <summary>
		/// Removes an <see cref="IProductBase"/> from the ProductBases collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductBase"/> to remove.</param>
		void RemoveProductBas(IProductBase item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ITestimonial))]
		internal abstract class TestimonialContracts : ITestimonial
		{
		    #region Primitive properties
		
			int ITestimonial.TestimonialID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ITestimonial.HtmlContentID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ITestimonial.QuotedBy
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> ITestimonial.DateQuotedUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IProductBase> ITestimonial.ProductBases
			{
				get { throw new NotImplementedException(); }
			}
		
			void ITestimonial.AddProductBas(IProductBase item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ITestimonial.RemoveProductBas(IProductBase item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
