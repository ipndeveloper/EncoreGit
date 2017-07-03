using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NetSteps.Common.Base;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business.HelperObjects;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;

namespace AutoshipProcessor.Code
{
	public class Processor
	{
		private static readonly StringBuilder logMessage = new StringBuilder();
		private readonly DateTime processAutoshipsAfterDate = DateTime.Parse(ConfigurationManager.AppSettings["ProcessAutoshipsAfterDate"] ?? DateTime.Today.AddDays(-1).ToString("d"));

		// Include Error Logging - JHE
		// Use provider pattern to create an overridable Interface/Implementation of AutoshipProcessor for each client. - JHE



		public void UpdateNextRunDates(int autoshipScheduleID)
		{
			// TODO: Finish this to update all the NextRunDates on the Autoships (can call this method before running the autoship batches - JHE
			AutoshipOrder.Repository.UpdateNextRunDates(autoshipScheduleID);
		}

		public void ProcessConsultantsMonthlyReplenishmentAutoship()
		{
			var autoshipSchedule = SmallCollectionCache.Instance.AutoshipSchedules.GetById(Constants.AutoshipSchedule.ConsultantsMonthlyReplenishment.ToInt());
			UpdateNextRunDates(autoshipSchedule.AutoshipScheduleID);
			//var autoshipScheduleDays = SmallCollectionCache.Instance.AutoshipScheduleDays.Where(a => a.AutoshipScheduleID == autoshipSchedule.AutoshipScheduleID).ToList();
			//var autoshipScheduleDay = autoshipScheduleDays.FirstOrDefault(a => a.Day == DateTime.Today.Day);

			// TODO this line is temporary for testing - JHE
			LogMessage(string.Format("Beginning {0} Autoship Orders (Step 6)", autoshipSchedule.Name));

			var autoshipTemplates = AutoshipOrder.GetAutoshipTemplatesByNextDueDateByScheduleID(autoshipSchedule.AutoshipScheduleID, DateTime.Today.Date);
			Process("PC", autoshipTemplates);


			//if (autoshipScheduleDay != null)
			//{
			//    LogMessage(string.Format("Beginning {0} Autoship Orders (Step 6)", autoshipSchedule.Name));

			//    var autoshipTemplates = AutoshipOrder.GetAutoshipTemplatesByNextDueDateByScheduleID(autoshipScheduleDay.AutoshipScheduleDayID, DateTime.Today.Date);
			//    Process("PC", autoshipTemplates);
			//}
			//else
			//    LogMessage(string.Format("There are no batch days to process for AutoshipSchedule: '{0} - {1}' for today: {2}", autoshipSchedule.Name, autoshipSchedule.AutoshipScheduleID, DateTime.Today.Day));
		}

