using YunusExpress_MVC.Models;

namespace YunusExpress_MVC.ViewModels
{
    public class OrderFilterViewModel
    {
        public DateTime? StartDate { get; set; }
        public List<Order> Orders { get; set; } = new();
    }
}
