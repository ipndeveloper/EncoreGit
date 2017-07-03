using System;
using System.Collections.Generic;
using System.Text;
using NetSteps.Addresses.Common.Models;
using NetSteps.Common;
using NetSteps.Common.Base;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.Validation.NetTiers;
using NetSteps.Data.Entities.Business.HelperObjects.AddressDisplay;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Interfaces;
using NetSteps.Encore.Core;
using NetSteps.Encore.Core.IoC;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using System.Text.RegularExpressions;

namespace NetSteps.Data.Entities.Business.Logic
{
	public partial class AddressBusinessLogic
	{
		public virtual bool IsGeoCodeLookupEnabled(Address account)
		{
			// Add custom rules per client here (maybe lookups up address type ect..) - JHE
			return true;
		}

		private string NameValueToDisplay(IAddress address, string cultureInfo)
		{
			string name = address.Name;
			if(name.IsNullOrEmpty())
			{
				name = Account.ToFullName(address.FirstName, string.Empty, address.LastName, cultureInfo);
			}
			if(name.IsNullOrEmpty())
			{
				name = address.Attention;
			}

			return name;
		}

		private string StateValueToDisplay(IAddress address)
		{
			string state = string.IsNullOrEmpty(address.State)
							   ? address.StateProvinceID.HasValue && address.StateProvinceID > 0
									 ? SmallCollectionCache.Instance.StateProvinces.GetById(address.StateProvinceID.Value).StateAbbreviation
									 : ""
							   : address.State;

			return state;
		}

		public virtual StringBuilder CountryAddressDisplayFormat(StringBuilder builder, IAddress address, string delimiter, bool showName,
            bool showPhone, bool showCountry, bool showShipToEmail, bool showProfileName)
		{
			Country country = GetCountry(address.CountryID);

			string nameValue = NameValueToDisplay(address, country.CultureInfo);

			string stateValue = StateValueToDisplay(address);
            
			var displayModel = new AddressDisplayModel
									{
										Address = address,
										Country = country,
										Delimiter = delimiter,
										Name = nameValue,
										ShowName = showName,
										ShowPhone = showPhone,
										ShowCountry = showCountry,
										ShowProfileName = showProfileName,
                                        State = stateValue,
                                        ShowShipToEmail = showShipToEmail
									};

			IAddressDisplayProvider displayProvider = new AddressDisplayProvider();

			return displayProvider.CountryAddressDisplayFormat(builder, displayModel);
		}

