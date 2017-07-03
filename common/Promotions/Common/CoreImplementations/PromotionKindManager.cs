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
    public class PromotionKindManager : IPromotionKindManager
    {
        /// <summary>
        /// The dictionary of promotions registered with a string representation of their types as a key.
        /// </summary>
        private readonly ConcurrentDictionary<string, RegistrationRecord> _promotionConstructors = new ConcurrentDictionary<string, RegistrationRecord>();

        private readonly ConcurrentDictionary<string, string> _promotionTypeToPromotionKindKey = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// A provider registration record.
        /// </summary>
        abstract class RegistrationRecord
        {
            /// <summary>
            /// An untyped provider.
            /// </summary>
            /// <returns></returns>
            public abstract IPromotion UntypedConstruct();

			/// <summary>
			/// Determines if the registration is for a type that implements the specified interface.
			/// </summary>
			/// <typeparam name="IPromotionType">The type of the promotion type.</typeparam>
			/// <returns></returns>
			public abstract bool Implements<IPromotionType>() where IPromotionType : IPromotion;
        }

        /// <summary>
        /// Provides an abstraction for getting an untyped promotion.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        class RegistrationRecord<T> : RegistrationRecord
            where T : IPromotion
        {
            public override IPromotion UntypedConstruct()
            {
                return Create.New<T>();
            }

			public override bool Implements<IPromotionType>() 
			{
				return typeof(IPromotionType).IsAssignableFrom(typeof(T));
			}
        }

        public void RegisterPromotionKind<TPromotion>(string promotionKind) where TPromotion : IPromotion
        {
            Contract.Assert(promotionKind != null);
            var promotion = Create.New<TPromotion>();
            Contract.Assert(promotion != null);

            if (!_promotionConstructors.TryAdd(promotionKind, new RegistrationRecord<TPromotion>()))
            {
                throw new InvalidOperationException(String.Concat("Promotion already registered: ", promotionKind));
            }
            if (!_promotionTypeToPromotionKindKey.TryAdd(typeof(TPromotion).ToString(), promotionKind))
            {
                throw new InvalidOperationException(String.Concat("Promotion Type already registered:", typeof(TPromotion).ToString()));
            }
        }

		public bool UnregisterAdjustmentProvider(string promotionKind)
        {
            Contract.Assert(promotionKind != null);
            RegistrationRecord unused;
            return _promotionConstructors.TryRemove(promotionKind, out unused);
        }

        public TPromotion CreatePromotion<TPromotion>(string promotionKind) where TPromotion : IPromotion
        {
            Contract.Assert(promotionKind != null);
            var promotion = _promotionConstructors[promotionKind].UntypedConstruct();
            Contract.Assert(typeof(TPromotion).IsAssignableFrom(promotion.GetType()));
            return (TPromotion)(object)promotion;
        }


        public IPromotion CreatePromotion(string promotionKind)
        {
            Contract.Assert(promotionKind != null);
            return _promotionConstructors[promotionKind].UntypedConstruct();
        }

        public string GetSpecificPromotionKindString<TPromotion>() where TPromotion : IPromotion
        {
            return _promotionTypeToPromotionKindKey[typeof(TPromotion).ToString()];
        }

		public IEnumerable<string> GetPromotionKindStrings<TPromotion>() where TPromotion : IPromotion
		{
			return _promotionConstructors
				.Where(registrationRecord => registrationRecord.Value.Implements<TPromotion>())
				.Select(record => record.Key).Distinct();
		}
    }
}
