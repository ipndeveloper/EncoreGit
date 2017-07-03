using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NetSteps.Promotions.Common.Repository;
using NetSteps.Promotions.Plugins.Rewards.Base;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects;
using NetSteps.Promotions.Plugins.Common;
using NetSteps.Data.Common.Context;
using NetSteps.Encore.Core.IoC;
using NetSteps.Data.Common.Registries;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects.Components;
using System.Diagnostics.Contracts;
using NetSteps.OrderAdjustments.Common.Model;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Common.ModelConcrete;
using NetSteps.Promotions.Plugins.Common.Helpers;

namespace NetSteps.Promotions.Plugins.Rewards.Effects
{
	public class UserProductSelectionRewardHandler : BasePromotionRewardEffectExtensionHandler<IUserProductSelectionRewardEffect, IUserProductSelectionRewardEffectRepository, IEncorePromotionsPluginsUnitOfWork>, IOrderStepHandler, IPromotionRewardTargetingEffectExtensionHandler
	{
		public IEnumerable<IOrderAdjustmentProfileOrderItemTarget> CreateOrderLineModificationTargets(IOrderContext orderContext, IOrderAdjustmentProfile adjustmentProfile, IPromotionRewardEffect rewardEffect, PromotionQualificationResult matchResult, IPromotionRewardEffectResult effectResult)
		{
			Contract.Assert(adjustmentProfile != null);
			Contract.Assert(orderContext != null);
			Contract.Assert(rewardEffect != null);
			Contract.Assert(typeof(IUserProductSelectionRewardEffect).IsAssignableFrom(rewardEffect.Extension.GetType()));
			Contract.Assert(matchResult != null);

			List<IOrderAdjustmentProfileOrderItemTarget> targets = null;
			var selectionEffect = (IUserProductSelectionRewardEffect)rewardEffect.Extension;
			foreach (var customer in orderContext.Order.OrderCustomers)
			{
				if (matchResult.MatchForCustomerAccountID(customer.AccountID))
				{
					targets = targets ?? new List<IOrderAdjustmentProfileOrderItemTarget>();
					// has actual AccountID
					var parser = Create.New<IPromotionInjectedOrderStepReferenceParser>();
					parser.ComponentHandlerProviderKey = this.GetProviderKey();
					parser.ComponentExtensionID = selectionEffect.PromotionRewardEffectID;
					parser.CustomerAccountID = customer.AccountID;
					var referenceID = parser.GetStepReferenceID();

					var existingOrderStep = orderContext.InjectedOrderSteps.OfType<IUserProductSelectionOrderStep>().Where(step => step.OrderStepReferenceID == referenceID).SingleOrDefault();

					if (existingOrderStep == null)
					{
						// has no AccountID - currently using "0" as the AccountID
						parser.CustomerAccountID = 0;
						var unauthenticatedReferenceID = parser.GetStepReferenceID();
						existingOrderStep = orderContext.InjectedOrderSteps.OfType<IUserProductSelectionOrderStep>().Where(step => step.OrderStepReferenceID == unauthenticatedReferenceID).SingleOrDefault();

						// check to find out if the user has not been logged in and accountid has changed
						if (orderContext.Order.OrderCustomers.Count == 1 && existingOrderStep != null)
						{
							// set the unauthenticated step values to the correct customer
							existingOrderStep.CustomerAccountID = customer.AccountID;
							existingOrderStep.OrderStepReferenceID = referenceID;
						}
					}

					if (existingOrderStep == null)
					{
						// this is either a new handler call or the order context has been cleaned.
						var newOrderStep = Create.New<IUserProductSelectionOrderStep>();
						newOrderStep.MaximumOptionSelectionCount = selectionEffect.SelectionsAllowed;
						newOrderStep.CustomerAccountID = customer.AccountID;
						newOrderStep.OrderStepReferenceID = referenceID;
						foreach (var option in selectionEffect.Selections)
						{
							var customerOption = Create.New<IProductOption>();
							customerOption.ProductID = option.ProductID;
							customerOption.Quantity = option.Quantity;
							newOrderStep.AvailableOptions.Add(customerOption);
						}
						adjustmentProfile.AddedOrderSteps.Add(newOrderStep);
					}
					else
					{
						if (ValidateStepResponse(existingOrderStep))
						{
							var selectionResponse = (IUserProductSelectionOrderStepResponse)existingOrderStep.Response;
							foreach (var customerSelection in selectionResponse.SelectedOptions)
							{
								var target = Create.New<IOrderAdjustmentProfileOrderItemTarget>();
								target.OrderCustomerAccountID = customer.AccountID;
								target.ProductID = customerSelection.ProductID;
								target.Quantity = customerSelection.Quantity;
								adjustmentProfile.OrderLineModificationTargets.Add(target);

								var addAdjust = Create.New<IOrderAdjustmentProfileOrderLineModification>();
								addAdjust.Description = String.Format("Added {0}.", target.Quantity);
								addAdjust.ModificationOperationID = (int)OrderAdjustmentOrderLineOperationKind.AddedItem;
								addAdjust.ModificationValue = target.Quantity;
								addAdjust.Property = "Added";
								target.Modifications.Add(addAdjust);

								targets.Add(target);
							}
						}
						else
						{
							// do nothing.
						}
						adjustmentProfile.AddedOrderSteps.Add(existingOrderStep);
					}
				}
			}
			return targets ?? Enumerable.Empty<IOrderAdjustmentProfileOrderItemTarget>();
		}

