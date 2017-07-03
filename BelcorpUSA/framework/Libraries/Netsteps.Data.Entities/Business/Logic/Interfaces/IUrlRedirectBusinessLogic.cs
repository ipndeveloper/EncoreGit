using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Common;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
	[ContractClass(typeof(Contracts.UrlRedirectBusinessLogicContracts))]
    public partial interface IUrlRedirectBusinessLogic
    {
        IList<IUrlRedirect> GetUrlRedirects(IUrlRedirectRepository repository, IEnumerable<short> siteTypeIDs);
    }

	namespace Contracts
	{
		[ContractClassFor(typeof(IUrlRedirectBusinessLogic))]
		abstract class UrlRedirectBusinessLogicContracts : IUrlRedirectBusinessLogic
		{
			public IList<IUrlRedirect> GetUrlRedirects(IUrlRedirectRepository repository, IEnumerable<short> siteTypeIDs)
			{
				Contract.Requires<ArgumentNullException>(repository != null);
				Contract.Requires<ArgumentNullException>(siteTypeIDs != null);

				throw new NotImplementedException();
			}

			public Func<UrlRedirect, int> GetIdColumnFunc
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public Action<UrlRedirect, int> SetIdColumnFunc
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public Func<UrlRedirect, string> GetTitleColumnFunc
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public Action<UrlRedirect, string> SetTitleColumnFunc
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public void DefaultValues(IUrlRedirectRepository repository, UrlRedirect entity)
			{
				throw new NotImplementedException();
			}

			public UrlRedirect Load(IUrlRedirectRepository repository, int primaryKey)
			{
				throw new NotImplementedException();
			}

			public UrlRedirect LoadFull(IUrlRedirectRepository repository, int primaryKey)
			{
				throw new NotImplementedException();
			}

			public List<UrlRedirect> LoadAll(IUrlRedirectRepository repository)
			{
				throw new NotImplementedException();
			}

			public List<UrlRedirect> LoadAllFull(IUrlRedirectRepository repository)
			{
				throw new NotImplementedException();
			}

			public void Save(IUrlRedirectRepository repository, UrlRedirect entity)
			{
				throw new NotImplementedException();
			}

			public void Delete(IUrlRedirectRepository repository, UrlRedirect entity)
			{
				throw new NotImplementedException();
			}

			public void Delete(IUrlRedirectRepository repository, int primaryKey)
			{
				throw new NotImplementedException();
			}

			public NetSteps.Common.Base.PaginatedList<AuditLogRow> GetAuditLog(IUrlRedirectRepository repository, int primaryKey, AuditLogSearchParameters param)
			{
				throw new NotImplementedException();
			}

			public void StartEntityTracking(UrlRedirect entity)
			{
				throw new NotImplementedException();
			}

			public void StartEntityTrackingAndEnableLazyLoading(UrlRedirect entity)
			{
				throw new NotImplementedException();
			}

			public void StopEntityTracking(UrlRedirect entity)
			{
				throw new NotImplementedException();
			}

			public void AcceptChanges(UrlRedirect entity, List<IObjectWithChangeTracker> allTrackerItems = null)
			{
				throw new NotImplementedException();
			}

			public void Validate(UrlRedirect entity)
			{
				throw new NotImplementedException();
			}

			public bool IsValid(UrlRedirect entity)
			{
				throw new NotImplementedException();
			}

			public void AddValidationRules(UrlRedirect entity)
			{
				throw new NotImplementedException();
			}

			public List<string> ValidatedChildPropertiesSetByParent(IUrlRedirectRepository repository)
			{
				throw new NotImplementedException();
			}

			public void CleanDataBeforeSave(IUrlRedirectRepository repository, UrlRedirect entity)
			{
				throw new NotImplementedException();
			}

			public void UpdateCreatedFields(IUrlRedirectRepository repository, UrlRedirect entity)
			{
				throw new NotImplementedException();
			}
		}
	}
}