using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
   public  class SupportMotivePropertyValues
    {
        public int SupportMotivePropertyValueID	{get;set;}
        public  int SupportMotivePropertyTypeID	{get;set;}
        public string Value	{get;set;}
        public string TermName	{get;set;}
        public int SortIndex	{get;set;}
    }
}
