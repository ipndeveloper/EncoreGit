using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchParameters
{
    public class ParameterCountriesData
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string Controls { get; set; }
        public bool Active { get; set; }
        public string Descriptions { get; set; }
        public int Step { get; set; }
        public int Sites { get; set; }
    }
}
