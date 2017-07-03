using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Common.Entities;

namespace NetSteps.Data.Common.Services
{
	public interface IPriceTypeService
	{
		IEnumerable<IPriceType> GetCurrencyPriceTypes();
		IEnumerable<IPriceType> GetVolumePriceTypes();
		IPriceType GetPriceType(int priceTypeID);
		IPriceType GetPriceType(string priceTypeName);
		IPriceType GetPriceType(int accountTypeID, int priceRelationshipTypeID, int storeFrontID, int? orderTypeID = null);
	}
}
