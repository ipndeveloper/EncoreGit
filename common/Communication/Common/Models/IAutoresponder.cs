using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for Autoresponder.
	/// </summary>
	[ContractClass(typeof(Contracts.AutoresponderContracts))]
	public interface IAutoresponder
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AutoresponderID for this Autoresponder.
		/// </summary>
		int AutoresponderID { get; set; }
	
		/// <summary>
		/// The AutoresponderTypeID for this Autoresponder.
		/// </summary>
		short AutoresponderTypeID { get; set; }
	
		/// <summary>
		/// The Name for this Autoresponder.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The LinkUrl for this Autoresponder.
		/// </summary>
		string LinkUrl { get; set; }
	
		/// <summary>
		/// The IsInternal for this Autoresponder.
		/// </summary>
		bool IsInternal { get; set; }
	
		/// <summary>
		/// The IsExternal for this Autoresponder.
		/// </summary>
		bool IsExternal { get; set; }
	
		/// <summary>
		/// The Active for this Autoresponder.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The AutoresponderType for this Autoresponder.
		/// </summary>
	    IAutoresponderType AutoresponderType { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The AutoresponderMessages for this Autoresponder.
		/// </summary>
		IEnumerable<IAutoresponderMessage> AutoresponderMessages { get; }
	
		/// <summary>
		/// Adds an <see cref="IAutoresponderMessage"/> to the AutoresponderMessages collection.
		/// </summary>
		/// <param name="item">The <see cref="IAutoresponderMessage"/> to add.</param>
		void AddAutoresponderMessage(IAutoresponderMessage item);
	
		/// <summary>
		/// Removes an <see cref="IAutoresponderMessage"/> from the AutoresponderMessages collection.
		/// </summary>
		/// <param name="item">The <see cref="IAutoresponderMessage"/> to remove.</param>
		void RemoveAutoresponderMessage(IAutoresponderMessage item);
	
		/// <summary>
		/// The Translations for this Autoresponder.
		/// </summary>
		IEnumerable<IAutoresponderTranslation> Translations { get; }
	
		/// <summary>
		/// Adds an <see cref="IAutoresponderTranslation"/> to the Translations collection.
		/// </summary>
		/// <param name="item">The <see cref="IAutoresponderTranslation"/> to add.</param>
		void AddTranslation(IAutoresponderTranslation item);
	
		/// <summary>
		/// Removes an <see cref="IAutoresponderTranslation"/> from the Translations collection.
		/// </summary>
		/// <param name="item">The <see cref="IAutoresponderTranslation"/> to remove.</param>
		void RemoveTranslation(IAutoresponderTranslation item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAutoresponder))]
		internal abstract class AutoresponderContracts : IAutoresponder
		{
		    #region Primitive properties
		
			int IAutoresponder.AutoresponderID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IAutoresponder.AutoresponderTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAutoresponder.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAutoresponder.LinkUrl
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAutoresponder.IsInternal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAutoresponder.IsExternal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAutoresponder.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IAutoresponderType IAutoresponder.AutoresponderType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAutoresponderMessage> IAutoresponder.AutoresponderMessages
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAutoresponder.AddAutoresponderMessage(IAutoresponderMessage item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAutoresponder.RemoveAutoresponderMessage(IAutoresponderMessage item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IAutoresponderTranslation> IAutoresponder.Translations
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAutoresponder.AddTranslation(IAutoresponderTranslation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAutoresponder.RemoveTranslation(IAutoresponderTranslation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
