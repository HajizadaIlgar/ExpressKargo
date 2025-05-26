namespace YunusExpress_MVC.Models
{
    public class Courier
    {
        public int CourierId { get; set; }
        public int CourierCode { get; set; }
        public string CourierName { get; set; }
        public string? CourierPhoneNum { get; set; }


        public ICollection<Order> Orders { get; set; }
        public int OrdersId { get; set; }
    }
}
