using YunusExpress_MVC.Models;

namespace YunusExpress_MVC.ViewModels
{
    public class ReceiverMontlySalaryVM
    {
        public List<Order> Orders { get; set; }
        public List<Receiver> Receivers { get; set; }
        public decimal TotalSalary { get; set; }
        public decimal FinalPrice { get; set; }
    }
}