		public override bool AreEqual(Promotions.Common.Model.IPromotionRewardEffectExtension effect1, Promotions.Common.Model.IPromotionRewardEffectExtension effect2)
		{
			Contract.Assert(effect1 != null);
			Contract.Assert(effect2 != null);
			Contract.Assert(effect1 is IUserProductSelectionRewardEffect);
			Contract.Assert(effect2 is IUserProductSelectionRewardEffect);

			var extension1 = effect1 as IUserProductSelectionRewardEffect;
			var extension2 = effect2 as IUserProductSelectionRewardEffect;
            if (extension1.SelectionsAllowed != extension2.SelectionsAllowed)
				return false;
			if (extension1.Selections.Any(selection1 => !extension2.Selections.Any(selection2 => selection1.ProductID == selection2.ProductID && selection1.Quantity == selection2.Quantity)))
				return false;
			if (extension2.Selections.Any(selection2 => !extension1.Selections.Any(selection1 => selection2.ProductID == selection1.ProductID && selection2.Quantity == selection1.Quantity)))
				return false;
			return true;
		}

		public override string GetProviderKey()
		{
			return NetStepsPromotionRewardEffectExtensionProviderKeys.UserProductSelection;
		}

		public bool ValidateStepResponse(IOrderStep step)
		{
			if (step.Response == null)
			{
				return false;
			}
			else
			{
				if (!typeof(IUserProductSelectionOrderStep).IsAssignableFrom(step.GetType()))
				{
					throw new InvalidOperationException(String.Format("Promotions.Plugins.Rewards.Effects.UserProductSelectionRewardHandler expected step of type IUserProductSelectionOrderStep but got a step of type {0}.", step.GetType().ToString()));
				}
				var selectionStep = (IUserProductSelectionOrderStep)step;
				var selectionResponse = (IUserProductSelectionOrderStepResponse)selectionStep.Response;
				if (selectionResponse.SelectedOptions.Count() > selectionStep.MaximumOptionSelectionCount)
				{
					// there are too many selected.
					selectionResponse.SelectedOptions.Clear();
					return false;
				}
				bool hasInvalidOption = false;
				foreach (var selectedOption in selectionResponse.SelectedOptions)
				{
					if (!selectionStep.AvailableOptions.Any(option => option.ProductID == selectedOption.ProductID && option.Quantity == selectedOption.Quantity))
					{
						hasInvalidOption = true;
						break;
					}
				}
				if (hasInvalidOption)
				{
					selectionResponse.SelectedOptions.Clear();
					return false;
				}
				else
				{
					return true;
				}
			}
		}

		public bool VerifyStepCompletion(IOrderStep step)
		{
			bool validated = ValidateStepResponse(step);
			if (!validated)
			{
				return false;
			}
			else
			{
				var selectionStep = (IUserProductSelectionOrderStep)step;
				var selectionResponse = (IUserProductSelectionOrderStepResponse)selectionStep.Response;
				return selectionResponse.SelectedOptions.Count == selectionStep.MaximumOptionSelectionCount;
			}
		}

		public override void CheckValidity(string promotionRewardEffectKey, IUserProductSelectionRewardEffect rewardEffect, IPromotionState state)
		{
			if (rewardEffect.Selections == null || !rewardEffect.Selections.Any())
			{
				state.AddConstructionError
					(
						String.Format("Promotion Qualification {0}", promotionRewardEffectKey),
						"Product Selections collection is null or empty."
					);
			}

			if (rewardEffect.SelectionsAllowed <= 0)
			{
				state.AddConstructionError
					(
						String.Format("Promotion Reward Effect {0}", promotionRewardEffectKey),
						"Selections Allowed cannot be less than or equal to 0."
					);
			}
		}
	}
}
