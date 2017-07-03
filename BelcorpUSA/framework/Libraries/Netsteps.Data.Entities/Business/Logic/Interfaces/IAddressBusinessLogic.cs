using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Addresses.Common.Models;
using NetSteps.Common.Base;
using NetSteps.Common.Validation.NetTiers;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
	[ContractClass(typeof(Contracts.AddressBusinessLogicContracts))]
    public partial interface IAddressBusinessLogic
    {
        bool IsGeoCodeLookupEnabled(Address account);
        string ToDisplay(IAddress address, Extensions.IAddressExtensions.AddressDisplayTypes type, bool showPhone, bool showName = false, bool showProfileName = false, bool showCountry = true, bool showShipToEmail = false, string tagText = "");

        bool IsCountryIdOnAddressValid(object target, ValidationRuleArgs e);
        BasicResponseItem<List<IAddress>> ValidateAddressAccuracy(Address entity, ValidationRuleArgs e);

		void CopyPropertiesTo(IAddress address, IAddress targetAddress, bool copyAddressId);
    	void CopyPropertiesTo(IAddress source, IAddress target);
    }

	namespace Contracts
	{
		[ContractClassFor(typeof(IAddressBusinessLogic))]
		abstract class AddressBusinessLogicContracts : IAddressBusinessLogic
		{
			public bool IsGeoCodeLookupEnabled(Address account)
			{
				throw new NotImplementedException();
			}

			public string ToDisplay(IAddress address, Extensions.IAddressExtensions.AddressDisplayTypes type, bool showPhone, bool showName = false, bool showProfileName = false, bool showCountry = true, bool showShipToEmail = false, string tagText = "")
			{
				throw new NotImplementedException();
			}

			public bool IsCountryIdOnAddressValid(object target, ValidationRuleArgs e)
			{
				Contract.Requires<ArgumentNullException>(target != null);

				throw new NotImplementedException();
			}

			public BasicResponseItem<List<IAddress>> ValidateAddressAccuracy(Address entity, ValidationRuleArgs e)
			{
				throw new NotImplementedException();
			}

			public void CopyPropertiesTo(IAddress address, IAddress targetAddress, bool copyAddressId)
			{
				throw new NotImplementedException();
			}

			public void CopyPropertiesTo(IAddress source, IAddress target)
			{
				throw new NotImplementedException();
			}

			public Func<Address, int> GetIdColumnFunc
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public Action<Address, int> SetIdColumnFunc
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public Func<Address, string> GetTitleColumnFunc
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public Action<Address, string> SetTitleColumnFunc
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public void DefaultValues(Repositories.IAddressRepository repository, Address entity)
			{
				throw new NotImplementedException();
			}

			public Address Load(Repositories.IAddressRepository repository, int primaryKey)
			{
				throw new NotImplementedException();
			}

			public Address LoadFull(Repositories.IAddressRepository repository, int primaryKey)
			{
				throw new NotImplementedException();
			}

			public List<Address> LoadAll(Repositories.IAddressRepository repository)
			{
				throw new NotImplementedException();
			}

			public List<Address> LoadAllFull(Repositories.IAddressRepository repository)
			{
				throw new NotImplementedException();
			}

			public void Save(Repositories.IAddressRepository repository, Address entity)
			{
				throw new NotImplementedException();
			}

			public void Delete(Repositories.IAddressRepository repository, Address entity)
			{
				throw new NotImplementedException();
			}

			public void Delete(Repositories.IAddressRepository repository, int primaryKey)
			{
				throw new NotImplementedException();
			}

			public PaginatedList<AuditLogRow> GetAuditLog(Repositories.IAddressRepository repository, int primaryKey, AuditLogSearchParameters param)
			{
				throw new NotImplementedException();
			}

			public void StartEntityTracking(Address entity)
			{
				throw new NotImplementedException();
			}

			public void StartEntityTrackingAndEnableLazyLoading(Address entity)
			{
				throw new NotImplementedException();
			}

			public void StopEntityTracking(Address entity)
			{
				throw new NotImplementedException();
			}

			public void AcceptChanges(Address entity, List<IObjectWithChangeTracker> allTrackerItems = null)
			{
				throw new NotImplementedException();
			}

			public void Validate(Address entity)
			{
				throw new NotImplementedException();
			}

			public bool IsValid(Address entity)
			{
				throw new NotImplementedException();
			}

			public void AddValidationRules(Address entity)
			{
				throw new NotImplementedException();
			}

			public List<string> ValidatedChildPropertiesSetByParent(Repositories.IAddressRepository repository)
			{
				throw new NotImplementedException();
			}

			public void CleanDataBeforeSave(Repositories.IAddressRepository repository, Address entity)
			{
				throw new NotImplementedException();
			}

			public void UpdateCreatedFields(Repositories.IAddressRepository repository, Address entity)
			{
				throw new NotImplementedException();
			}
		}
	}
}