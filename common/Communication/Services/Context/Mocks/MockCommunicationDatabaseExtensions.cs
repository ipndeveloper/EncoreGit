using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using NetSteps.Communication.Common;
using E = NetSteps.Communication.Services.Entities;

namespace NetSteps.Communication.Services.Context.Mocks
{
    public static class MockCommunicationDatabaseExtensions
    {
        private static Random _rng = new Random();
        private static int randomInt()
        {
            return _rng.Next(1000000000, int.MaxValue);
        }
        private static readonly DateTime _startDate = new DateTime(2000, 1, 1);
        private static DateTime randomDate()
        {
            return _startDate.AddSeconds(_rng.NextDouble() * ((DateTime.Today - _startDate).TotalSeconds));
        }
        private static int _lastAccountAlertId = 1000000000;
        private static int newAccountAlertId()
        {
            return Interlocked.Increment(ref _lastAccountAlertId);
        }

        public static MockCommunicationDatabase InitializeData(this MockCommunicationDatabase database)
        {
            Contract.Requires<ArgumentNullException>(database != null);

            for (int i = 0; i < 10000; i++)
            {
                database.AddPromotionAccountAlert();
                database.AddMessageAccountAlert();
            }

            return database;
        }

        public static MockCommunicationDatabase AddMessageAccountAlert(
            this MockCommunicationDatabase database,
            int? accountAlertId = null,
            int? accountId = null,
            DateTime? createdDateUtc = null,
            DateTime? expirationDateUtc = null,
            DateTime? dismissedDateUtc = null,
            int? accountAlertDisplayKindId = null,
            string message = null)
        {
            Contract.Requires<ArgumentNullException>(database != null);

            var messageAccountAlert = CreateMessageAccountAlert(
                accountAlertId: accountAlertId,
                accountId: accountId,
                createdDateUtc: createdDateUtc,
                expirationDateUtc: expirationDateUtc,
                dismissedDateUtc: dismissedDateUtc,
                accountAlertDisplayKindId: accountAlertDisplayKindId,
                message: message
            );

            database.MessageAccountAlerts.Add(messageAccountAlert);
            database.AccountAlerts.Add(messageAccountAlert.AccountAlert);

            return database;
        }

        public static MockCommunicationDatabase AddPromotionAccountAlert(
            this MockCommunicationDatabase database,
            int? accountAlertId = null,
            int? accountId = null,
            DateTime? createdDateUtc = null,
            DateTime? expirationDateUtc = null,
            DateTime? dismissedDateUtc = null,
            int? accountAlertDisplayKindId = null,
            int? promotionId = null)
        {
            Contract.Requires<ArgumentNullException>(database != null);

            var promotionAccountAlert = CreatePromotionAccountAlert(
                accountAlertId: accountAlertId,
                accountId: accountId,
                createdDateUtc: createdDateUtc,
                expirationDateUtc: expirationDateUtc,
                dismissedDateUtc: dismissedDateUtc,
                accountAlertDisplayKindId: accountAlertDisplayKindId,
                promotionId: promotionId
            );

            database.PromotionAccountAlerts.Add(promotionAccountAlert);
            database.AccountAlerts.Add(promotionAccountAlert.AccountAlert);

            return database;
        }

        public static E.MessageAccountAlert CreateMessageAccountAlert(
            int? accountAlertId = null,
            int? accountId = null,
            DateTime? createdDateUtc = null,
            DateTime? expirationDateUtc = null,
            DateTime? dismissedDateUtc = null,
            int? accountAlertDisplayKindId = null,
            string message = null)
        {
            var accountAlert = CreateAccountAlert(
                providerKey: CommunicationConstants.AccountAlertProviderKey.Message,
                accountAlertId: accountAlertId,
                accountId: accountId,
                createdDateUtc: createdDateUtc,
                expirationDateUtc: expirationDateUtc,
                dismissedDateUtc: dismissedDateUtc,
                accountAlertDisplayKindId: accountAlertDisplayKindId
            );

            return new E.MessageAccountAlert
            {
                AccountAlertId = accountAlert.AccountAlertId,
                Message = message ?? randomInt().ToString(),

                AccountAlert = accountAlert
            };
        }

        public static E.AccountAlert CreateAccountAlert(
            int? accountAlertId = null,
            int? accountId = null,
            DateTime? createdDateUtc = null,
            DateTime? expirationDateUtc = null,
            DateTime? dismissedDateUtc = null,
            Guid? providerKey = null,
            int? accountAlertDisplayKindId = null)
        {
            return new E.AccountAlert
            {
                AccountAlertId = accountAlertId ?? newAccountAlertId(),
                AccountId = accountId ?? randomInt(),
                CreatedDateUtc = createdDateUtc ?? randomDate(),
                ExpirationDateUtc = expirationDateUtc,
                DismissedDateUtc = dismissedDateUtc,
                ProviderKey = providerKey ?? _rng.RandomStaticFieldValue<Guid>(typeof(CommunicationConstants.AccountAlertProviderKey)),
                AccountAlertDisplayKindId = accountAlertDisplayKindId ?? (int)_rng.RandomEnumValue<CommunicationConstants.AccountAlertDisplayKind>(true)
            };
        }

        public static E.PromotionAccountAlert CreatePromotionAccountAlert(
            int? accountAlertId = null,
            int? accountId = null,
            DateTime? createdDateUtc = null,
            DateTime? expirationDateUtc = null,
            DateTime? dismissedDateUtc = null,
            int? accountAlertDisplayKindId = null,
            int? promotionId = null)
        {
            var accountAlert = CreateAccountAlert(
                providerKey: CommunicationConstants.AccountAlertProviderKey.Promotion,
                accountAlertId: accountAlertId,
                accountId: accountId,
                createdDateUtc: createdDateUtc,
                expirationDateUtc: expirationDateUtc,
                dismissedDateUtc: dismissedDateUtc,
                accountAlertDisplayKindId: accountAlertDisplayKindId
            );

            return new E.PromotionAccountAlert
            {
                AccountAlertId = accountAlert.AccountAlertId,
                PromotionId = promotionId ?? randomInt(),

                AccountAlert = accountAlert
            };
        }

        private static T RandomStaticFieldValue<T>(this Random rng, Type type)
        {
            return type
                .GetMembers()
                .Where(x => x.MemberType == MemberTypes.Field)
                .Cast<FieldInfo>()
                .Where(x => x.IsStatic && x.FieldType == typeof(T))
                .Select(x => x.GetValue(null))
                .Cast<T>()
                .RandomElement(rng);
        }

        private static T RandomEnumValue<T>(this Random rng, bool ignoreFirstValue = false)
        {
            return Enum.GetValues(typeof(T))
                .OfType<T>()
                .Skip(ignoreFirstValue ? 1 : 0)
                .RandomElement(rng);
        }

        private static T RandomElement<T>(this IEnumerable<T> source, Random rng)
        {
            T current = default(T);
            int count = 0;
            foreach (T element in source)
            {
                count++;
                if (rng.Next(count) == 0)
                {
                    current = element;
                }
            }
            if (count == 0)
            {
                throw new InvalidOperationException("Sequence was empty");
            }
            return current;
        }
    }
}
