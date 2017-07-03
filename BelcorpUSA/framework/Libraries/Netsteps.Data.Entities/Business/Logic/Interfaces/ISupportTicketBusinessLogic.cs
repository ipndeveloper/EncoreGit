using System.Collections.Generic;
using System.Collections;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Repositories;
using System;
namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
    public partial interface ISupportTicketBusinessLogic
    {
        void GenerateAndSetNewSupportTicketNumber(SupportTicket supportTicket);
        int SupportTicketNumberOffset();
        SupportTicket LoadBySupportTicketNumber(ISupportTicketRepository repository, string supportTicketNumber);
        SupportTicket LoadBySupportTicketNumberFull(ISupportTicketRepository repository, string supportTicketNumber);
        SupportTicket RequestNewTicket(ISupportTicketRepository repository, int assignedUserID);
        PaginatedList<SupportTicketSearchData> Search(Repositories.ISupportTicketRepository repository, SupportTicketSearchParameters supportTicketSearchParameters);
        void BuildReadOnlyNotesTree(SupportTicket supportTicket);
        PaginatedList<AuditLogRow> GetAuditLog(Repositories.ISupportTicketRepository repository, SupportTicket fullyLoadedSupportTicket, AuditLogSearchParameters param);


        #region gestion de tickets  

        List<Tuple<int, string, int>> GetLevelSupportLevel(ISupportTicketRepository repository, int ParentSupportLevelID, bool IsVisibleToWorkStation);
        List<Tuple<int, string, int,int>> GetLevelSupportLevelIsActive(ISupportTicketRepository repository, int ParentSupportLevelID, bool IsVisibleToWorkStation);
        List<Tuple<int, string, int>> GetLevelSupportLevelMotive(ISupportTicketRepository repository, int SupportLevelID, bool IsVisibleToWorkStation);
        List<Tuple<int, string, int, int>> GetLevelSupportLevelMotiveIsActive(ISupportTicketRepository repository, int SupportLevelID, bool IsVisibleToWorkStation);
            List<SupportMotivePropertyTypes> ListarSupportMotivePropertyTypesPorMotivo(ISupportTicketRepository repository, int SupportMotiveID, int SupportTicketID,Boolean IsVisibleToWorkStation);
            List<SupportMotivePropertyValues> ListarSupportMotivePropertyValuesPorMotivo(ISupportTicketRepository repository, int SupportMotiveID);
            List<SupportMotiveTask> ListarSupportMotiveTaskPorMotivo(ISupportTicketRepository repository,int SupportMotiveID);

            int InsertarSuportTickets(
                        ISupportTicketRepository repository,
                       SupportTicketsBE objSupportTicketsBE,
                       List<SupportTicketsPropertyBE> LstSupportTicketsProperty,
                       List<SupportTicketsFilesBE> LstSupportTicketsFiles,
                       List<int> ListaEliminarSupportTicketsFiles,
                      SupportTicketGestionBE objSupportTicketGestionBE
               );

            List<SupportTicketGestionBE> ListarSupportTicketGestionBE(ISupportTicketRepository repository, int SupportTicketID);

            SupportTicketsBE ObtenerSupportTicketsBE(ISupportTicketRepository repository ,int SupportTicketID);

            List<SupportTicketsFilesBE> ObtenerSupportTicketsFilesporSupporMotive(ISupportTicketRepository repository, int SupportTicketID);


         int InsertarArchivos(ISupportTicketRepository repository, List<SupportTicketsFilesBE> LstSupportTicketsFiles, int SupportTicketID);
         Dictionary<int, string> GetFileName(ISupportTicketRepository repository, int SupportTicketFileID);
        #endregion





      
    }
}
