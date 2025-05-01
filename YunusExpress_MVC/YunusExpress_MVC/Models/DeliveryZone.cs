namespace YunusExpress_MVC.Models
{
    public class DeliveryZone
    {
        public int Id { get; set; }
        public string Name { get; set; } // "Yasamal", "Quba", "Türkiyə" və s.

        public int DeliveryZoneTypeId { get; set; }
        public DeliveryZoneType DeliveryZoneType { get; set; }
        
        public ICollection<Order> Orders { get; set; }

    }
}
