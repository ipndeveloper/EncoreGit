using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Repositories.Interfaces;
using NetSteps.Data.Entities.Dto;
using NetSteps.Data.Entities.EntityModels;

namespace NetSteps.Data.Entities.Repositories
{
    public class PromoPromotionTypeConfigurationsRepository : IPromoPromotionTypeConfigurationsRepository
    {
        #region Private

        /// <summary>
        /// Transforms dto object in table object
        /// </summary>
        /// <param name="dto">Promotion Type Configurations Dto</param>
        /// <returns>Promotion Type Configurations Table</returns>
        private PromoPromotionTypeConfigurationTable DtoToTable(PromoPromotionTypeConfigurationsDto dto)
        {
            if (dto == null)
                return null;

            return new PromoPromotionTypeConfigurationTable()
            {
               PromotionTypeConfigurationID = dto.PromotionTypeConfigurationID,
               PromotionTypeID = dto.PromotionTypeID,
               Active = dto.Active
            };
        }

        private PromoPromotionTypeConfigurationsDto TableToDto(PromoPromotionTypeConfigurationTable table)
        {
            if (table == null)
                return null;

            return new PromoPromotionTypeConfigurationsDto()
            {
                PromotionTypeConfigurationID = table.PromotionTypeConfigurationID,
                PromotionTypeID = table.PromotionTypeID,
                Active = table.Active
            };
        }

        #endregion

        /// <summary>
        /// Inactivates all records
        /// </summary>
        /// <returns></returns>
        public bool InactivateAll()
        {
            var result = false;
            using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {                
                var data =(from r in DbContext.PromoPromotionTypeConfigurations select r).ToList();

                data.ForEach(d => d.Active = false);

                DbContext.SaveChanges();

                result = true;
                return result;
            }
        }
      
        /// <summary>
        /// Inserts a new record
        /// </summary>
        /// <param name="oPromotionConfiguration">object PromoPromotionTypeConfigurationsDto</param>
        /// <param name="outLastGeneratedID">out parameter</param>
        /// <returns>returns true if the operations was successfully</returns>
        public bool Insert(PromoPromotionTypeConfigurationsDto oPromotionConfiguration, out int outLastGeneratedID) 
        {
            var result = false;
            using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                PromoPromotionTypeConfigurationTable table = DtoToTable(oPromotionConfiguration);

                DbContext.PromoPromotionTypeConfigurations.Add(table);
                DbContext.SaveChanges();
                outLastGeneratedID = table.PromotionTypeConfigurationID;
                result = true;
                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<PromoPromotionTypeConfigurationsDto> ListAll()
        {
            using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var data = (from r in DbContext.PromoPromotionTypeConfigurations
                            select new PromoPromotionTypeConfigurationsDto()
                            {
                                PromotionTypeConfigurationID = r.PromotionTypeConfigurationID,
                                PromotionTypeID = r.PromotionTypeID,
                                Active = r.Active
                            });
                if (data == null)
                    throw new Exception("Promotion Type Configurations were not found");
                else
                    return data.ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetLastConfiguration()
        {
            using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var data = (DbContext.PromoPromotionTypeConfigurations.Where(r => r.Active == true).Select(r => r.PromotionTypeConfigurationID));
                            
                if (data == null)
                    throw new Exception("Promotion Type Configurations were not found");
                
                 return data.FirstOrDefault();
            }
        }

        //Descuento Acumulativo = 2 {Tabla: BelcorpUSACore.Promo.PromotionTypes}
        public int GetPromocionTypeDescuentoAcumulativo()
        {
            using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var data = (DbContext.PromoPromotionTypes.Where(r => r.Active == true & r.PromotionTypeID == 2).Select(r => r.PromotionTypeID));

                if (data == null)
                    throw new Exception("Promotion Type Configurations were not found");

                return data.FirstOrDefault();
            }
        }

        public int GetActive()
        {
            using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var data = (DbContext.PromoPromotionTypeConfigurations.Where(r => r.Active).Select(r => r.PromotionTypeID));

                if (data == null)
                    throw new Exception("Promotion Type Configurations were not found");

                return data.FirstOrDefault();
            }
        }
    }
}
