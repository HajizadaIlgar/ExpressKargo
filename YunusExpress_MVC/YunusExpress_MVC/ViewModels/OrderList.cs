using YunusExpress_MVC.Models;

namespace YunusExpress_MVC.ViewModels
{
    public class OrderList
    {
        public int Id { get; set; } // unikal ID (OrderNo-dan fərqli ola bilər)

        public int OrderNo { get; set; } // Sifariş nömrəsi
        public int InvoiceNo { get; set; } // Qaimə nömrəsi

        // DateTime
        public DateTime StartDate { get; set; } // saat formatında
        //public DateTime EndDate { get; set; }


        // Services: Daily, Regular, Express, Urgent
        public ServiceTypes? ServiceType { get; set; }
        public int ServiceId { get; set; }

        public string ZengEdeninAdi { get; set; }

        public string ReceiverName { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverPhoneNum { get; set; }
        // Client - Qəbul edən
        //public int ReceiverId { get; set; }
        //public Receiver? Receiver { get; set; }


        public string SenderName { get; set; }
        public string SenderPhoneNum { get; set; }
        public string SenderAddress { get; set; }

        public int DeliveryZoneId { get; set; }
        public DeliveryZone DeliveryZone { get; set; }

        // Göndərən müştəri
        //public int Senderid { get; set; }
        //public Sender Sender { get; set; }

        public int CourierId { get; set; }
        public Courier Courier { get; set; }



        // Qiymətlər hissəsi
        public decimal OrderPrice { get; set; }
        public decimal? SpecialPrice { get; set; }
        public int? Discount { get; set; } // Güzəşt (faizlə)
        //public decimal TotalPrices { get; set; }
        public bool EDV { get; set; } // ƏDV 18%, əgər şirkət ƏDV verirsə, əks halda nullable
        //public decimal FinalPrice { get; set; } // qiymət, güzəşt və s. hesablandıqdan sonra yekun
        public string? Note { get; set; } // əlavə qeydlər üçün
    }
}
