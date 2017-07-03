using System;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Repositories;
using System.Collections.Generic;

namespace NetSteps.Data.Entities.Business.Logic
{
    public partial class SupportTicketBusinessLogic
    {
        /// <summary>
        /// Sets the OrderNumber property for new orders according to the numbering pattern defined by the client.
        /// Just setting it to the same number as the OrderID by default. May cause order to save first to get the 
        ///     value of OrderID set by the identity. - JHE
        /// </summary>
        /// <param name="order"></param>
        public virtual void GenerateAndSetNewSupportTicketNumber(SupportTicket supportTicket)
        {
            if (supportTicket.SupportTicketNumber.IsNullOrEmpty() || supportTicket.SupportTicketNumber.Contains("temp"))
            {
                if (supportTicket.SupportTicketID == 0)
                    supportTicket.Save();

                supportTicket.SupportTicketNumber = string.Format("{0}", supportTicket.SupportTicketID + SupportTicketNumberOffset());
            }
        }

        public virtual int SupportTicketNumberOffset()
        {
            return 10000;
        }

        public virtual SupportTicket LoadBySupportTicketNumber(ISupportTicketRepository repository, string supportTicketNumber)
        {
            try
            {
                var account = repository.LoadBySupportTicketNumber(supportTicketNumber);
                account.StartEntityTracking();
                account.IsLazyLoadingEnabled = true;

                //BuildReadOnlyNotesTree(account);

                return account;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public virtual SupportTicket LoadBySupportTicketNumberFull(ISupportTicketRepository repository, string supportTicketNumber)
        {
            try
            {
                var account = repository.LoadBySupportTicketNumberFull(supportTicketNumber);
                account.StartEntityTracking();
                account.IsLazyLoadingEnabled = true;

                //BuildReadOnlyNotesTree(account);

                return account;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public virtual SupportTicket RequestNewTicket(ISupportTicketRepository repository, int assignedUserID)
        {
            try
            {
                var supportTicket = repository.RequestNewTicket(assignedUserID);

                // If unassigned tickets available
                if (supportTicket != null)
                {
                    supportTicket.StartEntityTracking();
                    supportTicket.IsLazyLoadingEnabled = true;

                    //BuildReadOnlyNotesTree(account);

                    return supportTicket;
                }

                // If no unassigned tickets then return null
                return null;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public virtual PaginatedList<SupportTicketSearchData> Search(Repositories.ISupportTicketRepository repository, SupportTicketSearchParameters supportTicketSearchParameters)
        {
            try
            {
                return repository.Search(supportTicketSearchParameters);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public virtual void BuildReadOnlyNotesTree(SupportTicket supportTicket)
        {
            // Hookup read-only tree of notes - JHE
            foreach (var note in supportTicket.Notes.ToList())
                note.FollowupNotes = supportTicket.Notes.Where(n => n.ParentID == note.NoteID).ToList();
        }

        
        public virtual PaginatedList<AuditLogRow> GetAuditLog(Repositories.ISupportTicketRepository repository, SupportTicket fullyLoadedSupportTicket, AuditLogSearchParameters param)
        {
            try
            {
                return repository.GetAuditLog(fullyLoadedSupportTicket, param);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        public List<Tuple<int, string, int>> GetLevelSupportLevel(ISupportTicketRepository repository, int ParentSupportLevelID, bool IsVisibleToWorkStation)
        {
            
            try
            {
                return repository.GetLevelSupportLevel(ParentSupportLevelID, IsVisibleToWorkStation);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        public List<Tuple<int, string, int, int>> GetLevelSupportLevelIsActive(ISupportTicketRepository repository, int ParentSupportLevelID, bool IsVisibleToWorkStation)
        {

            try
            {
                return repository.GetLevelSupportLevelIsActive(ParentSupportLevelID, IsVisibleToWorkStation);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<Tuple<int, string, int>> GetLevelSupportLevelMotive(ISupportTicketRepository repository ,int SupportLevelID,bool IsVisibleToWorkStation)
        {
            try
            {
                return repository.GetLevelSupportLevelMotive(SupportLevelID, IsVisibleToWorkStation);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public List<Tuple<int, string, int, int>> GetLevelSupportLevelMotiveIsActive(ISupportTicketRepository repository, int SupportLevelID, bool IsVisibleToWorkStation)
        {
            try
            {
                return repository.GetLevelSupportLevelMotiveIsActive(SupportLevelID, IsVisibleToWorkStation);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<SupportMotivePropertyTypes> ListarSupportMotivePropertyTypesPorMotivo(ISupportTicketRepository repository, int SupportMotiveID, int SupportTicketID, Boolean IsVisibleToWorkStation)
        {
            return repository.ListarSupportMotivePropertyTypesPorMotivo(SupportMotiveID, SupportTicketID, IsVisibleToWorkStation);
        }
       public List<SupportMotivePropertyValues> ListarSupportMotivePropertyValuesPorMotivo(ISupportTicketRepository repository, int SupportMotiveID)
        {
            return repository.ListarSupportMotivePropertyValuesPorMotivo(SupportMotiveID);
        }
        public List<SupportMotiveTask> ListarSupportMotiveTaskPorMotivo(ISupportTicketRepository repository, int SupportMotiveID)
        {
            return repository.ListarSupportMotiveTaskPorMotivo(SupportMotiveID);
        }


        public int InsertarSuportTickets(
                          ISupportTicketRepository repository,
                          SupportTicketsBE objSupportTicketsBE,
                          List<SupportTicketsPropertyBE> LstSupportTicketsProperty,
                          List<SupportTicketsFilesBE> LstSupportTicketsFiles,
                         List<int> ListaEliminarSupportTicketsFiles,
                         SupportTicketGestionBE objSupportTicketGestionBE
                  )
        {
            return repository.InsertarSuportTickets(objSupportTicketsBE, LstSupportTicketsProperty, LstSupportTicketsFiles, ListaEliminarSupportTicketsFiles, objSupportTicketGestionBE);
        }
        public SupportTicketsBE ObtenerSupportTicketsBE(ISupportTicketRepository repository, int SupportTicketID)
        {
            return repository.ObtenerSupportTicketsBE(SupportTicketID);
        }
        public List<SupportTicketsFilesBE> ObtenerSupportTicketsFilesporSupporMotive(ISupportTicketRepository repository, int SupportTicketID)
        {
            return repository.ObtenerSupportTicketsFilesporSupporMotive(SupportTicketID);
        }
        public int InsertarArchivos(ISupportTicketRepository repository, List<SupportTicketsFilesBE> LstSupportTicketsFiles, int SupportTicketID)
        {
            return repository.InsertarArchivos(LstSupportTicketsFiles, SupportTicketID);
        }

        public Dictionary<int, string> GetFileName(ISupportTicketRepository repository, int SupportTicketFileID)
        {
            return repository.GetFileName(SupportTicketFileID);
        }
        public List<SupportTicketGestionBE> ListarSupportTicketGestionBE(ISupportTicketRepository repository,int SupportTicketID)
        {
            return repository.ListarSupportTicketGestionBE(SupportTicketID);
        }

    }
}


