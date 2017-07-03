using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    public class MainProcessesDetailSearchData
    {
        public string MainProcessCode { get; set; }
        public int MainProcessID { get; set; }
        public string MainProcessName { get; set; }
        public DateTime MainProcessStarTime { get; set; }
        public int SubProcessID { get; set; }
        public string SubProcessName { get; set; }
        public int StatusSubProcessID { get; set; }
        public string StatusSubProcessName { get; set; }
        public bool Reprocess { get; set; }
        public string RowTotal { get; set; }
    }
}
