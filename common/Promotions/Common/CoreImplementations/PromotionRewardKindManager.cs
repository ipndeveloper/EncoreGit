using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Common.Model;

namespace NetSteps.Promotions.Common.CoreImplementations
{
    public class PromotionRewardKindManager : IPromotionRewardKindManager
    {
        /// <summary>
        /// The dictionary of promotions registered with a string representation of their types as a key.
        /// </summary>
        private readonly ConcurrentDictionary<string, RegistrationRecord> _promotionRewardConstructors = new ConcurrentDictionary<string, RegistrationRecord>();

        private readonly ConcurrentDictionary<string, string> _promotionTypeToPromotionRewardKindKey = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// A provider registration record.
        /// </summary>
        abstract class RegistrationRecord
        {
            /// <summary>
            /// An untyped provider.
            /// </summary>
            /// <returns></returns>
            public abstract IPromotionReward UntypedConstruct();
        }

        /// <summary>
        /// Provides an abstraction for getting an untyped promotion.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        class RegistrationRecord<T> : RegistrationRecord
            where T : IPromotionReward
        {
            public override IPromotionReward UntypedConstruct()
            {
                return Create.New<T>();
            }
        }

        public void RegisterPromotionRewardKind<TPromotionReward>(string promotionKind) where TPromotionReward : IPromotionReward
        {
            Contract.Assert(promotionKind != null);
            var promotion = Create.New<TPromotionReward>();
            Contract.Assert(promotion != null);

            if (!_promotionRewardConstructors.TryAdd(promotionKind, new RegistrationRecord<TPromotionReward>()))
            {
                throw new InvalidOperationException(String.Concat("Promotion already registered: ", promotionKind));
            }
            if (!_promotionTypeToPromotionRewardKindKey.TryAdd(typeof(TPromotionReward).ToString(), promotionKind))
            {
                throw new InvalidOperationException(String.Concat("Promotion Type already registered:", typeof(TPromotionReward).ToString()));
            }
        }

        public bool UnregisterRewardKind(string promotionKind)
        {
            Contract.Assert(promotionKind != null);
            RegistrationRecord unused;
            return _promotionRewardConstructors.TryRemove(promotionKind, out unused);
        }

        public TPromotionReward CreatePromotionReward<TPromotionReward>(string promotionKind) where TPromotionReward : IPromotionReward
        {
            Contract.Assert(promotionKind != null);
            var promotion = _promotionRewardConstructors[promotionKind].UntypedConstruct();
            Contract.Assert(typeof(TPromotionReward).IsAssignableFrom(promotion.GetType()));
            return (TPromotionReward)(object)promotion;
        }


        public IPromotionReward CreatePromotionReward(string promotionKind)
        {
            Contract.Assert(promotionKind != null);
            return _promotionRewardConstructors[promotionKind].UntypedConstruct();
        }

        public string GetPromotionRewardKindString<TPromotionReward>() where TPromotionReward : IPromotionReward
        {
            return _promotionTypeToPromotionRewardKindKey[typeof(TPromotionReward).ToString()];
        }
    }
}
