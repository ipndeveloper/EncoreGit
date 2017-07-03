using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities;
using System.Collections.Generic;
namespace DistributorBackOffice.Models.SupportTicketModels
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