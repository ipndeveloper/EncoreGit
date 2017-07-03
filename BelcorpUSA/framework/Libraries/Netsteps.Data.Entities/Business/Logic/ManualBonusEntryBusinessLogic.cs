namespace NetSteps.Data.Entities.Business.Logic
{
    using System.Collections.Generic;
    using System.Linq;
    using NetSteps.Data.Entities.Repositories;
    using NetSteps.Data.Entities.Repositories.Interfaces;
    using NetSteps.Common.Base;
    using System;
    using NetSteps.Data.Entities.Dto;
    using System.Data;
    using System.Web;

    public partial class ManualBonusEntryBusinessLogic
    {
        
        #region constructor - singleton

        private ManualBonusEntryBusinessLogic()
        {   
        }

        public static ManualBonusEntryBusinessLogic Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ManualBonusEntryBusinessLogic();
                    repositoryManualBonusEntry = new ManualBonusEntryRepository();
                }

                return instance;
            }
        }
        #endregion

        #region privates

        private static ManualBonusEntryBusinessLogic instance;
        private static IManualBonusEntryRepository repositoryManualBonusEntry;

        #endregion

        #region Methods

        public List<ManualBonusEntrySearchData> ManualBonusEntryValidation(DataTable values)
        {
            return repositoryManualBonusEntry.ManualBonusEntryValidation(values);
        }

        public Tuple<int, string> ManualBonusEntryLoad(DataTable values)
        {
            return repositoryManualBonusEntry.ManualBonusEntryLoad(values);
        }

        #endregion        
    }
}
