using System;
using System.Globalization;
using System.Web;
using NetSteps.Data.Entities;

namespace NetSteps.Web.Business
{
    [Serializable]
    public static class SessionState
    {
        public static CultureInfo CurrentCulture
        {
            get
            {
                if (HttpContext.Current.Session["CurrentCulture"] == null)
                    HttpContext.Current.Session["CurrentCulture"] = new CultureInfo("en-US", false);
                return (CultureInfo)HttpContext.Current.Session["CurrentCulture"];
            }
            set
            {
                HttpContext.Current.Session["CurrentCulture"] = value;
            }
        }

        public static Site CurrentSite
        {
            get
            {
                if (HttpContext.Current.Session["CurrentSite"] != null)
                    return (Site)HttpContext.Current.Session["CurrentSite"];
                else
                    return null;
            }
            set
            {
                HttpContext.Current.Session["CurrentSite"] = value;
            }
        }

        public static OrderSearchParams OrderSearchParams
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Session["OrderSearchParams"] != null)
                    return (OrderSearchParams)HttpContext.Current.Session["OrderSearchParams"];
                else
                    return new OrderSearchParams();
            }
            set
            {
                HttpContext.Current.Session["OrderSearchParams"] = value;
            }
        }

        public static AutoshipOrderSearchParams AutoshipOrderSearchParams
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Session["AutoshipOrderSearchParams"] != null)
                    return (AutoshipOrderSearchParams)HttpContext.Current.Session["AutoshipOrderSearchParams"];
                else
                    return new AutoshipOrderSearchParams();
            }
            set
            {
                HttpContext.Current.Session["AutoshipOrderSearchParams"] = value;
            }
        }

        public static AccountSearchParams AccountSearchParams
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Session["AccountSearchParams"] != null)
                    return (AccountSearchParams)HttpContext.Current.Session["AccountSearchParams"];
                else
                    return new AccountSearchParams();
            }
            set
            {
                HttpContext.Current.Session["AccountSearchParams"] = value;
            }
        }



        public static bool IsHttpContextNull
        {
            get
            {
                return (HttpContext.Current == null);
            }
        }

    }

    public class OrderSearchParams
    {
        public Constants.OrderStatus? OrderStatus { get; set; }
        public Constants.OrderType? OrderType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Customer { get; set; }
        public int AccountId { get; set; }
        public int CustomerId { get; set; }
        public int SearchId { get; set; }
        public string OrderNumber { get; set; }

    }

    public class AutoshipOrderSearchParams
    {
        public Constants.OrderStatus? OrderStatus { get; set; }
        public int AccountTypeId { get; set; }
        public string Customer { get; set; }
        public int CustomerId { get; set; }
        public DateTime? NextDueDate { get; set; }
        public string LastStatus { get; set; }
    }

    public class AccountSearchParams
    {
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Constants.AccountType? AccountTypeId { get; set; }
        public int AccountStatusId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Email { get; set; }
        public string Sponsor { get; set; }
        public string UserName { get; set; }
        public string Location { get; set; }
        public string State { get; set; }
    }
}
