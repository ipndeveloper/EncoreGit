using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Interfaces;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Encore.Core.IoC;
using NetSteps.Common.Configuration; 
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;

namespace NetSteps.Data.Entities.Shipping
{
	/// <summary>
	/// Description: Default Implementation of ShippingCalculator. This class is overridable for per client customizations.
	/// </summary>
	[ContainerRegister(typeof(IShippingCalculator), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
	public class ShippingCalculator : IShippingCalculator, IDefaultImplementation
	{
		protected readonly object _lock = new object();

		private const short _totalOrderCostID = (short)Constants.ShippingRateType.TotalOrderCost;
		private const short _totalShipmentWeightID = (short)Constants.ShippingRateType.TotalShippmentWeight;
		protected ConcurrentDictionary<string, StateProvince> _stateProvinceIndexedLookup = null;

		#region Lazy-loading Properties

		private IProductRepository _productRepository;
		public IProductRepository ProductRepository
		{
			get { return Encore.Core.Util.NonBlockingLazyInitializeVolatile(ref _productRepository, Create.New<IProductRepository>); }
		}


		/// Gets the inventory repository.
		/// </summary>
		public InventoryBaseRepository Inventory
		{
			get
			{
				return NetSteps.Encore.Core.Util.NonBlockingLazyInitializeVolatile(
					ref _inventory,
					Create.New<InventoryBaseRepository>
				);
			}
		}
		InventoryBaseRepository _inventory;

		//Dictionary<int, decimal?> _sRMaxCost = null;
		//Dictionary<int, decimal?> _sRMaxWeight = null;
		private Dictionary<int, ShippingRateDataProfile> _costShippingRateDataProfiles;
		private Dictionary<int, ShippingRateDataProfile> _weightShippingRateDataProfiles;
		protected Dictionary<int, ShippingRateDataProfile> CostShippingRateDataProfiles
		{
			get
			{
				if (_costShippingRateDataProfiles == null)
				{
					_costShippingRateDataProfiles =
						SmallCollectionCache.Instance.ShippingRates.Where(x => x.ShippingRateTypeID == _totalOrderCostID).Join(
							SmallCollectionCache.Instance.ShippingRateGroups,
							sr => sr.ShippingRateGroupID,
							srg => srg.ShippingRateGroupID,
							(sr, srg) => new { ShippingRate = sr, ShippingRateGroup = srg }).ToDictionary(
								x => x.ShippingRate.ShippingRateID,
								x =>
								new ShippingRateDataProfile
								{
									CanOverride = IncludeMaxValuesWhenExceedingAllRangesForShippingRates,
									MaxValue = ShippingRateMaxValues[x.ShippingRate.ShippingRateGroupID],
									ValueFrom = x.ShippingRate.ValueFrom ?? 0,
									ValueTo = x.ShippingRate.ValueTo ?? 0
								});
					SmallCollectionCache.Instance.ShippingRates.DataChanged -= ShippingRates_DataChanged;
					SmallCollectionCache.Instance.ShippingRates.DataChanged += ShippingRates_DataChanged;
				}

				return _costShippingRateDataProfiles;
			}
		}

		protected Dictionary<int, ShippingRateDataProfile> WeightShippingRateDataProfiles
		{
			get
			{
				if (_weightShippingRateDataProfiles == null)
				{
					_weightShippingRateDataProfiles =
						SmallCollectionCache.Instance.ShippingRates.Where(x => x.ShippingRateTypeID == _totalShipmentWeightID).Join(
							SmallCollectionCache.Instance.ShippingRateGroups,
							sr => sr.ShippingRateGroupID,
							srg => srg.ShippingRateGroupID,
							(sr, srg) => new { ShippingRate = sr, ShippingRateGroup = srg }).ToDictionary(
								x => x.ShippingRate.ShippingRateID,
								x =>
								new ShippingRateDataProfile
								{
									CanOverride = IncludeMaxValuesWhenExceedingAllRangesForShippingRates,
									MaxValue = ShippingRateMaxValues[x.ShippingRate.ShippingRateGroupID],
									ValueFrom = x.ShippingRate.ValueFrom ?? 0,
									ValueTo = x.ShippingRate.ValueTo ?? 0
								});
					SmallCollectionCache.Instance.ShippingRates.DataChanged -= ShippingRates_DataChanged;
					SmallCollectionCache.Instance.ShippingRates.DataChanged += ShippingRates_DataChanged;
				}
				return _weightShippingRateDataProfiles;
			}
		}

		protected Dictionary<int, decimal> _shippingRateMaxValues;
		protected Dictionary<int, decimal> ShippingRateMaxValues
		{
			get
			{
				if (_shippingRateMaxValues == null)
				{
					var shippingRateMaxValueList = from sr in SmallCollectionCache.Instance.ShippingRates
												   group sr by sr.ShippingRateGroupID
													   into g
													   select new { g.Key, MaxValue = g.Max(sr => sr.ValueTo ?? 0) };
					_shippingRateMaxValues = shippingRateMaxValueList.ToDictionary(x => x.Key, x => x.MaxValue);
					SmallCollectionCache.Instance.ShippingRates.DataChanged -= ShippingRates_DataChanged;
					SmallCollectionCache.Instance.ShippingRates.DataChanged += ShippingRates_DataChanged;
				}
				return _shippingRateMaxValues;
			}
		}

		#endregion

		public virtual bool IncludeMaxValuesWhenExceedingAllRangesForShippingRates
		{
			get
			{
				return ConfigurationManager.GetAppSetting("IncludeMaxValuesForShippingRates", true); ;
			}
		}


		public virtual List<ShippingMethodWithRate> GetShippingMethodsWithRates(Order order, OrderShipment orderShipment)
		{
			return GetShippingMethodsWithRates(order.OrderCustomers[0], orderShipment);
		}

		public virtual List<ShippingMethodWithRate> GetShippingMethodsWithRates(Order order)
		{
			try
			{
				using (new ApplicationUsageLogger(new ExecutionContext(this)))
				{
					var partyOrderType = SmallCollectionCache.Instance.OrderTypes.FirstOrDefault(t => t.Name == "Party Order");

					OrderShipment orderShipment = order.OrderShipments.FirstOrDefault(os => !os.IsDirectShipment);
					if (orderShipment == null)
					{
						// In case the order is not a party order and there is only shipment that is a direct shipment - JHE
						if (order.OrderCustomers != null && order.OrderCustomers.Count == 1 && order.OrderTypeID != partyOrderType.OrderTypeID)
							orderShipment = order.OrderShipments.FirstOrDefault(os => os.IsDirectShipment);

						if (orderShipment == null)
							return null;
					}

					int orderTypeID = order.OrderTypeID;

					if (order.CurrencyID == 0)
						throw new Exception("Unable to lookup shipping rates. CurrencyID not set on order.");

					decimal totalForShipping = 0;
					decimal packageTotalCost = 0;
					decimal packageTotalWeight = 0;

					// In case the order is not a party order and there is only shipment that is a direct shipment - JHE
					var orderCustomers = order.OrderCustomers.Where(oc => !oc.OrderShipments.Any(os => os.IsDirectShipment)).ToList();
					if (partyOrderType != null && (order.OrderTypeID != partyOrderType.OrderTypeID && (orderCustomers.Count == 0)))
					{
						orderCustomers = order.OrderCustomers.Where(oc => oc.OrderShipments.Any(os => os.IsDirectShipment)).ToList();
					}

					foreach (OrderCustomer orderCustomer in orderCustomers)
					{
						totalForShipping += CalculateTotalForShipping(orderCustomer, orderShipment);
						packageTotalCost += CalculateTotalForShipping(orderCustomer, orderShipment);
						packageTotalWeight += CalculateTotalWeightForShipping(orderCustomer, orderShipment);
					}

					short totalOrderCostId = (short)Constants.ShippingRateType.TotalOrderCost;
					short totalShippmentWeightId = (short)Constants.ShippingRateType.TotalShippmentWeight;

					return GetRates(totalForShipping, packageTotalCost, packageTotalWeight, totalOrderCostId, totalShippmentWeightId, orderShipment, orderTypeID, order.CurrencyID);
				}
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsException);
			}
		}

		public virtual List<ShippingMethodWithRate> GetShippingMethodsWithRates(OrderCustomer orderCustomer)
		{
			var isPartyOrder = orderCustomer.Order.OrderTypeID == (int)Constants.OrderType.PartyOrder;
			var orderShipment = isPartyOrder
											? orderCustomer.Order.OrderShipments.First()
											: orderCustomer.OrderShipments.First();
			return GetShippingMethodsWithRates(orderCustomer, orderShipment);
		}

		public virtual List<ShippingMethodWithRate> GetShippingMethodsWithRates(OrderCustomer orderCustomer, OrderShipment orderShipment)
		{
			try
			{
				using (new ApplicationUsageLogger(new ExecutionContext(this)))
				{
					if (orderShipment == null)
						throw new Exception("Order shipment is null.");

					int orderTypeID = orderCustomer.Order.OrderTypeID;

					if (orderCustomer.Order.CurrencyID == 0)
						throw new Exception("Unable to lookup shipping rates. CurrencyID not set on order.");

					decimal totalForShipping = CalculateTotalForShipping(orderCustomer, orderShipment);

					decimal packageTotalCost = totalForShipping;
					decimal packageTotalWeight = CalculateTotalWeightForShipping(orderCustomer, orderShipment);

					short totalOrderCostId = (short)Constants.ShippingRateType.TotalOrderCost;
					short totalShippmentWeightId = (short)Constants.ShippingRateType.TotalShippmentWeight;

					var shippingRates = GetRates(totalForShipping, packageTotalCost, packageTotalWeight, totalOrderCostId, totalShippmentWeightId, orderShipment, orderTypeID, orderCustomer.Order.CurrencyID);
                     
					return shippingRates;
				}
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsException);
			}
		}

		public virtual bool OnlyIncludePOBoxShippingAddresses(OrderShipment ordersShipment)
		{
			Contract.Requires(ordersShipment != null);

			bool result = false;

			result = ordersShipment.IsPoBoxAddress();

			return result;
		}

		private List<ShippingMethodWithRate> GetRates(decimal totalForShipping, decimal packageTotalCost, decimal packageTotalWeight, short totalOrderCostId, short totalShipmentWeightId, OrderShipment orderShipment, int orderTypeID, int currencyID)
		{
			Contract.Requires(orderShipment != null);
            //var exception = EntityExceptionHelper.GetAndLogNetStepsException("GetRates", NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);//,null, 1,"GetRates()");
			
            string state = string.Empty;

			bool onlyIncludePOBoxShippingMethods = OnlyIncludePOBoxShippingAddresses(orderShipment);

			if (orderShipment.StateProvinceID.HasValue)
			{
				var stateProvince = SmallCollectionCache.Instance.StateProvinces.GetById((int)orderShipment.StateProvinceID);

				if (stateProvince != null && stateProvince.StateProvinceID > 0)
				{
					state = stateProvince.StateAbbreviation;
				}
			}

			if (state.ToCleanString().IsNullOrEmpty())
			{
				state = orderShipment.State.ToCleanString().ToUpper(); // NOTE: The StateAbbreviation's on the StateProvinces table must all be trimmed AND uppercase. - JHE
			}

			int countryID = orderShipment.CountryID;
			currencyID = ValidateCurrencyID(countryID, currencyID);
			bool isDirectShipment = orderShipment.IsDirectShipment;

			if (state.ToCleanString().IsNullOrEmpty() || countryID == 0 || orderTypeID == 0)
				return new List<ShippingMethodWithRate>();

			// Code duplicating the Proc [usp_shippingmethodswithrates_select] that uses cached in memory data for faster calculations. - JHE
			List<ShippingMethodWithRate> rates = GetShippingRates(totalForShipping, packageTotalCost, packageTotalWeight, totalOrderCostId,
				totalShipmentWeightId, orderShipment, orderTypeID, currencyID, state, onlyIncludePOBoxShippingMethods, isDirectShipment);

			rates = OnGetRates(rates, orderShipment, totalForShipping, orderTypeID);

			return rates;
		}

		protected virtual List<ShippingMethodWithRate> GetShippingRates(decimal totalForShipping, decimal packageTotalCost,
			decimal packageTotalWeight, short totalOrderCostId, short totalShipmentWeightId, OrderShipment orderShipment, int orderTypeID,
			int currencyID, string state, bool onlyIncludePOBoxShippingMethods, bool isDirectShipment)
		{
			Contract.Requires(orderShipment != null);

			if (_stateProvinceIndexedLookup == null)
			{
				IndexStateProvinceLookup();
			}

			string key = string.Format("{0}-{1}", orderShipment.CountryID, state);
			StateProvince stateProvince = null;
			if (!_stateProvinceIndexedLookup.TryGetValue(key, out stateProvince))
			{
				stateProvince = SmallCollectionCache.Instance.StateProvinces.FirstOrDefault(s => s.StateAbbreviation == state && s.CountryID == orderShipment.CountryID);
			} 

			var candidateList = GetShippingRateCandidates(
				orderTypeID,
				orderShipment.CountryID,
				null,
                //stateProvince.ShippingRegionID,
				currencyID,
				orderShipment.IsWillCall,
                onlyIncludePOBoxShippingMethods, 
                orderShipment.PostalCode);
			var filteredCandidates = GetFilteredRateCandidates(candidateList, packageTotalCost, packageTotalWeight);
			var returnList = this.GetShippingMethodRatesFromCandidates(filteredCandidates, isDirectShipment, packageTotalCost, packageTotalWeight, totalForShipping);

			return returnList;
		}

		#region The New Hotness

		private List<ShippingMethodWithRate> GetShippingMethodRatesFromCandidates(IEnumerable<ShippingRateCandidate> filteredCandidates, bool isDirectShipment, decimal packageTotalCost, decimal packageTotalWeight, decimal totalForShipping)
		{
			var returnShippingRatesList = new List<ShippingMethodWithRate>();

			foreach (var candidate in filteredCandidates)
			{
				if (candidate.shippingOrderTypes == null || candidate.shippingRates == null || candidate.shippingMethods == null)
				{
					continue;
				}

                returnShippingRatesList.Add(new ShippingMethodWithRate
                    {
                        ShippingMethodID = candidate.GetShipping.ShippingOrderTypeID,
                        //candidate.shippingMethods.ShippingMethodID,
                        Name = candidate.GetShipping.DaysForDelivery + " Day(s) " + candidate.GetShipping.Name,
                        DisplayName = SmallCollectionCache.Instance.ShippingMethodTranslations.GetTranslatedName(candidate.shippingMethods.ShippingMethodID, candidate.shippingMethods.Name),
                        Description = SmallCollectionCache.Instance.ShippingMethodTranslations.GetTranslatedShortDescription(candidate.shippingMethods.ShippingMethodID, string.Empty),
                        ShortName = candidate.GetShipping.DaysForDelivery + candidate.GetShipping.Name,
                        AllowDirectShipments = candidate.shippingOrderTypes.AllowDirectShipments,
                        IsDefaultShippingMethod = candidate.shippingOrderTypes.IsDefaultShippingMethod,
                        ShippingAmount = (candidate.shippingRates.ShippingRateTypeID.HasValue && candidate.shippingRates.ShippingRateTypeID.Value == _totalOrderCostID)
                                                            ? (CalculateBasedOnPackageTotalCost(candidate.shippingRates, candidate.shippingOrderTypes, packageTotalCost, totalForShipping)).ToDecimal() +
                                                            AddDirectShipment(isDirectShipment, candidate.shippingRates, (CalculateBasedOnPackageTotalCost(candidate.shippingRates, candidate.shippingOrderTypes, packageTotalCost, totalForShipping)).ToDecimal())
                                                            : (CalculateBasedOnPackageTotalWeight(candidate.shippingRates, candidate.shippingOrderTypes, packageTotalWeight, totalForShipping)).ToDecimal() +
                                                            AddDirectShipment(isDirectShipment, candidate.shippingRates, (CalculateBasedOnPackageTotalCost(candidate.shippingRates, candidate.shippingOrderTypes, packageTotalCost, totalForShipping)).ToDecimal()),
                        DirectShipmentFee = candidate.shippingRates.DirectShipmentFee.ToDecimal(),
                        HandlingFee = candidate.shippingRates.HandlingFee.ToDecimal(),
                        ShippingPercentage = candidate.shippingRates.ShippingPercentage,
                        ShippingRateTypeID = candidate.shippingRates.ShippingRateTypeID,
                        ShippingRateID = candidate.shippingRates.ShippingRateID,
                        DateEstimated = candidate.GetShipping.DateEstimated,
                        ShippingMethodID2 = candidate.GetShipping.ShippingMethodID
                    }
                );
			}

			return returnShippingRatesList;
		}

		private IEnumerable<ShippingRateCandidate> GetFilteredRateCandidates(IEnumerable<ShippingRateCandidate> candidateList, decimal packageTotalCost, decimal packageTotalWeight)
		{
			var returnValue = candidateList.Where(
					cl =>
					(CostShippingRateDataProfiles.ContainsKey(cl.shippingRates.ShippingRateID)
					 && CostShippingRateDataProfiles[cl.shippingRates.ShippingRateID].IsValid(packageTotalCost))
					||
					(WeightShippingRateDataProfiles.ContainsKey(cl.shippingRates.ShippingRateID)
					 && WeightShippingRateDataProfiles[cl.shippingRates.ShippingRateID].IsValid(packageTotalWeight))).OrderBy(
						clo => clo.shippingMethods.SortIndex);
			return returnValue;
		}


        private string GetDateTimeCultureInfo(string fecha)
        {
             DateTime dt = DateTime.Now;
             if (string.IsNullOrEmpty(fecha)==true)
                 return dt.ToString("d");
             else
             {
                 var part = fecha.Split('-');

                 var fec = new DateTime(part[2].ToInt(), part[1].ToInt(), part[0].ToInt());

                 var fec1 = fec.ToString("d");

                 return fec1;

             }
            
        }
        private IEnumerable<ShippingRateCandidate> GetShippingRateCandidates(int orderTypeID, int countryID, int? shippingRegionID, int currencyID, bool isWillCall, bool onlyIncludePoBoxShippingMethods, string postalCode)
        {
            //// get my collection objects


         
            List<ShippingCalculatorSearchData.GetShipping> objGetShipping = ShippingCalculatorExtensions.GetShippingResult(postalCode);

            foreach (var item in objGetShipping.Where(x => x.OrderTypeID == orderTypeID))
            {
                ShippingMethods objE = new ShippingMethods();
                ShippingCalculatorSearchParameters objSC = new ShippingCalculatorSearchParameters();
                objSC.LogisticsProviderID = item.LogisticsProviderID;
                objSC.ShippingRateGroupID = item.ID;
                objSC.PostalCode = postalCode;
                objSC.OrderTypeID = orderTypeID;
                objSC.ShippingOrderTypeID = item.ShippingOrderTypeID;
                item.DateEstimated = GetDateTimeCultureInfo(ShippingCalculatorExtensions.GetDateDelivery(objSC));
                //item.DateEstimated = ShippingCalculatorExtensions.GetDateDelivery(objSC);
                objE.ShippingMethod = item.ShippingMethodID;
            }
            var shippingOrderTypes = (from sot in objGetShipping
                                     join sp in SmallCollectionCache.Instance.ShippingOrderTypes
                                     on sot.ShippingOrderTypeID equals sp.ShippingOrderTypeID
                                     where sp.OrderTypeID == orderTypeID && 
                                     sp.CountryID == countryID
                                     select sp ).ToList();
             
            var shippingRates = SmallCollectionCache.Instance.ShippingRates.Where(sr => sr.CurrencyID == currencyID);
            var shippingMethods = SmallCollectionCache.Instance.ShippingMethods.Where(sm => sm.IsWillCall == isWillCall &&
                (sm.AllowPoBox == onlyIncludePoBoxShippingMethods || sm.AllowPoBox));

            IEnumerable<ShippingRateCandidate> candidates = (from st in shippingOrderTypes
                                                             join sr in shippingRates on st.ShippingRateGroupID equals
                                                                sr.ShippingRateGroupID
                                                             join sm in shippingMethods on st.ShippingMethodID equals
                                                                sm.ShippingMethodID
                                                             join ob in objGetShipping on st.ShippingOrderTypeID equals
                                                                ob.ShippingOrderTypeID where ob.OrderTypeID == orderTypeID
                                                             select
                                                                new ShippingRateCandidate()
                                                                {
                                                                    shippingOrderTypes = st,
                                                                    shippingRates = sr,
                                                                    shippingMethods = sm,
                                                                    GetShipping = ob
                                                                }); 

            return candidates;
        }

		#endregion

		/// <summary>
		/// Allows clients to override which currency id should be used to search for shipping methods.
		/// If they need to search shipping methods based on the currencyid of the country being shipped to, they can override it here.
		/// </summary>
		/// <param name="countryID"></param>
		/// <param name="currencyID"></param>
		/// <returns></returns>
		protected virtual int ValidateCurrencyID(int countryID, int currencyID)
		{
			return currencyID;
		}

		private decimal? CalculateBasedOnPackageTotalCost(ShippingRate rate, ShippingOrderType orderType, decimal packageTotalCost, decimal totalForShipping)
		{
			var value = MaxCostGreaterThanPackageTotalCost(rate, packageTotalCost)
				? OverrideWithIncrementFee(orderType, rate, totalForShipping)
				: ValueWithOverrideIfInclusive(orderType, rate, totalForShipping);

			return value;
		}

		private bool MaxCostGreaterThanPackageTotalCost(ShippingRate rate, decimal packageTotalCost)
		{
			return CostShippingRateDataProfiles.ContainsKey(rate.ShippingRateID) && CostShippingRateDataProfiles.Any(x => x.Value.MaxValue > packageTotalCost);

			//if (!_sRMaxCost.ContainsKey(rate.ShippingRateGroupID))
			//{
			//    return false;
			//}

			//return _sRMaxCost[rate.ShippingRateGroupID].Value > packageTotalCost;
		}

		private decimal? OverrideWithIncrementFee(ShippingOrderType orderType, ShippingRate rate, decimal totalForShipping)
		{
			var value = ValueWithOverrideIfInclusive(orderType, rate, totalForShipping) + AddIncrementFee(rate, totalForShipping, CostShippingRateDataProfiles[rate.ShippingRateID].MaxValue);
			return value;
		}

		private decimal? ValueWithOverrideIfInclusive(ShippingOrderType orderType, ShippingRate rate, decimal totalForShipping)
		{
			Contract.Requires(orderType != null);
			Contract.Requires(rate != null);

			var value = (orderType.OverrideInclusive.ToBool())
				? ShippingPlusOverrideAmount(rate, totalForShipping, orderType)
				: GetShippingAmount(rate.ShippingAmount, rate.ShippingPercentage, totalForShipping, rate.MinimumAmount);

			return value;
		}


		private decimal? CalculateBasedOnPackageTotalWeight(ShippingRate rate, ShippingOrderType orderType, decimal packageTotalWeight, decimal totalForShipping)
		{
			return MaxWeightGreaterThanPackageTotalWeight(rate, packageTotalWeight)
				? ValueWithOverrideIfInclusive(orderType, rate, totalForShipping) + AddIncrementFee(rate, totalForShipping, WeightShippingRateDataProfiles[rate.ShippingRateID].MaxValue)
				: ValueWithOverrideIfInclusive(orderType, rate, totalForShipping);
		}



		private bool MaxWeightGreaterThanPackageTotalWeight(ShippingRate rate, decimal packageTotalWeight)
		{
			return WeightShippingRateDataProfiles[rate.ShippingRateID].MaxValue > packageTotalWeight;
		}

		private decimal? ShippingPlusOverrideAmount(ShippingRate rate, decimal totalForShipping, ShippingOrderType orderType)
		{
			Contract.Requires(rate != null);
			Contract.Requires(orderType != null);

			var value = GetShippingAmount(rate.ShippingAmount, rate.ShippingPercentage, totalForShipping, rate.MinimumAmount) + orderType.OverrideAmount;

			return value;
		}


		private decimal? AddIncrementFee(ShippingRate rate, decimal totalForShipping, decimal maxCostOrMaxWeight)
		{
			return (rate.IncrementalAmount > 0) ? ((maxCostOrMaxWeight - totalForShipping) / rate.IncrementalAmount) * rate.IncrementalFee : 0;
		}

		decimal AddDirectShipment(bool isDirectShipment, ShippingRate rate, decimal shippingCost)
		{
			Contract.Requires(rate != null);

			var addDirectShipmentMarkup = 0m;
			if (isDirectShipment)
			{
				addDirectShipmentMarkup = rate.DirectShipmentPercentage.HasValue ? ApplyDirectShipmentPercentageMarkup(rate, shippingCost) : rate.DirectShipmentFee.ToDecimal();
			}

			return addDirectShipmentMarkup;
		}

		decimal ApplyDirectShipmentPercentageMarkup(ShippingRate rate, decimal shippingCost)
		{
			Contract.Requires(rate != null);

			var directShipmentMarkup = 0m;
			if (rate.ShippingAmount.HasValue)
			{
				directShipmentMarkup = rate.DirectShipmentPercentage.ToDecimal() * rate.ShippingAmount.ToDecimal();
			}

			if (rate.ShippingPercentage.HasValue)
			{
				directShipmentMarkup = shippingCost * rate.DirectShipmentPercentage.ToDecimal();
			}

			return directShipmentMarkup;
		}

		private decimal HandlingFee(ShippingRate rate)
		{
			Contract.Requires(rate != null);

			return (rate.HandlingFee.HasValue ? rate.HandlingFee : 0).ToDecimal();
		}

		public virtual List<ShippingMethodWithRate> OnGetRates(List<ShippingMethodWithRate> rates, OrderShipment orderShipment, decimal totalForShipping, int orderTypeID)
		{
			return rates;
		}

		private void ShippingRates_DataChanged(object sender, EventArgs e)
		{
			_costShippingRateDataProfiles = null;
			_weightShippingRateDataProfiles = null;
			_shippingRateMaxValues = null;
		}

		private decimal? GetShippingAmount(decimal? shippingAmount, decimal? shippingPercentage, decimal totalForShipping, decimal? minimumAmount)
		{
			if (shippingPercentage.HasValue && shippingPercentage.Value > 0 && (!shippingAmount.HasValue || (shippingAmount.HasValue && shippingAmount.Value == 0)))
			{
				shippingAmount = shippingPercentage.Value * totalForShipping;
				if (minimumAmount.HasValue && minimumAmount.Value > shippingAmount && shippingAmount > 0) //if free shipping then don't use minimum
				{
					shippingAmount = minimumAmount;
				}
			}

			return shippingAmount;
		}

		public virtual ShippingMethodWithRate GetLeastExpensiveShippingMethod(Order order)
		{
			var shippingRates = GetShippingMethodsWithRates(order);
			if (shippingRates.IsNullOrEmpty())
				throw new Exception("No shipping method found for address. Please try editing the shipping address to validate its Postal Code and State.");

			var lowestPriceRate = shippingRates.MinBy(r => r.ShippingAmount);

			return lowestPriceRate;
		}

		public virtual ShippingMethodWithRate GetLeastExpensiveShippingMethod(OrderCustomer orderCustomer, OrderShipment orderShipment)
		{
			var shippingRates = GetShippingMethodsWithRates(orderCustomer, orderShipment);
			if (shippingRates.IsNullOrEmpty())
				throw new Exception("No shipping method found for address. Please try editing the shipping address to validate its Postal Code and State.");

			var lowestPriceRate = shippingRates.MinBy(r => r.ShippingAmount);

			return lowestPriceRate;
		}

		protected virtual bool IsFreeOrderItemTypeId(short orderItemTypeId)
		{
			return false;
		}

		public virtual void CalculateShipping(OrderCustomer orderCustomer)
		{
			if (!orderCustomer.OverrideShippingAmount)
			{
				var shipment = orderCustomer.OrderShipments.FirstOrDefault();
				if (shipment == null)
				{
					orderCustomer.ShippingTotal = orderCustomer.HandlingTotal = 0;
					if (orderCustomer.Order.ShouldDividePartyShipping)
					{
						SetPartyShipmentChargeForCustomer(orderCustomer);
						SetPartyHandlingChargeForCustomer(orderCustomer);
					}

					return;
				}

				var shippingRates = GetShippingMethodsWithRates(orderCustomer, shipment);

				// Unset shipping method if it is no longer available
				if (shipment.ShippingMethodID != null && !shippingRates.Select(s => s.ShippingMethodID).Contains(shipment.ShippingMethodID.Value))
				{
					shipment.ShippingMethodID = null;
				}

				// No rates or no shippable items means we're done
				if (!shippingRates.Any() || !orderCustomer.ContainsShippableItems())
				{
					orderCustomer.ShippingTotal = orderCustomer.HandlingTotal = 0;
					return;
				}

				// If no shipping method, default to cheapest shipping method
				if (shipment.ShippingMethodID == null)
				{
					shipment.ShippingMethodID = shippingRates.OrderBy(x => x.ShippingAmount).First().ShippingMethodID;
				}

				var selectedShippingMethodWithRate = shippingRates.First(x => x.ShippingMethodID == shipment.ShippingMethodID.Value);

				decimal orderItemsShippingTotal = 0;
				decimal orderItemsHandlingTotal = 0;
				//bool atLeastOneOrderItemChargesShipping = false;
				var inventory = Create.New<InventoryBaseRepository>();

				foreach (OrderItem orderItem in orderCustomer.OrderItems)
				{
					if (orderItem.ProductID == null || IsFreeOrderItemTypeId(orderItem.OrderItemTypeID))
					{
						continue;
					}

					var product = inventory.GetProduct(orderItem.ProductID.Value);

					// Calculate order item shipping
					var productShippingFeeAmount = orderItem.GetAdjustedPrice((int)Constants.ProductPriceType.ShippingFee);
					if (product.ProductBase.ChargeShipping)
					{
						//atLeastOneOrderItemChargesShipping = true;
						orderItem.ShippingTotal = orderItem.Quantity * productShippingFeeAmount;
					}

					orderItemsShippingTotal += orderItem.ShippingTotalOverride ?? orderItem.ShippingTotal ?? 0m;

					// Calculate order item handling
					var productHandlingFeeAmount = orderItem.GetAdjustedPrice((int)Constants.ProductPriceType.HandlingFee);
					orderItem.HandlingTotal = orderItem.Quantity * productHandlingFeeAmount;
					orderItemsHandlingTotal += orderItem.HandlingTotal ?? 0m;
				}

				orderCustomer.ShippingTotal = orderItemsShippingTotal;
				orderCustomer.HandlingTotal = orderItemsHandlingTotal;

				//if (atLeastOneOrderItemChargesShipping)
				//{
				orderCustomer.ShippingTotal += selectedShippingMethodWithRate.ShippingAmount;
				orderCustomer.HandlingTotal += selectedShippingMethodWithRate.HandlingFee;
				//}
				orderCustomer.ShippingTotal = orderCustomer.ShippingTotal.GetRoundedNumber();
				orderCustomer.HandlingTotal = orderCustomer.HandlingTotal.GetRoundedNumber();
			}
		}

		public virtual void CalculatePartyShipping(Order order)
		{
			if (order.OrderTypeID != (short)Constants.OrderType.PartyOrder && order.OrderTypeID != (short)Constants.OrderType.FundraiserOrder)
				return;

			//OrderShipment shipment = order.OrderShipments.Where(os => !os.IsDirectShipment).FirstOrDefault();
			OrderShipment shipment = order.GetDefaultShipment();
			if (shipment == null)
				return;

			var shippingRates = GetShippingMethodsWithRates(order);

			if (shippingRates == null || shippingRates.Count == 0)
			{
				return;
			}

			if (!order.HandlingTotal.HasValue)
			{
				order.HandlingTotal = 0m;
			}

			if (!order.PartyHandlingTotal.HasValue)
			{
				order.PartyHandlingTotal = 0m;
			}

			decimal shippingAmount = 0;
			decimal handlingFee;
			decimal extraShippingFees = 0;

			bool chargeShipping = false;
			var inventory = Create.New<InventoryBaseRepository>();

			foreach (OrderItem orderItem in order.OrderCustomers.Where(oc => !oc.OrderShipments.Any(os => os.IsDirectShipment)).SelectMany(oc => oc.OrderItems))
			{
				if (IsFreeOrderItemTypeId(orderItem.OrderItemTypeID))
				{
					continue;
				}

				extraShippingFees += orderItem.GetAdjustedPrice((int)Constants.ProductPriceType.ShippingFee) * orderItem.Quantity;
				handlingFee = orderItem.GetAdjustedPrice((int)Constants.ProductPriceType.HandlingFee) * orderItem.Quantity;
				orderItem.HandlingTotal = handlingFee;
				order.PartyHandlingTotal += handlingFee;
				orderItem.OrderCustomer.HandlingTotal += handlingFee;

				if (orderItem.ProductID.HasValue)
				{
					var product = inventory.GetProduct(orderItem.ProductID ?? 0);
					if (!chargeShipping && product.ProductBase.ChargeShipping)
						chargeShipping = true;
				}
			}

			if (chargeShipping)
			{
				var shippingMethodWithRate = shippingRates.FirstOrDefault(r1 => r1.ShippingMethodID == shipment.ShippingMethodID);
				if (shippingMethodWithRate == null)
				{
					bool defaultToCheapestRate = false;
					if (shippingRates.Count > 0)
					{
						bool isTemplate = false;
						if (order.OrderTypeID > 0)
						{
							isTemplate = SmallCollectionCache.Instance.OrderTypes.GetById(order.OrderTypeID).IsTemplate;
						}
						if (!isTemplate && (order.OrderStatusID == Constants.OrderStatus.Paid.ToShort() || order.OrderStatusID == Constants.OrderStatus.PartiallyShipped.ToShort()
							|| order.OrderStatusID == Constants.OrderStatus.Shipped.ToShort() || order.OrderStatusID == Constants.OrderStatus.Pending.ToShort()))
						{
							defaultToCheapestRate = true;
						}

						if (defaultToCheapestRate)
						{
							shippingMethodWithRate = shippingRates.OrderBy(s => s.ShippingAmount).FirstOrDefault();
							shipment.ShippingMethodID = shippingMethodWithRate.ShippingMethodID;
							shippingAmount = shippingMethodWithRate.ShippingAmount;
						}
					}
					if (!defaultToCheapestRate)
					{
						throw new ShippingMethodNotAvailableException("Available Shipping Method not found for specified ShippingMethodID: " + shipment.ShippingMethodID.ToString());
					}
				}
				else
				{
					shippingAmount = shippingMethodWithRate.ShippingAmount;
				}
			}
			else
			{
				shippingAmount = 0;
			}

			order.PartyShipmentTotal = shippingAmount.GetRoundedNumber() + extraShippingFees.GetRoundedNumber();
		}

		public virtual void SetPartyShipmentChargeForCustomer(OrderCustomer orderCustomer)
		{
			if (!orderCustomer.Order.ShouldDividePartyShipping)
				return;

			var partyShipmentCustomers = orderCustomer.Order.OrderCustomers.Where(oc => !oc.OrderShipments.Any(os => os.IsDirectShipment)).ToList();
			var partyShipmentCustomersSubtotal = partyShipmentCustomers.Sum(oc => oc.GetShippableSubtotal());
			if (partyShipmentCustomersSubtotal <= 0)
				return;

			var remainingPartyShippingTotal = orderCustomer.Order.PartyShipmentTotal.Value;
			foreach (var customer in partyShipmentCustomers)
			{
				customer.ShippingTotal = Math.Round(((customer.GetShippableSubtotal() / partyShipmentCustomersSubtotal)), 6, MidpointRounding.AwayFromZero) * orderCustomer.Order.PartyShipmentTotal;
				remainingPartyShippingTotal -= customer.ShippingTotal.Value;
			}

			if (remainingPartyShippingTotal != 0m)
			{
				partyShipmentCustomers.OrderByDescending(x => x.ShippingTotal).First().ShippingTotal += remainingPartyShippingTotal;
			}
		}

		public virtual void SetPartyHandlingChargeForCustomer(OrderCustomer orderCustomer)
		{
			Contract.Requires(orderCustomer != null);

			Order order = null;

			if (orderCustomer.Order == null)
			{
				order = Order.Load(order.OrderID);
			}
			else
			{
				order = orderCustomer.Order;
			}

			foreach (OrderItem orderItem in orderCustomer.OrderItems)
			{
				var product = Inventory.GetProduct(orderItem.ProductID.ToInt());
				if (product.Prices.Any(p => p.ProductPriceTypeID == (int)Constants.ProductPriceType.HandlingFee && p.CurrencyID == order.CurrencyID))
				{
					decimal handlingFee = product.Prices.First(p => p.ProductPriceTypeID == (int)Constants.ProductPriceType.HandlingFee && p.CurrencyID == order.CurrencyID).Price;
					orderItem.HandlingTotal = handlingFee;
					orderCustomer.HandlingTotal += handlingFee;
				}
			}
			orderCustomer.HandlingTotal = orderCustomer.HandlingTotal.GetRoundedNumber();
		}

		public virtual decimal CalculateTotalForShipping(OrderCustomer orderCustomer, OrderShipment orderShipment)
		{
			decimal subTotalForShipping;

			var retailItems = orderCustomer.ParentSubtotalForShippingOrderItemTotalByType(Constants.OrderItemType.Retail);
			var hostCreditItems = orderCustomer.ParentSubtotalForShippingOrderItemTotalByType(Constants.OrderItemType.HostCredit);
			var percentOffItems = orderCustomer.ParentSubtotalForShippingOrderItemTotalByType(Constants.OrderItemType.PercentOff);
			var itemDiscountItems = orderCustomer.ParentSubtotalForShippingOrderItemTotalByType(Constants.OrderItemType.ItemDiscount);
			var bookingCreditItems = orderCustomer.ParentSubtotalForShippingOrderItemTotalByType(Constants.OrderItemType.BookingCredit);
			var exclusiveItems = orderCustomer.ParentSubtotalForShippingOrderItemTotalByType(Constants.OrderItemType.ExclusiveProduct);

			switch (orderShipment.State)
			{
				default:
					subTotalForShipping = retailItems
						+ hostCreditItems
						+ percentOffItems
						+ itemDiscountItems
						+ bookingCreditItems
						+ exclusiveItems;
					break;
			}

			return subTotalForShipping;
		}

		public virtual decimal CalculateTotalWeightForShipping(OrderCustomer customer, OrderShipment shipment)
		{
			Contract.Requires(customer != null);
			Contract.Requires(shipment != null);

			decimal totalWeightForShipping = 0;

			var retailItems = customer.WeightTotalForShippingOrderItemTotalByType(Constants.OrderItemType.Retail);
			var hostCreditItems = customer.WeightTotalForShippingOrderItemTotalByType(Constants.OrderItemType.HostCredit);
			var percentOffItems = customer.WeightTotalForShippingOrderItemTotalByType(Constants.OrderItemType.PercentOff);
			var itemDiscountItems = customer.WeightTotalForShippingOrderItemTotalByType(Constants.OrderItemType.ItemDiscount);
			var bookingCreditItems = customer.WeightTotalForShippingOrderItemTotalByType(Constants.OrderItemType.BookingCredit);
			var exclusiveItems = customer.WeightTotalForShippingOrderItemTotalByType(Constants.OrderItemType.ExclusiveProduct);

			switch (shipment.State)
			{
				default:
					totalWeightForShipping = retailItems
						+ hostCreditItems
						+ percentOffItems
						+ itemDiscountItems
						+ bookingCreditItems
						+ exclusiveItems;
					break;
			}

			return totalWeightForShipping;
		}

		private void IndexStateProvinceLookup()
		{
			lock (_lock)
			{
				if (_stateProvinceIndexedLookup == null)
				{
					_stateProvinceIndexedLookup = new ConcurrentDictionary<string, StateProvince>();
				}
				_stateProvinceIndexedLookup.Clear();
				// race condition
				foreach (var stateProvince in SmallCollectionCache.Instance.StateProvinces)
				{
					string key = string.Format("{0}-{1}", stateProvince.CountryID, stateProvince.StateAbbreviation);
					_stateProvinceIndexedLookup.Add(key, stateProvince);
				}

				SmallCollectionCache.Instance.StateProvinces.DataChanged -= new EventHandler(StateProvinces_DataChanged);
				SmallCollectionCache.Instance.StateProvinces.DataChanged += new EventHandler(StateProvinces_DataChanged);
			}
		}

		void StateProvinces_DataChanged(object sender, EventArgs e)
		{
			IndexStateProvinceLookup();
		}

		private IEnumerable<ShippingMethodWithRate> FilterOutExcludedShippingOptions(IEnumerable<ShippingMethodWithRate> shippingMethods, Order order)
		{
			Contract.Requires(shippingMethods != null);

			// Short-circuit
			if (!shippingMethods.Any())
			{
				return shippingMethods;
			}

			IList<int> productIdsWithExclusions;
			var filteredShippingMethods = OrderShipment.BusinessLogic.FilterOutExcludedShippingMethods(order, shippingMethods, ProductRepository, out productIdsWithExclusions);
			if (!filteredShippingMethods.Any())
			{
				// All shipping methods have been filtered out due to product exclusions, notify the user of the offending products.
				var productsWithExclusions = Inventory.GetProducts(productIdsWithExclusions);
				throw new ProductShippingExcludedShippingException(productsWithExclusions);
			}

			return filteredShippingMethods;
		}
	}

	public class ShippingRateCandidate
	{
        public ShippingOrderType shippingOrderTypes;
		public ShippingRate shippingRates;
        public ShippingMethod shippingMethods;
        public ShippingCalculatorSearchData.GetShipping GetShipping;
	}

	public class ShippingRateDataProfile
	{
		public decimal ValueFrom;
		public decimal ValueTo;
		public decimal MaxValue;
		public bool CanOverride;

		public bool IsValid(decimal totalValue)
		{
			if (totalValue >= ValueFrom && totalValue <= ValueTo)
			{
				return true;
			}

			return this.CanOverride && totalValue > this.MaxValue;
		}
	}
}