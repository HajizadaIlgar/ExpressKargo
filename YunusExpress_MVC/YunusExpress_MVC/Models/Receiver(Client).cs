namespace YunusExpress_MVC.Models
{
    public class Receiver //Qebul Eden
    {
        public int Id { get; set; }

        public string ClientCode { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverPhoneNum { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
