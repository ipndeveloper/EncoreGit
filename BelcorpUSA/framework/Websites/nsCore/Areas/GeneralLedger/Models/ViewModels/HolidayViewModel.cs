using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;

namespace nsCore.Areas.GeneralLedger.Models.ViewModels
{
    public class HolidayViewModel
    {
        public IEnumerable<Country> Countries { get; set; }
        public IEnumerable<StateProvince> StateProvinces { get; set; }
        public Holiday Holiday { get; set; }
        public string dateHidden { get; set; }

    }
}