using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nsDistributor.Models.Shared
{
    public class ParameterCountryModel
    {
        public virtual int Id { get; set; }
        public virtual int CountryId { get; set; }
        public virtual string Controls { get; set; }
        public virtual bool Active { get; set; }
        public virtual string Descriptions { get; set; }
        public virtual int Step { get; set; }
        public virtual int Sites { get; set; }
    }
}