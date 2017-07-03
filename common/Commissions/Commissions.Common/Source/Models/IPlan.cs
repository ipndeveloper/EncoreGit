using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for Plan.
	/// </summary>
	[ContractClass(typeof(Contracts.PlanContracts))]
	public interface IPlan
	{
	    #region Primitive properties
	
		/// <summary>
		/// The PlanID for this Plan.
		/// </summary>
		int PlanID { get; set; }
	
		/// <summary>
		/// The PlanCode for this Plan.
		/// </summary>
		string PlanCode { get; set; }
	
		/// <summary>
		/// The Enabled for this Plan.
		/// </summary>
		bool Enabled { get; set; }
	
		/// <summary>
		/// The DefaultPlan for this Plan.
		/// </summary>
		Nullable<bool> DefaultPlan { get; set; }
	
		/// <summary>
		/// The Name for this Plan.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this Plan.
		/// </summary>
		string TermName { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The BonusTypes for this Plan.
		/// </summary>
		IEnumerable<IBonusType> BonusTypes { get; }
	
		/// <summary>
		/// Adds an <see cref="IBonusType"/> to the BonusTypes collection.
		/// </summary>
		/// <param name="item">The <see cref="IBonusType"/> to add.</param>
		void AddBonusType(IBonusType item);
	
		/// <summary>
		/// Removes an <see cref="IBonusType"/> from the BonusTypes collection.
		/// </summary>
		/// <param name="item">The <see cref="IBonusType"/> to remove.</param>
		void RemoveBonusType(IBonusType item);
	
		/// <summary>
		/// The Periods for this Plan.
		/// </summary>
		IEnumerable<IPeriod> Periods { get; }
	
		/// <summary>
		/// Adds an <see cref="IPeriod"/> to the Periods collection.
		/// </summary>
		/// <param name="item">The <see cref="IPeriod"/> to add.</param>
		void AddPeriod(IPeriod item);
	
		/// <summary>
		/// Removes an <see cref="IPeriod"/> from the Periods collection.
		/// </summary>
		/// <param name="item">The <see cref="IPeriod"/> to remove.</param>
		void RemovePeriod(IPeriod item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IPlan))]
		internal abstract class PlanContracts : IPlan
		{
		    #region Primitive properties
		
			int IPlan.PlanID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPlan.PlanCode
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IPlan.Enabled
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<bool> IPlan.DefaultPlan
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPlan.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPlan.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IBonusType> IPlan.BonusTypes
			{
				get { throw new NotImplementedException(); }
			}
		
			void IPlan.AddBonusType(IBonusType item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IPlan.RemoveBonusType(IBonusType item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IPeriod> IPlan.Periods
			{
				get { throw new NotImplementedException(); }
			}
		
			void IPlan.AddPeriod(IPeriod item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IPlan.RemovePeriod(IPeriod item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
