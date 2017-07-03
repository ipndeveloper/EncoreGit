using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Content.Common.Models
{
	/// <summary>
	/// Common interface for NavigationTranslation.
	/// </summary>
	[ContractClass(typeof(Contracts.NavigationTranslationContracts))]
	public interface INavigationTranslation
	{
	    #region Primitive properties
	
		/// <summary>
		/// The NavigationTranslationID for this NavigationTranslation.
		/// </summary>
		int NavigationTranslationID { get; set; }
	
		/// <summary>
		/// The NavigationID for this NavigationTranslation.
		/// </summary>
		int NavigationID { get; set; }
	
		/// <summary>
		/// The LanguageID for this NavigationTranslation.
		/// </summary>
		int LanguageID { get; set; }
	
		/// <summary>
		/// The LinkText for this NavigationTranslation.
		/// </summary>
		string LinkText { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Navigation for this NavigationTranslation.
		/// </summary>
	    INavigation Navigation { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(INavigationTranslation))]
		internal abstract class NavigationTranslationContracts : INavigationTranslation
		{
		    #region Primitive properties
		
			int INavigationTranslation.NavigationTranslationID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int INavigationTranslation.NavigationID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int INavigationTranslation.LanguageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string INavigationTranslation.LinkText
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    INavigation INavigationTranslation.Navigation
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
