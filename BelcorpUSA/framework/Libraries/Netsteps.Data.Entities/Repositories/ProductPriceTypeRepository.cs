namespace NetSteps.Data.Entities.Repositories
{
    using System;
    using System.Linq;
    using NetSteps.Common.Utility;
    using NetSteps.Data.Entities.Exceptions;
    using System.Collections.Generic;
    using NetSteps.Data.Entities.Dto;

    public partial class ProductPriceTypeRepository : IProductPriceTypeRepository
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountTypeID"></param>
        /// <param name="relationshipType"></param>
        /// <param name="storeFrontID"></param>
        /// <returns></returns>
		public ProductPriceType LoadPriceType(int accountTypeID, Constants.PriceRelationshipType relationshipType, int storeFrontID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				if (accountTypeID == 0 || storeFrontID == 0)
					throw new Exception("Error loading PriceType. accountTypeID or storeFrontID is 0.");

				using (NetStepsEntities context = CreateContext())
				{
					AccountPriceType accountPriceType = context.AccountPriceTypes.FirstOrDefault(apt => apt.AccountTypeID == accountTypeID && apt.PriceRelationshipTypeID == (int)relationshipType && apt.StoreFrontID == storeFrontID);
					return accountPriceType == default(AccountPriceType) ? null : accountPriceType.ProductPriceType;
				}
			});
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="primaryKey"></param>
		public override void Delete(int primaryKey)
		{
			ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					string sql = @"DELETE FROM ProductPrices WHERE ProductPriceTypeID = @p0;
						UPDATE AccountPriceTypes SET ProductPriceTypeID = (SELECT TOP 1 ProductPriceTypeID FROM ProductPriceTypes) WHERE ProductPriceTypeID = @p0;
						UPDATE OrderItems SET ProductPriceTypeID = NULL WHERE ProductPriceTypeID = @p0;
						DELETE FROM ProductPriceTypes WHERE ProductPriceTypeID = @p0;";

					context.ExecuteStoreCommand(sql, primaryKey);
				}
			});
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool GetMandatory(int id)
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                string query = string.Format("SELECT Mandatory FROM ProductPriceTypes WHERE ProductPriceTypeID = '{0}'", id);
                var result = context.Database.SqlQuery<bool>(query);

                return result.First();
            }
        }

        /// <summary>
        /// Returns all product price types active
        /// </summary>
        /// <returns></returns>
        public List<ProductPriceTypeDto> ListProductPriceTypes()
        {
            var List = new List<ProductPriceTypeDto>();
            using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var data = (from r in DbContext.ProductPriceTypes.Where(r => r.Active == true)
                            select new ProductPriceTypeDto() { 
                               ProductPriceTypeID = r.ProductPriceTypeID,
                               Name = r.Name,
                               TermName = r.TermName,
                               Description = r.Description,
                               Active = r.Active,
                               Editable = r.Editable,
                               Mandatory = r.Mandatory
                            });
                if (data == null)
                    return List;
                else
                    return data.ToList();
            }
        }
    }
}
