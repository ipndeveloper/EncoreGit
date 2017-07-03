using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;

namespace nsCore.Areas.Support.Models.Ticket
{
    public class JerarquiaSupportLevelModel
    {

        public int SupportMotiveID { get; set; }
        public List<SupportLevelSearchData> lstHijosJerarquiaDescenedenteSupportLevel { get; set; }
        public List<SupportLevelSearchData> lstHijosJerarquiaAscendenteSupportLevel { get; set; }
        public JerarquiaSupportLevelModel(int SupportLevelID, int SupportMotiveID)
        {
            this.SupportMotiveID = SupportMotiveID;
            if (SupportLevelID != 0)
            {
                this.lstHijosJerarquiaDescenedenteSupportLevel = SupportLevels.ListarJerarquiaSupporLevel(SupportLevelID);
                this.lstHijosJerarquiaAscendenteSupportLevel = SupportLevels.TraerJeraquiaSupportLevel(0);
            }
            else 
            {
                this.lstHijosJerarquiaDescenedenteSupportLevel = new List<SupportLevelSearchData>();
                this.lstHijosJerarquiaAscendenteSupportLevel = new List<SupportLevelSearchData>();
            }
        }
    }
}