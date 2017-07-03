using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace EFTQuery.Common
{
    public static class IEFTQueryProcessorResultExtensions
    {

        public static string ToXml(this IEnumerable<IEFTQueryProcessorResult> processorResults)
        {
            var paymentXML = processorResults.Select(res =>
                {
                    var node = new XElement("Payment");
                    node.Add(new XElement("EntryClassCode", res.NachaClassType));
                    node.Add(new XElement("TransactionCode", res.TransactionCode));
                    node.Add(new XElement("RoutingNumber", res.RoutingNumber));
                    node.Add(new XElement("AccountNumber", res.AccountNumber));
                    node.Add(new XElement("Amount", res.Amount));
                    node.Add(new XElement("TransactionDate", res.TransactionDate));
                    node.Add(new XElement("IndividualName", res.IndividualName));
                    node.Add(new XElement("OrderId", res.OrderId));
                    node.Add(new XElement("CountryCode", res.CountryCode));
                    node.Add(new XElement("TransactionNumber", res.TransactionNumber));
                    node.Add(new XElement("BankAccountType", res.BankAccountType));
                    node.Add(new XElement("OrderPaymentId", res.OrderPaymentId));
                    return node;
                });

            XElement configuration = new XElement("EFTPayments", paymentXML);
            return configuration.ToString();
        }

        public static string ToCsv(this IEnumerable<IEFTQueryProcessorResult> processorResults)
        {
            var paymentsList = processorResults.Select(res =>
                {
                    return string.Join(",", new [] {
                        res.NachaClassType.ToString(), 
                        res.TransactionCode.ToString(),
                        res.RoutingNumber.ToString(),
                        res.AccountNumber.ToString(),
                        res.Amount.ToString(),
                        res.TransactionDate.ToString(),
                        res.IndividualName.ToString(),
                        string.IsNullOrEmpty(res.OrderId) == true ? "" : res.OrderId.ToString(),
                        res.CountryCode.ToString(),
                        res.TransactionNumber.ToString(),
                        res.BankAccountType.ToString(),
                        res.OrderPaymentId
                    });
                });

            StringBuilder delimitedPayments = new StringBuilder("");
            foreach (string payment in paymentsList)
            {
                delimitedPayments.Append(payment);
                delimitedPayments.Append("\r\n");
            }
            return delimitedPayments.ToString();
        }

        public static string DictionaryToXML(this Dictionary<int, bool> updateResults)
        {
            var paymentXML = updateResults.Select(res =>
            {
                var node = new XElement("Result");
                node.Add(new XElement("OrderPaymentId", res.Key.ToString()));
                node.Add(new XElement("Success", res.Value.ToString()));
                return node;
            });

            XElement configuration = new XElement("EFTPayments", paymentXML);
            return configuration.ToString();
        }

        public static string DictionaryToCSV(this Dictionary<int, bool> updateResults)
        {
            var paymentsList = updateResults.Select(res =>
            {
                return string.Join(",", new[] {
                        res.Key.ToString(), 
                        res.Value.ToString()
                });
            });

            StringBuilder delimitedPayments = new StringBuilder("");
            foreach (string payment in paymentsList)
            {
                delimitedPayments.Append(payment);
                delimitedPayments.Append("\r\n");
            }
            return delimitedPayments.ToString();
        }
    }
}
