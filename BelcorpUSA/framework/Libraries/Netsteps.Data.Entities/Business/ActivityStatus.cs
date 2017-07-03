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
using NetSteps.Common.Exceptions;

namespace NetSteps.Data.Entities
{
    public partial class ActivityStatus : ITermName
    {
        #region Properties
        /// <summary>
        /// This is the 'DataAdapter' used to persist the data to a data store of some Kind. (DataBase, Web Service, ect..)
        /// This property is static to optimize memory usage. Every Entity Type will only have one of these Repositories 
        /// instantiated per app pool. - JHE
        /// </summary>
        public static IActivityStatusRepository Repository
        {
            get
            {
                try
                {
                    return Create.New<IActivityStatusRepository>();
                }
                catch (ContainerException cex)
                {
                    throw new NetStepsBusinessException(string.Format("Repository not set on Entity: {0}", typeof(IActivityStatusRepository)), cex);
                }
            }
        }

        /// <summary>
        /// This is the Logic used for this Entity Type - JHE
        /// </summary>
        internal static IActivityStatusBusinessLogic BusinessLogic
        {
            get
            {
                try
                {
                    return Create.New<IActivityStatusBusinessLogic>();
                }
                catch (ContainerException cex)
                {
                    throw new NetStepsBusinessException(string.Format("BusinessLogic not set on Entity: {0}", typeof(IActivityStatusBusinessLogic)), cex);
                }
            }
        }
        #endregion

        #region Methods

        public static List<ActivityStatus> GetAll()
        {
            return Repository.GetAll();
        }

        #endregion
    }
}
