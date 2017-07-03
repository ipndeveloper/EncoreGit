using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{

    public class MarketSearchData
    {
        

        public string Name { get; set; }

        public int MarketID { get; set; }

        public int Relacionado { get; set; }
    }
}
