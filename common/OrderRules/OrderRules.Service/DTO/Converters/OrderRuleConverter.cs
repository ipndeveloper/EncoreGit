using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OrderRules.Core.Model;
using OrderRules.Data.Repository.Interface;
//using NetSteps.Data.Entities;
using NetSteps.Encore.Core.IoC;

namespace OrderRules.Service.DTO.Converters
{
    public class OrderRuleConverter<TRule, TModel>
		where TRule : Rules
		where TModel : RulesDTO
	{

		#region ToModel

        public virtual TModel Convert(TRule Rule)
		{
			var model = Create.New<TModel>();
			model.RuleID = Rule.RuleID;
            SetDates(model, Rule.StartDate, Rule.EndDate);
            SetName(model, Rule.Name);
            SetTermName(model, Rule.TermName);
            SetRuleStatus(model, Rule.RuleStatus, Rule.RuleStatuses);
            SetRuleValidations(model, Rule.RuleValidations.ToList());
            return model;
		}

		#region Helpers



        internal void SetDates(TModel model, DateTime? pStartDate, DateTime? pEndDate)
		{
            model.StartDate = pStartDate;
            model.EndDate = pEndDate;
            model.HasDates = pStartDate.HasValue || pEndDate.HasValue;
		}

        internal void SetName(TModel model, string pName)
		{
            model.Name = pName;
		}

        internal void SetTermName(TModel model, string pTermName)
		{
            model.TermName = pTermName;
		}

        internal void SetRuleStatus(TModel model, int pRuleStatus, RuleStatuses pRuleStatuses)
        {
            model.RuleStatus = pRuleStatus;
            model.RuleStatuses = pRuleStatuses;
        }

        internal void SetRuleValidations(TModel model, List<RuleValidations> pRuleValidations)
        {
            if (pRuleValidations.Count > 0)
            {
                foreach (var ruleValidations in pRuleValidations)
                {
                    var ruleValidationsDTO = Create.New<RuleValidationsDTO>();
                    ruleValidationsDTO.RuleValidationID = ruleValidations.RuleValidationID;
                    ruleValidationsDTO.RuleID = ruleValidations.RuleID;
                    ruleValidationsDTO.ProductTypeIDs = ruleValidations.RuleValidationProductTypeLists != null ? ruleValidations.RuleValidationProductTypeLists.RuleValidationProductTypeListItems.Select(x => x.ProductTypeID).ToList() : new List<int>();
                    ruleValidationsDTO.ProductIDs = ruleValidations.RuleValidationProductLists != null ? ruleValidations.RuleValidationProductLists.RuleValidationProductListItems.Select(x => x.ProductID).ToList(): new List<int>();
                    ruleValidationsDTO.StoreFrontIDs = ruleValidations.RuleValidationStoreFrontLists != null ? ruleValidations.RuleValidationStoreFrontLists.RuleValidationStoreFrontListItems.Select(x => x.StoreFrontID).ToList() : new List<int>();
                    ruleValidationsDTO.AccountIDs = ruleValidations.RuleValidationAccountLists != null ? ruleValidations.RuleValidationAccountLists.RuleValidationAccountListItems.Select(x => x.AccountID).ToList() : new List<int>();
                    ruleValidationsDTO.AccountTypeIDs = ruleValidations.RuleValidationAccountTypeLists != null ? ruleValidations.RuleValidationAccountTypeLists.RuleValidationAccountTypeListItems.Select(x => x.AccountTypeID).ToList() : new List<short>();
                    ruleValidationsDTO.OrderTypeIDs = ruleValidations.RuleValidationOrderTypeLists != null ? ruleValidations.RuleValidationOrderTypeLists.RuleValidationOrderTypeListItems.Select(x => x.OrderTypeID).ToList() : new List<short>();

                    if (ruleValidations.RuleValidationCustomerPriceTypeTotalRangesKeys != null)
                    {
                        foreach (var customerPriceTotal in ruleValidations.RuleValidationCustomerPriceTypeTotalRangesKeys.RuleValidationCustomerPriceTypeTotalRanges)
                        {
                            foreach (var customerPriceTotalItems in customerPriceTotal.RuleValidationCustomerPriceTypeTotalRangeCurrencyAmounts)
                            {
                                var customerPriceTotalDTO = Create.New<CustomerPriceTotalDTO>();
                                customerPriceTotalDTO.CustomerPriceTotalID = customerPriceTotal.RuleValidationCustomerPriceTypeTotalRangeID;
                                customerPriceTotalDTO.RuleValidationID = (int)customerPriceTotal.RuleValidationID;
                                customerPriceTotalDTO.ProductPriceTypeID = customerPriceTotal.ProductPriceTypeID;

                                customerPriceTotalDTO.CurrencyID = customerPriceTotalItems.CurrencyID;
                                customerPriceTotalDTO.MaximumAmount = customerPriceTotalItems.MaximumAmount;
                                customerPriceTotalDTO.MinimumAmount = customerPriceTotalItems.MinimumAmount;
                                ruleValidationsDTO.CustomerPriceTotalDTO.Add(customerPriceTotalDTO);
                            }
                        }
                    }
                    if (ruleValidations.RuleValidationCustomerSubtotalRanges != null)
                    {
                        foreach (var customerPriceSubTotal in ruleValidations.RuleValidationCustomerSubtotalRanges.RuleValidationCustomerSubtotalRangeCurrencyAmounts)
                        {
                            var customerPriceSubTotalDTO = Create.New<CustomerPriceSubTotalDTO>();
                            customerPriceSubTotalDTO.CustomerPriceSubTotalID = customerPriceSubTotal.RuleValidationCustomerSubtotalRangeCurrencyAmountID;
                            customerPriceSubTotalDTO.RuleValidationID = (int)customerPriceSubTotal.RuleValidationID;
                            customerPriceSubTotalDTO.CurrencyID = customerPriceSubTotal.CurrencyID;
                            customerPriceSubTotalDTO.MaximumAmount = customerPriceSubTotal.MaximumAmount;
                            customerPriceSubTotalDTO.MinimumAmount = customerPriceSubTotal.MinimumAmount;
                            ruleValidationsDTO.CustomerPriceSubTotalDTO.Add(customerPriceSubTotalDTO);
                        }
                    }
                    model.RuleValidationsDTO.Add(ruleValidationsDTO);
                }
            }
            else
            {
                model.RuleValidationsDTO = new List<RuleValidationsDTO>();
            }
        }

        #endregion

		#endregion
        
        #region ToRule

        public virtual TRule Convert(TModel model)
		{
			var rule = Create.New<TRule>();
            if (model.RuleID > 0)
            {
                rule = (TRule)Create.New<IRulesRepository>().GetByID(model.RuleID);
            }
            SetRuleDates(rule, model.HasDates, model.StartDate, model.EndDate);
            SetRuleName(rule, model.Name);
            SetRuleTermName(rule, model.TermName);
            SetRuleStatus(rule, model.RuleStatus, model.RuleStatuses);
            SetRuleValidations(rule, model.RuleValidationsDTO.ToList());

            return rule;
		}

       

		#region Helpers

        public void SetRuleDates(TRule rule, bool pHasDates, DateTime? pStartDate, DateTime? pEndDate)
		{
            if (pHasDates)
            {
                rule.StartDate = pStartDate;
                rule.EndDate = pEndDate;
            }
            else
            {
                rule.StartDate = null;
                rule.EndDate = null;
            }
		}

        public void SetRuleName(TRule rule, string pName)
		{
            rule.Name = pName;
		}

        public void SetRuleTermName(TRule rule, string pName)
		{
            rule.TermName = pName;
		}

        public void SetRuleStatus(TRule rule, int pRuleStatus, RuleStatuses pRuleStatuses)
		{
            if (pRuleStatus > 0)
            {
                rule.RuleStatus = pRuleStatus;
                rule.RuleStatuses = pRuleStatuses;
            }
		}
        
        public void SetRuleValidations(TRule rule, List<RuleValidationsDTO> pRuleValidationsDTO)
		{
            var ruleValidations = rule.RuleID > 0 ? rule.RuleValidations.FirstOrDefault() : Create.New<RuleValidations>();
            foreach (var ruleValidationDTO in pRuleValidationsDTO)
            {
                ruleValidations.RuleValidationID = ruleValidationDTO.RuleValidationID;
                ruleValidations.RuleID = ruleValidationDTO.RuleID;

                /*Product Types*/
                if (ruleValidationDTO.ProductTypeIDs != null)
                {
                    if (ruleValidationDTO.ProductTypeIDs.Count > 0)
                    {
                        if (ruleValidations.RuleValidationProductTypeLists != null)
                        {
                            var productTypeItems = ruleValidations.RuleValidationProductTypeLists.RuleValidationProductTypeListItems;
                            var existing = productTypeItems.Select(x => x.ProductTypeID).ToList();
                            var added = ruleValidationDTO.ProductTypeIDs.Where(newtype => !(existing.Contains(newtype))).ToList();
                            var removed = productTypeItems.Where(x => !(ruleValidationDTO.ProductTypeIDs.Contains(x.ProductTypeID))).ToList();
                            foreach (var newType in added)
                            {
                                var ruleProductTypeItems = Create.New<RuleValidationProductTypeListItems>();
                                ruleProductTypeItems.RuleValidationID = ruleValidations.RuleValidationProductTypeLists.RuleValidationID;
                                ruleProductTypeItems.ProductTypeID = newType;
                                ruleProductTypeItems.RuleValidationProductTypeLists = ruleValidations.RuleValidationProductTypeLists;
                                productTypeItems.Add(ruleProductTypeItems);
                            }
                            foreach (var oldType in removed)
                            {
                                productTypeItems.Remove(oldType);
                            }
                        }
                        else
                        {
                            var ruleProductType = Create.New<RuleValidationProductTypeLists>();
                            ruleProductType.IsIncludeList = false;
                            ruleProductType.RuleValidationID = ruleValidations.RuleValidationID;
                            ruleProductType.RuleValidations = ruleValidations;
                            foreach (var newType in ruleValidationDTO.ProductTypeIDs)
                            {
                                var ruleProductTypeItems = Create.New<RuleValidationProductTypeListItems>();
                                ruleProductTypeItems.RuleValidationID = ruleProductType.RuleValidationID;
                                ruleProductTypeItems.ProductTypeID = newType;
                                ruleProductTypeItems.RuleValidationProductTypeLists = ruleProductType;
                                ruleProductType.RuleValidationProductTypeListItems.Add(ruleProductTypeItems);
                            }
                            ruleValidations.RuleValidationProductTypeLists = ruleProductType;
                        }
                    }
                }
                else
                {
                    if (ruleValidations.RuleValidationProductTypeLists != null)
                    {
                        ruleValidations.RuleValidationProductTypeLists.RuleValidationProductTypeListItems.Clear();
                        ruleValidations.RuleValidationProductTypeLists = null;
                    }
                }

                /*Product IDS*/
                if (ruleValidationDTO.ProductIDs != null)
                {
                    if (ruleValidationDTO.ProductIDs.Count > 0)
                    {
                        if (ruleValidations.RuleValidationProductLists != null)
                        {
                            var products = ruleValidations.RuleValidationProductLists.RuleValidationProductListItems;
                            var existing = products.Select(x => x.ProductID).ToList();
                            var added = ruleValidationDTO.ProductIDs.Where(newtype => !(existing.Contains(newtype))).ToList();
                            var removed = products.Where(x => !(ruleValidationDTO.ProductIDs.Contains(x.ProductID))).ToList();
                            foreach (var newType in added)
                            {
                                var ruleProducts = Create.New<RuleValidationProductListItems>();
                                ruleProducts.RuleValidationID = ruleValidations.RuleValidationID;
                                ruleProducts.ProductID = newType;
                                ruleProducts.RuleValidationProductLists = ruleValidations.RuleValidationProductLists;
                                products.Add(ruleProducts);
                            }
                            foreach (var oldType in removed)
                            {
                                products.Remove(oldType);
                            }
                        }
                        else
                        {
                            var ruleProducts = Create.New<RuleValidationProductLists>();
                            ruleProducts.IsIncludeList = false;
                            ruleProducts.RuleValidationID = ruleValidations.RuleValidationID;
                            ruleProducts.RuleValidations = ruleValidations;
                            foreach (var newType in ruleValidationDTO.ProductIDs)
                            {
                                var ruleProductItems = Create.New<RuleValidationProductListItems>();
                                ruleProductItems.RuleValidationID = ruleProducts.RuleValidationID;
                                ruleProductItems.ProductID = newType;
                                ruleProductItems.RuleValidationProductLists = ruleProducts;
                                ruleProducts.RuleValidationProductListItems.Add(ruleProductItems);
                            }
                            ruleValidations.RuleValidationProductLists = ruleProducts;
                        }
                    }
                }
                else
                {
                    if (ruleValidations.RuleValidationProductLists != null)
                    {
                        ruleValidations.RuleValidationProductLists.RuleValidationProductListItems.Clear();
                        ruleValidations.RuleValidationProductLists = null;
                    }
                }

                /*Account IDS*/
                if (ruleValidationDTO.AccountIDs != null)
                {
                    if (ruleValidationDTO.AccountIDs.Count > 0)
                    {
                        if (ruleValidations.RuleValidationAccountLists != null)
                        {
                            var accountItems = ruleValidations.RuleValidationAccountLists.RuleValidationAccountListItems;
                            var existing = accountItems.Select(x => x.AccountID).ToList();
                            var added = ruleValidationDTO.AccountIDs.Where(newtype => !(existing.Contains(newtype))).ToList();
                            var removed = accountItems.Where(x => !(ruleValidationDTO.AccountIDs.Contains(x.AccountID))).ToList();
                            foreach (var newType in added)
                            {
                                var ruleAccountItems = Create.New<RuleValidationAccountListItems>();
                                ruleAccountItems.RuleValidationID = ruleValidations.RuleValidationAccountLists.RuleValidationID;
                                ruleAccountItems.AccountID = newType;
                                ruleAccountItems.RuleValidationAccountLists = ruleValidations.RuleValidationAccountLists;
                                accountItems.Add(ruleAccountItems);
                            }
                            foreach (var oldType in removed)
                            {
                                accountItems.Remove(oldType);
                            }
                        }
                        else
                        {
                            var ruleAccounts = Create.New<RuleValidationAccountLists>();
                            ruleAccounts.IsIncludeList = false;
                            ruleAccounts.RuleValidationID = ruleValidations.RuleValidationID;
                            ruleAccounts.RuleValidations = ruleValidations;
                            foreach (var newType in ruleValidationDTO.AccountIDs)
                            {
                                var ruleAccountItems = Create.New<RuleValidationAccountListItems>();
                                ruleAccountItems.RuleValidationID = ruleAccounts.RuleValidationID;
                                ruleAccountItems.AccountID = newType;
                                ruleAccountItems.RuleValidationAccountLists = ruleAccounts;
                                ruleAccounts.RuleValidationAccountListItems.Add(ruleAccountItems);
                            }
                            ruleValidations.RuleValidationAccountLists = ruleAccounts;
                        }
                    }
                }
                else
                {
                    if (ruleValidations.RuleValidationAccountLists != null)
                    {
                        ruleValidations.RuleValidationAccountLists.RuleValidationAccountListItems.Clear();
                        ruleValidations.RuleValidationAccountLists = null;
                    }
                }


                /*Account Types*/
                if (ruleValidationDTO.AccountTypeIDs != null)
                {
                    if (ruleValidationDTO.AccountTypeIDs.Count > 0)
                    {
                        if (ruleValidations.RuleValidationAccountTypeLists != null)
                        {
                            var accountTypeItems = ruleValidations.RuleValidationAccountTypeLists.RuleValidationAccountTypeListItems;
                            var existing = accountTypeItems.Select(x => x.AccountTypeID).ToList();
                            var added = ruleValidationDTO.AccountTypeIDs.Where(newtype => !(existing.Contains(newtype))).ToList();
                            var removed = accountTypeItems.Where(x => !(ruleValidationDTO.AccountTypeIDs.Contains(x.AccountTypeID))).ToList();
                            foreach (var newType in added)
                            {
                                var ruleAccountTypeItems = Create.New<RuleValidationAccountTypeListItems>();
                                ruleAccountTypeItems.RuleValidationID = ruleValidations.RuleValidationAccountTypeLists.RuleValidationID;
                                ruleAccountTypeItems.AccountTypeID = newType;
                                ruleAccountTypeItems.RuleValidationAccountTypeLists = ruleValidations.RuleValidationAccountTypeLists;
                                accountTypeItems.Add(ruleAccountTypeItems);
                            }
                            foreach (var oldType in removed)
                            {
                                accountTypeItems.Remove(oldType);
                            }
                        }
                        else
                        {
                            var ruleAccountType = Create.New<RuleValidationAccountTypeLists>();
                            ruleAccountType.IsIncludeList = false;
                            ruleAccountType.RuleValidationID = ruleValidations.RuleValidationID;
                            ruleAccountType.RuleValidations = ruleValidations;
                            foreach (var newType in ruleValidationDTO.AccountTypeIDs)
                            {
                                var ruleAccountTypeItems = Create.New<RuleValidationAccountTypeListItems>();
                                ruleAccountTypeItems.RuleValidationID = ruleAccountType.RuleValidationID;
                                ruleAccountTypeItems.AccountTypeID = newType;
                                ruleAccountTypeItems.RuleValidationAccountTypeLists = ruleAccountType;
                                ruleAccountType.RuleValidationAccountTypeListItems.Add(ruleAccountTypeItems);
                            }
                            ruleValidations.RuleValidationAccountTypeLists = ruleAccountType;
                        }
                    }
                }
                else
                {
                    if (ruleValidations.RuleValidationAccountTypeLists != null)
                    {
                        ruleValidations.RuleValidationAccountTypeLists.RuleValidationAccountTypeListItems.Clear();
                        ruleValidations.RuleValidationAccountTypeLists = null;
                    }
                }

                /*Store Fronts*/
                if (ruleValidationDTO.StoreFrontIDs != null)
                {
                    if (ruleValidationDTO.StoreFrontIDs.Count > 0)
                    {
                        if (ruleValidations.RuleValidationStoreFrontLists != null)
                        {
                            var storeFrontItems = ruleValidations.RuleValidationStoreFrontLists.RuleValidationStoreFrontListItems;
                            var existing = storeFrontItems.Select(x => x.StoreFrontID).ToList();
                            var added = ruleValidationDTO.StoreFrontIDs.Where(newtype => !(existing.Contains(newtype))).ToList();
                            var removed = storeFrontItems.Where(x => !(ruleValidationDTO.StoreFrontIDs.Contains(x.StoreFrontID))).ToList();
                            foreach (var newType in added)
                            {
                                var ruleStoreFrontItems = Create.New<RuleValidationStoreFrontListItems>();
                                ruleStoreFrontItems.RuleValidationID = ruleValidations.RuleValidationStoreFrontLists.RuleValidationID;
                                ruleStoreFrontItems.StoreFrontID = newType;
                                ruleStoreFrontItems.RuleValidationStoreFrontLists = ruleValidations.RuleValidationStoreFrontLists;
                                storeFrontItems.Add(ruleStoreFrontItems);
                            }
                            foreach (var oldType in removed)
                            {
                                storeFrontItems.Remove(oldType);
                            }
                        }
                        else
                        {
                            var ruleStoreFronts = Create.New<RuleValidationStoreFrontLists>();
                            ruleStoreFronts.IsIncludeList = false;
                            ruleStoreFronts.RuleValidationID = ruleValidations.RuleValidationID;
                            ruleStoreFronts.RuleValidations = ruleValidations;
                            foreach (var newType in ruleValidationDTO.StoreFrontIDs)
                            {
                                var ruleStoreFrontItems = Create.New<RuleValidationStoreFrontListItems>();
                                ruleStoreFrontItems.RuleValidationID = ruleStoreFronts.RuleValidationID;
                                ruleStoreFrontItems.StoreFrontID = newType;
                                ruleStoreFrontItems.RuleValidationStoreFrontLists = ruleStoreFronts;
                                ruleStoreFronts.RuleValidationStoreFrontListItems.Add(ruleStoreFrontItems);
                            }
                            ruleValidations.RuleValidationStoreFrontLists = ruleStoreFronts;
                        }
                    }
                }
                else
                {
                    if (ruleValidations.RuleValidationStoreFrontLists != null)
                    {
                        ruleValidations.RuleValidationStoreFrontLists.RuleValidationStoreFrontListItems.Clear();
                        ruleValidations.RuleValidationStoreFrontLists = null;
                    }
                }

                /*Order Types*/
                if (ruleValidationDTO.OrderTypeIDs != null)
                {
                    if (ruleValidationDTO.OrderTypeIDs.Count > 0)
                    {
                        if (ruleValidations.RuleValidationOrderTypeLists != null)
                        {
                            var orderTypeItems = ruleValidations.RuleValidationOrderTypeLists.RuleValidationOrderTypeListItems;
                            var existing = orderTypeItems.Select(x => x.OrderTypeID).ToList();
                            var added = ruleValidationDTO.OrderTypeIDs.Where(newtype => !(existing.Contains(newtype))).ToList();
                            var removed = orderTypeItems.Where(x => !(ruleValidationDTO.OrderTypeIDs.Contains(x.OrderTypeID))).ToList();
                            foreach (var newType in added)
                            {
                                var ruleOrderTypeItems = Create.New<RuleValidationOrderTypeListItems>();
                                ruleOrderTypeItems.RuleValidationID = ruleValidations.RuleValidationOrderTypeLists.RuleValidationID;
                                ruleOrderTypeItems.OrderTypeID = newType;
                                ruleOrderTypeItems.RuleValidationOrderTypeLists = ruleValidations.RuleValidationOrderTypeLists;
                                orderTypeItems.Add(ruleOrderTypeItems);
                            }
                            foreach (var oldType in removed)
                            {
                                orderTypeItems.Remove(oldType);
                            }
                        }
                        else
                        {
                            var ruleOrderType = Create.New<RuleValidationOrderTypeLists>();
                            ruleOrderType.IsIncludeList = false;
                            ruleOrderType.RuleValidationID = ruleValidations.RuleValidationID;
                            ruleOrderType.RuleValidations = ruleValidations;
                            foreach (var newType in ruleValidationDTO.OrderTypeIDs)
                            {
                                var ruleOrderTypeItems = Create.New<RuleValidationOrderTypeListItems>();
                                ruleOrderTypeItems.RuleValidationID = ruleOrderType.RuleValidationID;
                                ruleOrderTypeItems.OrderTypeID = newType;
                                ruleOrderTypeItems.RuleValidationOrderTypeLists = ruleOrderType;
                                ruleOrderType.RuleValidationOrderTypeListItems.Add(ruleOrderTypeItems);
                            }
                            ruleValidations.RuleValidationOrderTypeLists = ruleOrderType;
                        }
                    }
                }
                else
                {
                    if (ruleValidations.RuleValidationOrderTypeLists != null)
                    {
                        ruleValidations.RuleValidationOrderTypeLists.RuleValidationOrderTypeListItems.Clear();
                        ruleValidations.RuleValidationOrderTypeLists = null;
                    }
                }

                /*Customer PriceType Total*/
                if (ruleValidationDTO.CustomerPriceTotalDTO != null)
                {
                    if (ruleValidationDTO.CustomerPriceTotalDTO.Count > 0)
                    {
                        if (ruleValidations.RuleValidationCustomerPriceTypeTotalRangesKeys != null)
                        {
                            var existingCustomerPriceTypeTotal = ruleValidations.RuleValidationCustomerPriceTypeTotalRangesKeys.RuleValidationCustomerPriceTypeTotalRanges
                                                                .Where(x => (ruleValidationDTO.CustomerPriceTotalDTO.Select(y => y.CustomerPriceTotalID)
                                                                                        .Contains(x.RuleValidationCustomerPriceTypeTotalRangeID))).ToList();

                            foreach (var updateCustomerPriceTypeTotal in existingCustomerPriceTypeTotal)
                            {
                                var CustomerPriceTypeTotalDTO = ruleValidationDTO.CustomerPriceTotalDTO
                                                                    .Where(x => x.CustomerPriceTotalID == updateCustomerPriceTypeTotal.RuleValidationCustomerPriceTypeTotalRangeID)
                                                                        .FirstOrDefault();

                                updateCustomerPriceTypeTotal.RuleValidationID = CustomerPriceTypeTotalDTO.RuleValidationID;
                                updateCustomerPriceTypeTotal.ProductPriceTypeID = CustomerPriceTypeTotalDTO.ProductPriceTypeID;

                                foreach (var CustomerPriceTypeTotalItems in updateCustomerPriceTypeTotal.RuleValidationCustomerPriceTypeTotalRangeCurrencyAmounts)
                                {
                                    CustomerPriceTypeTotalItems.MaximumAmount = CustomerPriceTypeTotalDTO.MaximumAmount;
                                    CustomerPriceTypeTotalItems.MinimumAmount = CustomerPriceTypeTotalDTO.MinimumAmount;
                                    CustomerPriceTypeTotalItems.CurrencyID = CustomerPriceTypeTotalDTO.CurrencyID;
                                }
                            }

                            var removedCustomerPriceTypeTotal = ruleValidations.RuleValidationCustomerPriceTypeTotalRangesKeys.RuleValidationCustomerPriceTypeTotalRanges
                                                                .Where(x => !(ruleValidationDTO.CustomerPriceTotalDTO.Select(y => y.CustomerPriceTotalID)
                                                                                        .Contains(x.RuleValidationCustomerPriceTypeTotalRangeID))).ToList();

                            if (removedCustomerPriceTypeTotal.Count > 0)
                            { 
                                foreach (var deleteCustomerPriceTypeTotal in removedCustomerPriceTypeTotal)
                                {
                                    ruleValidations.RuleValidationCustomerPriceTypeTotalRangesKeys.RuleValidationCustomerPriceTypeTotalRanges.Remove(deleteCustomerPriceTypeTotal);
                                }
                                ruleValidations.RuleValidationCustomerPriceTypeTotalRangesKeys.RuleValidationCustomerPriceTypeTotalRanges.Remove(removedCustomerPriceTypeTotal.First());
                            }

                            var adedCustomerPriceTypeTotal = ruleValidationDTO.CustomerPriceTotalDTO
                                                                .Where(x => !(ruleValidations.RuleValidationCustomerPriceTypeTotalRangesKeys.RuleValidationCustomerPriceTypeTotalRanges.Select(y => y.RuleValidationCustomerPriceTypeTotalRangeID)
                                                                                        .Contains(x.CustomerPriceTotalID))).ToList();


                            foreach (var insertCustomerPriceTypeTotal in adedCustomerPriceTypeTotal)
                            {
                                var ruleCustomerPriceTotal = Create.New<RuleValidationCustomerPriceTypeTotalRanges>();
                                var ruleCustomerPriceTotalItem = Create.New<RuleValidationCustomerPriceTypeTotalRangeCurrencyAmounts>();

                                ruleCustomerPriceTotal.RuleValidationCustomerPriceTypeTotalRangesKeys = ruleValidations.RuleValidationCustomerPriceTypeTotalRangesKeys;
                                ruleCustomerPriceTotal.RuleValidationID = insertCustomerPriceTypeTotal.RuleValidationID;

                                ruleCustomerPriceTotal.ProductPriceTypeID = insertCustomerPriceTypeTotal.ProductPriceTypeID;

                                ruleCustomerPriceTotalItem.RuleValidationCustomerPriceTypeTotalRangeID = ruleCustomerPriceTotal.RuleValidationCustomerPriceTypeTotalRangeID;
                                ruleCustomerPriceTotalItem.MaximumAmount = insertCustomerPriceTypeTotal.MaximumAmount;
                                ruleCustomerPriceTotalItem.MinimumAmount = insertCustomerPriceTypeTotal.MinimumAmount;
                                ruleCustomerPriceTotalItem.CurrencyID = insertCustomerPriceTypeTotal.CurrencyID;
                                ruleCustomerPriceTotalItem.RuleValidationCustomerPriceTypeTotalRanges = ruleCustomerPriceTotal;

                                ruleCustomerPriceTotal.RuleValidationCustomerPriceTypeTotalRangeCurrencyAmounts.Add(ruleCustomerPriceTotalItem);

                                ruleValidations.RuleValidationCustomerPriceTypeTotalRangesKeys.RuleValidationCustomerPriceTypeTotalRanges.Add(ruleCustomerPriceTotal);
                            }

                        }
                        else
                        {
                            var ruleCustomerPriceTotalKey = Create.New<RuleValidationCustomerPriceTypeTotalRangesKeys>();
                            ruleCustomerPriceTotalKey.RuleValidationID = ruleValidations.RuleValidationID;
                            ruleCustomerPriceTotalKey.RuleValidations = ruleValidations;
                            foreach (var newType in ruleValidationDTO.CustomerPriceTotalDTO)
                            {
                                var ruleCustomerPriceTotal = Create.New<RuleValidationCustomerPriceTypeTotalRanges>();
                                var ruleCustomerPriceTotalItem = Create.New<RuleValidationCustomerPriceTypeTotalRangeCurrencyAmounts>();

                                ruleCustomerPriceTotal.RuleValidationID = newType.RuleValidationID;
                                ruleCustomerPriceTotal.ProductPriceTypeID = newType.ProductPriceTypeID;

                                ruleCustomerPriceTotalItem.RuleValidationCustomerPriceTypeTotalRangeID = ruleCustomerPriceTotal.RuleValidationCustomerPriceTypeTotalRangeID;
                                ruleCustomerPriceTotalItem.MaximumAmount = newType.MaximumAmount;
                                ruleCustomerPriceTotalItem.MinimumAmount = newType.MinimumAmount;
                                ruleCustomerPriceTotalItem.CurrencyID = newType.CurrencyID;
                                ruleCustomerPriceTotalItem.RuleValidationCustomerPriceTypeTotalRanges = ruleCustomerPriceTotal;

                                ruleCustomerPriceTotal.RuleValidationCustomerPriceTypeTotalRangeCurrencyAmounts.Add(ruleCustomerPriceTotalItem);
                                ruleCustomerPriceTotal.RuleValidationCustomerPriceTypeTotalRangesKeys = ruleCustomerPriceTotalKey;

                                ruleCustomerPriceTotalKey.RuleValidationCustomerPriceTypeTotalRanges.Add(ruleCustomerPriceTotal);
                            }

                            ruleValidations.RuleValidationCustomerPriceTypeTotalRangesKeys = ruleCustomerPriceTotalKey;
                        }
                    }
                    else
                    {
                        if (ruleValidations.RuleValidationCustomerPriceTypeTotalRangesKeys != null)
                        {
                            foreach (var item in ruleValidations.RuleValidationCustomerPriceTypeTotalRangesKeys.RuleValidationCustomerPriceTypeTotalRanges)
                            {
                                item.RuleValidationCustomerPriceTypeTotalRangeCurrencyAmounts.Clear();
                            }
                            ruleValidations.RuleValidationCustomerPriceTypeTotalRangesKeys.RuleValidationCustomerPriceTypeTotalRanges.Clear();
                            ruleValidations.RuleValidationCustomerPriceTypeTotalRangesKeys = null;
                        } 
                    }
                }
                else
                {
                    if (ruleValidations.RuleValidationCustomerPriceTypeTotalRangesKeys != null)
                    {
                        foreach (var item in ruleValidations.RuleValidationCustomerPriceTypeTotalRangesKeys.RuleValidationCustomerPriceTypeTotalRanges)
                        {
                            item.RuleValidationCustomerPriceTypeTotalRangeCurrencyAmounts.Clear();
                        }
                        ruleValidations.RuleValidationCustomerPriceTypeTotalRangesKeys.RuleValidationCustomerPriceTypeTotalRanges.Clear();
                        ruleValidations.RuleValidationCustomerPriceTypeTotalRangesKeys = null;
                    }
                }

                /*Customer SubTotal*/
                if (ruleValidationDTO.CustomerPriceSubTotalDTO != null)
                {
                    if (ruleValidationDTO.CustomerPriceSubTotalDTO.Count > 0)
                    {
                        if (ruleValidations.RuleValidationCustomerSubtotalRanges != null)
                        {
                            var existingCustomerSubTotal = ruleValidations.RuleValidationCustomerSubtotalRanges.RuleValidationCustomerSubtotalRangeCurrencyAmounts
                                                                .Where(x => (ruleValidationDTO.CustomerPriceSubTotalDTO.Select(y => y.CustomerPriceSubTotalID)
                                                                                        .Contains(x.RuleValidationCustomerSubtotalRangeCurrencyAmountID))).ToList();

                            foreach (var updateCustomerSubTotal in existingCustomerSubTotal)
                            {
                                var CustomerSubTotalDTO = ruleValidationDTO.CustomerPriceSubTotalDTO
                                                                    .Where(x => x.CustomerPriceSubTotalID == updateCustomerSubTotal.RuleValidationCustomerSubtotalRangeCurrencyAmountID)
                                                                        .FirstOrDefault();

                                updateCustomerSubTotal.RuleValidationID = CustomerSubTotalDTO.RuleValidationID;
                                updateCustomerSubTotal.CurrencyID = CustomerSubTotalDTO.CurrencyID;
                                updateCustomerSubTotal.MaximumAmount = CustomerSubTotalDTO.MaximumAmount;
                                updateCustomerSubTotal.MinimumAmount = CustomerSubTotalDTO.MinimumAmount;
                                updateCustomerSubTotal.RuleValidationID = CustomerSubTotalDTO.RuleValidationID;
                            }

                            var removedCustomerSubTotal = ruleValidations.RuleValidationCustomerSubtotalRanges.RuleValidationCustomerSubtotalRangeCurrencyAmounts
                                                                .Where(x => !(ruleValidationDTO.CustomerPriceSubTotalDTO.Select(y => y.CustomerPriceSubTotalID)
                                                                                        .Contains(x.RuleValidationCustomerSubtotalRangeCurrencyAmountID))).ToList();

                            foreach (var deleteCustomerSubTotal in removedCustomerSubTotal)
                            {
                                ruleValidations.RuleValidationCustomerSubtotalRanges.RuleValidationCustomerSubtotalRangeCurrencyAmounts.Remove(deleteCustomerSubTotal);
                                ruleValidations.RuleValidationCustomerSubtotalRanges = null;
                            }

                            var adedCustomerSubTotal = ruleValidationDTO.CustomerPriceSubTotalDTO
                                                                .Where(x => !(ruleValidations.RuleValidationCustomerSubtotalRanges.RuleValidationCustomerSubtotalRangeCurrencyAmounts.Select(y => y.RuleValidationCustomerSubtotalRangeCurrencyAmountID)
                                                                                        .Contains(x.CustomerPriceSubTotalID))).ToList();


                            foreach (var insertCustomerSubTotal in adedCustomerSubTotal)
                            {
                                var ruleCustomerSubTotal = Create.New<RuleValidationCustomerSubtotalRanges>();
                                var ruleCustomerSubTotalItem = Create.New<RuleValidationCustomerSubtotalRangeCurrencyAmounts>();

                                ruleCustomerSubTotal.RuleValidationID = insertCustomerSubTotal.RuleValidationID;

                                ruleCustomerSubTotalItem.MaximumAmount = insertCustomerSubTotal.MaximumAmount;
                                ruleCustomerSubTotalItem.MinimumAmount = insertCustomerSubTotal.MinimumAmount;
                                ruleCustomerSubTotalItem.CurrencyID = insertCustomerSubTotal.CurrencyID;
                                ruleCustomerSubTotalItem.RuleValidationCustomerSubtotalRanges = ruleCustomerSubTotal;

                                ruleCustomerSubTotal.RuleValidationCustomerSubtotalRangeCurrencyAmounts.Add(ruleCustomerSubTotalItem);

                                ruleCustomerSubTotal.RuleValidations = ruleValidations;
                                ruleValidations.RuleValidationCustomerSubtotalRanges = ruleCustomerSubTotal;
                            }
                        }
                        else
                        {
                            foreach (var newType in ruleValidationDTO.CustomerPriceSubTotalDTO)
                            {
                                var ruleCustomerSubTotal = Create.New<RuleValidationCustomerSubtotalRanges>();
                                var ruleCustomerSubTotalItem = Create.New<RuleValidationCustomerSubtotalRangeCurrencyAmounts>();

                                ruleCustomerSubTotal.RuleValidationID = newType.RuleValidationID;

                                ruleCustomerSubTotalItem.MaximumAmount = newType.MaximumAmount;
                                ruleCustomerSubTotalItem.MinimumAmount = newType.MinimumAmount;
                                ruleCustomerSubTotalItem.CurrencyID = newType.CurrencyID;
                                ruleCustomerSubTotalItem.RuleValidationCustomerSubtotalRanges = ruleCustomerSubTotal;

                                ruleCustomerSubTotal.RuleValidationCustomerSubtotalRangeCurrencyAmounts.Add(ruleCustomerSubTotalItem);

                                ruleCustomerSubTotal.RuleValidations = ruleValidations;
                                ruleValidations.RuleValidationCustomerSubtotalRanges = ruleCustomerSubTotal;
                            }
                        }
                    }
                    else
                    {
                        if (ruleValidations.RuleValidationCustomerSubtotalRanges != null)
                        {
                            ruleValidations.RuleValidationCustomerSubtotalRanges.RuleValidationCustomerSubtotalRangeCurrencyAmounts.Clear();
                            ruleValidations.RuleValidationCustomerSubtotalRanges = null;
                        } 
                    }
                }
                else
                {
                    if (ruleValidations.RuleValidationCustomerSubtotalRanges != null)
                    {
                        ruleValidations.RuleValidationCustomerSubtotalRanges.RuleValidationCustomerSubtotalRangeCurrencyAmounts.Clear();
                        ruleValidations.RuleValidationCustomerSubtotalRanges = null;
                    }
                }
            }

            rule.RuleValidations.Clear();
            rule.RuleValidations.Add(ruleValidations);
		}
		#endregion
        
		#endregion
        
       
	}
}
