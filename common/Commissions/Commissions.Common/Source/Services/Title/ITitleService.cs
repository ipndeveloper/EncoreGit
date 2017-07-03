using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Services.Title
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
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		IEnumerable<ITitle> GetTitles(Predicate<ITitle> filter);
	}
}
