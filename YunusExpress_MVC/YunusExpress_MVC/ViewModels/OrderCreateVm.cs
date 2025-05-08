namespace YunusExpress_MVC.ViewModels
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class OrderCreateVm
    {
        [Required(ErrorMessage = "Sifariş nömrəsi tələb olunur")]
        [Remote(action: "IsOrderNoAvailable", controller: "Order", ErrorMessage = "Bu Sifaris nömrəsi artıq mövcuddur.")]
        public int OrderNo { get; set; }

        [Required(ErrorMessage = "Qaimə nömrəsi tələb olunur")]
        [Remote(action: "IsInvoiceNoAvailable", controller: "Order", ErrorMessage = "Bu Qaimə nömrəsi artıq mövcuddur.")]
        public int InvoiceNo { get; set; }

        [Required(ErrorMessage = "Başlama tarixi tələb olunur")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        //[Required(ErrorMessage = "Bitmə tarixi tələb olunur")]
        //[DataType(DataType.DateTime)]
        //public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Xidmət seçilməlidir")]
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Qəbul edən seçilməlidir")]
        public int ReceiverId { get; set; }

        [Required(ErrorMessage = "Zəng edən şəxsin adı tələb olunur")]
        [MaxLength(100, ErrorMessage = "Ad maksimum 100 simvol ola bilər")]
        public string ZengEdeninAdi { get; set; }

        [Required(ErrorMessage = "Qəbul edənin adı tələb olunur")]
        [MaxLength(100)]
        public string ReceiverName { get; set; }

        [Required(ErrorMessage = "Qəbul edənin ünvanı tələb olunur")]
        public string ReceiverAddress { get; set; }

        [Required(ErrorMessage = "Qəbul edənin nömrəsi tələb olunur")]
        [Phone(ErrorMessage = "Telefon nömrəsi düzgün formatda deyil")]
        public string ReceiverPhoneNum { get; set; }

        [Required(ErrorMessage = "Zona seçilməlidir")]
        public int DeliveryZoneId { get; set; }

        [Required(ErrorMessage = "Göndərənin adı tələb olunur")]
        public string SenderName { get; set; }

        [Required(ErrorMessage = "Göndərənin nömrəsi tələb olunur")]
        [Phone(ErrorMessage = "Telefon nömrəsi düzgün formatda deyil")]
        public string SenderPhoneNum { get; set; }

        [Required(ErrorMessage = "Göndərənin ünvanı tələb olunur")]
        public string SenderAddress { get; set; }

        [Required(ErrorMessage = "Kuryer seçilməlidir")]
        public int CourierId { get; set; }

        [Required(ErrorMessage = "Sifariş qiyməti tələb olunur")]
        [Range(0, 100000, ErrorMessage = "Qiymət 0 ilə 100000 arasında olmalıdır")]
        public decimal OrderPrice { get; set; }

        [Range(0, 100000, ErrorMessage = "Xüsusi qiymət 0 ilə 100000 arasında olmalıdır")]
        public decimal? SpecialPrice { get; set; }

        [Range(0, 100, ErrorMessage = "Güzəşt 0-100% arasında olmalıdır")]
        public int? Discount { get; set; }

        public bool EDV { get; set; }

        [MaxLength(500, ErrorMessage = "Qeyd maksimum 500 simvol ola bilər")]
        public string? Note { get; set; }
    }

}
