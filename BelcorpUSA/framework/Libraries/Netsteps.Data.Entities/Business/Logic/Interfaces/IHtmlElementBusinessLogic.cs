using System;
using System.Diagnostics.Contracts;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
	[ContractClass(typeof(Contracts.HtmlElementBusinessLogicContracts))]
	public partial interface IHtmlElementBusinessLogic
	{
		string BuildElement(HtmlElement element, bool wrap = true);
	}

	namespace Contracts
	{
		[ContractClassFor(typeof(IHtmlElementBusinessLogic))]
		abstract class HtmlElementBusinessLogicContracts : IHtmlElementBusinessLogic
		{
			public string BuildElement(HtmlElement element, bool wrap = true)
			{
				Contract.Requires(element != null);

				throw new NotImplementedException();
			}

			public Func<HtmlElement, long> GetIdColumnFunc
			{
				get { throw new NotImplementedException(); }
			}

			public Action<HtmlElement, long> SetIdColumnFunc
			{
				get { throw new NotImplementedException(); }
			}

			public Func<HtmlElement, string> GetTitleColumnFunc
			{
				get { throw new NotImplementedException(); }
			}

			public Action<HtmlElement, string> SetTitleColumnFunc
			{
				get { throw new NotImplementedException(); }
			}

			public void DefaultValues(Repositories.IHtmlElementRepository repository, HtmlElement entity)
			{
				throw new NotImplementedException();
			}

			public HtmlElement Load(Repositories.IHtmlElementRepository repository, long primaryKey)
			{
				throw new NotImplementedException();
			}

			public HtmlElement LoadFull(Repositories.IHtmlElementRepository repository, long primaryKey)
			{
				throw new NotImplementedException();
			}

			public System.Collections.Generic.List<HtmlElement> LoadAll(Repositories.IHtmlElementRepository repository)
			{
				throw new NotImplementedException();
			}

			public System.Collections.Generic.List<HtmlElement> LoadAllFull(Repositories.IHtmlElementRepository repository)
			{
				throw new NotImplementedException();
			}

			public void Save(Repositories.IHtmlElementRepository repository, HtmlElement entity)
			{
				throw new NotImplementedException();
			}

			public void Delete(Repositories.IHtmlElementRepository repository, HtmlElement entity)
			{
				throw new NotImplementedException();
			}

			public void Delete(Repositories.IHtmlElementRepository repository, long primaryKey)
			{
				throw new NotImplementedException();
			}

			public NetSteps.Common.Base.PaginatedList<AuditLogRow> GetAuditLog(Repositories.IHtmlElementRepository repository, int primaryKey, AuditLogSearchParameters param)
			{
				throw new NotImplementedException();
			}

			public void StartEntityTracking(HtmlElement entity)
			{
				throw new NotImplementedException();
			}

			public void StartEntityTrackingAndEnableLazyLoading(HtmlElement entity)
			{
				throw new NotImplementedException();
			}

			public void StopEntityTracking(HtmlElement entity)
			{
				throw new NotImplementedException();
			}

			public void AcceptChanges(HtmlElement entity, System.Collections.Generic.List<IObjectWithChangeTracker> allTrackerItems = null)
			{
				throw new NotImplementedException();
			}

			public void Validate(HtmlElement entity)
			{
				throw new NotImplementedException();
			}

			public bool IsValid(HtmlElement entity)
			{
				throw new NotImplementedException();
			}

			public void AddValidationRules(HtmlElement entity)
			{
				throw new NotImplementedException();
			}

			public System.Collections.Generic.List<string> ValidatedChildPropertiesSetByParent(Repositories.IHtmlElementRepository repository)
			{
				throw new NotImplementedException();
			}

			public void CleanDataBeforeSave(Repositories.IHtmlElementRepository repository, HtmlElement entity)
			{
				throw new NotImplementedException();
			}

			public void UpdateCreatedFields(Repositories.IHtmlElementRepository repository, HtmlElement entity)
			{
				throw new NotImplementedException();
			}
		}
	}
}