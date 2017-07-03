using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;
using System.Data;


namespace NetSteps.Data.Entities.Business
{
      [Serializable]
    public class MaterialsSearchData
    {

            public int MaterialID { get; set; }
            public string Name { get; set; }
            [Display(AutoGenerateField = false)]
            public string SKU { get; set; }
            public bool  Active  { get; set; }
            public decimal  EANCode { get; set; }
            public string  BPCSCode { get; set; }
            [DisplayFormat(ConvertEmptyStringToNull = false)]
            public string UnityType { get; set; }
            public decimal Weight   { get; set; }
            public decimal Volume    { get; set; }
            [DisplayFormat(ConvertEmptyStringToNull = false)]
            [Display(AutoGenerateField = false)]
            public string NCM  { get; set; }
            [Display(AutoGenerateField = false)]
            public decimal Origin { get; set; }
            [Display(AutoGenerateField = false)]
            public decimal OriginCountry { get; set; }
            [DisplayFormat(ConvertEmptyStringToNull = false)]
            public string Brand  { get; set; }
            [DisplayFormat(ConvertEmptyStringToNull = false)]
            public string  Group { get; set; }

            [TermName("CoApplicant")]
            [Display(AutoGenerateField = false)]
            public int MarketID { get; set; }

            [TermName("CoApplicant")]
            [Display(AutoGenerateField = false)]
            public int iTransaccion { get; set; }

            [Display(AutoGenerateField = false)]
            public int BrandID { get; set; }

            [Display(AutoGenerateField = false)]
            public int Duplicados { get; set; }

            
    }

    public class MaterialsSearchDataType : List<MaterialsSearchData>, IEnumerable<SqlDataRecord>
      {


          IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
          {
              var MaterialsSearchType = new SqlDataRecord(
                                        new SqlMetaData("MaterialID", SqlDbType.Int),
                                        new SqlMetaData("Name", SqlDbType.VarChar, 100),
                                        new SqlMetaData("SKU", SqlDbType.VarChar, 20),
                                        new SqlMetaData("Active", SqlDbType.Bit),
                                        new SqlMetaData("EANCode", SqlDbType.Decimal),
                                        new SqlMetaData("BPCSCode", SqlDbType.VarChar, 12),
                                        new SqlMetaData("UnityType", SqlDbType.VarChar, 2),
                                        new SqlMetaData("Weight", SqlDbType.Decimal),
                                        new SqlMetaData("Volume", SqlDbType.Decimal, 6, 2),
                                        //new SqlMetaData("NCM", SqlDbType.VarChar, 12),
                                        //new SqlMetaData("Origin", SqlDbType.Decimal),
                                        new SqlMetaData("OriginCountry", SqlDbType.Decimal, 9, 2),
                                        //new SqlMetaData("Brand", SqlDbType.VarChar, 6),
                                        new SqlMetaData("Group", SqlDbType.VarChar, 6),
                                        //new SqlMetaData("MarketID", SqlDbType.Int),
                                        new SqlMetaData("iTransaccion", SqlDbType.Int),
                                        new SqlMetaData("BrandID", SqlDbType.Int)
                                       
                                        );

              foreach (MaterialsSearchData Material in this)
              {

                    MaterialsSearchType.SetInt32(0, Material.MaterialID);
                    MaterialsSearchType.SetString(1, Material.Name);
                    MaterialsSearchType.SetString(2, Material.SKU);
                    MaterialsSearchType.SetBoolean(3, Material.Active);
                    MaterialsSearchType.SetDecimal(4, Material.EANCode);
                    MaterialsSearchType.SetString(5, Material.BPCSCode);
                    MaterialsSearchType.SetString(6, Material.UnityType);
                    MaterialsSearchType.SetDecimal(7, Material.Weight);
                    MaterialsSearchType.SetDecimal(8, Material.Volume);
                    //MaterialsSearchType.SetString(9, Material.NCM);
                    //MaterialsSearchType.SetDecimal(10, Material.Origin);
                    MaterialsSearchType.SetDecimal(9, Material.OriginCountry);
                    //MaterialsSearchType.SetString(12, Material.Brand);
                    MaterialsSearchType.SetString(10, Material.Group);
                    //MaterialsSearchType.SetInt32(13, Material.MarketID);
                    MaterialsSearchType.SetInt32(11, Material.iTransaccion);
                    //MaterialsSearchType.SetInt32(15, Material.iTransaccion);
                    MaterialsSearchType.SetInt32(12, Material.BrandID);

                    yield return MaterialsSearchType;
              }
      }

    }

    public class BrandSP
    {
        public int BrandID { get; set; }
        public string Name { get; set; }
    
    }
}
