using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for AlertPriority.
	/// </summary>
	[ContractClass(typeof(Contracts.AlertPriorityContracts))]
	public interface IAlertPriority
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AlertPriorityID for this AlertPriority.
		/// </summary>
		short AlertPriorityID { get; set; }
	
		/// <summary>
		/// The Name for this AlertPriority.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this AlertPriority.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this AlertPriority.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The SortIndex for this AlertPriority.
		/// </summary>
		short SortIndex { get; set; }
	
		/// <summary>
		/// The Active for this AlertPriority.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The AlertTemplates for this AlertPriority.
		/// </summary>
		IEnumerable<IAlertTemplate> AlertTemplates { get; }
	
		/// <summary>
		/// Adds an <see cref="IAlertTemplate"/> to the AlertTemplates collection.
		/// </summary>
		/// <param name="item">The <see cref="IAlertTemplate"/> to add.</param>
		void AddAlertTemplate(IAlertTemplate item);
	
		/// <summary>
		/// Removes an <see cref="IAlertTemplate"/> from the AlertTemplates collection.
		/// </summary>
		/// <param name="item">The <see cref="IAlertTemplate"/> to remove.</param>
		void RemoveAlertTemplate(IAlertTemplate item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAlertPriority))]
		internal abstract class AlertPriorityContracts : IAlertPriority
		{
		    #region Primitive properties
		
			short IAlertPriority.AlertPriorityID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAlertPriority.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAlertPriority.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAlertPriority.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IAlertPriority.SortIndex
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAlertPriority.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAlertTemplate> IAlertPriority.AlertTemplates
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAlertPriority.AddAlertTemplate(IAlertTemplate item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAlertPriority.RemoveAlertTemplate(IAlertTemplate item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
