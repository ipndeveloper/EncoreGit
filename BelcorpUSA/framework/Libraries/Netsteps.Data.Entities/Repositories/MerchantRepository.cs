using System;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class MerchantRepository
    {
        #region Members
        protected override Func<NetStepsEntities, IQueryable<Merchant>> loadAllFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<Merchant>>(
                   (context) => from m in context.Merchants
                                               .Include("Addresses")
                                               .Include("Addresses.AddressProperties")
                                select m);
            }
        }
        #endregion
        public Merchant GetByNumber(string merchantNumber)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var merchant = loadAllFullQuery(context).FirstOrDefault(m => m.MerchantNumber == merchantNumber);
                    if (merchant == null)
                        throw new NetStepsDataException("Error loading merchant");
                    else
                        return merchant;
                }
            });
        }

        public Merchant GetById(int merchantId)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var merchant = loadAllFullQuery(context).FirstOrDefault(m => m.MerchantID == merchantId);
                    if (merchant == null)
                        throw new NetStepsDataException("Error loading merchant");
                    else
                        return merchant;
                }
            });
        }

        /// <summary>
        /// Gets the first merchant which contains an address in its address list matching the provided addressID
        /// </summary>
        /// <param name="addressID">The desired AddressID to match </param>
        /// <returns>null if not found, merchant if found</returns>
        public Merchant GetByAddressID(int addressID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return loadAllFullQuery(context).FirstOrDefault(m => m.Addresses.Where(address => address.AddressID == addressID).FirstOrDefault() != null);
                }
            });
        }
    }
}
