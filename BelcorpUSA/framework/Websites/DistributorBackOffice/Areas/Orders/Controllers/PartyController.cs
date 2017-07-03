using System.Linq;
using System.Web.Mvc;
using DistributorBackOffice.Areas.Orders.Helpers;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Extensions;
using Constants = NetSteps.Data.Entities.Constants;

namespace DistributorBackOffice.Areas.Orders.Controllers
{
	/// <summary>
	/// The party controller, kari.
	/// </summary>
	public class PartyController : AbstractPartyAndFundraiserController
	{
		[PartySetup]
		public virtual ActionResult DetermineStep(int partyId)
		{
            // probadiña
			if (CurrentParty == null || CurrentParty.PartyID != partyId
				|| OrderContext.Order == null || CurrentParty.Order == null
				|| OrderContext.Order.OrderID != CurrentParty.Order.OrderID)
			{
				var party = Party.LoadFull(partyId);
				if (!CurrentAccountCanAccessParty(party)) return this.RedirectToSafePage();
				CurrentParty = party;
				OrderContext.Order = CurrentParty.Order;
				SetHasChangesToFalseOnAllItems();
			}

			//Check If Hostess Exists
			if (CurrentParty.Order.GetHostess() == null)
			{
				OrderContext.Order.AsOrder().OrderPendingState = Constants.OrderPendingStates.Open;
				return this.RedirectToAction("Index");
			}

			//the party was submitted - DES
			if (OrderContext.Order.AsOrder().IsCommissionable())
			{
				return this.RedirectToAction(this.PartySubmittedActionName);
			}

			//Any payments means that we were on the payment step - DES
			if (CurrentParty.Order.OrderPayments.Count > 0
				|| CurrentParty.Order.OrderCustomers.Any(oc => oc.OrderPayments.Count > 0))
			{
				OrderContext.Order.AsOrder().OrderPendingState = Constants.OrderPendingStates.Quote;
				OrderService.UpdateOrder(OrderContext);
				return this.RedirectToAction("Payments");
			}

			//Any host credit or percent off items means that we were on the host rewards step - DES
			// Or any invalid Hostess rewards - JHE
			if (CurrentParty.Order.OrderCustomers.SelectMany(oc => oc.OrderItems).Any(oi => oi.IsHostReward)
				|| !CurrentParty.Order.ValidateHostessRewards().Success)
			{
				OrderContext.Order.AsOrder().OrderPendingState = Constants.OrderPendingStates.Open;
				return this.RedirectToAction("HostRewards");
			}

			//Any order customers means that we were on the cart step - DES
			if (CurrentParty.Order.OrderCustomers.Count > 0)
			{
				OrderContext.Order.AsOrder().OrderPendingState = Constants.OrderPendingStates.Open;
				return this.RedirectToAction("Cart");
			}

			//Something bad happened, let's try to start over - DES
			return this.RedirectToAction("Index");
		}
	}
}