		public void Process(string autoshipScheduleType, List<AutoshipProcessInfo> autoshipProcessInfos)
		{
			try
			{
				if (DateTime.Today <= processAutoshipsAfterDate)
				{
					LogMessage(string.Format("Did not process '{0}' autoship type because it's off until after {1}", autoshipScheduleType, processAutoshipsAfterDate));
					return;
				}

				foreach (var autoshipProcessInfo in autoshipProcessInfos)
				{
					try
					{
						if (!autoshipProcessInfo.IsActive || !autoshipProcessInfo.IsDue)
							continue;

						//int newOrderID;
						//string msg;

						WriteToConsole(ConsoleColor.White, "Processing {0} {1} (AccountID: {2}, TemplateOrderID: {4}) {3}. ", autoshipProcessInfo.FirstName, autoshipProcessInfo.LastName, autoshipProcessInfo.AccountID, autoshipScheduleType, autoshipProcessInfo.TemplateOrderID);
						var result = GenerateChildOrder(autoshipProcessInfo.AutoshipOrderID, autoshipProcessInfo.AccountID, autoshipProcessInfo.EmailAddress.ToString());
						if (!result.Success)
						{
							AutoshipOrder.LogResults(autoshipProcessInfo.TemplateOrderID, result.Item.OrderID, false, result.Message);
							LogMessage(string.Format("Error Processing CRP Autoship Order for {0} {1} ({2}).  Message: {3}", autoshipProcessInfo.FirstName, autoshipProcessInfo.LastName, autoshipProcessInfo.AccountID, result.Message));
							WriteToConsole(ConsoleColor.Red, "  FAILED! {0}", result.Message);
						}
						else
						{
							AutoshipOrder.LogResults(autoshipProcessInfo.TemplateOrderID, result.Item.OrderID, true, result.Message);
							WriteToConsole(ConsoleColor.Green, "  Success");
						}
					}
					catch (Exception ex)
					{
						LogMessage("*** Error in Processing Autoship ID: " + autoshipProcessInfo.TemplateOrderID.ToString() + " ***");
						LogMessage("*** *** Message: " + ex.GetBaseException().Message);
					}
				}
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public BasicResponseItem<NetSteps.Data.Entities.Order> GenerateChildOrder(int autoshipOrderID, int accountID, string email)
		{
			BasicResponseItem<NetSteps.Data.Entities.Order> response = new BasicResponseItem<NetSteps.Data.Entities.Order>();
			try
			{
				AutoshipOrder autoshipOrder = AutoshipOrder.LoadFull(autoshipOrderID);

				var validateAutoshipResult = ValidateAutoship(autoshipOrder);
				if (!validateAutoshipResult.Success)
				{
					response.Message = validateAutoshipResult.Message;
					response.Success = false;
					return response;
				}

				#region Legacy Validation code (don't delete this yet) - JHE
				//Order order = autoshipOrder.Order; //new Order(templateId);
				////order.LoadAll();

				//order.OrderStatusID = Constants.OrderStatus.Pending.ToInt(); // Temporarily set to pending so that calctotals will calc tax
				//order.CalculateTotals();
				//order.OrderStatusID = Constants.OrderStatus.SubmittedTemplate.ToInt();
				//order.Save();

				//if (order.OrderCustomers.Count == 0)
				//{
				//    response.Message = "No Order Customer.";
				//    response.Success = false;
				//    return response;
				//}

				//if (order.ConsultantID != account.SponsorID && account.SponsorID != null)
				//{
				//    order.ConsultantID = account.SponsorID.ToInt();
				//    order.Save();
				//}

				//// RJB 11-20-09, This will change the customer on the order if they do not match the account of the autoship owner
				//ValidateCurrentCustomer(order, account);

				//OrderPayment payment = order.OrderCustomers[0].OrderPayments.FirstOrDefault(op => op.PaymentTypeID == Constants.PaymentType.CreditCard.ToInt());
				//if (payment == null)
				//{
				//    response.Message = "No Order Payment.";
				//    response.Success = false;
				//    return response;
				//}
				//payment.Amount = order.GrandTotal.ToDecimal();
				//payment.Save();

				//// Make sure it totals at least $80 for PC Autoship, or put in bad list
				//if (order.OrderTypeID == Constants.OrderType.PcAutoShipTemplate.ToInt() && order.Subtotal < 80.0M)
				//{
				//    response.Message = "Did not meet $80 requirement.";
				//    response.Success = false;
				//    return response;
				//}
				//else if (order.OrderTypeID == Constants.OrderType.ConsultantAutoShipTemplate.ToInt() && autoshipOrder.AutoshipScheduleID == 1) // $100 minimum for Consultant Autoship
				//{
				//    bool hasPulse = false;
				//    hasPulse = AutoshipOrder.LoadByAccount(accountId).Find(t1 => t1.AutoshipScheduleId == 3) != null;

				//    if (hasPulse && order.CommissionableTotal < 80.0M)
				//    {
				//        response.Message = "Did not meet $80 CV requirement (has pulse).";
				//        response.Success = false;
				//        return response;
				//    }
				//    else if (!hasPulse && order.CommissionableTotal < 100.0M)
				//    {
				//        response.Message = "Did not meet $100 CV requirement.";
				//        response.Success = false;
				//        return response;
				//    }
				//}
				//else if (order.OrderTypeID == 5 && autoshipOrder.AutoshipScheduleID == 3) // Pulse autoship
				//{
				//    if (order.OrderCustomers[0].OrderItems.Count != 1)
				//    {
				//        response.Message = "There was more than just a Pulse item on the order.";
				//        response.Success = false;
				//        return response;
				//    }
				//    else if (order.OrderCustomers[0].OrderItems[0].SKU != "PULSE01")
				//    {
				//        response.Message = "The item on the order was not the Pulse item.";
				//        response.Success = false;
				//        return response;
				//    }
				//}

				//Order newOrder = null;

				#endregion

				NetSteps.Data.Entities.Order newOrder = null;
				try
				{
					// Generate the new order
					//newOrderId = AutoshipOrder.CreateOrderFromTemplate(order, order.OrderTypeID == 4 ? 6 : 7); // HACK : 6 for PC Autship Order and 7 for Consultant Autoship Order
					//newOrder = new Order(order.Consultant, newOrderId);
					//newOrder.LoadAll();
					newOrder = autoshipOrder.GenerateChildOrderFromTemplate();
					response.Item = newOrder;

					var result = newOrder.SubmitOrder();
					if (!result.Success)
					{
						response.Message = result.Message;
						response.Success = false;
						return response;
					}
					else
					{
						if (autoshipOrder.AutoshipScheduleID == Constants.AutoshipSchedule.PulseMonthlySubscription.ToInt())
						{
							// Mark the order shipment as shipped so that the processor doesn't pick it up
							newOrder.OrderShipments[0].DateShipped = DateTime.Now;
							newOrder.Save();
						}

						autoshipOrder = AutoshipOrder.Load(autoshipOrderID);
						autoshipOrder.DateLastCreated = DateTime.Today;
						autoshipOrder.NextRunDate = autoshipOrder.GetNextDueDate();
						autoshipOrder.ConsecutiveOrders += 1;
						autoshipOrder.Save();

						// Send confirmation email(s)
						if (!NetSteps.Common.ApplicationContext.Instance.IsDeveloperEnvironment && autoshipOrder.AutoshipScheduleID != Constants.AutoshipSchedule.PulseMonthlySubscription.ToInt())
							SendEmailConfirmations(email, autoshipOrder.AccountInfo.AccountTypeID, autoshipOrder.NextRunDate.ToDateTime(), newOrder);
					}
				}
				catch (Exception ex)
				{
					EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
					response.Message = ex.Message;
					response.Success = false;
					return response;
				}

				response.Success = true;
				return response;
			}
			catch (Exception ex)
			{
				EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				response.Message = ex.Message;
				response.Success = false;
				return response;
			}
		}

		public BasicResponseItem<AutoshipOrder> ValidateAutoship(AutoshipOrder autoshipOrder)
		{
			BasicResponseItem<AutoshipOrder> response = new BasicResponseItem<AutoshipOrder>();

			bool saveOrder = false;

			if (autoshipOrder.Order.OrderCustomers == null || autoshipOrder.Order.OrderCustomers.Count == 0)
			{
				response.Message = "Error with TemplateOrder. Must contain at least 1 OrderCustomer.";
				response.Success = false;
				return response;
			}
			OrderCustomer orderCustomer = autoshipOrder.Order.OrderCustomers[0];

			// Check OrderType - JHE
			var orderTypeID = AutoshipOrder.BusinessLogic.GetDefaultOrderTypeID(autoshipOrder.AccountInfo.AccountTypeID, true);
			if (autoshipOrder.Order.OrderTypeID != orderTypeID)
			{
				autoshipOrder.Order.OrderTypeID = orderTypeID;
				saveOrder = true;
			}

			// TODO: Remove this check/fix later since it defaults ShippingMethodID if not set and that may not be desired behavior - JHE
			OrderShipment shipment = autoshipOrder.Order.GetDefaultShipmentNoDefault();
			if (shipment != null)
			{
				if (shipment.ShippingMethodID == null || shipment.ShippingMethodID.ToInt() == 0)
				{
					OrderCustomer customer = autoshipOrder.Order.OrderCustomers[0];

					autoshipOrder.Order.CalculateTotals();
					var shippingMethods = Calculations.ShippingCalculator.GetShippingMethodsWithRates(customer, shipment).OrderBy(sm => sm.ShippingAmount).ToList();

					// Set default shipping method
					if (shippingMethods.Count() > 0)
					{
						var cheapestShippingMethod = shippingMethods.OrderBy(sm => sm.ShippingAmount).First();

						shipment.ShippingMethodID = cheapestShippingMethod.ShippingMethodID;
						shipment.Name = cheapestShippingMethod.Name;
						saveOrder = true;
					}
				}
			}

			// Check Payment - JHE
			OrderPayment payment = orderCustomer.OrderPayments.GetByPaymentTypeID(Constants.PaymentType.CreditCard.ToInt());
			if (payment == null)
			{
				response.Message = "No Order Payment.";
				response.Success = false;
				return response;
			}
			else
			{
				// TODO: Check for empty payment address. - JHE
			}


			if (saveOrder)
				autoshipOrder.Save();

			// TODO: Finish this - JHE
			response.Success = true;
			return response;
		}



		private static bool SendEmailConfirmations(string email, int accountType, DateTime nextDue, NetSteps.Data.Entities.Order newOrder)
		{
			try
			{
				// TODO: Finish implementing this - JHE

				////Email to customer
				//string templateName = accountType == RodanFields.Objects.AccountType.PreferredCustomer ? "~ReplenishmentOrderConfirmationPC" : "~ReplenishmentOrderConfirmationCRP";
				//var emailTemplate = EmailTemplate.Load(BodyTextType.None, templateName);
				//if (emailTemplate == null)
				//{
				//    return false;
				//}

				//Dictionary<string, string> emailValues = new Dictionary<string, string>();
				//emailValues.Add("OrderDate", newOrder.CompleteDate.ToShortDateString());
				//emailValues.Add("OrderNumber", newOrder.OrderNumber);
				//emailValues.Add("OrderDetails", LogicFactory.Current.nsCoreLogicAdapter.GenerateOrderHTML(newOrder));
				////emailValues.Add("NextShipDate", nextDue.ToShortDateString());

				//RFMailMessage emailMessage = new RFMailMessage(emailTemplate, emailValues);
				//if (CustomConfigurationHandler.Config.Debug.IsDeveloperEnvironment)
				//    emailMessage.To.Add(CustomConfigurationHandler.Config.Debug.DevEmail);
				//else
				//    emailMessage.To.Add(email);

				//emailMessage.SendFromNoReply();
				return true;
			}
			catch (Exception ex)
			{
				EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return false;
			}
		}




		public void ValidateCurrentCustomer(NetSteps.Data.Entities.Order order, Account currentAccount)
		{
			if (order.OrderCustomers.Count > 1)
				throw new ApplicationException("Retail order can only have one order customer.");

			OrderCustomer orderCustomer = null;
			if (order.OrderCustomers != null && order.OrderCustomers.Count > 0)
				orderCustomer = order.OrderCustomers[0];

			int accountTypeID = currentAccount.AccountTypeID;
			int orderTypeID = order.OrderTypeID;

			//If customer started Anonymous/Retail order then set to correct type.
			//Other ordertypes should be set in context of shopping/enrollment.
			if (orderTypeID == Constants.OrderType.Retail.ToInt())
				order.OrderTypeID = AutoshipOrder.BusinessLogic.GetDefaultOrderTypeID(accountTypeID);

			if (orderCustomer != null)
			{
				if (orderCustomer.AccountID != currentAccount.AccountID)
				{
					//Set Order Customer to correct Customer.
					//This occurs when order originated at an anonymous order 
					//and then the customer logged-in / enrolled.
					//orderCustomer.SetIdentity(currentAccount.AccountID);
					//orderCustomer.AccountID.AccountNumber = currentAccount.AccountNumber;
					//orderCustomer.LoadAccountInfo();
					//orderCustomer.SaveOrderCustomer();

					orderCustomer.Account = currentAccount;
					orderCustomer.AccountID = currentAccount.AccountID;

					//Swap ship-to names when swapping customer.
					OrderShipment shipment = order.GetDefaultShipmentNoDefault();
					if (shipment != null)
					{
						shipment.Attention = String.Format("{0} {1}", currentAccount.FirstName, currentAccount.LastName);
						shipment.FirstName = currentAccount.FirstName;
						shipment.LastName = currentAccount.LastName;
						//shipment.Save();
					}
				}
				//else if (orderCustomer.Account.AccountNumber != currentAccount.AccountNumber)
				//{
				//    orderCustomer.Account.AccountNumber = currentAccount.AccountNumber;
				//}

				// Verify correct sponsor. If retail then use current site always.
				// For Consultants and PCs then use Sponsor current on file.
				if (orderCustomer.AccountInfo.SponsorID != order.ConsultantInfo.AccountID || order.ConsultantInfo.AccountID == 0)
				{
					// If PC or Consultant then always use Sponsor on file regardless of current site.
					if (accountTypeID == Constants.AccountType.Distributor.ToInt() || accountTypeID == Constants.AccountType.PreferredCustomer.ToInt())
					{
						if (currentAccount.SponsorID > 0)
						{
							Account consultant = Account.Load(currentAccount.SponsorID.ToInt());
							//consultant.LoadAccountInfo();
							order.Consultant = consultant;
							//order.Save();
						}
					}
				}
			}

			AutoshipOrder.BusinessLogic.ValidateOrderTypeByAccountType(orderTypeID, accountTypeID);
		}

		public void WriteToConsole(string format, params object[] arg)
		{
			WriteToConsole(ConsoleColor.White, format, arg);
		}
		public void WriteToConsole(ConsoleColor consoleColor, string format, params object[] arg)
		{
			Console.ForegroundColor = consoleColor;
			Console.Write(format, arg);
		}
		private static void LogMessage(string message)
		{
			try
			{
				logMessage.AppendLine("# " + DateTime.Now.ToString() + " | " + message);
				StreamWriter writer = new StreamWriter(ConfigurationManager.GetAppSetting<string>("LogFilePath") + "AutoProcessorLog.txt", true);
				writer.WriteLine("# " + DateTime.Now + " | " + message);
				writer.Close();
			}
			catch (Exception ex)
			{
				ex.ToString(); // to get rid of compiler warning
			}
		}

	}
}
