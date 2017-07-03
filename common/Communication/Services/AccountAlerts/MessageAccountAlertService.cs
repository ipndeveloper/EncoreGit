using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using NetSteps.Communication.Common;

namespace NetSteps.Communication.Services
{
    public class MessageAccountAlertService : IMessageAccountAlertService
    {
        protected readonly Func<ICommunicationContext> _contextFactory;
        protected readonly IMessageAccountAlertRepository _repository;

        public MessageAccountAlertService(
            Func<ICommunicationContext> contextFactory,
            IMessageAccountAlertRepository repository)
        {
            Contract.Requires<ArgumentNullException>(contextFactory != null);
            Contract.Requires<ArgumentNullException>(repository != null);

            _contextFactory = contextFactory;
            _repository = repository;
        }

        public void Add(IMessageAccountAlert model)
        {
            model.CreatedDateUtc = DateTime.UtcNow;

            using (var context = _contextFactory())
            {
                var entity = _repository.Add(context, model);
                context.SaveChanges();
                _repository.UpdateModel(model, entity);
            }
        }

        public void Dismiss(int accountAlertId, int accountId)
        {
            using (var context = _contextFactory())
            {
                // An exception will be thrown if accountId does not match.
                var model = _repository
                    .First(
                        context,
                        x => x.AccountAlertId == accountAlertId
                            && x.AccountAlert.AccountId == accountId
                    );

                model.DismissedDateUtc = DateTime.UtcNow;

                _repository.Update(context, model);
                context.SaveChanges();
            }
        }

        public IList<IMessageAccountAlert> GetBatch(IEnumerable<int> accountAlertIds)
        {
            if (!accountAlertIds.Any())
            {
                return new List<IMessageAccountAlert>();
            }

            using (var context = _contextFactory())
            {
                return _repository.Where(context, x => accountAlertIds.Contains(x.AccountAlertId));
            }
        }

        public IList<IMessageAccountAlert> GetAll()
        {
            using (var context = _contextFactory())
            {
                return _repository.ToList(context);
            }
        }
    }
}
