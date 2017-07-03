
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;
using System.Collections.Generic;
namespace NetSteps.Data.Entities.Repositories
{
    public partial interface ISupportTicketRepository
    {
        SupportTicket LoadBySupportTicketNumber(string supportTicketNumber);
        SupportTicket LoadBySupportTicketNumberFull(string supportTicketNumber);
        SupportTicket RequestNewTicket(int assignedUserID);
        PaginatedList<SupportTicketSearchData> Search(SupportTicketSearchParameters supportTicketSearchParamaters);
        PaginatedList<AuditLogRow> GetAuditLog(SupportTicket fullyLoadedSupportTicket, AuditLogSearchParameters searchParameters);
        System.Collections.Generic.List<System.Tuple<int, string, int>> GetLevelSupportLevel(int ParentSupportLevelID, bool IsVisibleToWorkStation);
        System.Collections.Generic.List<System.Tuple<int, string, int, int>> GetLevelSupportLevelIsActive(int ParentSupportLevelID, bool IsVisibleToWorkStation);
        System.Collections.Generic.List<System.Tuple<int, string, int>> GetLevelSupportLevelMotive(int SupportLevelID, bool IsVisibleToWorkStation);
        System.Collections.Generic.List<System.Tuple<int, string, int, int>> GetLevelSupportLevelMotiveIsActive(int SupportLevelID, bool IsVisibleToWorkStation);

        Dictionary<int, string> GetFileName(int SupportTicketFileID);
        System.Collections.Generic.List<SupportMotivePropertyTypes> ListarSupportMotivePropertyTypesPorMotivo(int SupportMotiveID, int SupportTicketID, bool  IsVisibleToWorkStation);
        System.Collections.Generic.List<SupportMotiveTask>ListarSupportMotiveTaskPorMotivo(int SupportMotiveID);
        System.Collections.Generic.List<SupportMotivePropertyValues> ListarSupportMotivePropertyValuesPorMotivo(int SupportMotiveID);


         int InsertarSuportTickets(
                    SupportTicketsBE objSupportTicketsBE,
                    List<SupportTicketsPropertyBE> LstSupportTicketsProperty,
                    List<SupportTicketsFilesBE> LstSupportTicketsFiles,
                    List<int> ListaEliminarSupportTicketsFiles,
                    SupportTicketGestionBE objSupportTicketGestionBE
            );

         SupportTicketsBE ObtenerSupportTicketsBE(int SupportTicketID);

         List<SupportTicketsFilesBE> ObtenerSupportTicketsFilesporSupporMotive(int SupportTicketID);
         int InsertarArchivos(List<SupportTicketsFilesBE> LstSupportTicketsFiles, int SupportTicketID);
           byte BloquearSUpportTickets(int SupportTicketID, int BlockUserID);

           List<SupportTicketGestionBE> ListarSupportTicketGestionBE(int SupportTicketID);
    }
}
