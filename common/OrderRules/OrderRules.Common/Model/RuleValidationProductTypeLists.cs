//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OrderRules.Core.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class RuleValidationProductTypeLists
    {
     
        public RuleValidationProductTypeLists()
        {
            this.RuleValidationProductTypeListItems = new HashSet<RuleValidationProductTypeListItems>();
        }
    
        public int RuleValidationID { get; set; }
        public bool IsIncludeList { get; set; }
    
        public virtual ICollection<RuleValidationProductTypeListItems> RuleValidationProductTypeListItems { get; set; }
        public virtual RuleValidations RuleValidations { get; set; }
    }
}
