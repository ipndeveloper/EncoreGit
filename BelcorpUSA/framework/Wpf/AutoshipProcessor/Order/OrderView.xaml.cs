using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using NetSteps.Common;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Repositories;

namespace AutoshipProcessor.Order
{
	/// <summary>
	/// Interaction logic for OrderView.xaml
	/// </summary>
	public partial class OrderView : UserControl
	{
		CultureInfo CurrentCulture = ApplicationContext.Instance.ApplicationDefaultCultureInfo;

		ProductRepository _productRepository;
		OrderRepository _orderRepository;

		public NetSteps.Data.Entities.Order CurrentOrder
		{
			get
			{
				return (this.DataContext as NetSteps.Data.Entities.Order);
			}
			set
			{
				this.DataContext = value;
			}
		}
		public OrderCustomer CurrentCustomer
		{
			get
			{
				return uxOrderCustomer.SelectedItem as OrderCustomer;
			}
			//set
			//{
			//    (uxOrderCustomer.SelectedItem as OrderCustomer) = value;
			//}
		}

		public OrderView()
		{
			InitializeComponent();

			_productRepository = new ProductRepository();
			_orderRepository = new OrderRepository();

			//OrderCustomer test = new OrderCustomer();
			//test.Account.FirstName
			//OrderType status = new OrderType();
			//status.OrderTypeID

			uxOrderType.ItemsSource = SmallCollectionCache.Instance.OrderTypes;
			uxOrderStatus.ItemsSource = SmallCollectionCache.Instance.OrderStatuses;

			uxItems.ItemsSource = _productRepository.LoadAllFull();
		}

		private void uxOrderCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			OrderCustomer orderCustomer = CurrentCustomer;
			uxOrderItems.ItemsSource = orderCustomer.OrderItems;
			uxPayments.ItemsSource = orderCustomer.OrderPayments;
			//uxShippments.ItemsSource = orderCustomer.OrderShipments;  // This is what should be used but OrderCustomerId is null in OrderShipments table for the data - JHE
			uxShippments.ItemsSource = CurrentOrder.OrderShipments;

			uxSubTotal.Text = (orderCustomer.Total - orderCustomer.ShippingTotal - orderCustomer.TaxAmountTotal).ToMoneyString(CurrentCulture);
			uxShipping.Text = orderCustomer.ShippingTotal.ToMoneyString(CurrentCulture);
			uxTax.Text = (orderCustomer.TaxAmountTotal - orderCustomer.TaxAmountShipping).ToMoneyString(CurrentCulture);
			uxShippingTaxes.Text = orderCustomer.TaxAmountShipping.ToMoneyString(CurrentCulture);
			uxGrandTotal.Text = orderCustomer.Total.ToMoneyString(CurrentCulture);
		}

		private void uxAddItem_Click(object sender, RoutedEventArgs e)
		{
			Product product = (uxItems.SelectedItem as Product);

			if (product != null)
			{
				CurrentOrder.AddItem(CurrentCustomer, product, 1);
				CurrentOrder.Save();
			}
		}

		private void uxAddPayment_Click(object sender, RoutedEventArgs e)
		{
			OrderPayment orderPayment = FakeObjects.GetFakeOrderPayment();
			orderPayment.OrderID = CurrentOrder.OrderID;

			CurrentCustomer.OrderPayments.Add(orderPayment);
			CurrentOrder.Save();


			//Window window = new Window()
			//{
			//    Width = 500,
			//    Height = 500
			//};

			//OrderPayment orderPayment = FakeObjects.GetFakeOrderPayment();

			//PaymentEdit paymentEdit = new PaymentEdit();
			//paymentEdit.Saved += new NetSteps.Common.Events.EntityEventHandler<OrderNote>(paymentEdit_Saved);
			//paymentEdit.DataContext = orderPayment;
			//paymentEdit.Margin = new Thickness(10);

			//window.Content = paymentEdit;
			//window.Show();
		}

		//void paymentEdit_Saved(object sender, NetSteps.Common.Events.EntityEventHandlerArgs<OrderNote> e)
		//{
		//    throw new NotImplementedException();
		//}

		private void uxAddShippment_Click(object sender, RoutedEventArgs e)
		{
			OrderShipment orderShipment = FakeObjects.GetFakeOrderShipment();

			CurrentOrder.OrderShipments.Add(orderShipment);
			CurrentOrder.Save();
		}

		private void uxAddNote_Click(object sender, RoutedEventArgs e)
		{

			Note note = FakeObjects.GetFakeOrderNote();

			CurrentOrder.Notes.Add(note);
			CurrentOrder.Save();
			return;


			Window window = new Window()
			{
				Width = 500,
				Height = 500
			};

			//OrderNote orderNote = new OrderNote()
			//{
			//    // TODO: Finish this - JHE
			//};

			//OrderNoteEdit orderNoteEdit = new OrderNoteEdit();
			//orderNoteEdit.Saved +=new NetSteps.Common.Events.EntityEventHandler<OrderNote>(orderNoteEdit_Saved);
			//orderNoteEdit.DataContext = orderNote;
			//orderNoteEdit.Margin = new Thickness(10);


			//window.Content = orderNoteEdit;
			window.Show();
		}

		//void orderNoteEdit_Saved(object sender, NetSteps.Common.Events.EntityEventHandlerArgs<OrderNote> e)
		//{
		//    throw new NotImplementedException();
		//}

		private void uxSaveOrder_Click(object sender, RoutedEventArgs e)
		{
			CurrentOrder.Save();
		}

		private void uxDeleteShippment_Click(object sender, RoutedEventArgs e)
		{
			OrderShipment orderShipment = ((sender as Button).DataContext as OrderShipment);

			orderShipment.MarkAsDeleted();
			//CurrentOrder.OrderShipments.Remove(orderShipment);
			CurrentOrder.Save();
		}

		private void uxDeleteNote_Click(object sender, RoutedEventArgs e)
		{

		}

		private void uxDeletePayment_Click(object sender, RoutedEventArgs e)
		{
			OrderPayment orderPayment = ((sender as Button).DataContext as OrderPayment);

			orderPayment.MarkAsDeleted();
			//CurrentCustomer.OrderPaymentss.Remove(orderPayment);
			CurrentOrder.Save();
		}

		private void uxDeleteItem_Click(object sender, RoutedEventArgs e)
		{
			OrderItem orderItem = ((sender as Button).DataContext as OrderItem);

			orderItem.MarkAsDeleted();
			//CurrentCustomer.OrderItems.Remove(orderItem);
			CurrentOrder.Save();
		}
	}
}
