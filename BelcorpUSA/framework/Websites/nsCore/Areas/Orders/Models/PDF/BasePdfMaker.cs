using System.Collections.Generic;
using System.IO;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;

namespace nsCore.Areas.Orders.Models.PDF
{
    public abstract class BasePdfMaker<TModel> : IBasePdfMaker
    {
        private readonly MemoryStream _outputStream = new MemoryStream();
        private readonly string _filePath;
		public InventoryBaseRepository Inventory { get { return Create.New<InventoryBaseRepository>(); } }

        protected BasePdfMaker(string fileName, string folderName)
        {
            _filePath = GetAbsolutePath(fileName, folderName);
        }


        public abstract TModel BuildViewModel(Order order, List<OrderCustomer> customers, OrderShipment shipment, List<OrderPayment> payments);

        public abstract void AddDocumentDetails(PdfStamper form, TModel pdfModel);


        public virtual string GetAbsolutePath(string fileName, string folderName)
        {
            return fileName.AddAbsoluteUploadPath(folderName);
        }

        private byte[] Generate(IEnumerable<TModel> pdfViewModels)
        {
            Document document = new Document();
            PdfWriter writer = PdfWriter.GetInstance(document, _outputStream);

            document.Open();

            foreach (var pdfModel in pdfViewModels)
            {
                MemoryStream scratchMemory = new MemoryStream();

                AddContent(scratchMemory, pdfModel);

                CopyPagesToDocument(document, scratchMemory, writer);
            }

            document.Close();

            return _outputStream.ToArray();
        }

        private static void CopyPagesToDocument(IDocListener document, MemoryStream stream, PdfWriter writer)
        {
            PdfContentByte cb = writer.DirectContent;
            PdfReader reader = new PdfReader(stream.ToArray());
            int numPages = reader.NumberOfPages;

            for (int i = 1; i <= numPages; i++)
            {
                document.NewPage();
                PdfImportedPage page = writer.GetImportedPage(reader, i);
                cb.AddTemplate(page, 0, 0);
            }
        }

        private void AddContent(Stream scratchMemory, TModel pdfModel)
        {
            PdfReader firstPageTemplate = new PdfReader(_filePath);
            PdfStamper form = new PdfStamper(firstPageTemplate, scratchMemory);

            AddDocumentDetails(form, pdfModel);
        }


        public virtual Stream GeneratePackingSlipPdf(List<Order> orders)
        {
            var finalModel = new List<TModel>();

            MemoryStream mStream = new MemoryStream();

            // Loop through each order and create a list of pdfViewModels (packing-slips) for each.
            foreach (var order in orders)
            {
                IEnumerable<TModel> viewModel = PdfViewModels(order);
                finalModel.AddRange(viewModel);
            }

            byte[] content = Generate(finalModel);

            mStream.Write(content, 0, content.Length);
            mStream.Position = 0;

            return mStream;
        }

        private IEnumerable<TModel> PdfViewModels(Order order)
        {
            var pdfViewModels = new List<TModel>();

            var custWithNoShipments = new List<OrderCustomer>();

            foreach (var customer in order.OrderCustomers)
            {
                // Process customers that have shipment records first
                if (customer.OrderShipments.Count > 0)
                {
                    TModel model = CustomerWithOrderShipments(customer, order);
                    pdfViewModels.Add(model);
                }
                else
                {
                    // Store all customers with no shipments. Need to combine their order information into one packing slip.
                    custWithNoShipments.Add(customer);
                }
            }

            // At this point all of the customers that have shipment details are set and ready to go.
            // If there are any customers with no shipment details, merge their order into one slip.
            if (custWithNoShipments.Count > 0)
            {
                TModel model = CustomerWithoutOrderShipments(custWithNoShipments, order);
                pdfViewModels.Add(model);
            }

            return pdfViewModels;
        }

        public TModel CustomerWithoutOrderShipments(List<OrderCustomer> customers, Order order)
        {
            OrderShipment shipment = order.GetDefaultShipment();

            List<OrderPayment> allPayments = AllOrderPayments(order, customers);

            return BuildViewModel(order, customers, shipment, allPayments);
        }

        public List<OrderPayment> AllOrderPayments(Order order, List<OrderCustomer> customers)
        {
            var allPayments = new List<OrderPayment>();

            // Add all of the successful payments into one list
            if (OrderPaymentExists(customers))
            {
                foreach (var orderCustomer in customers.Where(c => c.OrderPayments.Count > 0))
                {
                    allPayments.AddRange(orderCustomer.OrderPayments.Where(p =>
                                                                           p.OrderPaymentStatusID ==
                                                                           (short)Constants.OrderPaymentStatus.Completed).ToList());
                }
            }
            else
            {
                // If none of the order customer has payment details then use the Order's OrderPayment details instead.
                allPayments.Add(order.OrderPayments.First(p => p.OrderPaymentStatusID == (short)Constants.OrderPaymentStatus.Completed));
            }

            return allPayments;
        }


        public TModel CustomerWithOrderShipments(OrderCustomer customer, Order order)
        {
            // Need to put customer in a list to satisfy "BuildViewModel" method parameter
            var custTempHolder = new List<OrderCustomer> { customer };

            OrderShipment customerShipment = customer.OrderShipments.First();

            List<OrderPayment> payments = GetPaymentInfo(customer, order);

            var model = BuildViewModel(order, custTempHolder, customerShipment, payments);

            custTempHolder.Clear();

            return model;
        }



        public virtual List<OrderPayment> GetPaymentInfo(OrderCustomer customer, Order order)
        {
            return customer.OrderPayments.Count > 0
                       ? customer.OrderPayments.Where(p => p.OrderPaymentStatusID == (short)Constants.OrderPaymentStatus.Completed).ToList()
                       : order.AllOrderPayments.Where(p => p.OrderPaymentStatusID == (short)Constants.OrderPaymentStatus.Completed).ToList();
        }

