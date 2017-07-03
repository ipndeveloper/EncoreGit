using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for AutoresponderType.
	/// </summary>
	[ContractClass(typeof(Contracts.AutoresponderTypeContracts))]
	public interface IAutoresponderType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AutoresponderTypeID for this AutoresponderType.
		/// </summary>
		short AutoresponderTypeID { get; set; }
	
		/// <summary>
		/// The Name for this AutoresponderType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this AutoresponderType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this AutoresponderType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this AutoresponderType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The Autoresponders for this AutoresponderType.
		/// </summary>
		IEnumerable<IAutoresponder> Autoresponders { get; }
	
		/// <summary>
		/// Adds an <see cref="IAutoresponder"/> to the Autoresponders collection.
		/// </summary>
		/// <param name="item">The <see cref="IAutoresponder"/> to add.</param>
		void AddAutoresponder(IAutoresponder item);
	
		/// <summary>
		/// Removes an <see cref="IAutoresponder"/> from the Autoresponders collection.
		/// </summary>
		/// <param name="item">The <see cref="IAutoresponder"/> to remove.</param>
		void RemoveAutoresponder(IAutoresponder item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAutoresponderType))]
		internal abstract class AutoresponderTypeContracts : IAutoresponderType
		{
		    #region Primitive properties
		
			short IAutoresponderType.AutoresponderTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAutoresponderType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAutoresponderType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAutoresponderType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAutoresponderType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAutoresponder> IAutoresponderType.Autoresponders
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAutoresponderType.AddAutoresponder(IAutoresponder item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAutoresponderType.RemoveAutoresponder(IAutoresponder item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
