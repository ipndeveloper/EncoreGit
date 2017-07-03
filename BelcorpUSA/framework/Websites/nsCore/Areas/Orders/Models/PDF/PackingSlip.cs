using System.Collections.Generic;
using System.Linq;
using iTextSharp.text.pdf;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Mvc.Helpers;

namespace nsCore.Areas.Orders.Models.PDF
{
    public class PackingSlip : BasePdfMaker<PDFViewModel>
    {
        private const string FILE_NAME = "PDXPackSlip.pdf";
        private const string FOLDER_NAME = "PdfTemplates";

        public PackingSlip()
            : base(FILE_NAME, FOLDER_NAME)
        {
        }

        public override PDFViewModel BuildViewModel(Order order, List<OrderCustomer> customers, OrderShipment shipment, List<OrderPayment> payments)
        {
            return new PDFViewModel
            {
                DistNumber = Account.Load(order.ConsultantID).AccountNumber,                                    // order

                InvoiceNumber = order.OrderNumber,                                                              // order

                Shipping = ShippingAddress(shipment),

                Billing = BillingAddress(payments.Count > 0 ? payments.First() : new OrderPayment()),           // only space to show one billing address on PDF

                Date = order.CompleteDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo),             // order

                Type = SmallCollectionCache.Instance.OrderTypes.GetById(order.OrderTypeID).GetTerm(),           // order

                Status = SmallCollectionCache.Instance.OrderStatuses.GetById(order.OrderStatusID).GetTerm(),    // order

                ShippingType = shipment.ShippingMethodName,

                Items = AllOrderItems(customers),   // Include VAT in this method as well

                PaymentDetails = Details(payments),

                SubTotal = MoneyFormat(GetSubTotal(customers)),

                VatSubtotal = MoneyFormat(GetVat(customers)),

                ShippingSubtotal = MoneyFormat(GetShippingTotal(order, customers)),

                HandlingSubtotal = MoneyFormat(GetHandlingTotal(order, customers)),

                GrandTotal = MoneyFormat(GetTotal(customers)),

                Payment = MoneyFormat(GetPayments(customers)),

                BalanceDue = MoneyFormat(GetBalanceDue(order, customers)),
            };
        }

        public override void AddDocumentDetails(PdfStamper form, PDFViewModel pdfModel)
        {
            AcroFields fields = form.AcroFields;
            fields.SetField(PdfFields.DistributorNumber, pdfModel.DistNumber);
            fields.SetField(PdfFields.Date, pdfModel.Date);
            fields.SetField(PdfFields.InvoiceNumber, pdfModel.InvoiceNumber);

            // SHIP TO
            fields.SetField(PdfFields.ShipTo1, pdfModel.Shipping.Name);
            fields.SetField(PdfFields.ShipTo2, pdfModel.Shipping.Address1);
            fields.SetField(PdfFields.ShipTo3, pdfModel.Shipping.City);
            fields.SetField(PdfFields.ShipTo4, pdfModel.Shipping.Zip);
            fields.SetField(PdfFields.ShipTo5, pdfModel.Shipping.Country);

            // BILL TO
            fields.SetField(PdfFields.BillTo1, pdfModel.Billing.Name);
            fields.SetField(PdfFields.BillTo2, pdfModel.Billing.Address1);
            fields.SetField(PdfFields.BillTo3, pdfModel.Billing.City);
            fields.SetField(PdfFields.BillTo4, pdfModel.Billing.Zip);
            fields.SetField(PdfFields.BillTo5, pdfModel.Billing.Country);

            fields.SetField(PdfFields.InvoiceNumber2, pdfModel.InvoiceNumber);
            fields.SetField(PdfFields.Date2, pdfModel.Date);
            fields.SetField(PdfFields.Type, pdfModel.Type);
            fields.SetField(PdfFields.Status, pdfModel.Status);
            fields.SetField(PdfFields.Shipping, pdfModel.ShippingType);

            // ORDER ITEMS
            PopulateOrderItems(fields, pdfModel);

            // BOTTOM LEFT SECTION: 1
            PopulatePaymentDetails(fields, pdfModel);

            // BOTTOM LEFT SECTION: 2
            // TODO: VAT stuff 



            // BOTTOM RIGHT SECTION
            fields.SetField(PdfFields.Subtotal, pdfModel.SubTotal);
            fields.SetField(PdfFields.VatSubtotal, pdfModel.VatSubtotal);
            fields.SetField(PdfFields.ShippingSubtotal, pdfModel.ShippingSubtotal);
            fields.SetField(PdfFields.HandlingSubtotal, pdfModel.HandlingSubtotal);
            fields.SetField(PdfFields.GrandTotal, pdfModel.GrandTotal);
            fields.SetField(PdfFields.Payments, pdfModel.Payment);
            fields.SetField(PdfFields.BalanceDue, pdfModel.BalanceDue);

            form.Writer.CloseStream = false;
            form.FormFlattening = true;
            form.Close();
        }

