using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Dto
{
   public class AccountLocator
    {
    public int AccountID {get;set;}
    public string     FirstName  {get;set;}
    public string     LastName   {get;set;}
    public string City   {get;set;}
    public string State   {get;set;}
    public int CountryID   {get;set;}
    public string PwsUrl  {get;set;}
    public string html { get; set; }
    public string Street { get; set; }
   
   
    public string Country { get; set; }
    public string Lograduro { get; set; }
    public string EmailAddress { get; set; }

    public string PhoneNumber { get; set; }
    }
}
