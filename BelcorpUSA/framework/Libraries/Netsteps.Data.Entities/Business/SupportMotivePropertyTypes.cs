using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public class SupportMotivePropertyTypes
    {
    public int SupportMotivePropertyTypeID	{get;set;}
    public Int16 SupportMotiveID	{get;set;}
    public string Name	{get;set;}
    public string TermName	{get;set;}
    public string DataType	{get;set;}
    public Boolean   Required	{get;set;}
    public int  SortIndex { get; set; }
    public Boolean Editable { get; set; }
    public Boolean IsVisibleToWorkStation { get; set; }
    public string PropertyValue { get; set; }
    public int SupportTicketsPropertyValueID { get; set; }
    public int SupportTicketsPropertyID { get; set; }
    public Boolean FieldSolution { get; set; }
    public string DinamicName { get; set; }
    }
}