        public virtual decimal GetSubTotal(List<OrderCustomer> customers)
        {
            return customers.Sum(c => c.Subtotal.ToDecimal());
        }

        public virtual decimal GetTotal(List<OrderCustomer> customers)
        {
            return customers.Sum(c => c.Total.ToDecimal());
        }

        public virtual decimal GetPayments(List<OrderCustomer> customers)
        {
            return customers.Sum(c => c.PaymentTotal.ToDecimal());
        }

        public virtual decimal GetBalanceDue(Order order, List<OrderCustomer> customers)
        {
            return OrderPaymentExists(customers)
                       ? customers.Sum(c => c.Balance.ToDecimal())
                       : order.Balance.ToDecimal();
        }

        public virtual bool OrderPaymentExists(List<OrderCustomer> customers)
        {
            return customers.Any(c => c.OrderPayments.Count > 0);
        }

        public virtual decimal GetShippingTotal(Order order, List<OrderCustomer> customers)
        {
            return customers.Any(c => c.ShippingTotal.HasValue)
                       ? customers.First(x => x.ShippingTotal.HasValue).ShippingTotal.ToDecimal()
                       : order.ShippingTotal.ToDecimal();
        }

        public virtual decimal GetHandlingTotal(Order order, List<OrderCustomer> customers)
        {
            return customers.Any(c => c.HandlingTotal.HasValue)
                       ? customers.First(c => c.HandlingTotal.HasValue).HandlingTotal.ToDecimal()
                       : order.HandlingTotal.ToDecimal();
        }

        public virtual IEnumerable<PaymentDetail> Details(IEnumerable<OrderPayment> payments)
        {
            return payments.Select(payment => new PaymentDetail
                                                  {
                                                      PaymentDate = payment.ProcessedDate.ToShortDateString(),
                                                      PaymentType = SmallCollectionCache.Instance.PaymentTypes.GetById(payment.PaymentTypeID).GetTerm(),
                                                      PaymentAmount = MoneyFormat(payment.Amount)
                                                  }).ToList();
        }

        public virtual string MoneyFormat(decimal value)
        {
            return string.Format("{0:0.00}", value);
        }

        public virtual string GetName(OrderShipment shipment)
        {
            return !string.IsNullOrEmpty(shipment.Attention)
                       ? shipment.Attention.ToCleanString()
                       : string.Format("{0} {1}", shipment.FirstName.ToCleanString(), shipment.LastName.ToCleanString());
        }

        public virtual string GetName(OrderPayment payment)
        {
            return !string.IsNullOrEmpty(payment.BillingName)
                       ? payment.BillingName.ToCleanString()
                       : string.Format("{0} {1}", payment.BillingFirstName.ToCleanString(),
                                       payment.BillingLastName.ToCleanString());
        }

        public virtual string GetCountry(int id)
        {
            return SmallCollectionCache.Instance.Countries.GetById(id).GetTerm();
        }

        public virtual PdfAddress ShippingAddress(OrderShipment shipment)
        {
            if (shipment != default(OrderShipment))
            {
                return new PdfAddress
                           {
                               Name = GetName(shipment),
                               Address1 = shipment.Address1.ToCleanString(),
                               City = shipment.City.ToCleanString(),
                               State = shipment.State.ToCleanString(),
                               Zip = shipment.PostalCode.ToCleanString(),
                               Country = GetCountry(shipment.CountryID)
                           };
            }
            return new PdfAddress();
        }

        public virtual PdfAddress BillingAddress(OrderPayment payment)
        {
            if (payment != default(OrderPayment) && payment.BillingCountryID != null)
            {
                return new PdfAddress
                           {
                               Name = GetName(payment),
                               Address1 = payment.BillingAddress1.ToCleanString(),
                               City = payment.BillingCity.ToCleanString(),
                               State = payment.BillingState.ToCleanString(),
                               Zip = payment.BillingPostalCode.ToCleanString(),
                               Country = GetCountry(payment.BillingCountryID.Value)
                           };
            }

            return new PdfAddress();
        }

        public virtual IEnumerable<Item> AllOrderItems(List<OrderCustomer> customers)
        {
            var allItems = new List<Item>();

            foreach (var customer in customers)
            {
                allItems.AddRange(GetOrderItems(customer.ParentOrderItems));
            }

            return allItems;
        }

        public virtual IEnumerable<Item> GetOrderItems(List<OrderItem> orderItems)
        {
            IEnumerable<OrderItem> shippableOnly = ShippableItemsOnly(orderItems);

            return shippableOnly.Select(item => new Item
                                                 {
                                                     Quantity = item.Quantity.ToString(),
                                                     SKU = item.SKU,
                                                     ItemDescription = ProductDescription(item.ProductID.ToInt()),
                                                     UnitPrice = item.GetAdjustedPrice().ToString("c"),
													 Amount = (item.GetAdjustedPrice() * item.Quantity).ToString("c")
                                                 }).ToList();
        }

        public virtual string ProductDescription(int productID)
        {
            var product = Inventory.GetProduct(productID);
            return product.Translations.Name();
        }


        /// <summary>
        /// Filter items that are shippable only.
        /// </summary>
        public virtual IEnumerable<OrderItem> ShippableItemsOnly(IEnumerable<OrderItem> orderItems)
        {
            return orderItems.Where(oi => oi.ProductID.HasValue
                                && GetProductBase(oi.ProductID.Value).IsShippable);
        }

        public virtual ProductBase GetProductBase(int productID)
        {
            Product product = Inventory.GetProduct(productID);

            return product.ProductBase ?? ProductBase.Load(product.ProductID);
        }

    }
}