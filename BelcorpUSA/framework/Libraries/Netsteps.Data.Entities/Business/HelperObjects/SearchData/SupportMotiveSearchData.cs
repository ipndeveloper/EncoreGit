using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;
using System.Collections.Generic;

namespace NetSteps.Data.Entities.Business
{
    /// <summary>
    /// Period Business Entity to Search
    /// </summary>
    [Serializable]
    public class SupportMotiveSearchData
    {
        [Display(Name = "ID Motive")]
        public int SupportMotiveID { get; set; }

        [TermName("Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(AutoGenerateField = false)]
        public string TermName { get; set; }

        [TermName("Description")]
        [Display(Name = "Description")]
        public string Description { get; set; }

         
        [TermName("Status")]
        [Display(Name = "Status")]
        public bool Active { get; set; }

        [Display(AutoGenerateField = false)]
        public int SortIndex { get; set; }

        [Display(AutoGenerateField = false)]
        public bool IsVisibleToWorkStation { get; set; }

        [Display(AutoGenerateField = false)]
        public bool HasConfirmation { get; set; }

        [Display(AutoGenerateField = false)]
        public int MotiveSLA { get; set; }

        [TermName("Level")]
        [Display(Name = "Level")]
        public string LevelName { get; set; }

        [TermName("Created")]
        [Display(Name = "Created")]
        public DateTime DateCreatedUTC { get; set; }

        [TermName("Changed")]
        [Display(Name = "Changed")]
        public DateTime DateLastModifiedUTC { get; set; }


           [Display(AutoGenerateField = false)]
          public  int  SupportLevelID { get; set; } 

          [Display(AutoGenerateField = false)]
           public Boolean Edit { get; set; }
        
          [Display(AutoGenerateField = false)]
          public string Disabled 
          { get
              {
                  return
                      this.Edit ? "''" : "disabled='disabled'";
              }   
          }
         [Display(AutoGenerateField = false)]
          public string ColorFont
          {
              get
              {
                  return
                      this.Edit ? "''" : "#BCBCBC";
              }
          }

       public List<MarketSearchData> listaMarket = new List<MarketSearchData>();
       public List<SupportMotivePropertyTypeSearchData> listProperty = new List<SupportMotivePropertyTypeSearchData>();
       public SupportMotiveSearchData()
       {
           this.Edit = true;
       }
    }
}
