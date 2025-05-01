namespace YunusExpress_MVC.Models
{
    public class Sender //Gonderen
    {
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        public string SenderAddress { get; set; }
        public string SenderPhoneNum { get; set; }
        public ICollection<DeliveryZoneType> DeliveryZones { get; set; }

        //public ICollection<Order> Orders { get; set; }
    }
}
