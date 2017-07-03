using System;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Repositories;
using System.Collections.Generic; 

namespace NetSteps.Data.Entities
{
    public partial class SupportTicket
    {
        #region Properties

        #endregion

        #region Methods


        public static SupportTicket LoadBySupportTicketNumber(string supportTicketNumber)
        {
            try
            {
                return BusinessLogic.LoadBySupportTicketNumber(Repository, supportTicketNumber);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static SupportTicket LoadBySupportTicketNumberFull(string supportTicketNumber)
        {
            try
            {
                return BusinessLogic.LoadBySupportTicketNumberFull(Repository, supportTicketNumber);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static SupportTicket RequestNewTicket(int assignedUserID)
        {
            try
            {
                return BusinessLogic.RequestNewTicket(Repository, assignedUserID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static PaginatedList<SupportTicketSearchData> Search(SupportTicketSearchParameters supportTicketSearchParameters)
        {
            try
            {
                return BusinessLogic.Search(Repository, supportTicketSearchParameters);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        
         
         

        public void BuildReadOnlyNotesTree()
        {
            try
            {
                BusinessLogic.BuildReadOnlyNotesTree(this);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static PaginatedList<AuditLogRow> GetAuditLog(int supportTicketID, AuditLogSearchParameters param)
        {
            try
            {
                return BusinessLogic.GetAuditLog(Repository, supportTicketID, param);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static PaginatedList<AuditLogRow> GetAuditLog(SupportTicket fullyLoadedSupportTicket, AuditLogSearchParameters param)
        {
            try
            {
                return BusinessLogic.GetAuditLog(Repository, fullyLoadedSupportTicket, param);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        #endregion

        #region Basic Crud
        public override void Save()
        {
            try
            {
                if (this.SupportTicketNumber.IsNullOrEmpty())
                {
                    // This was being set to 0 but I changed it becasue if the save fails, no others can save after.  To avoid a recursive Save - JHE
                    this.SupportTicketNumber = "temp" + Guid.NewGuid();
                    BusinessLogic.GenerateAndSetNewSupportTicketNumber(this);
                }

                base.Save();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.SupportTicketID.ToIntNullable());
            }
        }
        #endregion

        public static PaginatedList<SupportTicketSearchDetailsData> SearchDetails(SupportTicketSearchDetailsParameter searchParameter)
        {
            try
            { 
                return SupportTicketRepository.SearchDetails(searchParameter);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        }

        #region Support Level Suppport Level Motive 

        public static System.Collections.Generic.List<System.Tuple<int, string, int>> GetLevelSupportLevel(int ParentSupportLevelID, bool IsVisibleToWorkStation = false)
        {
            return BusinessLogic.GetLevelSupportLevel(Repository, ParentSupportLevelID, IsVisibleToWorkStation);
        }

        public static System.Collections.Generic.List<System.Tuple<int, string, int, int>> GetLevelSupportLevelIsActive(int ParentSupportLevelID, bool IsVisibleToWorkStation = false)
        {
            return BusinessLogic.GetLevelSupportLevelIsActive(Repository, ParentSupportLevelID, IsVisibleToWorkStation);
        }
       
        public static System.Collections.Generic.List<System.Tuple<int, string, int>> GetLevelSupportLevelMotive(int SupportLevelID, bool IsVisibleToWorkStation=false)
        {
            return BusinessLogic.GetLevelSupportLevelMotive(Repository, SupportLevelID, IsVisibleToWorkStation);
        }

        public static System.Collections.Generic.List<System.Tuple<int, string, int, int>> GetLevelSupportLevelMotiveIsActive(int SupportLevelID, bool IsVisibleToWorkStation = false)
        {
            return BusinessLogic.GetLevelSupportLevelMotiveIsActive(Repository, SupportLevelID, IsVisibleToWorkStation);
        }
       


        public static List<SupportMotivePropertyTypes> ListarSupportMotivePropertyTypesPorMotivo(int SupportMotiveID, int SupportTicketID, Boolean IsVisibleToWorkStation=false)
        {
            return BusinessLogic.ListarSupportMotivePropertyTypesPorMotivo(Repository, SupportMotiveID, SupportTicketID, IsVisibleToWorkStation);
        }
        public static List<SupportMotivePropertyValues> ListarSupportMotivePropertyValuesPorMotivo(int SupportMotiveID)
        {
            return BusinessLogic.ListarSupportMotivePropertyValuesPorMotivo(Repository, SupportMotiveID);
        }
        public static List<SupportMotiveTask> ListarSupportMotiveTaskPorMotivo(int SupportMotiveID)
        {
            return BusinessLogic.ListarSupportMotiveTaskPorMotivo(Repository, SupportMotiveID);
        }



        public static int InsertarSuportTickets(
                   SupportTicketsBE objSupportTicketsBE,
                   List<SupportTicketsPropertyBE> LstSupportTicketsProperty,
                   List<SupportTicketsFilesBE> LstSupportTicketsFiles,
                   List<int> ListaEliminarSupportTicketsFiles,
                   SupportTicketGestionBE objSupportTicketGestionBE
           )
        {
          
            return BusinessLogic.InsertarSuportTickets(Repository, objSupportTicketsBE, LstSupportTicketsProperty, LstSupportTicketsFiles, ListaEliminarSupportTicketsFiles, objSupportTicketGestionBE);
        }

        public static List<SupportTicketGestionBE> ListarSupportTicketGestionBE(int SupportTicketID)
        {
            return BusinessLogic.ListarSupportTicketGestionBE(Repository, SupportTicketID);
        }
        public static SupportTicketsBE ObtenerSupportTicketsBE(int SupportTicketID)
        {
            return BusinessLogic.ObtenerSupportTicketsBE(Repository,SupportTicketID);
        }

        public static List<SupportTicketsFilesBE> ObtenerSupportTicketsFilesporSupporMotive(int SupportTicketID)
        {
            return BusinessLogic.ObtenerSupportTicketsFilesporSupporMotive(Repository, SupportTicketID);
        }
        public static byte BloquearSUpportTickets(int SupportTicketID, int BlockUserID)
        {
            return new SupportTicketRepository().BloquearSUpportTickets(SupportTicketID, BlockUserID);

        }

        public int InsertarArchivos( List<SupportTicketsFilesBE> LstSupportTicketsFiles, int SupportTicketID)
        {
            return BusinessLogic.InsertarArchivos(Repository, LstSupportTicketsFiles, SupportTicketID);
        }

        public static Dictionary<int, string> GetFileName(int SupportTicketFileID)
        {
            return BusinessLogic.GetFileName(Repository, SupportTicketFileID);
        }
        #endregion
    
    
    }
}
