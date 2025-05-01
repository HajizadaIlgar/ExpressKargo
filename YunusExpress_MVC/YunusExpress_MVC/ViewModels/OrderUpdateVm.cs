namespace YunusExpress_MVC.ViewModels
{
    public class OrderUpdateVm
    {
        public int Id { get; set; }
        public int OrderNo { get; set; } // Sifariş nömrəsi

        public int InvoiceNo { get; set; } // Qaimə nömrəsi


        // DateTime
        public DateTime StartDate { get; set; } // saat formatında
        public DateTime EndDate { get; set; }


        // Services: Daily, Regular, Express, Urgent
        public int ServiceId { get; set; }


        // Client - Qəbul edən
        public int ReceiverId { get; set; }



        // Göndərən müştəri
        public int Senderid { get; set; }

        public int CourierId { get; set; }



        // Qiymətlər hissəsi
        public decimal OrderPrice { get; set; }
        public decimal? SpecialPrice { get; set; }
        public int? Discount { get; set; } // Güzəşt (faizlə)
        //public decimal TotalPrices { get; set; }
        public int? EDV { get; set; } // ƏDV 18%, əgər şirkət ƏDV verirsə, əks halda nullable
        //public decimal FinalPrice { get; set; } // qiymət, güzəşt və s. hesablandıqdan sonra yekun
        public string? Note { get; set; } // əlavə qeydlər üçün
    }
}
