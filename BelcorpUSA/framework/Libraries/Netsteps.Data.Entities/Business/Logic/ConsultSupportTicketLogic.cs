using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Repositories.Interfaces;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Dto;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class ConsultSupportTicketLogic
    {
        private ConsultSupportTicketLogic()
        { 
        }

        public static ConsultSupportTicketLogic Instance
        {

            get
            {
                if (instance == null)
                {
                    instance = new ConsultSupportTicketLogic();
                    repositoryConsultSupportTicket = new ConsultSupportTicketRepository();
                }
                return instance;
            }
        }

        private static ConsultSupportTicketLogic instance;

        private static IConsultSupportTicketRepository repositoryConsultSupportTicket;



        public List<SupportTicketSearchDetailsData> GetSupportTicket(SupportTicketSearchDetailsParameter parameters)
        {
            var data = repositoryConsultSupportTicket.GetSupportTicket(parameters);
            return (from r in data
                    select DtoToBO(r)).ToList();
        }

        private SupportTicketSearchDetailsData DtoToBO(ConsultSupportTicketDto dto)
        {
            return new SupportTicketSearchDetailsData()
            {
                SupportTicketID = dto.SupportTicketID,
                AccountNumber = dto.AccountNumber,
                SupportTicketNumber = dto.SupportTicketNumber,
                AssignedUsername = dto.AssignedUsername,
                PriorityName = dto.PriorityName,
                Title = dto.Title,

                /*CS.20AGO2016.Inicio*/
                OrderNumber = dto.OrderNumber,
                State = dto.State,
                City = dto.City,
                InvoiceNumber = dto.InvoiceNumber,
                /*CS.20AGO2016.Inicio*/

                FirstName = dto.FirstName,
                LastName = dto.LastName,
                StatusName = dto.StatusName,
                CategoryName = dto.CategoryName,
                CreateUserName = dto.CreateUserName,
                DateCreated = dto.DateCreated,
                DateLastModified = dto.DateLastModified,
                Comfim = dto.Comfim,
                Close = dto.Close,
                Question = dto.Question,
                Respuesta = dto.Respuesta,
                RowTotal = dto.RowTotal 
            };
        }
    }
}
