using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NetSteps.Communication.Common;
using E = NetSteps.Communication.Services.Entities;

namespace NetSteps.Communication.Services
{
    public class PromotionAccountAlertService : IPromotionAccountAlertService
    {
        protected readonly Func<ICommunicationContext> _contextFactory;
        protected readonly IPromotionAccountAlertRepository _repository;

        public PromotionAccountAlertService(
            Func<ICommunicationContext> contextFactory,
            IPromotionAccountAlertRepository repository)
        {
            Contract.Requires<ArgumentNullException>(contextFactory != null);
            Contract.Requires<ArgumentNullException>(repository != null);

            _contextFactory = contextFactory;
            _repository = repository;
        }

        public void Add(IPromotionAccountAlert model)
        {
            model.CreatedDateUtc = DateTime.UtcNow;

            using (var context = _contextFactory())
            {
                var entity = _repository.Add(context, model);
                context.SaveChanges();
                _repository.UpdateModel(model, entity);
            }
        }

        public IList<IPromotionAccountAlert> GetAll()
        {
            using (var context = _contextFactory())
            {
                return _repository
                    .ToList(context);
            }
        }

        public IList<IPromotionAccountAlert> GetBatch(IEnumerable<int> accountAlertIds)
        {
            if (!accountAlertIds.Any())
            {
                return new List<IPromotionAccountAlert>();
            }

            using (var context = _contextFactory())
            {
                return _repository
                    .Where(context, x => accountAlertIds.Contains(x.AccountAlertId));
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
    }
}
