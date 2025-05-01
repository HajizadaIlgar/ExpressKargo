namespace YunusExpress_MVC.Models
{
    public class DeliveryZoneType
    {
        public int Id { get; set; }
        public string TypeName { get; set; } // Şəhər içi, Rayon, Xarici

        public ICollection<DeliveryZone> Zones { get; set; }
        public ICollection<Sender> Senders { get; set; }
    }
}
