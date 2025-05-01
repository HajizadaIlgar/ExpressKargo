using YunusExpress_MVC.Models;

namespace YunusExpress_MVC.Helpers
{
    public static class OrderHelper
    {
        public static decimal CalculateFinalPrice(this Order order)
        {
            // 1. Əsas qiyməti müəyyənləşdir
            decimal basePrice = order.SpecialPrice ?? order.OrderPrice;

            // 2. Güzəşt tətbiq et (əgər varsa)
            if (order.Discount.HasValue)
            {
                basePrice -= basePrice * order.Discount.Value / 100;
            }

            // 3. ƏDV tətbiq et (əgər varsa)
            if (order.EDV.HasValue)
            {
                basePrice += basePrice * order.EDV.Value / 100;
            }

            return Math.Round(basePrice, 2); // Yekun qiyməti yuvarlaqlaşdır
        }
    }

}