        private static void PopulatePaymentDetails(AcroFields fields, PDFViewModel pdf)
        {
            int count = pdf.PaymentDetails.Count();
            string[] paymentDates = new string[count];
            string[] paymentTypes = new string[count];
            string[] paymentAmounts = new string[count];

            for (int i = 0; i < count; i++)
            {
                paymentDates[i] = pdf.PaymentDetails.ElementAt(i).PaymentDate;
                paymentTypes[i] = pdf.PaymentDetails.ElementAt(i).PaymentType;
                paymentAmounts[i] = pdf.PaymentDetails.ElementAt(i).PaymentAmount;
            }

            fields.SetListOption(PdfFields.PaymentDate, paymentDates, paymentDates);
            fields.SetListOption(PdfFields.PaymentType, paymentTypes, paymentTypes);
            fields.SetListOption(PdfFields.PaymentAmount, paymentAmounts, paymentAmounts);

            fields.SetField(PdfFields.PaymentDate, "PaymentDate");
            fields.SetField(PdfFields.PaymentType, "PaymentType");
            fields.SetField(PdfFields.PaymentAmount, "PaymentAmount");
        }

        private static void PopulateOrderItems(AcroFields fields, PDFViewModel pdf)
        {
            int count = pdf.Items.Count();
            string[] quantities = new string[count];
            string[] itemNumbers = new string[count];
            string[] descriptions = new string[count];
            string[] unitprice = new string[count];
            string[] amounts = new string[count];
            string[] vat = new string[count];

            for (int i = 0; i < count; i++)
            {
                quantities[i] = pdf.Items.ElementAt(i).Quantity;
                itemNumbers[i] = pdf.Items.ElementAt(i).SKU;
                descriptions[i] = pdf.Items.ElementAt(i).ItemDescription.Truncate(40, true);
                unitprice[i] = pdf.Items.ElementAt(i).UnitPrice;
                amounts[i] = pdf.Items.ElementAt(i).Amount;
            }

            fields.SetListOption(PdfFields.Quantity, quantities, quantities);
            fields.SetListOption(PdfFields.ItemNumber, itemNumbers, itemNumbers);
            fields.SetListOption(PdfFields.Description, descriptions, descriptions);
            fields.SetListOption(PdfFields.UnitPrice, unitprice, unitprice);
            fields.SetListOption(PdfFields.Amount, amounts, amounts);

            fields.SetField(PdfFields.Quantity, "Quantity");
            fields.SetField(PdfFields.ItemNumber, "ItemNumber");
            fields.SetField(PdfFields.Description, "Description");
            fields.SetField(PdfFields.UnitPrice, "UnitPrice");
            fields.SetField(PdfFields.Amount, "Amount");
        }

        // TODO: assuming VAT and TaxAmount are same.
        public decimal GetVat(List<OrderCustomer> customers)
        {
            return customers.Sum(c => c.TaxAmount.ToDecimal());
        }
    }
}