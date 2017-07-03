using System;

namespace NetSteps.Testing.Integration
{
    public static class OrderStatus
    {
        public enum ID
        {
            Pending,
            PendingError,
            Paid,
            Cancelled,
            PartiallyPaid,
            Printed,
            Shipped,
            CreditCardDeclined,
            CreditCardDeclinedRetry,
            PartiallyShipped,
            CancelledPaid,
            DeferredOnlinePayment,
            Suspended,
            PartyOrderPending
        }

        public static string ToPattern(this OrderStatus.ID orderStatus)
        {
            string result;
            switch (orderStatus)
            {
                case ID.PendingError:
                    result = "Pending Error";
                    break;
                case ID.PartiallyPaid:
                    result = "Partially Paid";
                    break;
                case ID.CreditCardDeclined:
                    result = "Credit Card Declined";
                    break;
                case ID.CreditCardDeclinedRetry:
                    result = "Credit Card Declined - Retry";
                    break;
                case ID.PartiallyShipped:
                    result = "Partially Shipped";
                    break;
                case ID.CancelledPaid:
                    result = "Cancelled Paid";
                    break;
                case ID.DeferredOnlinePayment:
                    result = "Deferred Online Payment";
                    break;
                case ID.PartyOrderPending:
                    result = "Party Order Pending";
                    break;
                default:
                    result = orderStatus.ToString();
                    break;
            }
            return result;
        }

        public static ID Parse(string value)
        {
            ID result;
            switch (value)
            {
                case "Pending Error":
                    result = ID.PendingError;
                    break;
                case "Partially Paid":
                    result = ID.PartiallyPaid;
                    break;
                case "Credit Card Declined":
                    result = ID.CreditCardDeclined;
                    break;
                case "Credit Card Declined - Retry":
                    result = ID.CreditCardDeclinedRetry;
                    break;
                case "Partially Shipped":
                    result = ID.PartiallyShipped;
                    break;
                case "Cancelled Paid":
                    result = ID.CancelledPaid;
                    break;
                case "Deferred Online Payment":
                    result = ID.DeferredOnlinePayment;
                    break;
                case "Party Order Pending":
                    result = ID.PartyOrderPending;
                    break;
                default:
                    result = (ID)Enum.Parse(typeof(ID), value);
                    break;
            }
            return result;
        }
    }
}
