using System.Collections.Generic;
using System.IO;
using NetSteps.Data.Entities;

namespace nsCore.Areas.Orders.Models.PDF
{
    public interface IBasePdfMaker
    {
        Stream GeneratePackingSlipPdf(List<Order> orders);
    }
}
