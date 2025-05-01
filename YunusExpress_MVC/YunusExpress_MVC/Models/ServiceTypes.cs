namespace YunusExpress_MVC.Models
{
    public class ServiceTypes
    {
        public int Id { get; set; }
        public string? ServiceType { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
