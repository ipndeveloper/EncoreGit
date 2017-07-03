using System;
using System.Diagnostics.Contracts;
using NetSteps.Data.Common;
using NetSteps.Data.Common.Context;
using NetSteps.Encore.Core.IoC;
using NetSteps.Extensibility.Core;
using NetSteps.Promotions.Common;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Common.ModelConcrete;

namespace NetSteps.Promotions.Plugins.Base
{
    public abstract class BasePromotionQualificationHandler<TExtension, TRepository, TUnitOfWork> : IDataObjectExtensionProvider, IPromotionQualificationHandler
        where TExtension : IPromotionQualificationExtension
        where TRepository : IPromotionQualificationRepository<TExtension>
        where TUnitOfWork : IUnitOfWork
    {
        public BasePromotionQualificationHandler()
        {
        }

        public IDataObjectExtension SaveDataObjectExtension(IExtensibleDataObject entity)
        {
            Contract.Assert(entity is IPromotionQualification);
            Contract.Assert(typeof(TExtension).IsAssignableFrom(entity.Extension.GetType()));

            IPromotionQualification qualification = entity as IPromotionQualification;
            TExtension extension = (TExtension)(object)entity.Extension;

            extension.PromotionQualificationID = qualification.PromotionQualificationID;
            using (IContainer scopeContainer = Create.NewContainer())
            {
                using (TUnitOfWork uow = Container.Current.New<TUnitOfWork>())
                {
                    try
                    {
                        TRepository repository = Create.New<TRepository>();
                        extension = repository.SaveExtension(extension, uow);
                        qualification.Extension = extension;
                        uow.SaveChanges();
                        return extension;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

        public IDataObjectExtension GetDataObjectExtension(IExtensibleDataObject entity)
        {
            using (IContainer scopeContainer = Create.NewContainer())
            {
                using (TUnitOfWork uow = Container.Current.New<TUnitOfWork>())
                {
                    TRepository repository = Create.New<TRepository>();
                    entity.Extension = repository.RetrieveExtension(entity as IPromotionQualification, uow);
                }
                return entity.Extension;
            }
        }

        public virtual void UpdateDataObjectExtension(IExtensibleDataObject entity)
        {
            Contract.Assert(entity is IPromotionQualification);
            Contract.Assert(entity.Extension is TExtension);

            IPromotionQualification qualification = entity as IPromotionQualification;
            TExtension extension = (TExtension)(object)entity.Extension;
        }

        public void DeleteDataObjectExtension(IExtensibleDataObject entity)
        {
            Contract.Assert(entity is IPromotionQualification);

            IPromotionQualification qualification = entity as IPromotionQualification;

            using (IContainer scopeContainer = Create.NewContainer())
            {
                using (TUnitOfWork uow = Container.Current.New<TUnitOfWork>())
                {
                    TRepository repository = Create.New<TRepository>();
                    repository.DeleteExtension(qualification.PromotionQualificationID, uow);
                }
            }
        }

        public abstract PromotionQualificationResult Matches(IPromotion promotion, IPromotionQualificationExtension promotionQualification, IOrderContext orderContext);

        public abstract string GetProviderKey();

        public virtual bool RequiresCommitNotification { get { return false; } }
        public virtual bool RequiresRemoveNotification { get { return false; } }

        public virtual void Commit(IPromotion promotion, IPromotionQualificationExtension qualification, IOrderContext orderContext)
        { }

        public virtual void Remove(IPromotion promotion, IPromotionQualificationExtension qualification, IOrderContext orderContext)
        { }

        public abstract bool AreEqual(IPromotionQualificationExtension promotionQualification1, IPromotionQualificationExtension promotionQualification2);

		public abstract bool ValidFor<TProperty>(IPromotionQualificationExtension qualification, string propertyName, TProperty value);

		public void CheckValidity(string qualificationKey, IPromotionQualificationExtension qualification, IPromotionState state)
		{
			Contract.Assert(typeof(TExtension).IsAssignableFrom(qualification.GetType()));
			CheckValidity(qualificationKey, (TExtension)qualification, state);
		}

		public abstract void CheckValidity(string qualificationKey, TExtension qualification, IPromotionState state);
	}
}
