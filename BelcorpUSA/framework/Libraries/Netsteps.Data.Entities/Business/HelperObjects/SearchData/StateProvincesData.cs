using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    public class StateProvincesData
    {

        public int StateProvinceID { get; set; }      
        public string Name { get; set; }

        public string City { get; set; }
    }
}
