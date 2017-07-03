using System;
using NetSteps.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    [Serializable]
    public class BlockingTypeSearchData
    {
        public Int16 AccountBlockingTypeID { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public string TermName { get; set; }

        /*BlockingHistory*/
        public Int16 AccountBlockingSubTypeID { get; set; }
        public Int16 StatusID { get; set; }
        public DateTime DateCreatedUTC { get; set; }
        public string BlockTypeName { get; set; }
        public string BlockSubTypeName { get; set; }
        public string Reasons { get; set; }
        public string UserName { get; set; }
        public string StatusName { get; set; }
    }
}
