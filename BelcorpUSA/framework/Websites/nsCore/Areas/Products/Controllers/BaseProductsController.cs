using NetSteps.Data.Entities.Cache;
using NetSteps.Encore.Core.IoC;
using nsCore.Controllers;

namespace nsCore.Areas.Products.Controllers
{
	public class BaseProductsController : BaseController
	{
		public InventoryBaseRepository Inventory { get { return Create.New<InventoryBaseRepository>(); } }
	}
}