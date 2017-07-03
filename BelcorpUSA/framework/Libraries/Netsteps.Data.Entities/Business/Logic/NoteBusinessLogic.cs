using System;
using System.Collections.Generic;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Business.Logic
{
    public partial class NoteBusinessLogic
    {
        public override void DefaultValues(Repositories.INoteRepository repository, Note entity)
        {
            base.DefaultValues(repository, entity);

            entity.IsInternal = true;
        }

        public virtual List<Note> LoadByOrderNumber(Repositories.INoteRepository repository, string orderNumber)
        {
            try
            {
                var list = repository.LoadByOrderNumber(orderNumber);
                foreach (var item in list)
                {
                    item.StartEntityTracking();
                    item.IsLazyLoadingEnabled = true;
                }
                return list;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public virtual List<Note> LoadByOrderID(Repositories.INoteRepository repository, int orderID)
        {
            try
            {
                var list = repository.LoadByOrderID(orderID);
                foreach (var item in list)
                {
                    item.StartEntityTracking();
                    item.IsLazyLoadingEnabled = true;
                }
                return list;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public virtual List<Note> LoadByAccountNumber(Repositories.INoteRepository repository, string accountNumber)
        {
            try
            {
                var list = repository.LoadByAccountNumber(accountNumber);
                foreach (var item in list)
                {
                    item.StartEntityTracking();
                    item.IsLazyLoadingEnabled = true;
                }
                return list;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public virtual List<Note> LoadByAccountID(Repositories.INoteRepository repository, int accountID)
        {
            try
            {
                var list = repository.LoadByAccountID(accountID);
                foreach (var item in list)
                {
                    item.StartEntityTracking();
                    item.IsLazyLoadingEnabled = true;
                }
                return list;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

		public virtual List<Note> LoadBySupportTicketID(Repositories.INoteRepository repository, int supportTicketID)
		{
			try
			{
				var list = repository.LoadBySupportTicketID(supportTicketID);
				foreach (var item in list)
				{
					item.StartEntityTracking();
					item.IsLazyLoadingEnabled = true;
				}
				return list;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
    }
}

