using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using NetSteps.Data.Entities.Base;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Interfaces;
using NetSteps.Data.Entities.Business.Logic.Interfaces;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Encore.Core.IoC;
using NetSteps.Common.Validation.NetTiers;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Common.Exceptions;

namespace NetSteps.Data.Entities
{
    public partial class AccountConsistencyStatus : ITermName
    {
        #region Properties
        /// <summary>
        /// This is the 'DataAdapter' used to persist the data to a data store of some Kind. (DataBase, Web Service, ect..)
        /// This property is static to optimize memory usage. Every Entity Type will only have one of these Repositories 
        /// instantiated per app pool. - JHE
        /// </summary>
        public static IAccountConsistencyStatusRepository Repository
        {
            get
            {
                try
                {
                    return Create.New<IAccountConsistencyStatusRepository>();
                }
                catch (ContainerException cex)
                {
                    throw new NetStepsBusinessException(string.Format("Repository not set on Entity: {0}", typeof(IAccountConsistencyStatusRepository)), cex);
                }
            }
        }

        /// <summary>
        /// This is the Logic used for this Entity Type - JHE
        /// </summary>
        internal static IAccountConsistencyStatusBusinessLogic BusinessLogic
        {
            get
            {
                try
                {
                    return Create.New<IAccountConsistencyStatusBusinessLogic>();
                }
                catch (ContainerException cex)
                {
                    throw new NetStepsBusinessException(string.Format("BusinessLogic not set on Entity: {0}", typeof(IAccountConsistencyStatusBusinessLogic)), cex);
                }
            }
        }
        #endregion

        #region Methods

        public static List<AccountConsistencyStatus> GetAll()
        {
            try
            {
                return Repository.GetAll();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        #endregion
    }

}