		/// <summary>
		/// Return address formatted to specific countries address format for display. 
		/// Look up additional formats to add as needed in the future. - JHE
		/// http://www.bitboost.com/ref/international-address-formats.html
		/// </summary>
		public virtual string ToDisplay(IAddress address, IAddressExtensions.AddressDisplayTypes type,
            bool showPhone, bool showName = false, bool showProfileName = false, bool showCountry = true, bool showShipToEmail = false, string tagText = "")
		{
			try
			{
				if (address == null)
					return string.Empty;

				string delimiter = string.Empty;

				if (string.IsNullOrWhiteSpace(tagText))
				{
					switch (type)
					{
						case IAddressExtensions.AddressDisplayTypes.Web: delimiter = "<br />"; break;
						case IAddressExtensions.AddressDisplayTypes.QAS: delimiter = "|"; break;
						case IAddressExtensions.AddressDisplayTypes.Windows: delimiter = Environment.NewLine; break;
						case IAddressExtensions.AddressDisplayTypes.SingleLine: delimiter = ", "; break;
					}
				}
				else
				{
					delimiter = "|";
				}

                //var builder = new StringBuilder();

                //builder = CountryAddressDisplayFormat(builder, address, delimiter, showName, showPhone, showCountry, showShipToEmail, showProfileName);

                //string result = builder.ToString();

                //if (!string.IsNullOrWhiteSpace(tagText))
                //{
                //    var newBuilder = new StringBuilder();
                //    string openTag = string.Format("<{0}>", tagText);
                //    string closeTag = string.Format("</{0}>", tagText);

                //    foreach (var item in result.Split(Convert.ToChar(delimiter)))
                //    {
                //        newBuilder.Append(openTag).Append(item).Append(closeTag);
                //    }

                //    result = newBuilder.ToString();
                //}
                
                

                //var regex = new Regex(Regex.Escape(delimiter));
                //result = regex.Replace(result, street, 1);

                string AddressString = string.Empty;

                var Address = new AddressRepository().GetAddressByID(address.AddressID);

                /*CS.18JUN.Inicio.Obtener Calle x AddressID*/
                string street = " " + new AddressRepository().GetStreetByAddressID(address.AddressID);
                
                
                /*CS.18JUN.Fin*/
                if (Address != null)
                {
                    Address.Street = street;
                    if (!string.IsNullOrEmpty(Address.Street))
                        AddressString += Address.Street + delimiter;

                    if (!string.IsNullOrEmpty(Address.Address1))
                    {
                        AddressString += Address.Address1.Trim();

                        if (!string.IsNullOrEmpty(Address.Address2))
                        {
                            AddressString += ", " + Address.Address2.Trim();
                        }

                        if (!string.IsNullOrEmpty(Address.Address3))
                        {
                            AddressString += ", " + Address.Address3.Trim();
                        }

                        AddressString += delimiter;
                    }

                    if (!string.IsNullOrEmpty(Address.CountryStr))
                        AddressString += Address.CountryStr + delimiter;

                    if (!string.IsNullOrEmpty(Address.City))
                        AddressString += Address.City + delimiter;

                    if (!string.IsNullOrEmpty(Address.State))
                        AddressString += Address.State + " ";

                    if (!string.IsNullOrEmpty(Address.PostalCode))
                        AddressString += Address.PostalCode + delimiter;
                }

                return AddressString;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public override void AddValidationRules(Address entity)
		{
			bool allowPOBoxShipment = ConfigurationManager.GetAppSetting(ConfigurationManager.VariableKey.AllowPOBoxShipment, false);

			if(!allowPOBoxShipment && entity.AddressTypeID == Constants.AddressType.Shipping.ToShort())
			{
				entity.ValidationRules.AddRule(CommonRules.RegexIsNotMatch,
					new CommonRules.RegexRuleArgs("Address1", RegularExpressions.PoBox,
						Translation.GetTerm("InvalidShippingAddressErrorMessage", CustomValidationMessages.ShippingPOBox), true));
			}
			else
			{
				base.AddValidationRules(entity);
			}

			entity.ValidationRules.AddRule(Address.IsCountryIdOnAddressValid, new ValidationRuleArgs("AddressID", "Invalid CountryID"));
		}

		/// <summary>
		/// Validated that the StateProvinceID selected on an address has the same CountryID 
		/// on the Address as the CountryID on StateProvince table - JHE
		/// </summary>
		/// <param name="target"></param>
		/// <param name="e"></param>
		/// <returns></returns>
		public virtual bool IsCountryIdOnAddressValid(object target, ValidationRuleArgs e)
		{
			var address = target as IAddress;
			if (address == null)
			{
				e.Description = Translation.GetTerm("InvalidTargetNotOfTypeAddress", "Invalid target. Not of type Address.");
				return false;
			}

			StateProvince stateProvinces = null;
			if(address.StateProvinceID.HasValue && address.StateProvinceID.Value > 0)
			{
				stateProvinces = SmallCollectionCache.Instance.StateProvinces.GetById(address.StateProvinceID.Value);
			}

			if (stateProvinces != null && stateProvinces.CountryID != address.CountryID)
			{
				e.Description = Translation.GetTerm("InvalidAddressCountryDoesMatchState", "Invalid Address. Country does match state.");
				return false;
			}
			return true;
		}

		public virtual BasicResponseItem<List<IAddress>> ValidateAddressAccuracy(Address entity, ValidationRuleArgs e)
		{
			// TODO: Do 3rd party Address validation here - JHE
			var addressResponses = new List<IAddress> {entity};
			return new BasicResponseItem<List<IAddress>>
			{
				Item = addressResponses,
				Success = true
			};
		}

		public virtual Country GetCountry(int countryID)
		{
			Country country = SmallCollectionCache.Instance.Countries.GetById(countryID);
			if(country.IsNull() || String.IsNullOrEmpty(country.CultureInfo))
			{
				country = SmallCollectionCache.Instance.Countries.GetById(Constants.Country.UnitedStates.ToInt());
			}

			return country;
		}

		public virtual void CopyPropertiesTo(IAddress source, IAddress target, bool copyAddressId)
		{
			using (var c = Create.SharedOrNewContainer())
			{
				var copier = Create.New<ICopier<IAddress, IAddress>>();
				var copiedSource = source;

				if (!copyAddressId)
				{
					// Create a copy so as not to alter the source
					copiedSource = copier.Copy(source);
					copiedSource.AddressID = target.AddressID;
				}

				copier.CopyTo(target, copiedSource, CopyKind.Loose, c);
			}
		}

		public virtual void CopyPropertiesTo(IAddress source, IAddress target)
		{
        	target.Address1 = source.Address1;
        	target.Address2 = source.Address2;
        	target.Address3 = source.Address3;
			if (source.AddressTypeID > 0)
			{
				target.AddressTypeID = source.AddressTypeID;
			}
        	target.Attention = source.Attention;
        	target.City = source.City;
        	target.CountryID = source.CountryID;
        	target.County = source.County;
        	target.FirstName = source.FirstName;
        	target.IsDefault = source.IsDefault;
        	target.LastName = source.LastName;
        	target.Latitude = source.Latitude;
        	target.Longitude = source.Longitude;
        	target.Name = source.Name;
        	target.PhoneNumber = source.PhoneNumber;
        	target.PostalCode = source.PostalCode;
			if (!string.IsNullOrEmpty(source.ProfileName))
			{
				target.ProfileName = source.ProfileName;
			}
        	target.State = source.State;
			if (source.StateProvinceID > 0)
            {
				target.StateProvinceID = source.StateProvinceID;
            }
		}

        public void UpdateAddressStreet(Address address)
        {
            new AddressRepository().UpdateAddressStreet(address);
        }

        public CompanyAddressSearchData GetCompanyAddress(int CompanyID)
        {
            return new AddressRepository().GetCompanyAddress(CompanyID);
        }
	}
}