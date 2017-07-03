using System;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Common;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Plugins.Common.Helpers;
using NetSteps.Promotions.Plugins.Common.PromotionKinds;

namespace DependentClass
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = Create.New<IPromotionService>();
            string promoNamePrefix = "January Promotion"; 

            Console.WriteLine("Enter host promo product ID:");
            int hostProductId = int.Parse(Console.ReadLine());
            DateTime startDate = new DateTime(2014, 1, 1), endDate = new DateTime(2014, 2, 1);
            Console.WriteLine("Creating host promotion....");
            CreateNovHostPromo(service, promoNamePrefix, hostProductId, 1, 1, 500, startDate, endDate); //us host promo, min $500 
            CreateNovHostPromo(service, promoNamePrefix, hostProductId, 2, 2, 500, startDate, endDate); //ca host promo, min $500 
            Console.WriteLine("Host promotion created.");
            
            Console.WriteLine("Enter guest promo product ID:");
            int guestProductId = int.Parse(Console.ReadLine());

            Console.WriteLine("Creating guest promotion....");
            CreateNovGuestPromo(service, promoNamePrefix, guestProductId, 1, 1, 100, startDate, endDate); //us guest promo, min $125
            CreateNovGuestPromo(service, promoNamePrefix, guestProductId, 2, 2, 100, startDate, endDate); //ca guest promo, min $150
            Console.WriteLine("Guest promotion created");

            Console.WriteLine("Completed!");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        static void CreateNovHostPromo(IPromotionService service, string promoNamePrefix, int promoProductId, int marketId, int currencyId, int minSubtotal, DateTime start, DateTime end)
        {
            var hostPromo = Create.New<IOrderPromotionDefaultCartRewards>();
            hostPromo.StartDate = start;
            hostPromo.EndDate = end;
            hostPromo.Description = promoNamePrefix + " (Host)";
            hostPromo.PromotionStatusTypeID = (int)PromotionStatus.Enabled;
            hostPromo.AddOrderTypeID((int)OrderType.PartyOrder);
            hostPromo.MarketID = marketId;
            //set subtotals 
            hostPromo.SetOrderPriceTypeTotalRange(minSubtotal, null, currencyId, (int)ProductPriceType.QV);
            //mark as host only
            hostPromo.OnlyApplyToPartyHost = true;
            var productOption = Create.New<IProductOption>();
            productOption.ProductID = promoProductId;
            productOption.Quantity = 1;
            hostPromo.AddRewardProduct(productOption);

            IPromotionState promotionState = null;
            service.AddPromotion(hostPromo, out promotionState);
            Console.WriteLine(promotionState.ToString());
        }

        static void CreateNovGuestPromo(IPromotionService service, string promoNamePrefix, int promoProductId, int marketId, int currencyId, int minSubtotal, DateTime start, DateTime end)
        {
            var guestPromo = Create.New<IOrderPromotionDefaultCartRewards>();
            guestPromo.StartDate = start;
            guestPromo.EndDate = end;
            guestPromo.Description = promoNamePrefix + " (Guest)";
            guestPromo.PromotionStatusTypeID = (int)PromotionStatus.Enabled;
            guestPromo.AddOrderTypeID((int)OrderType.PartyOrder);
            guestPromo.AddOrderTypeID((int)OrderType.OnlineOrder);
            guestPromo.MarketID = marketId;
            //set subtotals 
            guestPromo.SetCustomerPriceTypeTotalRange(minSubtotal, null, currencyId, (int)ProductPriceType.QV);
            var productOption = Create.New<IProductOption>();
            productOption.ProductID = promoProductId; 
            productOption.Quantity = 1;
            guestPromo.AddRewardProduct(productOption);

            IPromotionState promotionState = null;
            service.AddPromotion(guestPromo, out promotionState);
            Console.WriteLine(promotionState.ToString());
        }
    }
}
