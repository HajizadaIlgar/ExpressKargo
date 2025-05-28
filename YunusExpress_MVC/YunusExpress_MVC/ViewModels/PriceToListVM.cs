namespace YunusExpress_MVC.ViewModels
{
    public class PriceToListVM
    {
        public int OrderNo { get; set; }
        public string? DaxiliCattirilma { get; set; }
        public int? Discount1 { get; set; }
        public string? BeynexalqCattirilma { get; set; }
        public int? DeliveryZone { get; set; }
        public int? Discount2 { get; set; }
        public bool Edv { get; set; }
        public string Total { get; set; }
    }
}
