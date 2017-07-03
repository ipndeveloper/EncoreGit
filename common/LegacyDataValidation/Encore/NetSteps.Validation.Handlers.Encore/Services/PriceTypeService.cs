using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.Handlers.Common.Services;
using NetSteps.Validation.Handlers.Common.Services.ServiceModels;
using NetSteps.Validation.Handlers.Encore.Common.Services;

namespace NetSteps.Validation.Handlers.Services
{
    public class PriceTypeService : IPriceTypeService
    {
        public const int PrimaryVolumePriceTypeID = 21;

        protected readonly Func<IProductPriceType> PriceTypeConstructor;

        public PriceTypeService(Func<IProductPriceType> priceTypeConstructor)
        {
            PriceTypes = new List<IProductPriceType>();
            PriceTypeConstructor = priceTypeConstructor;

            AddPriceType(PriceTypeCategory.Currency, "Retail", 1);
            AddPriceType(PriceTypeCategory.Currency, "Wholesale", 22);
            AddPriceType(PriceTypeCategory.Volume, "CV", 18);
            AddPriceType(PriceTypeCategory.Volume, "QV", 21);
        }

        protected bool AddPriceType(PriceTypeCategory category, string name, int priceTypeID)
        {
            var found = PriceTypes.SingleOrDefault(x => x.PriceTypeID == priceTypeID || x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            if (found != null)
            {
                return false;
            }
            else
            {
                var priceType = PriceTypeConstructor();
                priceType.Category = category;
                priceType.Name = name;
                priceType.PriceTypeID = priceTypeID;
                PriceTypes.Add(priceType);
                return true;
            }
        }

        protected bool RemovePriceType(int priceTypeID)
        {
            var found = PriceTypes.SingleOrDefault(x => x.PriceTypeID == priceTypeID);
            if (found != null)
            {
                PriceTypes.Remove(found);
                return false;
            }
            else
            {
                return true;
            }
        }
        protected List<IProductPriceType> PriceTypes { get; private set; }
        
        public IProductPriceType GetPriceType(int productPriceTypeId)
        {
            var priceType = PriceTypes.SingleOrDefault(x => x.PriceTypeID == productPriceTypeId);
            if (priceType != null)
            {
                return priceType;
            }
            throw new Exception(String.Format("Price type {0} not found.", productPriceTypeId));
        }

        public IProductPriceType GetPriceType(string priceTypeName)
        {
            var priceType = PriceTypes.SingleOrDefault(x => x.Name.Equals(priceTypeName, StringComparison.InvariantCultureIgnoreCase));
            if (priceType != null)
            {
                return priceType;
            }
            return null;
        }

        public IEnumerable<IProductPriceType> GetCurrencyPriceTypes()
        {
            return PriceTypes.Where(x => x.Category == PriceTypeCategory.Currency);
        }

        public IEnumerable<IProductPriceType> GetVolumePriceTypes()
        {
            return PriceTypes.Where(x => x.Category == PriceTypeCategory.Volume);
        }


        public bool IsCurrencyPriceType(int priceTypeID)
        {
            return null != PriceTypes.SingleOrDefault(x => x.PriceTypeID == priceTypeID && x.Category == PriceTypeCategory.Currency);
        }

        public IProductPriceType GetPrimaryVolumeType()
        {
            return PriceTypes.Single(x => x.PriceTypeID == PrimaryVolumePriceTypeID);
        }


        public IEnumerable<IProductPriceType> GetAllPriceTypes()
        {
            return PriceTypes;
        }
    }
}
