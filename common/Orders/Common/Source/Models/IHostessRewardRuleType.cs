using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for HostessRewardRuleType.
	/// </summary>
	[ContractClass(typeof(Contracts.HostessRewardRuleTypeContracts))]
	public interface IHostessRewardRuleType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The HostessRewardRuleTypeID for this HostessRewardRuleType.
		/// </summary>
		int HostessRewardRuleTypeID { get; set; }
	
		/// <summary>
		/// The Name for this HostessRewardRuleType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The Description for this HostessRewardRuleType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The TermName for this HostessRewardRuleType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Active for this HostessRewardRuleType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The HostessRewardRules for this HostessRewardRuleType.
		/// </summary>
		IEnumerable<IHostessRewardRule> HostessRewardRules { get; }
	
		/// <summary>
		/// Adds an <see cref="IHostessRewardRule"/> to the HostessRewardRules collection.
		/// </summary>
		/// <param name="item">The <see cref="IHostessRewardRule"/> to add.</param>
		void AddHostessRewardRule(IHostessRewardRule item);
	
		/// <summary>
		/// Removes an <see cref="IHostessRewardRule"/> from the HostessRewardRules collection.
		/// </summary>
		/// <param name="item">The <see cref="IHostessRewardRule"/> to remove.</param>
		void RemoveHostessRewardRule(IHostessRewardRule item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IHostessRewardRuleType))]
		internal abstract class HostessRewardRuleTypeContracts : IHostessRewardRuleType
		{
		    #region Primitive properties
		
			int IHostessRewardRuleType.HostessRewardRuleTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHostessRewardRuleType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHostessRewardRuleType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHostessRewardRuleType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IHostessRewardRuleType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IHostessRewardRule> IHostessRewardRuleType.HostessRewardRules
			{
				get { throw new NotImplementedException(); }
			}
		
			void IHostessRewardRuleType.AddHostessRewardRule(IHostessRewardRule item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IHostessRewardRuleType.RemoveHostessRewardRule(IHostessRewardRule item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
