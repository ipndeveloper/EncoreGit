using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Diagnostics.Contracts;
using System.Linq;
using NetSteps.Addresses.Common.Models;
using NetSteps.Common.Extensions;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Encore.Core.IoC;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using System.Configuration;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class AddressRepository
    {
        protected override Func<NetStepsEntities, IQueryable<Address>> loadAllFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<Address>>(
                   (context) => from l in context.Addresses
                                               .Include("AddressProperties")
                                select l);
            }
        }
        public Address GetByNumber(string addressNumber)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return context.Addresses.Include("AddressProperties").Include("Country").
                        Where(x => x.AddressNumber == addressNumber).FirstOrDefault();
                }
            });
        }

		public IEnumerable<Address> GetByAddressTypePostalCodeAndCity(int addressTypeId, string postalCode, string city)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return context.Addresses.Where(ad =>
						ad.AddressTypeID == addressTypeId &&
						postalCode.Equals(ad.PostalCode, StringComparison.InvariantCultureIgnoreCase) &&
						city.Equals(ad.City, StringComparison.InvariantCultureIgnoreCase)).ToList();
				}
			});
		}

		public List<Address> GetByAccountId(int accountId)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return context.Addresses.Include("Country").
                        Where(x => x.Accounts.Select(y => y.AccountID).Contains(accountId)).ToList();
                }
            });
        }

        public bool IsUsedByAnyActiveOrderTemplates(int addressID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return context.OrderShipments
                        .Any(os => os.SourceAddressID == addressID
                                   && os.Order.OrderType.IsTemplate
                                   && os.Order.OrderStatusID == (short)Constants.OrderStatus.Paid);
                }
            });
        }


        public IGeoCodeData LookUpGeoCodeFromExistingAddresses(IAddress address)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    IQueryable<Address> matchingItems = context.Addresses;

                    if (!address.County.ToCleanString().IsNullOrEmpty())
                        matchingItems = matchingItems.Where(a => a.County == address.County);
                    if (!address.City.ToCleanString().IsNullOrEmpty())
                        matchingItems = matchingItems.Where(a => a.City == address.City);
                    if (!address.State.ToCleanString().IsNullOrEmpty())
                        matchingItems = matchingItems.Where(a => a.State == address.State);
                    if (!address.PostalCode.ToCleanString().IsNullOrEmpty())
                        matchingItems = matchingItems.Where(a => a.PostalCode == address.PostalCode);
                    if (address.CountryID != 0)
                        matchingItems = matchingItems.Where(a => a.CountryID == address.CountryID);

                    matchingItems = matchingItems.Where(a => a.IsGeoCodeCurrent && a.Latitude.HasValue && a.Longitude.HasValue);

                    var geoCodeData = matchingItems.Select(a => new
                    {
                        a.Longitude,
                        a.Latitude
                    }).FirstOrDefault();

                    if (geoCodeData == null)
                        return null;

					var result = Create.New<IGeoCodeData>();
					result.Latitude = geoCodeData.Latitude.Value;
					result.Longitude = geoCodeData.Longitude.Value;
					return result;
                }
            });
        }

        public IGeoCodeData LookUpGeoCodeFromExistingAddressesByCityState(IAddress address)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    IQueryable<Address> matchingItems = context.Addresses;
                    if (!address.City.ToCleanString().IsNullOrEmpty())
                        matchingItems = matchingItems.Where(a => a.City == address.City);
                    if (!address.State.ToCleanString().IsNullOrEmpty())
                        matchingItems = matchingItems.Where(a => a.State == address.State);

                    matchingItems = matchingItems.Where(a => a.Latitude.HasValue && a.Longitude.HasValue);

                    var geoCodeData = matchingItems.Select(a => new
                    {
                        a.Longitude,
                        a.Latitude
                    }).FirstOrDefault();

                    if (geoCodeData == null)
                        return null;
					
					var result = Create.New<IGeoCodeData>();
					result.Latitude = geoCodeData.Latitude.Value;
					result.Longitude = geoCodeData.Longitude.Value;
					return result;
                }
            });
        }

        public void UpdateAddressStreet(Address address)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand command = connection.CreateCommand();                                      
                    command.Connection = connection;                    

                    try
                    {
                        command.CommandText = "UpdateAddressStreet";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@AddressID", SqlDbType.Int) { Value = address.AddressID });
                        command.Parameters.Add(new SqlParameter("@Street", SqlDbType.NVarChar, 200) { Value = address.Street });
                        command.ExecuteNonQuery();
                        
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CompanyAddressSearchData GetCompanyAddress(int CompanyID)
        {
            try
            {
                CompanyAddressSearchData address = null;

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[GetCompanyAddress]";
                    cmd.Parameters.AddWithValue("@CompanyID", CompanyID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            address = new CompanyAddressSearchData();

                            #region [Assign Values]

                            address.CompanyID = Convert.ToInt32(reader["CompanyID"]);
                            address.FirstName = reader["FirstName"] != DBNull.Value ? reader["FirstName"].ToString() : null;
                            address.LastName = reader["LastName"] != DBNull.Value ? reader["LastName"].ToString() : null;
                            address.Address1 = reader["Address1"] != DBNull.Value ? reader["Address1"].ToString() : null;
                            address.Address2 = reader["Address2"] != DBNull.Value ? reader["Address2"].ToString() : null;
                            address.City = reader["City"] != DBNull.Value ? reader["City"].ToString() : null;
                            address.StateProvinceID = reader["StateProvinceID"] != DBNull.Value ? Convert.ToInt32(reader["StateProvinceID"]) : -1;
                            address.PostalCode = reader["PostalCode"] != DBNull.Value ? reader["PostalCode"].ToString() : null;
                            #endregion

                        }
                    }
                }

                return address;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public string GetStreetByAddressID(int AddressID)
        {
            try
            {
                string result = string.Empty;

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[uspGetStreetByAddressID]";
                    cmd.Parameters.AddWithValue("@AddressID", AddressID);
                    cmd.Parameters.Add(new SqlParameter("@Street", SqlDbType.VarChar, 250));
                    cmd.Parameters["@Street"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    result = cmd.Parameters["@Street"].Value.ToString();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public Address GetAddressByID(int AddressID)
        {
            try
            {
                Address address = null;

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[uspGetAddressByID]";
                    cmd.Parameters.AddWithValue("@AddressID", AddressID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            address = new Address();

                            #region [Assign Values]
                            address.AddressID = Convert.ToInt32(reader["AddressID"]);
                            address.AddressTypeID = Convert.ToInt16(reader["AddressTypeID"]);
                            address.ProfileName = reader["ProfileName"] != DBNull.Value ? reader["ProfileName"].ToString() : null;
                            address.FirstName = reader["FirstName"] != DBNull.Value ? reader["FirstName"].ToString() : null;
                            address.LastName = reader["LastName"] != DBNull.Value ? reader["LastName"].ToString() : null;
                            address.Address1 = reader["Address1"] != DBNull.Value ? reader["Address1"].ToString() : null;
                            address.Address2 = reader["Address2"] != DBNull.Value ? reader["Address2"].ToString() : null;
                            address.Address3 = reader["Address3"] != DBNull.Value ? reader["Address3"].ToString() : null;
                            address.City = reader["City"] != DBNull.Value ? reader["City"].ToString() : null;
                            address.State = reader["State"] != DBNull.Value ? reader["State"].ToString() : null;
                            address.StateProvinceID = reader["StateProvinceID"] != DBNull.Value ? Convert.ToInt32(reader["StateProvinceID"]) : -1;
                            address.PostalCode = reader["PostalCode"] != DBNull.Value ? reader["PostalCode"].ToString() : null;
                            address.CountryStr = reader["Country"] != DBNull.Value ? reader["Country"].ToString() : null;
                            address.Street = reader["Street"] != DBNull.Value ? reader["Street"].ToString() : null;
                            #endregion

                        }
                    }
                }

                return address;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}
