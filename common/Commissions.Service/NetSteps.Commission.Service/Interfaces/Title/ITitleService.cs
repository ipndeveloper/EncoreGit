using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Interfaces.Title
{
	/// <summary>
	/// 
	/// </summary>
	public interface ITitleService
	{
		/// <summary>
		/// Gets the title.
		/// </summary>
		/// <param name="titleId">The title identifier.</param>
		/// <returns></returns>
		ITitle GetTitle(int titleId);

		/// <summary>
		/// Gets the title.
		/// </summary>
		/// <param name="titleName">Name of the title.</param>
		/// <returns></returns>
		ITitle GetTitle(string titleName);

		/// <summary>
		/// Gets the titles.
		/// </summary>
		/// <returns></returns>
		IEnumerable<ITitle> GetTitles();


        /// <summary>
        /// Gets the titles.
        /// </summary>
        /// <param name="accountId">Account Id</param>
        /// <param name="periodId">Period Id</param>
        /// <returns></returns>
        IEnumerable<ITitle> GetFromReportByPeriod(int periodId, int accountId);

		/// <summary>
		/// Gets the titles.
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		IEnumerable<ITitle> GetTitles(Predicate<ITitle> filter);
	}
}
