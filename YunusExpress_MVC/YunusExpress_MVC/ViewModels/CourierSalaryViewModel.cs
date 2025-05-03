namespace YunusExpress_MVC.ViewModels
{
    public class CourierSalaryViewModel
    {
        public int OrderNo { get; set; }
        //public int InvoiceNo { get; set; }

        //public string SenderName { get; set; }
        //public string SenderAddress { get; set; }
        //public string DeliveryZone { get; set; }

        //public string ReceiverName { get; set; }
        //public string ReceiverAddress { get; set; }

        //public int OrderPrice { get; set; }

        public string CourierName { get; set; }
        public decimal TotalDeliveredAmount { get; set; }
        public decimal Salary { get; set; }

    }
}
