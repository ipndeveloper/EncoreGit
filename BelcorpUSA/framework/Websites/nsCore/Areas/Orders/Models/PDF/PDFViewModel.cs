using System.Collections.Generic;

namespace nsCore.Areas.Orders.Models.PDF
{
    public class PDFViewModel
    {
        public string DistNumber { get; set; }          // 1

        public string Date { get; set; }                // 2

        public string InvoiceNumber { get; set; }       // 3

        public string VATNumber { get; set; }           // 4

        public PdfAddress Shipping { get; set; }           // 5

        public PdfAddress Billing { get; set; }            // 6

        public string InvoiceNumber2 { get; set; }      // 7

        public string Date2 { get; set; }               // 8

        public string Type { get; set; }                // 9

        public string Status { get; set; }              // 10

        public string ShippingType { get; set; }        // 11

        public IEnumerable<Item> Items { get; set; }   // 12, 13, 14, 15, 16, 17

        public IEnumerable<PaymentDetail> PaymentDetails { get; set; }  // 18, 19, 20

        public IEnumerable<VATDetail> VATDetails { get; set; }  // 21, 22, 23, 24, 25, 26

        public string SubTotal { get; set; }           // 27

        public string VatSubtotal { get; set; }        // 28

        public string ShippingSubtotal { get; set; }   // 29

        public string HandlingSubtotal { get; set; }   // 30

        public string GrandTotal { get; set; }         // 31

        public string Payment { get; set; }           // 32

        public string BalanceDue { get; set; }         // 33

    }

    public class VATDetail
    {
        public string Code { get; set; }

        public string VATPercent { get; set; }

        public string ExVATMoney { get; set; }

        public string VATAmountMoney { get; set; }

        public string FXRate { get; set; }

        public string GbpVATTotals { get; set; }

    }
}