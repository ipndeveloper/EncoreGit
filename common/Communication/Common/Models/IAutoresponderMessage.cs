using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for AutoresponderMessage.
	/// </summary>
	[ContractClass(typeof(Contracts.AutoresponderMessageContracts))]
	public interface IAutoresponderMessage
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AutoresponderMessageID for this AutoresponderMessage.
		/// </summary>
		int AutoresponderMessageID { get; set; }
	
		/// <summary>
		/// The AutoresponderID for this AutoresponderMessage.
		/// </summary>
		int AutoresponderID { get; set; }
	
		/// <summary>
		/// The AccountID for this AutoresponderMessage.
		/// </summary>
		int AccountID { get; set; }
	
		/// <summary>
		/// The HasBeenRead for this AutoresponderMessage.
		/// </summary>
		bool HasBeenRead { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Autoresponder for this AutoresponderMessage.
		/// </summary>
	    IAutoresponder Autoresponder { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The AutoresponderMessageTokens for this AutoresponderMessage.
		/// </summary>
		IEnumerable<IAutoresponderMessageToken> AutoresponderMessageTokens { get; }
	
		/// <summary>
		/// Adds an <see cref="IAutoresponderMessageToken"/> to the AutoresponderMessageTokens collection.
		/// </summary>
		/// <param name="item">The <see cref="IAutoresponderMessageToken"/> to add.</param>
		void AddAutoresponderMessageToken(IAutoresponderMessageToken item);
	
		/// <summary>
		/// Removes an <see cref="IAutoresponderMessageToken"/> from the AutoresponderMessageTokens collection.
		/// </summary>
		/// <param name="item">The <see cref="IAutoresponderMessageToken"/> to remove.</param>
		void RemoveAutoresponderMessageToken(IAutoresponderMessageToken item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAutoresponderMessage))]
		internal abstract class AutoresponderMessageContracts : IAutoresponderMessage
		{
		    #region Primitive properties
		
			int IAutoresponderMessage.AutoresponderMessageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAutoresponderMessage.AutoresponderID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAutoresponderMessage.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAutoresponderMessage.HasBeenRead
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IAutoresponder IAutoresponderMessage.Autoresponder
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAutoresponderMessageToken> IAutoresponderMessage.AutoresponderMessageTokens
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAutoresponderMessage.AddAutoresponderMessageToken(IAutoresponderMessageToken item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAutoresponderMessage.RemoveAutoresponderMessageToken(IAutoresponderMessageToken item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
