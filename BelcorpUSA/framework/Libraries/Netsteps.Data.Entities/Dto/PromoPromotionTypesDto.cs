using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Dto
{
    public class PromoPromotionTypesDto
    {
        public int PromotionTypeID { get; set; }
        public string Name { get; set; }
        public string TermName { get; set; }
        public bool Active { get; set; }
        public int SortIndex { get; set; }
    }
}
