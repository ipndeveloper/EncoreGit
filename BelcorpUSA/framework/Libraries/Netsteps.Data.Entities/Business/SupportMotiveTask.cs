using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
   public  class SupportMotiveTask
    {

    public  int SupportMotiveTaskID	 { get;set;}
    public   Int16 SupportMotiveID	 { get;set;}
    public string Name	 { get;set;}
    public string TermName	 { get;set;}
    public string Link	 { get;set;}
    public  int SupportMotivePropertyTypeID	 { get;set;}
    public int SortIndex { get; set; }
    }
}
