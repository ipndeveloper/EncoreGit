using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    [Serializable]
    public class BlockingSubTypeSearchData
    {
        public Int16 AccountBlockingSubTypeID { get; set; }
        public string Name { get; set; }
        public Int16 AccountBlockingTypeID { get; set; }
        public string BlockingType { get; set; }
        public bool Enabled { get; set; }
        public Int16 AccountBlockingProcessID { get; set; }
        public string CodProcess { get; set; }
        public string Description { get; set; }
    }

}
