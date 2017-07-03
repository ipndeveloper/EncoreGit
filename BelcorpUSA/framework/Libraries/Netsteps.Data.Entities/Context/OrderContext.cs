using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Dynamic;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Context
{
    [Serializable]
    [ContainerRegister(typeof(IOrderContext), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Default)]
    public class OrderContext : IOrderContext
    {
        public IList<ICouponCode> CouponCodes { get; private set; }
        public IActivityInfo CurrentActivity { get; set; } //INI - GR_Encore-07
        public IList<IOrderStep> InjectedOrderSteps { get; private set; }
        public static InventoryBaseRepository Inventory { get { return Create.New<InventoryBaseRepository>(); } }

        public IOrder Order { get; set; }

        /// <summary>
        /// A <see cref="DynamicDictionary"/> for storing custom order data.
        /// </summary>
        public dynamic Data { get; set; }

        public IOrderAdjustment CreateOrderAdjustment()
        {
            return Create.New<IOrderAdjustment>();
        }

        public int[] ValidOrderStatusIdsForOrderAdjustment
        {
            get
            {
                return NetSteps.Data.Entities.Order.GetValidOrderStatusIdsForOrderAdjustment().ToArray();
            }
        }

        public OrderContext()
        {
            Clear();
        }

        public void Clear()
        {
            CouponCodes = new List<ICouponCode>();
            InjectedOrderSteps = new List<IOrderStep>();
            Order = null;
            CurrentActivity = null;         //INI - GR_Encore-07
            Data = new DynamicDictionary();
            _sortedDynamicProducts = null;
            if (ApplicationContext.Instance.UseDefaultBundling)
            {
                //This is for the express purpose of populating _sortedDynamicProducts
                var tempVar = SortedDynamicKitProducts;
            }
        }

        private IList<IProduct> _sortedDynamicProducts;
        public IList<IProduct> SortedDynamicKitProducts
        {
            get
            {
                if (_sortedDynamicProducts == null)
                {
                    IEnumerable<IProduct> products = Inventory.GetDynamicKitProducts(NetSteps.Data.Entities.ApplicationContext.Instance.StoreFrontID, true, true);
                    _sortedDynamicProducts = products != null ? products.ToList() : new List<IProduct>();
                }

                return _sortedDynamicProducts;
            }
        }
    }

    //INI - GR_Encore-07
    [Serializable]
    [ContainerRegister(typeof(IActivityInfo), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Default)]
    public class ActivityInfo : IActivityInfo
    {
        public short AccountConsistencyStatusID { get; set; }
        public int AccountID { get; set; }
        public short ActivityStatusID { get; set; }
        public bool HasContinuity { get; set; }
        public int PeriodID { get; set; }
    }
    //FIN - GR_Encore-07
}
