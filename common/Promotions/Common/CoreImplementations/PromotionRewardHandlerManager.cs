using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Common.Model;

namespace NetSteps.Promotions.Common.CoreImplementations
{
    public class PromotionRewardHandlerManager : IPromotionRewardHandlerManager
    {
        /// <summary>
        /// The dictionary of promotion reward handlers registered with a string representation of their types as a key.
        /// </summary>
        private readonly ConcurrentDictionary<string, RegistrationRecord> _promotionRewardHandlerConstructors = new ConcurrentDictionary<string, RegistrationRecord>();

        private readonly ConcurrentDictionary<string, string> _promotionRewardHandlerTypeToPromotionRewardHandlerKindKey = new ConcurrentDictionary<string, string>();
        
        /// <summary>
        /// A provider registration record.
        /// </summary>
        abstract class RegistrationRecord
        {
            /// <summary>
            /// An untyped provider.
            /// </summary>
            /// <returns></returns>
            public abstract IPromotionRewardHandler UntypedConstruct();
        }

        /// <summary>
        /// Provides an abstraction for getting an untyped Promotion Reward Handler.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        class RegistrationRecord<T> : RegistrationRecord
            where T : IPromotionRewardHandler
        {
            public override IPromotionRewardHandler UntypedConstruct()
            {
                return Create.New<T>();
            }
        }

        public IPromotionRewardHandler GetRewardHandler(string RewardKindName)
        {
            return _promotionRewardHandlerConstructors[RewardKindName].UntypedConstruct();
        }

        public TPromotionRewardHandler GetRewardHandler<TPromotionRewardHandler>(string RewardKindName) where TPromotionRewardHandler : IPromotionRewardHandler
        {
			return (TPromotionRewardHandler)_promotionRewardHandlerConstructors[RewardKindName].UntypedConstruct();
        }

        public void RegisterHandler<TPromotionRewardHandler>(string RewardKindName) where TPromotionRewardHandler : IPromotionRewardHandler
        {
			var promotionRewardHandler = Create.New<TPromotionRewardHandler>();
            
            if (!_promotionRewardHandlerConstructors.TryAdd(RewardKindName, new RegistrationRecord<TPromotionRewardHandler>()))
            {
                throw new InvalidOperationException(String.Concat("Promotion reward handler already registered: ", RewardKindName));
            }
            if (!_promotionRewardHandlerTypeToPromotionRewardHandlerKindKey.TryAdd(typeof(TPromotionRewardHandler).ToString(), RewardKindName))
            {
                throw new InvalidOperationException(String.Concat("Promotion reward handler type already registered:", typeof(TPromotionRewardHandler).ToString()));
            }
        }
    }
}