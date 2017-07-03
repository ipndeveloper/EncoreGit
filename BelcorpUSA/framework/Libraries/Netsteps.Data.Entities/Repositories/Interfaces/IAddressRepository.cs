using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Addresses.Common.Models;

namespace NetSteps.Data.Entities.Repositories
{
	[ContractClass(typeof(Contracts.AddressRepositoryContracts))]
    public partial interface IAddressRepository
    {
        IGeoCodeData LookUpGeoCodeFromExistingAddresses(IAddress address);
        IGeoCodeData LookUpGeoCodeFromExistingAddressesByCityState(IAddress address);
        Address GetByNumber(string addressNumber);
				IEnumerable<Address> GetByAddressTypePostalCodeAndCity(int addressTypeId, string postalCode, string city);
        List<Address> GetByAccountId(int accountId);
        bool IsUsedByAnyActiveOrderTemplates(int addressID);
    }

	namespace Contracts
	{
		[ContractClassFor(typeof(IAddressRepository))]
		abstract class AddressRepositoryContracts : IAddressRepository
		{
			public IGeoCodeData LookUpGeoCodeFromExistingAddresses(IAddress address)
			{
				throw new NotImplementedException();
			}

			public IGeoCodeData LookUpGeoCodeFromExistingAddressesByCityState(IAddress address)
			{
				throw new NotImplementedException();
			}

			public Address GetByNumber(string addressNumber)
			{
				throw new NotImplementedException();
			}

			public IEnumerable<Address> GetByAddressTypePostalCodeAndCity(int addressTypeId, string postalCode, string city)
			{
				Contract.Requires<ArgumentException>(addressTypeId > 0);
				Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(postalCode));
				Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(city));

				throw new NotImplementedException();
			}

			public List<Address> GetByAccountId(int accountId)
			{
				throw new NotImplementedException();
			}

			public bool IsUsedByAnyActiveOrderTemplates(int addressID)
			{
				throw new NotImplementedException();
			}

			public NetSteps.Common.PrimaryKeyInfo PrimaryKeyInfo
			{
				get { throw new NotImplementedException(); }
			}

			public Address Load(int primaryKey)
			{
				throw new NotImplementedException();
			}

			public Address LoadFull(int primaryKey)
			{
				throw new NotImplementedException();
			}

			public List<Address> LoadAll()
			{
				throw new NotImplementedException();
			}

			public List<Address> LoadAllFull()
			{
				throw new NotImplementedException();
			}

			public List<Address> LoadBatch(IEnumerable<int> primaryKeys)
			{
				throw new NotImplementedException();
			}

			public List<Address> LoadBatchFull(IEnumerable<int> primaryKeys)
			{
				throw new NotImplementedException();
			}

			public NetSteps.Common.Base.SqlUpdatableList<Address> LoadAllFullWithSqlDependency()
			{
				throw new NotImplementedException();
			}

			public NetSteps.Common.Base.SqlUpdatableList<Address> LoadBatchWithSqlDependency(IEnumerable<int> primaryKeys)
			{
				throw new NotImplementedException();
			}

			public void Save(Address obj)
			{
				throw new NotImplementedException();
			}

			public void SaveBatch(IEnumerable<Address> items)
			{
				throw new NotImplementedException();
			}

			public void Delete(Address obj)
			{
				throw new NotImplementedException();
			}

			public void Delete(int primaryKey)
			{
				throw new NotImplementedException();
			}

			public void DeleteBatch(IEnumerable<int> primaryKeys)
			{
				throw new NotImplementedException();
			}

			public bool Exists(int primaryKey)
			{
				throw new NotImplementedException();
			}

			public NetSteps.Common.Base.PaginatedList<AuditLogRow> GetAuditLog(int primaryKey, Business.AuditLogSearchParameters param)
			{
				throw new NotImplementedException();
			}

			public Address GetRandomRecord()
			{
				throw new NotImplementedException();
			}

			public Address GetRandomRecordFull()
			{
				throw new NotImplementedException();
			}

			public int Count()
			{
				throw new NotImplementedException();
			}

			public int Count(System.Linq.Expressions.Expression<Func<Address, bool>> predicate)
			{
				throw new NotImplementedException();
			}

			public bool Any()
			{
				throw new NotImplementedException();
			}

			public bool Any(System.Linq.Expressions.Expression<Func<Address, bool>> predicate)
			{
				throw new NotImplementedException();
			}

			public List<Address> Where(System.Linq.Expressions.Expression<Func<Address, bool>> predicate)
			{
				throw new NotImplementedException();
			}

			public List<Address> Where(System.Linq.Expressions.Expression<Func<Address, bool>> predicate, IEnumerable<string> includes)
			{
				throw new NotImplementedException();
			}

			public List<TSelected> WhereSelect<TSelected>(System.Linq.Expressions.Expression<Func<Address, bool>> predicate, System.Linq.Expressions.Expression<Func<Address, TSelected>> selector)
			{
				throw new NotImplementedException();
			}

			public Address FirstOrDefault(System.Linq.Expressions.Expression<Func<Address, bool>> predicate)
			{
				throw new NotImplementedException();
			}

			public Address FirstOrDefault(System.Linq.Expressions.Expression<Func<Address, bool>> predicate, IEnumerable<string> includes)
			{
				throw new NotImplementedException();
			}

			public TSelected FirstOrDefaultSelect<TSelected>(System.Linq.Expressions.Expression<Func<Address, bool>> predicate, System.Linq.Expressions.Expression<Func<Address, TSelected>> selector)
			{
				throw new NotImplementedException();
			}
		}
	}
}
