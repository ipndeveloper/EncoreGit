

namespace NetSteps.Testing.Integration.DWS.Orders.Party
{
    public class DWS_Orders_Party_GuestSummary
    {
        public DWS_Orders_Party_GuestSummary(string name, decimal balance)
        {
            Name = name.Trim();
            Balance = balance;
        }

        public string Name
        { get; set; }

        public decimal Balance
        { get; set; }
    }
}
