using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using System;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchParameters
{
    public class BlockingTypeSearchParameters : FilterDateRangePaginatedListParameters<BlockingTypeSearchData>
    {
        public Int16? AccountBlockingTypeID { get; set; }
        public int LanguageID { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public int AccountID { get; set; }

        //PARÁMETROS PARA EL INSERTAR EL HISTORIAL DE BLOQUEOS
        public Int16? AccountBlockingSubTypeID { get; set; }
        public string Reasons { get; set; }
        public int CreateByUserID { get; set; }
        public DateTime DateCreatedUTC { get; set; }
        public bool IsLocked { get; set; }
    }
}
