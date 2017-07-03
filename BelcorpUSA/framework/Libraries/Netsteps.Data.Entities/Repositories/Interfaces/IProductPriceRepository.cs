namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    public partial interface IProductPriceRepository
    {
        decimal GetRetilPerItem(int OrderID);
    }
}
