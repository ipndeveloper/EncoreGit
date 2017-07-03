using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for Commissions_AccountTitles_View.
	/// </summary>
	[ContractClass(typeof(Contracts.Commissions_AccountTitles_ViewContracts))]
	public interface ICommissions_AccountTitles_View
	{
		#region Primitive properties

		/// <summary>
		/// The AccountID for this Commissions_AccountTitles_View.
		/// </summary>
		int AccountID { get; set; }

		/// <summary>
		/// The TitleID for this Commissions_AccountTitles_View.
		/// </summary>
		int TitleID { get; set; }

		/// <summary>
		/// The TitleTypeID for this Commissions_AccountTitles_View.
		/// </summary>
		int TitleTypeID { get; set; }

		/// <summary>
		/// The PeriodID for this Commissions_AccountTitles_View.
		/// </summary>
		int PeriodID { get; set; }

		#endregion
	}

	namespace Contracts
	{
		[ContractClassFor(typeof(ICommissions_AccountTitles_View))]
		internal abstract class Commissions_AccountTitles_ViewContracts : ICommissions_AccountTitles_View
		{
			#region Primitive properties

			int ICommissions_AccountTitles_View.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

			int ICommissions_AccountTitles_View.TitleID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

			int ICommissions_AccountTitles_View.TitleTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

			int ICommissions_AccountTitles_View.PeriodID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

			#endregion
		}
	}
}
