using YunusExpress_MVC.Models;

namespace YunusExpress_MVC.ViewModels
{
    public class CourierSalaryPageViewModel
    {
        public List<Order> Orders { get; set; }
        public List<Courier> Couriers { get; set; }
        public decimal TotalSalary { get; set; }
    }
}
