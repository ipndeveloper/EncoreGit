using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class PaymetTycketsReportBusinessLogic
    {

        public static PaginatedList<PaymetTycketsReportSearchData> SearchTickets(
               PaymentTicketsSearchParameters searchParameter

               )

        {
            try
            {
                return PaymetTycketsReportRepository.SearchTickets(
                    searchParameter);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static DataTable SearchTicketsReport(
               PaymentTicketsSearchParameters searchParameter

              )
        {
            try
            {
                return PaymetTycketsReportRepository.SearchTicketsReport(
                 searchParameter);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #region Obtener informacion order payment
        public static List<InformacionFacturacion> ObtenerInformacionFacturacion(DataTable dtOrderPaymentIDs)
        {
            try
            {
                return PaymetTycketsReportRepository.ObtenerInformacionFacturacion(
                  dtOrderPaymentIDs);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static List<PaymentInfoBancoOrden> GetInformacionBanco(int TicketNumbers)
        {
            try
            {
                return PaymetTycketsReportRepository.GetInformacionBanco(TicketNumbers);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static List<PaymentInfoBancoOrden> GetInformacionOrder(int OrderID)
        {
            try
            {
                return PaymetTycketsReportRepository.GetInformacionByOrder(OrderID);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region generacion de reportes
        public static DataSet GenerateTicketBB(int TicketNumber)
        {
            try
            {
                return PaymetTycketsReportRepository.GenerateTicketBB(
                  TicketNumber);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static DataSet GenerateTicketCaixa(int TicketNumber)
        {
            try
            {
                return PaymetTycketsReportRepository.GenerateTicketCaixa(
                  TicketNumber);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static DataSet GenerateTicketItau(int TicketNumber)
        {
            try
            {
                return PaymetTycketsReportRepository.GenerateTicketItau(
                  TicketNumber);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion 

    }
}
