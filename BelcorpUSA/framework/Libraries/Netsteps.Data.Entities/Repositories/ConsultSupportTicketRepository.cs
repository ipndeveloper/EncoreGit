using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Dto;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Repositories.Interfaces;

namespace NetSteps.Data.Entities.Repositories
{
    public class ConsultSupportTicketRepository : IConsultSupportTicketRepository
    {
        public List<ConsultSupportTicketDto> GetSupportTicket(SupportTicketSearchDetailsParameter parameters)
        {
            return DataAccess.ExecWithStoreProcedure<ConsultSupportTicketDto>(ConnectionStrings.BelcorpCore, "uspGetSupportTicket",
                new SqlParameter("TypeConsult", SqlDbType.Int) { Value = parameters.TypeConsult },
                new SqlParameter("PriorityID", SqlDbType.Int) { Value = (object)parameters.PriorityID ?? DBNull.Value },
                new SqlParameter("CategoryID", SqlDbType.Int) { Value = (object)parameters.CategoryID ?? DBNull.Value },
                new SqlParameter("StatusID", SqlDbType.Int) { Value = (object)parameters.StatusID ?? DBNull.Value },
                new SqlParameter("SupportTicket", SqlDbType.VarChar) { Value = parameters.SupportTicket },
                new SqlParameter("Title", SqlDbType.VarChar) { Value = parameters.Title },
                new SqlParameter("OrderNumber", SqlDbType.VarChar) { Value = parameters.OrderNumber },
                new SqlParameter("InvoiceNumber", SqlDbType.VarChar) { Value = parameters.InvoiceNumber },
                new SqlParameter("AssignedUserID", SqlDbType.Int) { Value = (object)parameters.AssignedUserID ?? DBNull.Value },
                new SqlParameter("ConsultSearchID", SqlDbType.Int) { Value = (object)parameters.ConsultSearchID ?? DBNull.Value },
                new SqlParameter("CreateByUserID", SqlDbType.Int) { Value = (object)parameters.CreateByUserID ?? DBNull.Value },
                new SqlParameter("TypeUserID", SqlDbType.Int) { Value = (object)parameters.TypeUserID ?? DBNull.Value },
                new SqlParameter("TypeConsultID", SqlDbType.Int) { Value = (object)parameters.TypeConsultID ?? DBNull.Value },
                new SqlParameter("IsConfirmID", SqlDbType.Int) { Value = (object)parameters.IsConfirmID ?? DBNull.Value },
                new SqlParameter("CampaignID", SqlDbType.Int) { Value = (object)parameters.CampaignID ?? DBNull.Value },
                new SqlParameter("StartDate", SqlDbType.Date) { Value = (object)parameters.StartDate ?? DBNull.Value },
                new SqlParameter("EndDate", SqlDbType.Date) { Value = (object)parameters.EndDate ?? DBNull.Value },
                new SqlParameter("PageSize", SqlDbType.Int) { Value =  parameters.PageSize },
                new SqlParameter("PageNumber", SqlDbType.Int) { Value =  parameters.PageIndex },
                new SqlParameter("Colum", SqlDbType.VarChar) { Value =  parameters.OrderBy},
                new SqlParameter("Order", SqlDbType.VarChar) {Value = parameters.Order },

                new SqlParameter("SupportLevelID", SqlDbType.Int) { Value = parameters.SupportLevelID },
                new SqlParameter("SupportMotiveID", SqlDbType.Int) { Value = parameters.SupportMotiveID },
                new SqlParameter("IsSiteDWS", SqlDbType.TinyInt) { Value = parameters.IsSiteDWS}
                 
                 
                ).ToList();
        }
    }
}
