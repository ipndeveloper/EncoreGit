using System.Collections.Generic;
using NetSteps.Data.Entities.Dto;
using NetSteps.Data.Entities.Business;
using System;
using System.Globalization;
/*
 * @01 20150820 BR-E020 CSTI JMO: Added ValidarNotaFiscal and UpdateOrderStatusToShipped Methods
 */

namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    interface IXmlGeneratorRepository
    {
        IEnumerable<OrderHeaderXmlDto> GetOrderHead(int OrderID);
        IEnumerable<OrderDetailXmlDto> GetOrderDetail(int OrderID);
        IEnumerable<AdvancePaymentXmlDto> GetAdvancePayment(int OrderID);

        /// <summary>
        /// Developed By KTC - CSTI
        /// BR-B200
        /// </summary>
        /// <param name="Material">MaterialXmlDto</param>
        /// <returns>List<MaterialLogXmlDto></returns>
        List<MaterialLogXmlDto> InsertMaterialDto(MaterialXmlDto Material);

        /// <summary>
        /// Developed By KTC - CSTI
        /// BR-B200
        /// </summary>
        /// <param name="MatCenter">MaterialCentersXmlDto</param>
        /// <returns>MaterialLogXmlDto</returns>
        MaterialLogXmlDto InsertWarehouseMaterialsDto(MaterialCentersXmlDto MatCenter);
        
        /// <summary>
        /// Developed By KLC - CSTI
        /// </summary>
        /// <param name="facturas"></param>
        List<BalancesBillOrdersXml> InsSAPEncoreFacturas(BalancesBillOrdersXmlDto facturas, BalancesBillOrdersItemsXmlDto InvoiceDetail);

        /// <summary>
        /// Developed By MAM - CSTI
        /// BR-B070
        /// </summary>
        /// <param name="facturas"></param>
        /// <returns></returns>
        List<BalancesBillOrdersXml> InsSAPEncoreFacturasUpdateCvValue(string orderNumber);


        /// <summary>
        /// Developed By KLC - CSTI
        /// BR-B055
        /// </summary>
        /// <param name="facturas"></param>
        /// <returns></returns>
        List<BalancesBillOrdersXml> InsSAPEncorePicking(BalancesBillOrdersXmlDto OrderInvoice, BalancesBillOrdersItemsXmlDto InvoiceDetail);

        /// <summary>
        /// Developed By MAM - CSTI
        /// BR-B055
        /// </summary>
        /// <param name="facturas"></param>
        /// <returns></returns>
        List<BalancesBillOrdersXml> InsSAPEncorePickingGenerateResidual(string orderNumber);

        /* @01 A01*/
        #region [E020]

        /// <summary>
        /// Valida integridad de Nota Fiscal
        /// </summary>
        Tuple<bool, bool, int, int> ValidarNotaFiscal(int NumeroNotaFiscal, int NumeroSerie);

        /// <summary>
        /// Updates Orders status to Shipped
        /// </summary>
        void UpdateOrderStatusToShipped(List<int> ListOrderID);

        #endregion
        /* @01 A01*/

        //E030-LIB
        #region [E030]

        bool ValidarCnpj(string cnpjValue);
        void InsertOrderShipmentTracking(int orderID, int situacao, string observacao, string dataOcorrencia);

        #endregion

        #region Req BR010 Pedidos Encore - SAP 3PL
        List<ClientXmlDto> GetClientOrder(int LoteID);
        List<AdiantamentoXmlDto> GetAdiantamentoOrder(int LoteID, CultureInfo CurrentCultureInfo);
        List<PedidoXmlDto> GetPedidoOrder(int LoteID, CultureInfo CurrentCultureInfo);
        List<OrderItemsXmlDto> GetDetailOrder(int LoteID, int OrderInvoiceIDIniPOut, int OrderInvoiceIDFinPOut, CultureInfo CurrentCultureInfo);
        #endregion

        #region Returned Order
        IEnumerable<ReturnOrderHeaderXmlDto> GetHeaderReturnedOrder(int OrderID);
        IEnumerable<ReturnOrderDetailXmlDto> GetDetailReturnedOrder(int OrderID);
        #endregion
    }
}
