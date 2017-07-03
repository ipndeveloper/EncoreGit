using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nsCore.Areas.GeneralLedger.Models
{
    public class HolidayBrowse
    {
        public int HolidayID { get; set; }

        public string CountryName { get; set; }

        public string StateProvinceName { get; set; }

        public DateTime DateHoliday { get; set; }

        public bool IsIterative { get; set; }

        public string Reason { get; set; }
    }
}