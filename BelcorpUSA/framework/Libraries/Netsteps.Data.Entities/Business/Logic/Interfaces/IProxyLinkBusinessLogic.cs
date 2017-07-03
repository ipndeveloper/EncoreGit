using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Data.Entities.Interfaces;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
	[ContractClass(typeof(Contracts.ProxyLinkBusinessLogicContracts))]
    public partial interface IProxyLinkBusinessLogic : IBusinessEntityLogic<ProxyLink, Int32, IProxyLinkRepository>, IBusinessLogic
    {
        IList<ProxyLinkData> GetAccountProxyLinks(int accountID, short accountTypeID);
        string FormatAccountProxyLinkUrl(ProxyLink proxyLink, int accountID, short accountTypeID);
    }

	namespace Contracts
	{
		[ContractClassFor(typeof(IProxyLinkBusinessLogic))]
		abstract class ProxyLinkBusinessLogicContracts : IProxyLinkBusinessLogic
		{
			public IList<ProxyLinkData> GetAccountProxyLinks(int accountID, short accountTypeID)
			{
				Contract.Requires<ArgumentOutOfRangeException>(accountID > 0);

				throw new NotImplementedException();
			}

			public string FormatAccountProxyLinkUrl(ProxyLink proxyLink, int accountID, short accountTypeID)
			{
				Contract.Requires<ArgumentNullException>(proxyLink != null);
				Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(proxyLink.URL));
				Contract.Requires<ArgumentOutOfRangeException>(accountID > 0);

				throw new NotImplementedException();
			}

			public Func<ProxyLink, int> GetIdColumnFunc
			{
				get { throw new NotImplementedException(); }
			}

			public Action<ProxyLink, int> SetIdColumnFunc
			{
				get { throw new NotImplementedException(); }
			}

			public Func<ProxyLink, string> GetTitleColumnFunc
			{
				get { throw new NotImplementedException(); }
			}

			public Action<ProxyLink, string> SetTitleColumnFunc
			{
				get { throw new NotImplementedException(); }
			}

			public void DefaultValues(IProxyLinkRepository repository, ProxyLink entity)
			{
				throw new NotImplementedException();
			}

			public ProxyLink Load(IProxyLinkRepository repository, int primaryKey)
			{
				throw new NotImplementedException();
			}

			public ProxyLink LoadFull(IProxyLinkRepository repository, int primaryKey)
			{
				throw new NotImplementedException();
			}

			public List<ProxyLink> LoadAll(IProxyLinkRepository repository)
			{
				throw new NotImplementedException();
			}

			public List<ProxyLink> LoadAllFull(IProxyLinkRepository repository)
			{
				throw new NotImplementedException();
			}

			public void Save(IProxyLinkRepository repository, ProxyLink entity)
			{
				throw new NotImplementedException();
			}

			public void Delete(IProxyLinkRepository repository, ProxyLink entity)
			{
				throw new NotImplementedException();
			}

			public void Delete(IProxyLinkRepository repository, int primaryKey)
			{
				throw new NotImplementedException();
			}

			public NetSteps.Common.Base.PaginatedList<AuditLogRow> GetAuditLog(IProxyLinkRepository repository, int primaryKey, AuditLogSearchParameters param)
			{
				throw new NotImplementedException();
			}

			public void StartEntityTracking(ProxyLink entity)
			{
				throw new NotImplementedException();
			}

			public void StartEntityTrackingAndEnableLazyLoading(ProxyLink entity)
			{
				throw new NotImplementedException();
			}

			public void StopEntityTracking(ProxyLink entity)
			{
				throw new NotImplementedException();
			}

			public void AcceptChanges(ProxyLink entity, List<IObjectWithChangeTracker> allTrackerItems = null)
			{
				throw new NotImplementedException();
			}

			public void Validate(ProxyLink entity)
			{
				throw new NotImplementedException();
			}

			public bool IsValid(ProxyLink entity)
			{
				throw new NotImplementedException();
			}

			public void AddValidationRules(ProxyLink entity)
			{
				throw new NotImplementedException();
			}

			public List<string> ValidatedChildPropertiesSetByParent(IProxyLinkRepository repository)
			{
				throw new NotImplementedException();
			}

			public void CleanDataBeforeSave(IProxyLinkRepository repository, ProxyLink entity)
			{
				throw new NotImplementedException();
			}

			public void UpdateCreatedFields(IProxyLinkRepository repository, ProxyLink entity)
			{
				throw new NotImplementedException();
			}
		}
	}
}