using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nsDistributor.Areas.Enroll.Models.Interfaces
{
    public class IProductModel
    {
        int ProductID { get; set; }

        decimal? RetailPrice { get; set; }

        decimal? CVPrice { get; set; }

        decimal? QVPrice { get; set; }

        string Name { get; set; }

        string SKU { get; set; }

        bool Active { get; set; }

        List<IPriceTypeInfoModel> AdditionalPriceTypeValues { get; set; }
    }

    public interface IPriceTypeInfoModel
    {
        string PriceTypeName { get; set; }
        decimal? Value { get; set; }
    }
}