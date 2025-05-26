using YunusExpress_MVC.Models;

namespace YunusExpress_MVC.ViewModels
{
    public class InvoiceDto
    {
        public IEnumerable<InvoiceToListVM> PartOfInvoice { get; set; }
        public IEnumerable<Receiver> Receivers { get; set; }
        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<PriceToListVM> PartOfPrice { get; set; }
        public decimal TotalPrice { get; set; } // ⬅ cəmi qiymət
    }
}
