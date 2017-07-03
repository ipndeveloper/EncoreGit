using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using NetSteps.Data.Common.Entities;

namespace NetSteps.Data.Common.Services
{
	[ContractClass(typeof(Contracts.TitleServiceContract))]
    public interface ITitleService
	{
        /// <summary>
        /// Gets all available titles.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ITitle> GetTitles();
        
        /// <summary>
        /// Gets an account's title.
        /// </summary>
        /// <param name="accountID">The account ID.</param>
        /// <param name="titleTypeID">The title type ID.</param>
        /// <param name="periodID">The period ID.  Defaults to the current period if none is provided</param>
        /// <returns></returns>
        IAccountTitle GetAccountTitle(int accountID, int titleTypeID, int? periodID);

		/// <summary>
		/// Gets the account titles.
		/// </summary>
		/// <param name="accountID">The account ID.</param>
		/// <param name="periodID">The period ID.</param>
		/// <returns></returns>
		IEnumerable<IAccountTitle> GetAccountTitles(int accountID, int? periodID);
	}

    
}

namespace NetSteps.Data.Common.Services.Contracts
{
    [ContractClassFor(typeof(ITitleService))]
    public abstract class TitleServiceContract : ITitleService
    {
        public IEnumerable<ITitle> GetTitles()
        {
            Contract.Ensures(Contract.Result<IEnumerable<ITitle>>() != null);
            throw new NotImplementedException();
        }

        public IAccountTitle GetAccountTitle(int accountID, int titleTypeID, int? periodID)
		{
			Contract.Requires<ArgumentException>(titleTypeID > 0);
			Contract.Requires<ArgumentException>(!periodID.HasValue || periodID > 0);
			throw new NotImplementedException();
		}

		public IEnumerable<IAccountTitle> GetAccountTitles(int accountID, int? periodID)
		{
			Contract.Requires<ArgumentException>(!periodID.HasValue || periodID > 0);
			throw new NotImplementedException();
		}
	}

}