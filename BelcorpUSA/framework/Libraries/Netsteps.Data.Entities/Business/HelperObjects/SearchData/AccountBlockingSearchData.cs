using System;
using NetSteps.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    [Serializable]
    public class AccountBlockingSearchData
    {
        public int AccountID { get; set; }
        public Int16? AccountBlockingTypeID { get; set; }
        public Int16? AccountBlockingSubTypeID { get; set; }
        public bool IsLocked { get; set; }
        public string Description { get; set; }
    }
}
